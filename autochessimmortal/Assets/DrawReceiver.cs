using Common.Code;
using Common.Dto;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawReceiver : MonoBehaviour, IReceiver
{
    public board gb;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch ((DrawCode)subCode)
        {
            case DrawCode.Refresh:
                break;
            case DrawCode.Send:
                
                gb.storepool = JsonUtility.FromJson<DrawDto>(response.Parameters[0].ToString()).storepool;
                for (int j = 0; j < 5; j++)
                {
                    string textname = "Canvas/Panel/Text" + j.ToString();
                    if (gb.storepool[j] == 1)
                    {
                        GameObject.Find(textname).GetComponent<Text>().text = "warrior";
                    }
                    else
                    {
                        GameObject.Find(textname).GetComponent<Text>().text = "OTHER";

                    }
                }
                break;
        }
    }
}
