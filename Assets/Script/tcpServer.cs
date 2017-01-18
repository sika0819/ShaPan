using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
public class tcpServer : MonoBehaviour {
    TcpListener MyListener;
    // Use this for initialization
    void Start () {
        MyListener = new TcpListener(new IPEndPoint(IPAddress.Any, 0));
        MyListener.Start();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
