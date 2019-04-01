using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minons : MonoBehaviour
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
    public Minons locked = null;
    private bool isMouseDown = false;
    private Vector3 lastMousePosition = Vector3.zero;
    private bool beingDragged;
    public GameObject deletebutton;
    float deleteTime = 3;
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
        
    }
    // Update is called once per frame
    void Update()
    {

        Vector2 mousePosition1 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        ///Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        bool overSprite=false;
        //bool overSprite = GetComponent<SpriteRenderer>().bounds.Contains(mousePosition);
        if (board.onedraged == "new")
        {
            overSprite = GetComponent<BoxCollider2D>().bounds.Contains(mousePosition1);
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
            deleteTime -= Time.deltaTime;
            if (deleteTime <= 0)
            {
                deletebutton.SetActive(true);
            }
            //Set the position to the mouse position
            transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                             Camera.main.ScreenToWorldPoint(Input.mousePosition).y,
                                              0.0f);
        }
        else
        {
            //deletebutton.SetActive(false);
            deleteTime = 1f;
            if (state == States.wait)
            {
                int newx = (int)(transform.position.x + 1) / 2;
                if (newx < 0)
                {
                    newx = 0;
                }
                if (newx > 4)
                {
                    newx = 4;
                }
                if (transform.position.x < 0 || transform.position.x > 8 || transform.position.y > 8 || transform.position.y < 0)
                {
                    transform.position = new Vector3((float)newx * 2, -4, 0.0f);
                }
                else
                {
                    int y = (int)(transform.position.y + 1) / 2;

                    int x = (int)(transform.position.x + 1) / 2;
                    px = x;
                    py = y;
                    gb.addBattleList(this);
                    state = States.battle;
                }
            }
           
            
            if (state==States.battle){
                int newy = (int)(transform.position.y + 1) / 2;

                int newx = (int)(transform.position.x + 1) / 2;

                if (newx < 0)
                {
                    newx = 0;
                }
                if (newx > 4)
                {
                    newx = 4;
                }
                if (newy < 0)
                {
                    newy = 0;
                }
                if (newy > 4)
                {
                    newy = 4;
                }
                int oldpx = px;
                int oldpy = py;
               
                
                
                transform.position = new Vector3((float)newx * 2, (float)newy * 2, 0.0f);
                gb.changePlace(px, py, newx, newy);
                px = newx;
                py = newy;
            }
           
            if (board.onedraged == this.gameObject.name)
            {
                board.onedraged = "new";
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
                return;
            }
        }
        Ifinrange();
        if (inrange == false)
        {
            blinkMove();
        }
    }

    public void clear()
    {
        Destroy(this.gameObject);
    }
    private void attackBehavior()
    {
        if (inrange)
        {
            attack();
        }
    }
    public Minons deathBehavior()
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
    private Minons findTarget()
    {
        Dictionary<Minons, int> distanceList = new Dictionary<Minons, int>();
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
        Minons maxMinon = null;
        foreach (KeyValuePair<Minons, int> entry in distanceList)
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
        if (locked == null)
        {
            return;
        }

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
