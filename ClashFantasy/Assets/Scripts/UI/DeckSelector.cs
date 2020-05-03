using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSelector : MonoBehaviour
{
    public Decks _deckType;

    public void SelectDeck()
    {
        string deckName = "";
        string enemyDeck = "";
        switch (_deckType)
        {
            case Decks.holy:
                deckName = StaticStrings.holy;
                enemyDeck = StaticStrings.dark;
                break;
            case Decks.dark:
                deckName = StaticStrings.dark;
                enemyDeck = StaticStrings.holy;
                break;
        }
        PlayerPrefs.SetString(StaticStrings.savedDeck,deckName);
        PlayerPrefs.SetString(StaticStrings.cpuSavedDeck, enemyDeck);
    } 
}
