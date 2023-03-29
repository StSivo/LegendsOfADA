using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 1)]
public class CardScriptableObject : ScriptableObject
{
    public string cardName;

    public string cardFaction;

    public string cardType;

    [TextArea]
    public string actionDescription, cardLore;

    public int codingValue, designValue, researchValue, manaCost;
    public float processValue;

    public Sprite characterSprite, factionSprite;
}
