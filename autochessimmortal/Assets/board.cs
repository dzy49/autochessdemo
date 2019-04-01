using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Common.Dto;
using Common.Code;

public class board : MonoBehaviour
{
    public int[] storepool;
    public int[] waitpool;
    private List<Minons> battleList;
    public static int SIZE=5;
    public Minons[,] gameBoard = new Minons[SIZE, SIZE];
    public static string onedraged="new";
    public static int count = 0;
    private List<Minons> deadList;
    public int gold;
    public game_state gamestate = 0;
    public static int id = -1;
    private Dictionary<byte, object> parameters = new Dictionary<byte, object>();
    public GameObject purchasePanel;
    public int playerHealth = 100;
    public int[] mybattleList;
    public enum game_state
    {
        purchase=0,
        battle=1
    }
    public void purchasePanelSwitch()
    {
        if (purchasePanel.activeInHierarchy == true)
        {
            purchasePanel.SetActive(false);
        }
        else
        {
            purchasePanel.SetActive(true);
        }
        
        
    }
    // Use this for initialization
    void Awake()
    {
        battleList = new List<Minons>();
        waitpool =new int[]{ -1, -1, -1, -1, -1};
        deadList = new List<Minons>();
        gold = 5;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void degugPrint()
    {
        int count = 0;
        string debugstring1 = "debug1: ";
        string debugstring2 = "debug2: ";
        string debugstring3 = "debug3: ";
        for (int i = 0; i < mybattleList.Length; i++)
        {
            debugstring1 += mybattleList[i];
        }
        for(int j = 0; j < 5; j++)
        {
            for (int k= 0; k < 5; k++)
            {
                if (gameBoard[j, k])
                {
                    debugstring2 += "1";
                }
            }
        }
        for (int i = 0; i < battleList.Count; i++)
        {
            if (battleList[i])
            {
                debugstring3+= "1";
            }
        }
        print(debugstring1);
        print(debugstring2);
        print(debugstring3);
    }
    private void FixedUpdate()
    {
        if (gamestate==game_state.battle) {
            bool AllOneSide = true;
            int playerID = -1;
            int count = 0;
            foreach(Minons m in battleList)
            {
                count++;
                if (playerID == -1)
                {
                    playerID = m.player;
                }
                else if (playerID != m.player)
                {
                    AllOneSide = false;
                    break;
                }
            }
            if (AllOneSide)
            {
                RoundEnd(playerID,count);
            }
            foreach (Minons m in battleList)
            {
                m.CallBack(0);
                //m.state = 0;
            }

            foreach (Minons m in battleList)
            {
                m.CallBack(1);
         
            }

            foreach (Minons m in battleList)
            {

                Minons temp = m.deathBehavior();
                // m.state = 2;
                if (temp != null) {
                    deadList.Add(temp);
                }
            }
            battleList = battleList.Except(deadList).ToList();
        }
    }

    public void StartBattle()
    {
        print("getcalled");
        gamestate = game_state.battle;
    }

    public void RestoreBattleGround()
    {
        foreach(Minons m in battleList)
        {

            m.clear();
        }
        battleList = new List<Minons>();
        int[][] twoDbattleList = BattleReceiver.OneDtoTwoD(mybattleList);
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (twoDbattleList[i][j] != 0)
                {
                    GameObject a = (Resources.Load("warrior") as GameObject);
                    a.GetComponent<Minons>().state = Minons.States.wait;
                    a.transform.localPosition = new Vector2(i * 2, j * 2);
                    a.GetComponent<Minons>().px = j;
                    a.GetComponent<Minons>().py = i;
                    a.GetComponent<Minons>().player = id;
                    a.name = board.count.ToString();
                    board.count++;
                    Instantiate(a, new Vector3(j * 2.0F, i * 2, 0), Quaternion.identity);

                }
            }
        }
    }
    public void RoundEnd(int winner,int damage)
    {
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters[0] = 0;
        gamestate = game_state.purchase;
        if (winner != id)
        {
            playerHealth -= damage;
            parameters[0] = damage;
        }
        else
        {
            gold += 1;
        }
        RestoreBattleGround();
        PhotonManager.Instance.OnOperationRequest((byte)OpCode.Battle, parameters, (byte)BattleCode.SendResult);
    }
    public void SendBattleListRequest()
    {
        mybattleList = GetCurrentBattleList();
    
        print("!!!" + count);
        MinonsDto dto = new MinonsDto();
        dto.battleList = GetCurrentBattleList();
        dto.playerID = id;
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters[0] = JsonUtility.ToJson(dto);
        print(JsonUtility.ToJson(dto));
        PhotonManager.Instance.OnOperationRequest((byte)OpCode.Battle, parameters, (byte)BattleCode.SendList);
    }

    public int[] GetCurrentBattleList()
    {
        int[] battlelist =new int[25];
        
        for (int i = 0; i < 5; i++)
        {
            
            for(int j = 0; j < 5; j++)
            {
                
                if (gameBoard[i, j] != null)
                {
                    print("gb:" + i + ":" + j);
                    battlelist[i+j*5] = 1;
                }
            }
        }
        return battlelist;
    }

    public void Changestate()
    {
        gamestate = game_state.battle;
    }

    public void addBattleList(Minons theMinion)
    {
        battleList.Add(theMinion);
        gameBoard[theMinion.px, theMinion.py] = theMinion;
    }

    public void deleBattleList(Minons theMinion)
    {
        battleList.Remove(theMinion);
        gameBoard[theMinion.px, theMinion.py] = null;
    }

    public void changePlace(int sx, int sy, int tx, int ty)
    {
        Minons temp = gameBoard[sx, sy];
        gameBoard[sx, sy] = null;
        gameBoard[tx, ty] = temp;
    }
    public Minons GetMinons(int px, int py)
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

