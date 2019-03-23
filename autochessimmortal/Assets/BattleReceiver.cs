using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReceiver : MonoBehaviour, IReceiver {
    public board gameboard;
    int playerID;
    public CountDown timer;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnReceive(byte subCode, OperationResponse response)
    {
        print("get!");
        switch ((BattleCode)subCode)
        {
            case BattleCode.AllReady:
                timer.StartCountDown();
                break;
            case BattleCode.SendList:
                playerID = board.id;
                MinonsDto battleDto = GetResponseFromJson<MinonsDto>(response);
                print(battleDto.playerID);
                print(playerID);
                if (battleDto.playerID != playerID)
                {
                    int[][] temparr = OneDtoTwoD(battleDto.battleList);
                    VerticalMirror(temparr, 5);
                    for(int i=0;i< 5; i++)
                    {
                        for(int j=0; j< 5; j++)
                        {
                            if (temparr[i][j] != 0)
                            {
                                GameObject a = (Resources.Load("warrior") as GameObject);
                                a.GetComponent<Minons>().state = Minons.States.wait;
                                a.transform.localPosition = new Vector2(i*2, j*2);
                                a.GetComponent<Minons>().px = j;
                                a.GetComponent<Minons>().py = i;
                                a.GetComponent<Minons>().player = battleDto.playerID;
                                 a.name = board.count.ToString();
                                board.count++;
                                Instantiate(a, new Vector3(j * 2.0F, i * 2, 0), Quaternion.identity);
                               
                            }
                        }
                    }
                    
                }
                //gameboard.gamestate = board.game_state.battle;
                gameboard.StartBattle();
                break;
            case BattleCode.SendResult:
                int player = Int32.Parse(response.Parameters[1].ToString());
                int health = Int32.Parse(response.Parameters[0].ToString());
                print("player:"+player +" health:"+ health);
                break;
        }
    }

    private Dto GetResponseFromJson<Dto>(OperationResponse response)
    {
        return JsonUtility.FromJson<Dto>(response.Parameters[0].ToString());
    }

    public static int[][] OneDtoTwoD(int[] arr) {
        int[][] newarr =new int[5][];
        for(int i = 0; i < 5; i++)
        {
            newarr[i] = new int[5];
            for(int j = 0; j < 5; j++)
            {
                newarr[i][j]=arr[i * 5 + j];
            }
            
        }
        //print(newarr);
        return newarr;
    }
    void VerticalMirror(int[][] arr, int n)
    {
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n / 2; j++) 
            {
                int temp = arr[j][i];
                arr[j][i] = arr[n - j - 1][i];
                arr[n - j - 1][i] = temp;
            }
    }
}
