using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class board : MonoBehaviour
{
    private ArrayList battleList;
    public minons[,] gameBoard = new minons[5, 5];

    // Use this for initialization
    void Start()
    {
        battleList = new ArrayList();
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
        }

        foreach (minons m in battleList)
        {
            m.CallBack(1);
        }

        foreach (minons m in battleList)
        {
            m.CallBack(2);
        }
    }

    public void addBattleList(minons theMinion)
    {
        battleList.Add(theMinion);
        gameBoard[theMinion.px, theMinion.py] = theMinion;
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
}

