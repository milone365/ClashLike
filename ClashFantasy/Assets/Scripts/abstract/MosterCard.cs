using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName ="cardInfo/Monster", fileName = "monster")]
public class MosterCard : Card
{

    public float hp = 100;
    public float damage = 100;
    public float hitSpeed=1.5f;
    public Speed speed;
    public float range = 5.5f;
    public troupType type;
    public bool isAreaDamage = false;
    public float A_bulletRange = 2.5f;
    public float DamageOnDeath = 0;
}
//スピード
public enum Speed
{
    slow=30,
    medium=60,
    fast=90,
    veryFast=120
}
//団タイプ
public enum troupType
{
    build,
    ground,
    fly
}
//目標タイプ
public enum targetType
{
    onlyBuild,
    onlyGround,
    All
}