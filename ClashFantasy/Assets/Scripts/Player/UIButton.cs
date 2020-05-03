using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    Card card;
    public Card getCard()
    {
        return card;
    }
    [SerializeField]
    Image cardImage=null;
    [SerializeField]
    Text costText = null;
    DeckController controller;
    
    //reference
    public void init(DeckController con)
    {
        controller = con;
    }
    //カードを選択する
    public void Click()
    {
        controller.selectCard(card,this);
    }
    //カードの状況をもらう
    public void giveInformation(Card c)
    {
        card = c;
        costText.text = c.cost.ToString();
        cardImage.sprite = c.image;  
    }
}
