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

    public TurnOrder currentPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayerMaxMana = startingMana;

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
        Debug.Log((int)currentPhase);
        currentPhase++;
        if((int)currentPhase >= System.Enum.GetValues(typeof(TurnOrder)).Length)
        {
            currentPhase = 0;
        }

        switch (currentPhase)
        {
            case TurnOrder.challengePhase:

                AdvanceTurn();

                break;

            case TurnOrder.playerEngage:

                UIController.instance.endTurnButton.SetActive(true);

                if(currentPlayerMaxMana < maxMana)
                {
                    currentPlayerMaxMana++;
                }

                FillPlayerMana();
                Debug.Log("card per turn: " + cardToDrawPerTurn);
                Debug.Log("held cards" + HandController.instance.heldCards.Count);
                Debug.Log(cardToDrawPerTurn - HandController.instance.heldCards.Count);
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

                break;
        }
    }

    public void EndPlayerTurn()
    {
        UIController.instance.endTurnButton.SetActive(false);

        AdvanceTurn();
    }
}
