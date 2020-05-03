using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    [SerializeField]
    string particleToPlay = "Blood";
    public float health = 10;
    [SerializeField]
    HpBar bar = null;
    bool isDead = false;
    //templateイベント
    public event Action<Transform> onDeath;
    //
    public HpBar getBar()
    {
        return bar;
    }
   
    public void initializeHP(float h)
    {
        health = h;
        bar.Initialize(this);
    }

    public virtual void takeDamage(float v)
    {
        if (isDead) return;
        health -= v;
        if (health <= 0)
        {
            isDead = true;
            //イベントに機能
            if (onDeath != null)
            {
                onDeath(this.transform);
            }
            Invoke("destroing", 1);
        }
    }
   void destroing()
    {
        //particle　play
        if (EffectManager.instance != null)
            EffectManager.instance.spawnInPlace(transform.position);
        Destroy(gameObject);
    }
}
