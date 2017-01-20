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
    int port=8800;
    string remoteIP = "127.0.0.1";
    int remortPort = 9900;
    public TcpClient client;
    static GameObject container;
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
        IPEndPoint remotePoint = new IPEndPoint(IPAddress.Parse(remoteIP), remortPort);
        client = new TcpClient(endPoint);
        try
        {
            client.Connect(remotePoint);
            
        }
        catch (Exception e)
        {
            Debug.Log("客户端连接异常：" + e);
        }
    }
    void Update()
    {
       
    }
    public void SendMessage(string msg)
    {
        Debug.Log("LocalEndPoint = " + client.Client.LocalEndPoint + ". RemoteEndPoint = " + client.Client.RemoteEndPoint);
        //客户端发送数据部分  
        NetworkStream streamToServer = client.GetStream();//获得客户端的流  
        byte[] buffer = Encoding.Unicode.GetBytes(msg);//将字符串转化为二进制  
        streamToServer.Write(buffer, 0, buffer.Length);//将转换好的二进制数据写入流中并发送  
        Debug.Log("发出消息：" + msg);
    }
    
}
