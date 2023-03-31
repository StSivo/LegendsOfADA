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

<<<<<<< HEAD
    public int startingMana = 7, maxMana = 15;
=======
    public int startingMana = 4, maxMana = 12;
>>>>>>> main
    public int enemyMana, playerMana;
    public int currentEnemyMaxMana, currentPlayerMaxMana;

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
<<<<<<< HEAD
        InitializeGame();
    }
=======
        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;
>>>>>>> main

    private void InitializeGame()
    {
        currentPlayerMaxMana = startingMana;
        currentEnemyMaxMana = startingMana;

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
        FillEnemyMana();
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

<<<<<<< HEAD
    public void CaluclateMentorBonusPoints(Card mentor)
    {
        float actualProcessValue = mentor.processValue - 1;

        UIController.instance.UpdatePlayerScoreText(0, actualProcessValue);
    }

    public void CalculatePoints(Card card)
=======
    public void CalculatePlayerPoints(Card card)
>>>>>>> main
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

    public void CalculateEnemyPoints(Card card)
    {
        float actualProcessValue = card.processValue - 1;

        switch (chosenFeature)
        {
            case "Coding":

                if (card.cardFaction == "Coding")
                {
                    UIController.instance.UpdateEnemyScoreText(card.codingValue * 2, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdateEnemyScoreText(card.codingValue, actualProcessValue);
                }
                break;

            case "Design":

                if (card.cardFaction == "Design")
                {
                    UIController.instance.UpdateEnemyScoreText(card.designValue * 2, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdateEnemyScoreText(card.designValue, actualProcessValue);
                }
                break;

            case "Research":

                if (card.cardFaction == "Research")
                {
                    UIController.instance.UpdateEnemyScoreText(card.researchValue * 2, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdateEnemyScoreText(card.researchValue, actualProcessValue);
                }
                break;

            case "C + D":

                if (card.cardFaction == "Coding")
                {
                    UIController.instance.UpdateEnemyScoreText((card.codingValue * 2) + card.designValue, actualProcessValue);
                }
                else if (card.cardFaction == "Design")
                {
                    UIController.instance.UpdateEnemyScoreText((card.designValue * 2) + card.codingValue, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdateEnemyScoreText(card.designValue + card.codingValue, actualProcessValue);
                }
                break;

            case "D + R":

                if (card.cardFaction == "Research")
                {
                    UIController.instance.UpdateEnemyScoreText((card.researchValue * 2) + card.designValue, actualProcessValue);
                }
                else if (card.cardFaction == "Design")
                {
                    UIController.instance.UpdateEnemyScoreText((card.designValue * 2) + card.researchValue, actualProcessValue);
                }
                else
                {
                    UIController.instance.UpdateEnemyScoreText(card.designValue + card.researchValue, actualProcessValue);
                }
                break;

        }
    }

<<<<<<< HEAD
    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana -= amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }
        UIController.instance.SetEnemyManaText(enemyMana);
    }

=======
>>>>>>> main
    public void SpendPlayerMana(int amountToSpend)
    {
        playerMana -= amountToSpend;

        if (playerMana < 0)
        {
            playerMana = 0;
        }

        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void SpendEnemyMana(int amountToSpend)
    {
        enemyMana -= amountToSpend;

        if (enemyMana < 0)
        {
            enemyMana = 0;
        }
        UIController.instance.SetEnemyManaText(enemyMana);
    }

    public void FillPlayerMana()
    {
        playerMana = currentPlayerMaxMana;
        UIController.instance.SetPlayerManaText(playerMana);
    }

    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemyManaText(enemyMana);

    }

    public void FillEnemyMana()
    {
        enemyMana = currentEnemyMaxMana;
        UIController.instance.SetEnemyManaText(enemyMana);
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
                    foreach (var card in EnemyController.instance.placedCards)
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

                if (currentEnemyMaxMana < maxMana)
                {
                    currentEnemyMaxMana++;
                }

                FillEnemyMana();
                EnemyController.instance.StartAction();
<<<<<<< HEAD
                EnemyController.instance.StartAction();
                EnemyController.instance.StartAction();
=======
>>>>>>> main
                break;

            case TurnOrder.playerInvestigate:

                UIController.instance.endTurnButton.SetActive(true);

                break;

            case TurnOrder.enemyInvestigate:
                EnemyController.instance.StartAction();
                break;

            case TurnOrder.playerAct:

                UIController.instance.endTurnButton.SetActive(true);

                break;

            case TurnOrder.enemyAct:
                EnemyController.instance.StartAction();
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
