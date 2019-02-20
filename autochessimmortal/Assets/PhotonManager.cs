using System.Collections;

using System.Collections.Generic;

using UnityEngine;

using ExitGames.Client.Photon;

using System;




public class PhotonManager : MonoBehaviour, IPhotonPeerListener
{



    private static PhotonManager instance = null;

    public static PhotonManager Instance { get { return instance; } }



    //用来和服务器连接

    private PhotonPeer peer;



    private ConnectionProtocol protocol = ConnectionProtocol.Udp;//默认使用udp协议

    private string serverAddress = "127.0.0.1:5055";//连接本机ip，端口5055

    private string applicationName = "ChatRoom";//连接名称

    private bool connected = false;

    void Awake()

    {

        instance = this;

        peer = new PhotonPeer(this, protocol);

        peer.Connect(serverAddress, applicationName);//与服务器做连接



    }



    void Update()
    {



        if (!connected) //如果与服务器断开了，就需要再连接一下

            peer.Connect(serverAddress, applicationName);



        peer.Service();//获取服务器的响应，需要每时每刻都获取，保持连接状态



        //这里为调试代码，当客户端按下空格是，想服务器发起请求，内容为"wowowowow"

        if (Input.GetKeyDown(KeyCode.Space))

        {

            var parameter = new Dictionary<byte, object>();

            parameter.Add(0, "wowowowow");

            peer.OpCustom(1, parameter, true);



        }



    }

    //停止客户端时，与服务器断开连接

    void OnDestroy()

    {

        peer.Disconnect();

    }





    public void DebugReturn(DebugLevel level, string message)

    {



    }



    public void OnEvent(EventData eventData)

    {



    }



    //服务器给客户端的响应

    public void OnOperationResponse(OperationResponse operationResponse)

    {





    }



    //状态改变时调用

    public void OnStatusChanged(StatusCode statusCode)

    {

        Debug.Log(statusCode.ToString());

        switch (statusCode)

        {

            case StatusCode.Connect:

                connected = true;

                break;

            case StatusCode.Disconnect:

                connected = false;

                break;

        }



    }

}



