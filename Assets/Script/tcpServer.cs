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
   // public string hostAddress="127.0.0.1";
    public int port = 9900;
    // List<Socket> ClientList;
    private Thread thStartServer;//定义启动socket的线程 
    XmlDocument xmlDoc;
    IPAddress ip;
    public List<Client> clientList;
    TcpListener tlistener;
    void Start()
    {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.streamingAssetsPath + "/ServerAddress.xml");
       // hostAddress= XmlTool.ReadSingleNode(xmlDoc,"IPAddress");
        port=int.Parse(XmlTool.ReadSingleNode(xmlDoc,"Port"));
        //ip = IPAddress.Parse(hostAddress);
        tlistener = new TcpListener(port);
        tlistener.Start();
        thStartServer = new Thread(StartServer);
        thStartServer.Start();//启动该线程  
        clientList = new List<Client>();
    }
    private void StartServer()
    { 
        Debug.Log("Socket服务器监听启动......");
        while (true)
        {
            tlistener.BeginAcceptTcpClient(Accept, tlistener);//接收已连接的客户端,阻塞方法  
        }
    }
    void Accept(IAsyncResult iar) {
        TcpListener nowListener = (TcpListener)iar.AsyncState;
        TcpClient client = nowListener.EndAcceptTcpClient(iar);
        Client _client = new Client(client);
        clientList.Add(_client);
        Debug.Log("客户端已连接！local:" + _client.tcpClient.Client.LocalEndPoint + "<---Client:" + _client.tcpClient.Client.RemoteEndPoint);
        _client.OnReceiveEvent += OnReceiveMsg;
        
    }
    public void OnReceiveMsg(object sender,msgEventArg msgArg)
    {
     //   Debug.Log(sender+":"+msgArg.Message);
     //   Debug.Log(msgArg.Message.Length);
        OnReceiveEvent(this, msgArg);
    }
    void OnApplicationQuit()
    {
        tlistener.Stop();
        thStartServer.Abort();
        if (clientList.Count > 0) {
            for (int iLoop = clientList.Count-1; iLoop >=0 ; iLoop--)
            {
                clientList[iLoop].tcpClient.Close();
            }
            clientList.Clear();
        }
    }
}
