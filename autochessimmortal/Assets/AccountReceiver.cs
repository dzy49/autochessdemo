﻿using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using Common.Code;

//登录注册的信息处理
public class AccountReceiver : MonoBehaviour, IReceiver
{
    public AccountView accountView;

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch ((AccountCode)subCode)
        {
            case AccountCode.Register:
                if (response.ReturnCode == 0)               //返回码正确时
                {
                    accountView.OnHideRegisterPanel();      //关闭注册面板。
                }
                break;
            case AccountCode.Login:
                print("account");
                if (response.ReturnCode == 0)               //返回码正确时
                {
                    //告诉服务器我可以进入了
                    PhotonManager.Instance.OnOperationRequest((byte)OpCode.Room, new Dictionary<byte, object>(), (byte)RoomCode.Enter);                     
                }
                break;
            default:
                break;
        }
    }
}
