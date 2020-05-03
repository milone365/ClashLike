using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Build : MonoBehaviour, Iteam
{
    public buildType type = buildType.defence;
    protected float attackCounter = 0;
    [SerializeField]
    protected float attackDelay = 0.8f;
    Animator anim;
    protected Transform target = null;
    public team tm;
    [SerializeField]
    GameObject monster=null;
    [SerializeField]
    Transform spawnPoint = null;
    [SerializeField]
    protected float viewField = 7f;
    float spawnCounter = 0;
    float damageForSecond = 0;
    float second = 1;
    Health h;
    bool isInitialized = false;
    public BuildCard card;
    Player enemy;
    //バレットスポナー
    public void castBullet()
    {
        if (spawnPoint == null) return;
        Bullet newBullet = Instantiate(card.bullet, spawnPoint.position, spawnPoint.rotation) as Bullet;
        newBullet.getTarget(target);
    }
    public void Init(team team)
    {
        h = GetComponent<Health>();
        h.initializeHP(card.hp);
        attackCounter = attackDelay;
        tm = team;
        anim = GetComponentInChildren<Animator>();
        spawnCounter = card.spawnTime;
        damageForSecond = h.health / card.lifeTime;
        isInitialized = true;
        Player[] allplayer = FindObjectsOfType<Player>();
        foreach(var p in allplayer)
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
        if (!isInitialized) return;
        switch (type)
        {
            //攻撃するビル
            case buildType.defence:
                defenceBeaviour();
                break;
                //団をスポンするビル
            case buildType.fucine:
                spawnMonster();
               
                break;
        }
        second -= Time.deltaTime;
        if (second <= 0)
        {
            second = 1;
            h.takeDamage(damageForSecond);
        }
    }
    
    public void spawnMonster()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = card.spawnTime;
        GameObject newMonster= Instantiate(monster, spawnPoint.position, Quaternion.identity)as GameObject;
         newMonster.GetComponent<Character>().Initialization(enemy);
        }
    }
  public  virtual void defenceBeaviour()
    {
        anim.SetFloat("MOVE", 0);
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
                anim.SetTrigger("ATK");

            }
        }

    }

    public team getTeam()
    {
        return tm;
    }
}
//ビル種類
public enum buildType
{
    defence,
    fucine
}
