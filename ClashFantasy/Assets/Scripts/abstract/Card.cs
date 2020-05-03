using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    //parameterｓ
    public string CardName = "";
    public int cost = 3;
    public Sprite image = null;
    public targetType targets;
    [SerializeField]
    public GameObject[] prefab;
    public SpellCard spellCard;
    public AudioClip attackSound;
}

