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
    public States state;
    public minons locked = null;
    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;
    public enum States
    {
        wait=1,
        battle=2

    }
    // Use this for initialization
    void Awake()
    {
        gb = GameObject.Find("mboard").GetComponent<board>();
    }
    void Start()
    {
        gb.addBattleList(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
            lastMousePosition = Vector3.zero;
        }
        if (isMouseDown)
        {
            if (lastMousePosition != Vector3.zero)
            {
                Vector3 offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastMousePosition;
                this.transform.position += offset;
            }
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        }

      
    }
    public void CallBack(int MAD)
    {
        if (state !=States.wait){
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
    }
    private void moveBehavior()
    {
        if (locked == null)
        {
            locked = findTarget();
            if(locked == null)
            {
                print("game end");
            }
        }
        Ifinrange();
        if (inrange == false)
        {
            blinkMove();
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
            gb.deleBattleList(this);
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
            if(entry.Key.player == this.player)
            {
                continue;
            }
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

    private void stepMove()
    {
        int verX = locked.px - px;
        int verY = locked.py - py;
        int biasX = 0;
        int biasY = 0;
        if (verX > 0)
        {
            biasX = 1;
        }
        if(verX < 0){
            biasX = -1;
        }
        if(verY > 0)
        {
            biasY = 1;
        }
        if(verY < 0)
        {
            biasY = -1;
        }
        if (biasX == 0 && biasY == 0)
        {
            return;
        }
        if (gb.placeLegal(px + biasX, py))
        {
            gb.changePlace(px, py, px + biasX, py);
            px += biasX;
            this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
            return;
        }
        if (gb.placeLegal(px, py + biasY))
        {
            gb.changePlace(px, py, px, py + biasY);
            py += biasY;
            this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
            return;
        }
    }

    private void blinkMove()
    {
        int biasX = 0;
        int biasY = 0;
        if(gb.placeLegal(locked.px, locked.py + 1))
        {
            biasX = 0;
            biasY = 1;
        }
        else if (gb.placeLegal(locked.px+1, locked.py))
        {
            biasX = 1;
            biasY = 0;
        }
        else if (gb.placeLegal(locked.px-1, locked.py))
        {
            biasX = -1;
            biasY = 0;
        }
        else if (gb.placeLegal(locked.px, locked.py-1))
        {
            biasX = 0;
            biasY = -1;
        }
        if(biasX == 0 && biasY == 0)
        {
            return;
        }
        gb.changePlace(px, py, locked.px + biasX, locked.py + biasY);
        px = locked.px + biasX;
        py = locked.py + biasY;
        this.gameObject.transform.localPosition = new Vector2(px * 2, py * 2);
        return;
    }

    public void attack()
    {

        if (MP < 100)
        {
            locked.attacked(AD);
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

    public void attacked(int damage)
    {
        HP -= damage;
    }
}
