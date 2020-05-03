using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using System;

public class Character : MonoBehaviour,Iteam
{
    public string meleeAreaEffect = "Orange";
    Player enemy = null;
    [SerializeField]
    bool canMove = false;
    Transform target = null;
    Animator anim;
    float attackDelay = 0;
    float attackCounter = 0;
    bool inBattle = false;
    team tm;
    AudioSource Atk_source;
    public Player getEnemy()
    {
        return enemy;
    }
    public troupType getTroupType()
    {
        return monstercard.type;
    }
    public team getTeam()
    {
        return tm;
    }
    Health h;
    public Transform getTarget()
    {
        return target;
    }
    public MosterCard monstercard=null;
    NavMeshAgent agent = null;
    charState state;

    public void Initialization(Player _enemy)
    {
        h = GetComponent<Health>();
        h.initializeHP(monstercard.hp);
        enemy = _enemy;
        FindNewTarget();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        attackDelay = monstercard.hitSpeed;
        attackCounter = attackDelay;
        changeColorToHPbar();
        float speed = (float)monstercard.speed / 60;
        agent.speed = speed;
        canMove = true;
        gameObject.AddComponent<AudioSource>();
        Atk_source = GetComponent<AudioSource>();
        Atk_source.playOnAwake = false;
        Atk_source.clip = monstercard.attackSound;
    }

  
    void Update()
    {
        
        if (!canMove) return;
        anim.SetFloat("MOVE", agent.velocity.magnitude);
        OverlapFinding();
        if (target == null)
        {
            inBattle = false;
            FindNewTarget();
            return;
        }
        updating();
    }
    //raycastでsphereを作る
    private void OverlapFinding()
    {
        if (inBattle) return;
        Collider[] allcolider = Physics.OverlapSphere(transform.position, 5);
        List<Transform> targetList = new List<Transform>();
        foreach(var item in allcolider)
        {
            if (item.GetComponent<Character>())
            {
                //建物だけの場合は続く
               if (monstercard.targets == targetType.onlyBuild) continue;
               //飛ぶモンスターに攻撃出来ない場合は続く
               troupType type= item.GetComponent<Character>().getTroupType();
                if (type == troupType.fly && monstercard.targets == targetType.onlyGround)
                {
                    continue;
                }
               
            }
            //敵チームのモンスターだったらリストに加える
            Iteam team = item.GetComponent<Iteam>();
            if (team!=null&&team.getTeam() != this.tm)
                targetList.Add(item.transform);
        }

        //リストがあったら一番近いエネルギーを選ぶ
        if (targetList.Count < 1) return;
        targetList = targetList.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToList();
        target = targetList[0];
        inBattle = true;
        changeState(charState.chase);
    }

    private void FindNewTarget()
    {
        List<Transform> allEnemies = new List<Transform>();
        foreach(var item in enemy.enemies)
        {
            if (item != null)
            {
                if (buildTargetCheck(item)==null)
                {
                    continue;
                }
                allEnemies.Add(item);
            }
        }
        //リストがあったら一番近いエネルギーを選ぶ
        if (allEnemies.Count<1) return;
      allEnemies = allEnemies.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToList();
        target = allEnemies[0];
        changeState(charState.chase);
        
    }

    void updating()
    {
        switch (state)
        {
            case charState.chase:
                chasing();
                break;
            case charState.attack:
                inBattle = true;
                Attacking();
                break;
        }
    }

    //モンスターのターゲットを確認する、例えば
    Transform buildTargetCheck(Transform t)
    {
        Transform target;
        Character c = t.GetComponent<Character>();
        //キャラクターじゃなければそのまま続く
        if (c == null)
        {
            return t;
        }
        else
        {
            //キャラクターのターゲットはビルしか出来ない場合
            if (monstercard.targets == targetType.onlyBuild)
            {
                target = null;
            }
            //キャラクターのターゲットは飛ぶモンスターじゃばければ
            else if (monstercard.targets == targetType.onlyGround
                ||c.monstercard.type==troupType.fly)
            {
                target = null;
            }
            else
            {
                target = t;
            }
        }
       
                return target;
    }
     void chasing()
    {
        //プレーヤーへ動く
        if (target == null) return;
        agent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance <= monstercard.range)
        {
            changeState(charState.attack);
        }
    }

    public void Attacking()
    {
        //止まる
        if(agent.destination!=transform.position)
        agent.SetDestination(transform.position);
        if (target == null) return;
        //距離チェック
        float distance = Vector3.Distance(transform.position, target.position);
        //ローテーション
        transform.LookAt(target, Vector3.up);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        if (distance > monstercard.range)
        {
            changeState(charState.chase);
        }
        //時間に沿って攻撃
        attackCounter -= Time.deltaTime;
        if (attackCounter <= 0)
        {
            attackCounter = attackDelay;
            anim.SetTrigger("ATK");
            Atk_source.Play();
        }
    }
    //ステータスを変更するため
    void changeState(charState st)
    {
        state = st;
    }
    //ダメージを付ける関数
    public void meleeAttack()
    {
        if (target == null) return;
        Health h = target.GetComponent<Health>();
        if (h != null)
        {
            h.takeDamage(monstercard.damage);
        }
    }
    public void AutoDestroy()
    {
       
        if (target == null) return;
        Health h = target.GetComponent<Health>();
        if (h != null)
        {
            h.takeDamage(monstercard.damage);
        }
        EffectManager.instance.playInPlace(transform.position, "Explosion");
        Destroy(gameObject);
    }
    //areaAttack
    public void meleeAreaAttack()
    {
        EffectManager.instance.playInPlace(transform.position, meleeAreaEffect);
        Collider[] allEnemyes = Physics.OverlapSphere(transform.position, monstercard.range);
        foreach(var c in allEnemyes)
        {
            Character character = c.GetComponent<Character>();
            if (character != null)
            {
                if (character.getTroupType() == troupType.fly && monstercard.targets != targetType.All)
                {
                    continue;
                }
            }
            Iteam team = c.GetComponent<Iteam>();
            if (team != null && team.getTeam() != tm)
            {
                
                Health h = c.GetComponent<Health>();
                if (h != null)
                {
                    h.takeDamage(monstercard.damage);
                }
            }
        }
        
    }
    //ｈｐバー色の変更
    void changeColorToHPbar()
    {
        
        if (h.getBar() == null) return;

            if (enemy.thisTeam == team.Ateam)
        {
            tm = team.Bteam;
            h.getBar().changeColor(new Color(255, 14, 0, 255));
        }
        else
        {
            tm = team.Ateam;
            h.getBar().changeColor(new Color(0,14,212,255));
        }

    }
  
}
public enum charState
{
    chase,
    attack
}

