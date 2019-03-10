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
    private bool beingDragged;

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

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool overSprite=false;
        //bool overSprite = GetComponent<SpriteRenderer>().bounds.Contains(mousePosition);
        if (board.onedraged == "new")
        {
            overSprite = GetComponent<BoxCollider2D>().bounds.Contains(mousePosition);
            board.onedraged = this.gameObject.name;
        }
        
        beingDragged = beingDragged && Input.GetButton("Fire1");
        if (overSprite)
        {
            //If we've pressed down on the mouse (or touched on the iphone)
            if (Input.GetButton("Fire1"))
            {
                beingDragged = true;
            }
        }
        if (beingDragged)
        {
            //Set the position to the mouse position
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                                             0.0f);
        }
        else
        {
            int newx=(int)(transform.position.x+1)/2;
            int newy = (int)(transform.position.y+1) / 2;
           
            transform.position = new Vector3((float)newx * 2, (float)newy * 2, 0.0f);
            
           
            if (state == States.battle)
            {
                gb.changePlace(px, py, newx, newy);
            }
            px = newx;
            py = newy;
            if (board.onedraged == this.gameObject.name)
            {
                board.onedraged = "new";
            }

           if (transform.position.x>=0&& transform.position.x <= 10&& transform.position.y <= 10&& transform.position.y >= 0&&state==States.wait)
            {
               gb.addBattleList(this);
                state = States.battle;
            }
            
            
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
        if (MAD == 2)
        {
            deathBehavior();
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
    public minons deathBehavior()
    {
        if (HP < 1)
        {
            Destroy(this.gameObject);
            return this;
        }
        else
        {
            return null;
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
                    if (distanceList.ContainsKey(gb.gameBoard[i, j]))
                    {
                        distanceList[gb.gameBoard[i, j]] = getDis(i, j);
                    }
                    else
                    {
                        distanceList.Add(gb.gameBoard[i, j], getDis(i, j));
                    }
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

    struct Vertice
    {
        public int pos;
        public int prepos;
        public int dis;
        public bool picked;
    };
    /*
    private int dijstraMove() {
        int boardScale = gb.SIZE * gb.SIZE;
        int boardSize = gb.SIZE;
        List<Vertice> Q = new List<Vertice>();
        List<Vertice> P = new List<Vertice>();
        List<int> occuList = new List<int>();
        for(int i = 0; i < boardSize; i++)
        {
            for(int j = 0;j < boardSize; j++)
            {
                if(gb.gameBoard[i][j] != null)
                {
                    occuList.Add(i + 5 * j);
                }
            }
        }
        for(int i = 0; i < boardScale; i++)
        {
            Vertice ver;
            Q.Add(ver);
            Q[i].dis = 70000;
            Q[i].pos = i;
            Q[i].prepos = -1;
            Q[i].picked = false;
        }
        int origin = px + py;
        int target = locked.px + locked.py;
        Q[origin].dis = 0;
        Q[origin].prepos = -2;
        Q[origin].picked = true;
        foreach(int u in occuList)
        {
            Q[u].picked = true;
        }
        foreach(int u in getAdjPos(origin, boardSize))
        {
            if (occuList.Contains(u) == false)
            {
                Q[u].dis = 1;
                Q[u].prepos = origin;
            }
        }
        bool allPicked = false;
        while(allPicked == false)
        {
            int mindis = 70000;
            int k = -1;
            foreach(Vertice ver in Q)
            {
                if(ver.dis < mindis && ver.picked == false)
                {
                    mindis = ver.dis;
                    k = ver.pos;
                }
            }
            Q[k].picked = true;
            if(k == target)
            {
                //route found
                int post = k;
                while(Q[post].prepos != origin)
                {
                    post = Q[post].prepos;
                }
                return post;//post = x + size * y;
            }
            foreach (int u in getAdjPos(k, boardSize))
            {
                if (Q[u].picked == false)
                {
                    Q[u].dis = 1;
                    Q[u].prepos = k;
                }
            }
            allPicked = true;
            foreach(Vertice ver in Q)
            {
                if(ver.picked == false)
                {
                    allPicked = false;
                }
            }
            if(mindis == 70000)
            {
                return 0;//route doesn't exist
            }
        }
    }
    private List<int> getAdjPos(int pos, int size)
    {
        List<int> adjPos = new List<int>();
        int scale = size * size - 1;
        if(pos+1 < scale)
        {
            adjPos.Add(pos + 1);
        }
        if (pos + size < scale)
        {
            adjPos.Add(pos + size);
        }
        if(pos - size > -1)
        {
            adjPos.Add(pos - size);
        }
        if(pos%size > 0)
        {
            adjPos.Add(pos - 1);
        }
    }
    */
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
        
                locked.HP -= AD;
                if (px == locked.px)
                {
                    if (py > locked.py)
                    {
                        //down
                        if (locked.py -1 >=0 && gb.GetMinons(px, locked.py - 1) != null)
                        {
                            gb.GetMinons(px, locked.py - 1).HP -= AD;
                        }
                    }
                    else
                    {
                        //up
                        if (locked.py + 1 <board.SIZE &&gb.GetMinons(px, locked.py + 1) != null)
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
                        if (locked.px - 1 >= 0 && gb.GetMinons(locked.px - 1, py) != null)
                        {
                            gb.GetMinons(locked.px - 1, py).HP -= AD;
                        }
                    }
                    else
                    {
                        //right
                        if (locked.px + 1 < board.SIZE&&gb.GetMinons(locked.px + 1, py) != null)
                        {
                            gb.GetMinons(locked.px + 1, py).HP -= AD;
                        }
                    }
                }
            
        
    }

    public void attacked(int damage)
    {
        HP -= damage;
    }
}
