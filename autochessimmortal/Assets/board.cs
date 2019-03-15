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
    private List<minons> battleList;
    public static int SIZE=5;
    public minons[,] gameBoard = new minons[SIZE, SIZE];
    public static string onedraged="new";
    public static int count = 0;
    private List<minons> deadList;
    public int gold;
    public game_state gamestate = 0;
    
    public enum game_state
    {
        purchase=0,
        battle=1
    }

    // Use this for initialization
    void Awake()
    {
        battleList = new List<minons>();
        waitpool =new int[]{ -1, -1, -1, -1, -1};
        deadList = new List<minons>();
        gold = 5;
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
        if (gamestate==game_state.battle) {
            foreach (minons m in battleList)
            {
                m.CallBack(0);
                //m.state = 0;
            }

            foreach (minons m in battleList)
            {
                m.CallBack(1);
                //m.state = minons.States.battle;
                //GameObject attacksword = (GameObject)Instantiate(Resources.Load("swordpre"));
                //attacksword.name = "sprite";
                //attacksword.transform.localPosition = new Vector2(m.gameObject.transform.position.x, m.gameObject.transform.position.y+1);
            }

            foreach (minons m in battleList)
            {

                minons temp = m.deathBehavior();
                // m.state = 2;
                if (temp != null) {
                    deadList.Add(temp);
                }
            }
            battleList = battleList.Except(deadList).ToList();
        }
    }

    public void OnBattleClick()
    {

    }

    public void SendBattleListRequest(string accountName, string password, byte subCode)
    {
        MinonsDto dto = new MinonsDto();
        dto.battleList = GetCurrentBattleList();
        Dictionary<byte, object> parameters = new Dictionary<byte, object>();
        parameters[0] = JsonUtility.ToJson(dto);
        PhotonManager.Instance.OnOperationRequest((byte)OpCode.Battle, parameters, subCode);
    }

    public int[][] GetCurrentBattleList()
    {
        return null;
    }
    public void Changestate()
    {
        gamestate = game_state.battle;
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

