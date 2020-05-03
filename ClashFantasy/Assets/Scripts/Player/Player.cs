using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Player : MonoBehaviour
{
    
    Tower[] myTowers = null;
    public List<Transform> enemies = new List<Transform>();
    public team thisTeam = team.Ateam;
    Tower kingTower = null;
    

    public Tower getKingTower()
    {
        return kingTower;
    }
    private void Start()
    {
        myTowers = GetComponentsInChildren<Tower>();
        foreach(var t in myTowers)
        {
            enemies.Add(t.transform);
            t.Ininialize(thisTeam);
            if (t.TowerType == towerType.king)
            {
                kingTower = t;
            }
        }
    }
    //リストに加えて、イベントを追加する
    public void addToEnemyList(Transform t)
    {
        enemies.Add(t);
        Health h = t.GetComponent<Health>();
        if (h != null)
        {
            h.onDeath += removeToList;
        }
    }
    public void removeToList(Transform t)
    {
        enemies.Remove(t);
    }
}

public enum team
{
    Ateam,
    Bteam
}