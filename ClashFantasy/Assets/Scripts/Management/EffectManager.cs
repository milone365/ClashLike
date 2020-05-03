using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    Dictionary<string, Transform> allParticles = new Dictionary<string, Transform>();
    public static EffectManager instance;
    [SerializeField]
    GameObject blood=null;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //gameobjectの子供で辞書を作る
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach(var item in particles)
        {
            if(!allParticles.ContainsValue(item.transform))
            allParticles.Add(item.name, item.transform);
        }
    }
    //好きな場所へparticleを移動して、Playする
   public void playInPlace(Vector3 pos, string s)
    {
        if (allParticles.ContainsKey(s))
        {
            allParticles[s].transform.position = pos;
            allParticles[s].GetComponent<ParticleSystem>().Play();
        }
    }

    public void spawnInPlace(Vector3 pos)
    {
        Instantiate(blood, pos, Quaternion.identity);
    }
}
