using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerCannon : MonoBehaviour,Iteam
{
   
    protected float attackCounter = 0;
    [SerializeField]
    protected float attackDelay = 0.8f;
    protected Transform target = null;
    public team tm;
    [SerializeField]
    Transform spawnPoint = null;
    [SerializeField]
    protected float viewField = 7f;
    [SerializeField]
    Bullet bullet = null;
    bool isInitialized = false;
    
    Player enemy;
    //バレットスポナー
    public void castBullet()
    {
        if (spawnPoint == null||target==null) return;
        Bullet newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation) as Bullet;
        newBullet.getTarget(target);
    }
    public void Init(team team)
    {
      
        attackCounter = attackDelay;
        tm = team;
        isInitialized = true;
        Player[] allplayer = FindObjectsOfType<Player>();
        foreach (var p in allplayer)
        {
            if (p.thisTeam != tm)
            {
                enemy = p;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        defenceBeaviour();
    }

   
    public virtual void defenceBeaviour()
    {
        
        //目標ヌルの場合は、sphereraycastで敵を探す
        if (target == null)
        {
            Collider[] visibileObjects = Physics.OverlapSphere(transform.position, viewField);
            foreach (var c in visibileObjects)
            {
                Character ch = c.transform.GetComponent<Character>();
                //敵チームだったターゲットになる
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
            //時間が経ったら攻撃する
            attackCounter -= Time.deltaTime;

            if (attackCounter <= 0)
            {
                attackCounter = attackDelay;
                //ビル上にあるキャラクターアニメーション
                castBullet();

            }
        }

    }

    public team getTeam()
    {
        return tm;
    }
}