using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minons : MonoBehaviour
{
    public int HP;
    public int MP;
    public int player;
    public board gb;
    public int AD;
    public int px;
    public int py;
    public bool inrange;
    public int maxdistance;
    public int attackrange, abilitytype;
    public minons locked = null;

    // Use this for initialization
    void Start()
    {
        gb.addBattleList(this);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void CallBack(int MAD)
    {
        switch (MAD)
        {
            case 0:
                moveBehavior();
                break;
            case 1:
                attackBehavior();
                break;
            case 2:
                deathBehavior();
                break;
        }
    }
    private void moveBehavior()
    {
        if (locked == null)
        {
            locked = findTarget();
        }
        Ifinrange();
        if (inrange == false)
        {
            moveMinon();
        }
    }
    private void attackBehavior()
    {
        if (inrange)
        {
            attack();
        }
    }
    private void deathBehavior()
    {
        if (HP < 1)
        {
            Destroy(this.gameObject);
        }
    }
    private void makeTarget()
    {
        locked = findTarget();
    }
    private minons findTarget()
    {
        Dictionary<minons, int> distanceList = new Dictionary<minons, int>();
        int i = 0;
        int j = 0;
        for (i = 0; i < 5; i++)
        {
            for (j = 0; j < 5; j++)
            {
                if (gb.gameBoard[i, j] != null)
                {
                    distanceList.Add(gb.gameBoard[i, j], getDis(i, j));
                }
            }
        }
        int maxDis = 0;
        minons maxMinon = null;
        foreach (KeyValuePair<minons, int> entry in distanceList)
        {
            if (maxDis < entry.Value)
            {
                maxDis = entry.Value;
                maxMinon = entry.Key;
            }
        }
        maxdistance = maxDis;
        return maxMinon;
    }

    private int getDis(int i, int j)
    {
        int disx = gb.gameBoard[i, j].px - px;
        int disy = gb.gameBoard[i, j].py - py;
        if (disx < 0)
        {
            disx = -disx;
        }
        if (disy < 0)
        {
            disy = -disy;
        }
        return disx + disy;
    }

    private void Ifinrange()
    {
        if (getTargetDis() > attackrange)
        {
            inrange = false;
        }
        else
        {
            inrange = true;
        }
    }

    private int getTargetDis()
    {
        int disx = locked.px - px;
        int disy = locked.py - py;
        if (disx < 0)
        {
            disx = -disx;
        }
        if (disy < 0)
        {
            disy = -disy;
        }
        return disx + disy;
    }

    private void moveMinon()
    {
        //temporary work
        if(gb.placeLegal(locked.px, locked.py + 1))
        {
            gb.changePlace(px, py, locked.px, locked.py + 1);
            px = locked.px;
            py = locked.py + 1;
            this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
            return;
        }
        if (gb.placeLegal(locked.px+1, locked.py))
        {
            gb.changePlace(px, py, locked.px+1, locked.py);
            px = locked.px+1;
            py = locked.py;
            this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
            return;
        }
        if (gb.placeLegal(locked.px-1, locked.py))
        {
            gb.changePlace(px, py, locked.px-1, locked.py);
            px = locked.px-1;
            py = locked.py;
            this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
            return;
        }
        if (gb.placeLegal(locked.px, locked.py-1))
        {
            gb.changePlace(px, py, locked.px, locked.py - 1);
            px = locked.px;
            py = locked.py-1;
            this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
            return;
        }

        
    }

    public void attack()
    {

        if (MP < 100)
        {
            locked.HP -= AD;
        }
        else
        {
            specialAttack();
        }
    }
    public void specialAttack()
    {
        switch (abilitytype)
        {
            case 0:
                locked.HP -= AD;
                if (px == locked.px)
                {
                    if (py > locked.py)
                    {
                        //down
                        if (gb.GetMinons(px, locked.py - 1) != null)
                        {
                            gb.GetMinons(px, locked.py - 1).HP -= AD;
                        }
                    }
                    else
                    {
                        //up
                        if (gb.GetMinons(px, locked.py + 1) != null)
                        {
                            gb.GetMinons(px, locked.py + 1).HP -= AD;
                        }
                    }
                }
                else
                {
                    if (px > locked.px)
                    {
                        //left
                        if (gb.GetMinons(locked.px - 1, py) != null)
                        {
                            gb.GetMinons(locked.px - 1, py).HP -= AD;
                        }
                    }
                    else
                    {
                        //right
                        if (gb.GetMinons(locked.px + 1, py) != null)
                        {
                            gb.GetMinons(locked.px + 1, py).HP -= AD;
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
}
