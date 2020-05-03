using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class AI : MonoBehaviour
{
    public Decks cpuDeck = Decks.dark;
    string deckName = "";
    int mp = 1;
    int maxMp = 10;
    int manaRegeneration = 1;
    public List<Card> deck = new List<Card>();
    Card currentcard, lastcard;
    bool wait=false;
    public List<Transform> spawnpoints = new List<Transform>();
    [SerializeField]
    Player enemy = null;
    Player player = null;
    float recoverTime = 1;
    float recoverTimer = 1;
    public GameMode difficult = GameMode.Normal;
    float waitingTime = 1;
    float waintingCounter;
    Vector3 spawnposition;
    bool gameEnd = false;
    public bool GAMEEND {get{ return gameEnd; } set { gameEnd = value; } }
    
    void Start()
    {
        //デクを選ぶ
        switch (cpuDeck)
        {
            case Decks.holy:
                deckName = StaticStrings.holy;
                break;
            case Decks.dark:
                deckName = StaticStrings.dark;
                break;
        }
        //デクロード
        Card[] allcard = Resources.LoadAll<Card>("Decks/"+deckName);
        foreach(var c in allcard)
        {
            deck.Add(c);
        }
        int rnd = Random.Range(0, deck.Count - 1);
        currentcard = deck[rnd];
        lastcard = currentcard;
        //プレーヤーを設定する
        player = GetComponentInParent<Player>();
        Player[] allplayers = FindObjectsOfType<Player>();
        foreach(var p in allplayers)
        {
            if (p != player)
            {
                enemy = p;
            }
        }
        //難しさ
        if (PlayerPrefs.HasKey(StaticStrings.gameMode))
        {
            waitingTime = PlayerPrefs.GetInt(StaticStrings.gameMode);
        }
        else
        {
            waitingTime = (int)difficult;
        }
      
        waintingCounter = waitingTime;
        if (difficult == GameMode.Impossible)
        {
            manaRegeneration++;
        }
    }


    void Update()
    {
        if (gameEnd) return;
        if (mp < maxMp)
        {
            recoverTimer -= Time.deltaTime;
            if (recoverTimer <= 0)
            {
                recoverTimer = recoverTime;
                mp+=manaRegeneration;
            }
        }
        if (wait) {
            waintingCounter -= Time.deltaTime;
            if (waintingCounter <= 0)
            {
                waintingCounter = waitingTime;
                wait = false;
            }
            return;
        }
        if (currentcard.cost <= mp)
        {
            wait = true;
            int rnd = Random.Range(0, spawnpoints.Count-1);
            mp -= currentcard.cost;
            spawnMonster(spawnpoints[rnd]);
        }
    }
    void spawnMonster(Transform point)
    {
        if(currentcard is BuildCard)
        {
            GameObject newCard = Instantiate(currentcard.prefab[0], spawnposition, Quaternion.identity) as GameObject;
            player.addToEnemyList(newCard.transform);
            Build c = newCard.GetComponent<Build>();
            if (c != null)
            {
                c.Init(player.thisTeam);
            }
        }
        else if(currentcard is SpellCard)
        {
            List<Transform> targets = new List<Transform>();
            foreach(var e in enemy.enemies)
            {
                if (e != null)
                {
                    targets.Add(e);
                }
            }
            targets.OrderBy(c => Vector3.Distance(player.getKingTower().transform.position,
                c.transform.position));
            Transform target = targets[0];
            if (target == null)
            {
                target = enemy.getKingTower().transform;
            }
            GameObject newCard = Instantiate(currentcard.prefab[0], player.getKingTower().transform.position, Quaternion.identity) as GameObject;
            AreaBullet b = newCard.GetComponent<AreaBullet>();
            b.passValues(player.thisTeam, currentcard.spellCard.radius, currentcard.spellCard.areaDamage, currentcard.spellCard.crownTowerDamage, target);
        }
        else
        {
            float x = 0; 
            for (int i = 0; i < currentcard.prefab.Length; i++)
            {
                if (i < 1)
                {
                    spawnposition = point.transform.position;
                }
                GameObject newCard = Instantiate(currentcard.prefab[i], point.position, Quaternion.identity) as GameObject;
                player.addToEnemyList(newCard.transform);
                Character c = newCard.GetComponent<Character>();
                if (c != null)
                {
                    c.Initialization(enemy);
                }
                if (i % 2 == 0)
                {
                    x = 1;
                    
                }
                else if (i % 3 == 0)
                {
                    x = -1; 
                }
                else if (i % 5 == 0)
                {
                    x = -1;
                   
                }
                spawnposition = new Vector3(newCard.transform.position.x + x, newCard.transform.position.y, newCard.transform.position.z);
            }
        }
        

        lastcard = currentcard;
        draw();
        
    }
    private void draw()
    {
        int rnd = Random.Range(0, deck.Count - 1);
        currentcard = deck[rnd];
        bool isDifferent = false;
        while (isDifferent)
        {
            if (lastcard == currentcard)
            {
                int rand = Random.Range(0, deck.Count - 1);
                currentcard = deck[rand];
                isDifferent = false;
            }
            else
            {
                isDifferent = true;
            }
        }
       
    }
    public void upgradeManaRegeneration()
    {
        manaRegeneration++;
    }
  public void addSpawnPoints(Transform t)
    {
        spawnpoints.Add(t);
    }
}
public enum GameMode
{
    ToEasy = 20,
    Easy = 10,
    Normal = 5,
    Hard = 4,
    Veryharad = 3,
    Crazy = 2,
    Impossible = 1
}