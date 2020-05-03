using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckController : MonoBehaviour
{
    
    string deckName = "";
    public team Team = team.Ateam;
    List<Card> deck = new List<Card>();
    int mp = 1;
    int maxMp = 10;
    float recoverTime = 1;
    float recoverCounter = 1;
    [SerializeField]
    Slider s = null;
    Card selectedCard=null;
    [SerializeField]
    LayerMask myGround=0;
    [SerializeField]
    UIButton[] buttons = null;
    UIButton selectedButton = null;
    public List<Card> hand = new List<Card>();
    Player player = null;
    Player enemy = null;
    int cardIndex = 0;
    [SerializeField]
    Text manaText = null;
    Vector3 spawnposition;
    bool gameEnd = false;
    public bool GAMEEND { get { return gameEnd; } set { gameEnd = value; } }
    int manaRegeneration = 1;
    void Start()
    {
        UIButton[] b = GetComponentsInChildren<UIButton>();
        if (PlayerPrefs.HasKey(StaticStrings.savedDeck))
        {
            deckName = PlayerPrefs.GetString(StaticStrings.savedDeck);
        }
        else
        {
            deckName = StaticStrings.holy;
        }
        Card[] c = Resources.LoadAll<Card>("Decks/"+ deckName);
        foreach(var item in c)
        {
            deck.Add(item);
        }
        shuffle();
        s.maxValue = maxMp;
        Player[] allplayers = FindObjectsOfType<Player>();
        foreach(var p in allplayers)
        {
            if (p.thisTeam == Team)
            {
                player = p;
            }
            else
            {
                enemy = p;
            }
        }
    }
    void shuffle()
    {
        List<int> numbers = new List<int>();
        while (numbers.Count < 4)
        {
            int rnd = Random.Range(0, deck.Count - 1);
            if (!numbers.Contains(rnd))
            {
                numbers.Add(rnd);
            }
        }
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].giveInformation(deck[numbers[i]]);
            buttons[i].init(this);
            hand.Add(deck[numbers[i]]);
        }
    }
    public void selectCard(Card _c,UIButton b)
    {
      selectedButton = b;
      selectedCard =_c; 
    }
    
    void Update()
    {
        if (gameEnd) return;
        if (s != null)
        {
            
            s.value = mp;
        }
        if (selectedCard != null && selectedCard.cost <= mp)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = (Camera.main.ScreenPointToRay(Input.mousePosition));
                RaycastHit hitInfo;
                if(Physics.Raycast(ray,out hitInfo))
                {
                    Transform t = hitInfo.collider.GetComponent<Transform>();
                    Evoche(hitInfo.point,t);
                }
            }
        }
        if (mp < maxMp)
        {
            recoverCounter -= Time.deltaTime;
            if (recoverCounter <= 0)
            {
                recoverCounter = recoverTime;
                mp+=manaRegeneration;
            }
            
            
        }

        if (manaText != null)
        {
            if (mp < 10)
            {
                manaText.text = mp.ToString();
            }
            else
            {
                manaText.text = "1" + "\n" + "0";
            }
           
        }
    }
    //モンスターを作る
    void Evoche(Vector3 pos,Transform t)
    {
      
        if(selectedCard is SpellCard)
        {
           if (t == null&&!t.GetComponent<Health>()) return;
           
           GameObject newCard = Instantiate(selectedCard.prefab[0], player.getKingTower().transform.position, Quaternion.identity) as GameObject;
           AreaBullet b= newCard.GetComponent<AreaBullet>();
           b.passValues(player.thisTeam, selectedCard.spellCard.radius, selectedCard.spellCard.areaDamage, selectedCard.spellCard.crownTowerDamage, t);
        }
        else if(selectedCard is BuildCard)
        {
            if (t.tag != "Player") return;
            GameObject newCard = Instantiate(selectedCard.prefab[0], pos, Quaternion.identity) as GameObject;
            player.addToEnemyList(newCard.transform);
            Build c = newCard.GetComponent<Build>();
            if (c != null)
            {
                c.Init(Team);
            }
        }
        else
        {
            if (t.tag != "Player") return;
            float x = 0;
            for(int i=0;i< selectedCard.prefab.Length;i++)
            {
                if (i < 1)
                {
                    spawnposition = pos;
                }
                GameObject newCard = Instantiate(selectedCard.prefab[i], spawnposition, Quaternion.identity) as GameObject;
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
                else
                {
                    x = 0;
                }
                spawnposition = new Vector3(newCard.transform.position.x + x, newCard.transform.position.y, newCard.transform.position.z);
            }
            
            
        }
        mp -= selectedCard.cost;
        selectedCard = null;
      　　Invoke("drawNewCard",1);
    }

    void drawNewCard()
    {
       Card newCard = null;
        cardIndex++;
        cardIndex %= deck.Count;
        newCard = deck[cardIndex];
        //同じカードが出ないように
        bool isDifferent = false;
        while (!isDifferent)
        {
            foreach(var h in hand)
            {
                if (deck[cardIndex].name != h.name)
                {
                    isDifferent = true;
                    newCard = deck[cardIndex];
                }
                else
                {
                    isDifferent = false;
                    cardIndex++;
                    cardIndex %= deck.Count;
                    
                }
            }
            
        }
        selectedButton.giveInformation(newCard);
        for(int i=0;i<hand.Count;i++)
        {
            hand[i] = buttons[i].getCard();
        }
    }
    public void givePlayer(Player p)
    {
        player = p;
    }
    public void upgradeManaRegeneration()
    {
        manaRegeneration++;
    }
}

public enum Decks
{
    holy,
    dark
}