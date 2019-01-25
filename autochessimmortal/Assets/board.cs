using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    private ArrayList battleList;
    public minons[,] gameBoard = new minons[5, 5];

    // Use this for initialization
    void Awake()
    {
        battleList = new ArrayList();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        foreach (minons m in battleList)
        {
            m.CallBack(0);
            m.state = 0;
        }

        foreach (minons m in battleList)
        {
            m.CallBack(1);
            m.state = 1;
            //GameObject attacksword = (GameObject)Instantiate(Resources.Load("swordpre"));
            //attacksword.name = "sprite";
            //attacksword.transform.localPosition = new Vector2(m.gameObject.transform.position.x, m.gameObject.transform.position.y+1);
        }

        foreach (minons m in battleList)
        {
            m.CallBack(2);
            m.state = 2;
        }
    }

    public void addBattleList(minons theMinion)
    {
        battleList.Add(theMinion);
        gameBoard[theMinion.px, theMinion.py] = theMinion;
    }

    public void deleBattleList(minons theMinion)
    {
        battleList.Remove(theMinion);
        gameBoard[theMinion.px, theMinion.py] = null;
    }

    public void changePlace(int sx, int sy, int tx, int ty)
    {
        minons temp = gameBoard[sx, sy];
        gameBoard[sx, sy] = null;
        gameBoard[tx, ty] = temp;
    }
    public minons GetMinons(int px, int py)
    {
        return gameBoard[px, py];
    }

    public bool placeLegal(int x, int y)
    {
        if(x < 0 || y < 0 || x> 4 || y > 4)
        {
            return false;
        }
        if(gameBoard[x,y] != null)
        {
            return false;
        }
        return true;
    }
}

