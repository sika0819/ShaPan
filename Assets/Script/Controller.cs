using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    // Use this for initialization
    bool isReceived = false;
    string message;
	void Start () {
        TCPServer.Instance.OnReceiveEvent+= OnReceiveMsg;
	}
	
	// Update is called once per frame
	void Update () {
        if (isReceived)
        {
            isReceived = false;
            switch (message)
            {
                case AreaName.WaterInArea:
                    Manager.Instance.WaterIn(10, 10);
                    break;
                case AreaName.ConsoleArea:
                    Manager.Instance.ConsoleWater(10, 10);
                    break;
                case AreaName.CleanWaterOutArea:
                    Manager.Instance.CleanWaterOut(10, 10);
                    break;
                case AreaName.DirtyArea:
                    Manager.Instance.DirtyOut(10, 10);
                    break;
                case AreaName.All:
                    Manager.Instance.ExcuteAll();
                    break;
            }
        }
	}
    void OnReceiveMsg(object sender,msgEventArg msg)
    {
        message = msg.Message;
        isReceived = true;
    }
}
