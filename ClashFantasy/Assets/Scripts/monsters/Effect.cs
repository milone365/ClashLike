using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public effectType effectTYPE = effectType.spawnOnDeath;
    [SerializeField]
    GameObject monster = null;
    [SerializeField]
    int NumberOfMonster = 4;
    [SerializeField]
    float spawningTime = 0;
    float spawningCounter = 0;
    float x, z;
    Character character;
    Build build;
    [SerializeField]
    GameObject obj=null;
    Animator anim;
    void Start()
    {
        spawningCounter = spawningTime;
        character = GetComponent<Character>();
        if (character == null)
        {
            build = GetComponent<Build>();
        }
        if (effectTYPE == effectType.spawnOnDeath)
        {
            Health health = GetComponent<Health>();
            health.onDeath += spawnBombOnDeath;
        }
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        if (effectTYPE == effectType.spawnOnTime)
        {
            spawningCounter -= Time.deltaTime;
            if (spawningCounter <= 0)
            {
                spawningCounter = spawningTime;
                if (anim == null)
                {
                    SpawnOnTime();
                    return;
                }
                else
                {
                    anim.SetBool("INVOKING", true);
                    GetComponent<Animator>().SetTrigger("INVOKE");
                }
                
            }
        }

    }

    void SpawnOnTime()
    {
        if (SoundManager.instance = null)
        {
            SoundManager.instance.playSE("spell");
        }
        for(int i = 0; i < NumberOfMonster; i++)
        {
            switch (i)
            {
                case 0: x = 1;z = 1; break;
                case 1: x = -1; z = 1;break;
                case 2: x = -1; z=-1;break;
                case 3: x = 1; z =-1; break;
            }
            Vector3 pos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
          GameObject newMonster=  Instantiate(monster, pos, Quaternion.identity)as GameObject;
            newMonster.GetComponent<Character>().Initialization(character.getEnemy());
            anim.SetBool("INVOKING", false);
        }
    }
    public void spawnBombOnDeath(Transform t)
    {
       
      GameObject newObj= Instantiate(obj, transform.position, transform.rotation)as GameObject;
        if (character != null)
        {
            newObj.GetComponent<TimerBomb>().INIT(character.monstercard.DamageOnDeath, character.getTeam());
        }
           
        else
        {
            newObj.GetComponent<TimerBomb>().INIT(build.card.DamageOnDeath,build.getTeam());
        }
    }
    public enum effectType
    {
        spawnOnDeath,
        spawnOnTime
    }
}
