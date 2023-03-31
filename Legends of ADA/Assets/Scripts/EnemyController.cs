    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    private void Awake()
    {
        instance = this;
    }

    public List<CardScriptableObject> deckToUse = new List<CardScriptableObject>();
    private List<CardScriptableObject> activeCards = new List<CardScriptableObject>();
    public CardPlacePoint[] enemyCardPoints;

    public enum AIType {placeFromDesk, handRandomPlace}
    public AIType enemyAIType;

    private List<CardScriptableObject> cardsInHand = new List<CardScriptableObject>();
    public int startHandSize;


    public Card cardToSpawn;
    public List<Card> placedCards = new List<Card>();
    public Transform cardSpawnPoint;
    private int iterations = 0; 
    // Start is called before the first frame update
    void Start()
    {
        SetUpDeck();
        if (enemyAIType != AIType.placeFromDesk)
        {


            SetupHand();
        }
    } 

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUpDeck()
    {
        activeCards.Clear();

        List<CardScriptableObject> tempDeck = new List<CardScriptableObject>();
        tempDeck.AddRange(deckToUse);

        int iterations = 0;
        while (tempDeck.Count > 0 && iterations < 500)
        {
            int selected = Random.Range(0, tempDeck.Count);
            activeCards.Add(tempDeck[selected]);
            tempDeck.RemoveAt(selected);

            iterations++;
        }
    }

    public void StartAction()
    { 
        if (activeCards.Count == 0)
        {
            SetUpDeck();
        }
        if (enemyAIType != AIType.placeFromDesk)
        {
            for (int i = 0; i < BattleController.instance.cardToDrawPerTurn; i++)
            {
                cardsInHand.Add(activeCards[0]);
                activeCards.RemoveAt(0);

                if(activeCards.Count == 0)
                {
                    SetUpDeck();
                }
            }
        }


        List<CardPlacePoint> cardPoints = new List<CardPlacePoint>();
        cardPoints.AddRange(enemyCardPoints);

        int randomPoint = Random.Range(0, cardPoints.Count);
        CardPlacePoint selectedPoint = cardPoints[randomPoint];

        selectedPoint = chooseSelectPoint(selectedPoint, cardPoints);

        CardScriptableObject selectedCard = null;

        
        switch (enemyAIType)
        {
            case AIType.placeFromDesk:
                if (selectedPoint.activeCard == null)
                {
                    Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);
                            newCard.cardSO = activeCards[0];
                            activeCards.RemoveAt(0);
                            newCard.SetupCard();
                            newCard.MoveToPoint(selectedPoint.transform.position, selectedPoint.transform.rotation);
                            selectedPoint.activeCard = newCard;
                            newCard.assignedPlace = selectedPoint;
                }
                break;
            case AIType.handRandomPlace:
                selectedCard = SelectedCardToPlay();
                while (selectedCard != null && selectedPoint.activeCard == null)
                {
                    if (iterations < 6)
                    {
                        if ((selectedPoint.isMentorPoint && selectedCard.cardType == "Mentor") ||
                                (selectedPoint.isBonusPoint && selectedCard.cardType == "Bonus") ||
                                (!selectedPoint.isMentorPoint && !selectedPoint.isBonusPoint && selectedCard.cardType == "Student"))
                        {
                            PlayCard(selectedCard, selectedPoint);
                            BattleController.instance.CalculateEnemyPoints(selectedPoint.activeCard);

                        }
                        iterations++;
                        selectedCard = SelectedCardToPlay();
                    }
                    else
                    {
                        selectedCard = null;
                    }
                }
                iterations = 0;
                break;

        }
        BattleController.instance.AdvanceTurn();
    }

    void SetupHand()
    {
        for(int i = 0; i<startHandSize; i++)
        {
            if (activeCards.Count == 0)
            {
                SetUpDeck();
            }

            cardsInHand.Add(activeCards[0]);
            activeCards.RemoveAt(0);
        }
    }
    public CardPlacePoint chooseSelectPoint(CardPlacePoint selectedPoint, List<CardPlacePoint> cardPoints)
    {
        while (selectedPoint.activeCard != null && cardPoints.Count > 0)
        {
            int randomPoint = Random.Range(0, cardPoints.Count);
            selectedPoint = cardPoints[randomPoint];
            cardPoints.RemoveAt(randomPoint);
        }
        return selectedPoint;
    }
    public void PlayCard(CardScriptableObject cardSO, CardPlacePoint placePoint)
    {
        Card newCard = Instantiate(cardToSpawn, cardSpawnPoint.position, cardSpawnPoint.rotation);

        newCard.cardSO = cardSO;
        newCard.SetupCard();
        newCard.MoveToPoint(placePoint.transform.position, placePoint.transform.rotation);
        

        placePoint.activeCard = newCard;
        newCard.assignedPlace = placePoint;
        placedCards.Add(newCard);
        cardsInHand.Remove(cardSO);
        BattleController.instance.SpendEnemyMana(cardSO.manaCost);
    }

    CardScriptableObject SelectedCardToPlay()
    {
        CardScriptableObject cardToPlay = null;
        List<CardScriptableObject> cardsToPlay = new List<CardScriptableObject>();

        foreach(CardScriptableObject card in cardsInHand)
        {
            if(card.manaCost <= BattleController.instance.enemyMana)
            {
                cardsToPlay.Add(card); 
            }
        }

        if(cardsToPlay.Count > 0)
        {
            int selected = Random.Range(0, cardsToPlay.Count);
            
            cardToPlay = cardsToPlay[selected];
        }


        return cardToPlay;
    }
}
