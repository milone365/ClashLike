using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
   protected Transform target=null;
    [SerializeField]
    float bulletSpeed = 1;
    [SerializeField]
   protected float damage = 0;
    [SerializeField]
   protected string particleToPlay = "Explosion";
    AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        Destroy(gameObject, 3);
    }
    public void getTarget(Transform t)
    {
        target = t;
       
    }
  
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return; }
        Vector3 pos = target.GetComponent<Collider>().transform.position;
        transform.position = Vector3.MoveTowards(transform.position, pos,bulletSpeed*Time.deltaTime);
        
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            Health h = other.GetComponent<Health>();
            if (h == null) return;
            h.takeDamage(damage);
            EffectManager.instance.playInPlace(transform.position, particleToPlay);
            if (audio != null)
            {
                audio.Play();
            }
            Destroy(gameObject);
        }
    }
}
