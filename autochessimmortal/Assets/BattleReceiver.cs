using Common.Code;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleReceiver : MonoBehaviour, IReceiver {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch ((BattleCode)subCode)
        {
            case BattleCode.AllReady:
                break;
            case BattleCode.SendList:
                break;
            case BattleCode.SendResult:
                break;
        }
    }
}
