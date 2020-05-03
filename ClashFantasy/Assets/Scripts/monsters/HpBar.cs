using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    
    Text hptext = null;
    Slider hpbar = null;
    [SerializeField]
    Image fillbar=null;
    Health health;
    bool isInitialized = false;
    public void Initialize(Health h)
    {
        //子供の中に探す
        hptext = GetComponentInChildren<Text>();
        hpbar = GetComponentInChildren<Slider>();

        health = h; 
        //healthmanagerから値を貰う
        hpbar.maxValue = health.health;
        hpbar.value = health.health;
        hptext.text = health.health.ToString();
        isInitialized = true;

    }
    private void Update()
    {
        if (!isInitialized) return;
        //ウイupdate
        hpbar.value = health.health;
        hptext.text = health.health.ToString();
    }
    //色を変更する
    public void changeColor(Color c)
    {
        fillbar.color = c;
    }
}

