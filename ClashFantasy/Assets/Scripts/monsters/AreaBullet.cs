using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaBullet : Bullet
{
   
    float range=2.5f;
    float towerCroneDamage = 110;
    float standardDamage = 200;
    team tm;
    AudioSource s;
    public void passValues(team t,float _range,float damage,float crownDamage,Transform tg)
    {

        tm = t;
        range = _range;
        standardDamage = damage;
        towerCroneDamage = crownDamage;
        target = tg;
        s = GetComponent<AudioSource>();
    }
    public override void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            AreaAttack();
            Destroy(gameObject);
        }
    }


    public void AreaAttack()
    {
        EffectManager.instance.playInPlace(transform.position, particleToPlay);
        Collider[] allEnemyes = Physics.OverlapSphere(transform.position, range);
        foreach (var c in allEnemyes)
        {
            Character character = c.GetComponent<Character>();
            if (character == null)
            {
                damage = towerCroneDamage;
            }
            else
            {
                damage = standardDamage;
            }
            Iteam team = c.GetComponent<Iteam>();
            if (team != null && team.getTeam() != tm)
            {

                Health h = c.GetComponent<Health>();
                if (h != null)
                {
                    h.takeDamage(damage);
                }
            }
        }
        if (s != null)
        {
            s.Play();
        }
        Destroy(gameObject);
    }
}
