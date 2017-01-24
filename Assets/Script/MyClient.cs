using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System;
using System.Net;
using System.Text;
using System.Xml;
public class MyClient:MonoBehaviour{
    string ipAddress = "127.0.0.1";
    int port=0;
    string remoteIP = "127.0.0.1";
    int remortPort = 9900;
    public TcpClient client;
    static GameObject container;
    float timer;
    public static MyClient Instance {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType(typeof(MyClient)) as MyClient;
                if (_instance==null)
                {
                    container = new GameObject();
                    container.name = "MyClient";
                    _instance = container.AddComponent<MyClient>() as MyClient;
                }
            }
            return _instance;
        }
    }
    private static MyClient _instance;
    XmlDocument xmlDoc;
    XmlDocument remoteDoc;
    IPEndPoint remotePoint;
    void Start() {
        xmlDoc = new XmlDocument();
        xmlDoc.Load(Application.streamingAssetsPath + "/ClientAddress.xml");
        ipAddress = XmlTool.ReadSingleNode(xmlDoc,"IPAddress");
        port = int.Parse(XmlTool.ReadSingleNode(xmlDoc, "Port"));
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        remoteDoc = new XmlDocument();
        remoteDoc.Load(Application.streamingAssetsPath + "/ServerAddress.xml");
        remoteIP = XmlTool.ReadSingleNode(remoteDoc, "IPAddress");
        
        remortPort = int.Parse(XmlTool.ReadSingleNode(remoteDoc, "Port"));
        remotePoint = new IPEndPoint(IPAddress.Parse(remoteIP), remortPort);
        client = new TcpClient(endPoint);
        client.BeginConnect(remoteIP, remortPort, OnBeginConnect, client);
    }

    private void OnBeginConnect(IAsyncResult ar)
    {
        
        try
        {
            TcpClient nowClient = (TcpClient)ar.AsyncState;
            nowClient.EndConnect(ar);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void Update()
    {

       

    }
    public void SendMessage(string msg)
    {
        Debug.Log("LocalEndPoint = " + client.Client.LocalEndPoint + ". RemoteEndPoint = " + client.Client.RemoteEndPoint);
        //客户端发送数据部分  
        try
        {
            NetworkStream streamToServer = client.GetStream();//获得客户端的流
            byte[] buffer = Encoding.ASCII.GetBytes(msg);//将字符串转化为二进制  
            streamToServer.BeginWrite(buffer, 0, buffer.Length,new AsyncCallback(OnBeginWrite),null);//将转换好的二进制数据写入流中并发送  
            streamToServer.Flush();
           // Debug.Log("发出消息：" + msg);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    private void OnBeginWrite(IAsyncResult ar)
    {
        Debug.Log(ar.AsyncState);
    }

    void OnApplicationQuit() {
        client.Close();
    }
}
