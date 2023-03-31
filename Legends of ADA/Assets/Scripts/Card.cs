using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public CardScriptableObject cardSO;

    public string cardFaction, cardType;

    public bool isPlayer;

    public int codingValue, designValue, researchValue, manaCost;
    public float processValue;

    public TMP_Text codingText, designText, researchText, nameText, actionDescriptionText, loreText, manaCostText, processText;

    public Image characterArt, factionArt;

    private Vector3 targetPoint;

    private Quaternion targetRot;

    public float moveSpeed = 5f, rotateSpeed = 540f;

    public bool inHand;

    public int handPosition;

    private HandController HC;

    public bool isSelected;

    private Collider col;

    public LayerMask whatIsDesktop, whatIsPlacement;

    private bool justPressed;

    public CardPlacePoint assignedPlace;

    public Material[] materials;


    // Start is called before the first frame update
    void Start()
    {

        if (targetPoint == Vector3.zero)
        {
            targetPoint = transform.position;
            targetRot = transform.rotation;
        }

        SetupCard();
        HC = FindObjectOfType<HandController>();
        col = GetComponent<Collider>();

        Renderer r = GetComponentInChildren<Renderer>();
        Material[] mat = r.materials;

        if (this.cardType == "Mentor")
        {
            mat[1] = materials[0];
        }
        else if (this.cardType == "Bonus")
        {
            mat[1] = materials[1];
        }
        else
        {
            mat[1] = materials[2];
        }

        r.materials = mat;

    }

    public void SetupCard()
    {
        codingValue = cardSO.codingValue;
        designValue = cardSO.designValue;
        researchValue = cardSO.researchValue;
        processValue = cardSO.processValue;
        manaCost = cardSO.manaCost;
        cardFaction = cardSO.cardFaction;
        cardType = cardSO.cardType;

        if (this.cardType == "Mentor")
        {
            codingText.text = "+" + codingValue.ToString();
            designText.text = "+" + designValue.ToString();
            researchText.text = "+" + researchValue.ToString();
            processText.text = "x" + processValue.ToString();
            loreText.text = cardSO.cardLore;
        }
        else
        {
            codingText.text = codingValue.ToString();
            designText.text = designValue.ToString();
            researchText.text = researchValue.ToString();
            processText.text = "x" + processValue.ToString();
            loreText.text = "Mentor: " + cardSO.cardLore;
        }

        manaCostText.text = manaCost.ToString();
        nameText.text = cardSO.cardName;
        actionDescriptionText.text = cardSO.actionDescription;

        characterArt.sprite = cardSO.characterSprite;
        factionArt.sprite = cardSO.factionSprite;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPoint, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);

        if (isSelected)
        { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, whatIsDesktop))
            {
                MoveToPoint(hit.point + new Vector3(0f, 2f, 0f), Quaternion.identity);
            }

            if (Input.GetMouseButtonDown(1))
            {
                ReturnToHand();
            }

            if (Input.GetMouseButtonDown(0) && justPressed == false)
            {
                if (Physics.Raycast(ray, out hit, 100f, whatIsPlacement) && (
                        BattleController.instance.currentPhase == BattleController.TurnOrder.playerEngage ||
                        BattleController.instance.currentPhase == BattleController.TurnOrder.playerInvestigate ||
                        BattleController.instance.currentPhase == BattleController.TurnOrder.playerAct))
                {
                    CardPlacePoint selectedPoint = hit.collider.GetComponent<CardPlacePoint>();

                    if (selectedPoint.activeCard == null && selectedPoint.isPlayerPoint)
                    {
                        if ((selectedPoint.isMentorPoint && cardType == "Mentor") ||
                            (selectedPoint.isBonusPoint && cardType == "Bonus") ||
                            (!selectedPoint.isMentorPoint && !selectedPoint.isBonusPoint && cardType == "Student"))
                        {
                            if (BattleController.instance.playerMana >= manaCost)
                            {
                                selectedPoint.activeCard = this;
                                assignedPlace = selectedPoint;

                                MoveToPoint(selectedPoint.transform.position, Quaternion.identity);

                                inHand = false;
                                isSelected = false;

                                HC.RemoveFromHand(this);

                                BattleController.instance.SpendPlayerMana(manaCost);
                                Debug.Log(cardType);

                                BattleController.instance.placedCards.Add(this);

                                if (this.cardType == "Student")
                                {
                                    foreach (var card in BattleController.instance.placedCards)
                                    {
                                        if (card.cardType == "Mentor")
                                        {
                                            UpdateSingleCardStats(card);
                                        }
                                    }
                                }

                                if (cardType == "Mentor")
                                {
                                    Debug.Log("ENTERED");
                                    UpdatePlacedCardsStats(this, ref BattleController.instance.placedCards);
                                }

                                BattleController.instance.CalculatePoints(this);

                            } else
                            {
                                ReturnToHand();
                                UIController.instance.ShowManaWarning();
                            }
                        } else
                        {
                            ReturnToHand();
                        }
                    } else if (selectedPoint.activeCard != null && selectedPoint.isBonusPoint && cardType == "Bonus")
                    {
                        if (BattleController.instance.playerMana >= manaCost)
                        {
                            selectedPoint.activeCard = this;
                            assignedPlace = selectedPoint;
                            Vector3 bonusPosition = selectedPoint.transform.position;
                            bonusPosition.y = (float)(bonusPosition.y + 0.01);

                            MoveToPoint(bonusPosition, Quaternion.identity);

                            inHand = false;
                            isSelected = false;

                            HC.RemoveFromHand(this);

                            BattleController.instance.SpendPlayerMana(manaCost);

                            BattleController.instance.CalculatePoints(this);
                        }
                        else
                        {
                            ReturnToHand();
                            UIController.instance.ShowManaWarning();
                        }
                    } else
                    {
                        ReturnToHand();
                    }
                } else
                {
                    ReturnToHand();
                }
            }
        }
        justPressed = false;
    }

    public void UpdatePlacedCardsStats(Card placedMentorCard, ref List<Card> placedCards)
    {
        int bonusCodingValue = placedMentorCard.codingValue;
        int bonusDesignValue = placedMentorCard.designValue;
        int bonusResearchValue = placedMentorCard.researchValue;

        foreach (var card in placedCards)
        {
            if (card.cardType == "Student")
            {
                if (cardSO.cardLore == placedMentorCard.nameText.text)
                {
                    bonusCodingValue *= 2;
                    bonusDesignValue *= 2;
                    bonusResearchValue *= 2;
                }
                Debug.Log(bonusCodingValue + " " + bonusDesignValue + " " + bonusResearchValue);
                Debug.Log(card.codingValue + " " + card.designValue + " " + card.researchValue);
                card.codingValue += bonusCodingValue;
                card.designValue += bonusDesignValue;
                card.researchValue += bonusResearchValue;

                Debug.Log(card.codingValue + " " + card.designValue + " " + card.researchValue);

                card.codingText.text = card.codingValue.ToString();
                card.designText.text = card.designValue.ToString();
                card.researchText.text = card.researchValue.ToString();
            }
        }
    }

    public void UpdateSingleCardStats(Card placedMentorCard)
    {
        int bonusCodingValue = placedMentorCard.codingValue;
        int bonusDesignValue = placedMentorCard.designValue;
        int bonusResearchValue = placedMentorCard.researchValue;

        if (cardSO.cardLore == placedMentorCard.nameText.text)
        {
            bonusCodingValue *= 2;
            bonusDesignValue *= 2;
            bonusResearchValue *= 2;
        }

        Debug.Log(bonusCodingValue + " " + bonusDesignValue + " " + bonusResearchValue);
        Debug.Log(this.codingValue + " " + this.designValue + " " + this.researchValue);
        this.codingValue += bonusCodingValue;
        this.designValue += bonusDesignValue;
        this.researchValue += bonusResearchValue;

        Debug.Log(this.codingValue + " " + this.designValue + " " + this.researchValue);

        this.codingText.text = this.codingValue.ToString();
        this.designText.text = this.designValue.ToString();
        this.researchText.text = this.researchValue.ToString();
    }

    public void MoveToPoint(Vector3 pointToMoveTo, Quaternion rotToMatch)
    {
        targetPoint = pointToMoveTo;
        targetRot = rotToMatch;
    }

    private void OnMouseOver()
    {
        if (inHand && isPlayer)
        {
            //the second parameter is rotation and is required, but since it's useless for us, we use the identity (0,0,0)
            MoveToPoint(HC.cardPositions[handPosition] + new Vector3(0f,1f,.5f), Quaternion.identity);
        }
    }

    private void OnMouseDown()
    {
        if (inHand && (
                BattleController.instance.currentPhase == BattleController.TurnOrder.playerEngage ||
                BattleController.instance.currentPhase == BattleController.TurnOrder.playerInvestigate ||
                BattleController.instance.currentPhase == BattleController.TurnOrder.playerAct)
                && isPlayer)
        {
            isSelected = true;
            col.enabled = false;

            justPressed = true;
        }
    }

    private void OnMouseExit()
    {
        if (inHand)
        {
            //the second parameter is rotation and is required, but since it's useless for us, we use the identity (0,0,0)
            MoveToPoint(HC.cardPositions[handPosition], HC.minPos.rotation);
        }
    }

    public void ReturnToHand()
    {
        isSelected = false;
        col.enabled = true;

        MoveToPoint(HC.cardPositions[handPosition], HC.minPos.rotation);
    }

}
