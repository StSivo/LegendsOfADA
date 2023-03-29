using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    public static BattleController instance;

    private void Awake()
    {
        instance = this;
    }

    public int startingMana = 4, maxMana = 12;
    public int playerMana;
    public int currentPlayerMaxMana;

    public int startingCardsAmount = 8;
    public int cardToDrawPerTurn = 8; //we want the player to always have 8 cards

    public enum TurnOrder { challengePhase, playerEngage, enemyEngage, playerInvestigate,
        enemyInvestigate, playerAct, enemyAct, presentationPhase }

    public string[] challengeNames = new string[] { "MC1", "MC2", "MC3", "NC1", "NCX" };
    public string[] challengeFeatures = new string[] { "Coding", "Design", "Research", "C + D", "D + R" };
    public int enemyScore, playerScore;

    private string chosenChallenge, chosenFeature;

    public TurnOrder currentPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMaxMana = startingMana;

        chosenChallenge = challengeNames[Random.Range(0, challengeNames.Length)];
        chosenFeature = challengeFeatures[Random.Range(0, challengeFeatures.Length)];

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

                chosenChallenge = challengeNames[Random.Range(0, challengeNames.Length)];
                chosenFeature = challengeFeatures[Random.Range(0, challengeFeatures.Length)];

                UIController.instance.SetChallengeNameText(chosenChallenge);
                UIController.instance.SetChallengeFeatureText(chosenFeature);
                UIController.instance.SetEnemyScoreText(0);
                UIController.instance.SetPlayerScoreText(0);

                break;

            case TurnOrder.playerEngage:

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
                    Debug.Log("HAI PERSO");

                } else if (UIController.instance.enemyTotalScore < UIController.instance.playerTotalScore)
                {
                    Debug.Log("HAI VINTO");
                }
                else
                {
                    Debug.Log("PAREGGIO");
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
