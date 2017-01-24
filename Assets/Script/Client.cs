using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;

public class Client{
    public TcpClient tcpClient;
    public delegate void OnReceive(object sender,msgEventArg msg);
    public event OnReceive OnReceiveEvent;
    private byte[] data;
    string clientIp;
   
    public Client(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
        this.clientIp = tcpClient.Client.RemoteEndPoint.ToString();
        data = new byte[this.tcpClient.ReceiveBufferSize];
        // 从服务端获取消息
        this.tcpClient.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this.tcpClient.ReceiveBufferSize), ReceiveMessage, null);

    }

    public void ReceiveMessage(IAsyncResult ar)
    {

        int bytesRead;
        try
        {
            lock (this.tcpClient.GetStream())
            {
                bytesRead = this.tcpClient.GetStream().EndRead(ar);
            }
            if (bytesRead < 1)
            {
               
                return;
            }
            else
            {
                string messageReceived = System.Text.Encoding.ASCII.GetString(data, 0, bytesRead).Replace("\0", null);
                msgEventArg msgArg = new msgEventArg(messageReceived);
                OnReceiveEvent(this, msgArg);
                Debug.Log(messageReceived);
            }
            lock (this.tcpClient.GetStream())
            {
                this.tcpClient.GetStream().BeginRead(data, 0, System.Convert.ToInt32(this.tcpClient.ReceiveBufferSize), ReceiveMessage, null);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public void OnDisConnected() {
        tcpClient.Close();
        TCPServer.Instance.clientList.Remove(this);
    }
}
