using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    public List<Card> placedCards = new();

    private void Awake()
    {
        instance = this;
    }

    public int startingMana = 7, maxMana = 15;
    public int playerMana;
    public int currentPlayerMaxMana;

    public int startingCardsAmount = 8;
    public int cardToDrawPerTurn = 8; //we want the player to always have 8 cards

    public enum TurnOrder { challengePhase, playerEngage, enemyEngage, playerInvestigate,
        enemyInvestigate, playerAct, enemyAct, presentationPhase }

    public string[] challengeNames = new string[] { "Challenge 1", "Challenge 2", "Challenge 3", "Challenge 4", "Finale" };
    public string[] challengeFeatures = new string[] { "Coding", "Design", "Research", "C + D", "D + R" };
    public int enemyScore, playerScore;

    private string chosenChallenge, chosenFeature;

    private int challengeNumber = 0;

    public TurnOrder currentPhase = 0;

    public Transform discardPoint;

    public int playerWins = 0, enemyWins = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        currentPlayerMaxMana = startingMana;

        chosenChallenge = challengeNames[challengeNumber];
        chosenFeature = challengeFeatures[Random.Range(0, challengeFeatures.Length)];

        UIController.instance.ShowChallengeScreen(chosenChallenge, chosenFeature);

        challengeNumber++;

        UIController.instance.SetChallengeNameText(chosenChallenge);
        UIController.instance.SetChallengeFeatureText(chosenFeature);
        UIController.instance.SetEnemyScoreText(0);
        UIController.instance.SetPlayerScoreText(0);
        UIController.instance.UpdateScoreMupltiplierText(1f);

        FillPlayerMana();

        DeckController.instance.DrawMultipleCards(startingCardsAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AdvanceTurn();
        }
    }

    public void CaluclateMentorBonusPoints(Card mentor)
    {
        float actualProcessValue = mentor.processValue - 1;

        UIController.instance.UpdatePlayerScoreText(0, actualProcessValue);
    }

    public void CalculatePoints(Card card)
    {
        float actualProcessValue = card.processValue - 1;

        switch (chosenFeature)
        {
            case "Coding":

                if (card.cardFaction == "Coding")
                {
                    UIController.instance.UpdatePlayerScoreText(card.codingValue * 2, actualProcessValue);
                } else
                {
                    UIController.instance.UpdatePlayerScoreText(card.codingValue, actualProcessValue);
                }
                break;

            case "Design":

                if (card.cardFaction == "Design")
                {
                    UIController.instance.UpdatePlayerScoreText(card.designValue * 2, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdatePlayerScoreText(card.designValue, actualProcessValue);
                }
                break;

            case "Research":

                if (card.cardFaction == "Research")
                {
                    UIController.instance.UpdatePlayerScoreText(card.researchValue * 2, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdatePlayerScoreText(card.researchValue, actualProcessValue);
                }
                break;

            case "C + D":

                if (card.cardFaction == "Coding")
                {
                    UIController.instance.UpdatePlayerScoreText((card.codingValue * 2) + card.designValue, actualProcessValue);
                }
                else if (card.cardFaction == "Design")
                {
                    UIController.instance.UpdatePlayerScoreText((card.designValue * 2) + card.codingValue, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdatePlayerScoreText(card.designValue + card.codingValue, actualProcessValue);
                }
                break;

            case "D + R":

                if (card.cardFaction == "Research")
                {
                    UIController.instance.UpdatePlayerScoreText((card.researchValue * 2) + card.designValue, actualProcessValue);
                }
                else if (card.cardFaction == "Design")
                {
                    UIController.instance.UpdatePlayerScoreText((card.designValue * 2) + card.researchValue, actualProcessValue);
                } else
                {
                    UIController.instance.UpdatePlayerScoreText(card.designValue + card.researchValue, actualProcessValue);
                }
                break;

        }
    }

    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana -= amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetManaText(playerMana);
    }

    public void FillPlayerMana()
    {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetManaText(playerMana);
    }

    public void AdvanceTurn()
    {
        
        currentPhase++;
        if((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }

        switch (currentPhase)
        {
            case TurnOrder.challengePhase:

                if (playerWins == 3)
                {
                    playerWins = 0;
                    challengeNumber = 0;
                    UIController.instance.HideFinalScreen();
                }
                else if (enemyWins == 3)
                {
                    enemyWins = 0;
                    challengeNumber = 0;
                    UIController.instance.HideFinalScreen();
                }

                chosenChallenge = challengeNames[challengeNumber];
                chosenFeature = challengeFeatures[Random.Range(0, challengeFeatures.Length)];

                UIController.instance.ShowChallengeScreen(chosenChallenge,chosenFeature);

                Debug.Log(challengeNumber);

                if (this.placedCards.Count != 0)
                {
                    foreach(var card in this.placedCards)
                    {
                        card.MoveToPoint(discardPoint.position, discardPoint.rotation);

                        Destroy(card, 5f);
                    }
                    
                }

                if (challengeNumber < challengeNames.Length)
                {
                    challengeNumber++;
                } else
                {
                    challengeNumber = 0;
                }

                UIController.instance.SetChallengeNameText(chosenChallenge);
                UIController.instance.SetChallengeFeatureText(chosenFeature);
                UIController.instance.SetEnemyScoreText(0);
                UIController.instance.SetPlayerScoreText(0);

                break;

            case TurnOrder.playerEngage:

                UIController.instance.HideChallengeScreen();

                UIController.instance.endTurnButton.SetActive(true);

                if(currentPlayerMaxMana < maxMana)
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();
                DeckController.instance.DrawMultipleCards(8 - HandController.instance.heldCards.Count);

                break;

            case TurnOrder.enemyEngage:

                break;

            case TurnOrder.playerInvestigate:

                UIController.instance.endTurnButton.SetActive(true);

                break;

            case TurnOrder.enemyInvestigate:

                break;

            case TurnOrder.playerAct:

                UIController.instance.endTurnButton.SetActive(true);

                break;

            case TurnOrder.enemyAct:

                break;

            case TurnOrder.presentationPhase:

                if (UIController.instance.enemyTotalScore > UIController.instance.playerTotalScore)
                {
                    enemyWins++;
                    Debug.Log("HAI PERSO");
                }
                else if (UIController.instance.enemyTotalScore < UIController.instance.playerTotalScore)
                {
                    playerWins++;
                }
                else
                {
                    playerWins++;
                    enemyWins++;
                    Debug.Log("PAREGGIO");
                }

                if (playerWins == 3)
                {
                    Debug.Log("FINE");
                    UIController.instance.ShowFinalScreen("You win");
                } else if (enemyWins == 3)
                {
                    UIController.instance.ShowFinalScreen("You lose");
                }

                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);

        AdvanceTurn();
    }
}
