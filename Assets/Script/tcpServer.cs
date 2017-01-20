using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;
using System.Xml;
public class msgEventArg : EventArgs {
    private string msg;
    public msgEventArg(string message):base()
    {
        msg = message;
    }
    public string Message {
        get {
            return msg;
        }
    }
}
public class TCPServer : MonoBehaviour {
    public static TCPServer Instance {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TCPServer>();
                if (_instance == null)
                    Debug.LogError("必须要有一个挂载着TCPServer脚本的物体！");
            }
            return _instance;
        }
    }
    private static TCPServer _instance;
    public delegate void OnReceive(object sender,msgEventArg msg);
    public event OnReceive OnReceiveEvent;
    public string hostAddress="127.0.0.1";
    public int port = 9900;
    // List<Socket> ClientList;
    private Thread thStartServer;//定义启动socket的线程 
    XmlDocument xmlDoc;
    IPAddress ip;

   

    const int bufferSize = 8792;//缓存大小,8192字节  
    List<Client> clientList;

    void Start()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.streamingAssetsPath + "/ServerAddress.xml");
        hostAddress= XmlTool.ReadSingleNode(xmlDoc,"IPAddress");
        port=int.Parse(XmlTool.ReadSingleNode(xmlDoc,"Port"));
        ip = IPAddress.Parse(hostAddress);
        thStartServer = new Thread(StartServer);
        thStartServer.Start();//启动该线程  
        clientList = new List<Client>();
    }
    private void StartServer()
    {
        
        TcpListener tlistener = new TcpListener(ip, port);
        tlistener.Start();
        Debug.Log("Socket服务器监听启动......");
        while (true)
        {
            Client _client=new Client(tlistener.AcceptTcpClient());//接收已连接的客户端,阻塞方法  
            clientList.Add(_client);
            _client.OnReceiveEvent += OnReceiveMsg;
            Debug.Log("客户端已连接！local:" + _client.tcpClient.Client.LocalEndPoint + "<---Client:" + _client.tcpClient.Client.RemoteEndPoint);
            if (!_client.tcpClient.Connected)
            {
                clientList.Remove(_client);
               
            }
        }
    }

    public void OnReceiveMsg(object sender,msgEventArg msgArg)
    {
        Debug.Log(sender+":"+msgArg.Message);
        OnReceiveEvent(this, msgArg);
    }
    void OnApplicationQuit()
    {
        thStartServer.Abort();
        if (clientList.Count > 0) {
            for (int iLoop = 0; iLoop < clientList.Count; iLoop++)
            {
                clientList[iLoop].tcpClient.Close();
            }
        }
    }
}
