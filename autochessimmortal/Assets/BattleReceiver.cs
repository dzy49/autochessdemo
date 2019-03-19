using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReceiver : MonoBehaviour, IReceiver {
    board gameboard;
    int playerID = board.id;
    GameObject timer;
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
                timer.GetComponent<CountDown>().StartCountDown();
                break;
            case BattleCode.SendList:
                MinonsDto battleDto = GetResponseFromJson<MinonsDto>(response);
                
                if (battleDto.playerID == playerID)
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
                                Instantiate(a, new Vector3(j * 2.0F, i*2, 0), Quaternion.identity);
                                a.GetComponent<minons>().state = minons.States.wait;
                                a.transform.localPosition = new Vector2(i*2, j*2);
                                a.GetComponent<minons>().px = j;
                                a.GetComponent<minons>().py = i;
                                a.GetComponent<minons>().player = playerID;
                                board.count++;
                                a.name = board.count.ToString();
                            }
                        }
                    }
                    
                }
                break;
            case BattleCode.SendResult:
                break;
        }
    }

    private Dto GetResponseFromJson<Dto>(OperationResponse response)
    {
        return JsonUtility.FromJson<Dto>(response.Parameters[0].ToString());
    }

    int[][] OneDtoTwoD(int[] arr) {
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
