using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "cardInfo/Spell", fileName = "Spell")]
public class SpellCard : Card
{
    [SerializeField]
    public float areaDamage = 500, crownTowerDamage = 200;
    [SerializeField]
    public float radius = 2.5f;
   
}
