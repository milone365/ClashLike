using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : MonoBehaviour
{
   protected float attackCounter = 0;
    [SerializeField]
    protected float attackDelay = 0.8f;
    Animator anim;
    protected Transform target = null;
    public team tm;
    [SerializeField]
    protected Bullet bullet = null;
    [SerializeField]
    protected Transform spawnPoint = null;
    [SerializeField]
    protected float viewField = 7f;

    public void castBullet()
    {
        if (spawnPoint == null) return;
        Bullet newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation) as Bullet;
        newBullet.getTarget(target);
    }
    void Start()
    {
        attackCounter = attackDelay;
        Player p = GetComponentInParent<Player>();
        tm = p.thisTeam;
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        defenceBeaviour();
    }
    public virtual void defenceBeaviour()
    {
        anim.SetFloat("MOVE", 0);

        if (target == null)
        {
            Collider[] visibileObjects = Physics.OverlapSphere(transform.position, viewField);
            foreach (var c in visibileObjects)
            {
                Character ch = c.transform.GetComponent<Character>();
                if (ch != null && ch.getTeam() != tm)
                {
                    target = c.transform;
                    break;
                }
            }
            return;
        }
        else
        {
            attackCounter -= Time.deltaTime;

            if (attackCounter <= 0)
            {
                attackCounter = attackDelay;
                anim.SetTrigger("ATK");

            }
        }

    }
}
