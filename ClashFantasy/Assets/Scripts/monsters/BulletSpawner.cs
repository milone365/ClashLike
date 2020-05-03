using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    Bullet bullet=null;
    [SerializeField]
    Transform spawnPoint = null;
    Character character = null;
    public delegate void onShoot();
    onShoot currentShoot;

    team tm;
    float range = 0;
    float damage = 10;
    private void Start()
    {
        character = GetComponent<Character>();
        if (character == null)
        {
            character = GetComponentInParent<Character>();
        }
        if (character.monstercard.isAreaDamage)
        {
            tm = character.getTeam();
            range = character.monstercard.A_bulletRange;
            damage = character.monstercard.damage;
            currentShoot = castAreaBullet;
        }
        else
        {
            currentShoot = normalBullet;
        }
        
    }
    public void castBullet()
    {
        currentShoot();
    }

    void normalBullet()
    {
        if (spawnPoint == null) return;
        Bullet newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation) as Bullet;
        newBullet.getTarget(character.getTarget());
    }
    void castAreaBullet()
    {
        if (spawnPoint == null) return;
        AreaBullet newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation) as AreaBullet;
        newBullet.passValues(tm,range,damage,damage,character.getTarget());
    }
}
