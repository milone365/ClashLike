using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
   
    Text winningMessage = null;
    AI cpu = null;
    DeckController playerDeck=null;
    Player[] allPlayers = null;
    Text ClockTXT;
    float TimerCount = 180;
    bool gameEnd = false;
    PlayerSta playerA, playerB;
    FadeScreen fadeScreen = null;
    GameObject endPanel;
    [SerializeField]
    Text TeamApointsTxt = null, TeamBpointsTxt = null;
    void Start()
    {
        endPanel = GameObject.Find("EndPanel");
        endPanel.SetActive(false);
        fadeScreen = GetComponentInChildren<FadeScreen>();
        fadeScreen.fadeOut();
        playerDeck = FindObjectOfType<DeckController>();
        cpu = FindObjectOfType<AI>();
        allPlayers = FindObjectsOfType<Player>();
        foreach(var p in allPlayers)
        {
            if (p.thisTeam == team.Ateam)
            {
                playerA.player = p;
            }
            else
            {
                playerB.player = p;
            }
        }
        winningMessage = GameObject.Find("WinMessage").GetComponent<Text>();
        ClockTXT = GameObject.Find("Clock").GetComponent<Text>();
        if(SoundManager.instance!=null)
        SoundManager.instance.playMusic("duel");
        if (TeamApointsTxt != null && TeamBpointsTxt != null)
        {
            TeamApointsTxt.text = "0";
            TeamBpointsTxt.text = "0";
        }
    }
    private void Update()
    {
        if (ClockTXT == null||gameEnd) return;
        TimerCount -= Time.deltaTime;
        string minutes = ((int)TimerCount / 60).ToString();
        string seconds= ((int)TimerCount % 60).ToString();
        ClockTXT.text = minutes + ":" + seconds;
        if (TimerCount <= 0)
        {
            pointCheck();
        }
    }

    //プレーヤーのポイントを確認する
    private void pointCheck()
    {
        if (playerA.point == playerB.point)
        {
            TimerCount += 60;
            duplicateMpRegeneration();
        }
        else
        {
            if (playerA.point > playerB.point)
            {
                LoseTeam(team.Bteam);
            }
            else
            {
                LoseTeam(team.Ateam);
            }
        }
    }

    private void duplicateMpRegeneration()
    {
        cpu.upgradeManaRegeneration();
        playerDeck.upgradeManaRegeneration();
    }

    //処理を止まる
    public void endGame()
    {
        cpu.GAMEEND = true;
        playerDeck.GAMEEND = true;
        gameEnd = true;
    }
    public void addPointToPlayer(team tm)
    {
        //反対チームにポイントをあげます
        if (tm != team.Ateam)
        {
            playerA.point++;
        }
        else
        {
            playerB.point++;
        }
        if (TeamApointsTxt == null || TeamBpointsTxt == null) return;
        TeamApointsTxt.text = playerA.point.ToString();
        TeamBpointsTxt.text = playerB.point.ToString();
    }
    //負けたチームを渡す
    public void LoseTeam(team tm)
    {
        endGame();
        string message="";
        if (tm == team.Ateam)
        {
             message = "You Lose";
            TeamBpointsTxt.text = "3";
        }
        else
        {
            message = "You Win";
            TeamApointsTxt.text = "3";
        }
        winningMessage.text = message;
        Invoke("ExitfromGame", 3);
    }

    void ExitfromGame()
    {
        winningMessage.text = "";
        endPanel.SetActive(true);
        Character[] charactes = FindObjectsOfType<Character>();
        foreach(var c in charactes)
        {
            if (c != null)
            {
                Destroy(c);
            }
        }
    }
}
struct PlayerSta
{
   public Player player;
   public int point;
}