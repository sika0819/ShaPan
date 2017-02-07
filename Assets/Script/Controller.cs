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
            if (!string.IsNullOrEmpty(message))
            {
                StartCoroutine(SelectAnim(message));
            }
            isReceived = false;
        }
	}
    IEnumerator SelectAnim(string msg) {
        Manager.Instance.RemainAll(1);
        yield return new WaitForSeconds(1);
        switch (msg)
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
            case AreaName.BuildYourSelf:
                Manager.Instance.RemainAll(1);
                Manager.Instance.ClearAllWater();
                break;
            default:
                Manager.Instance.RemainAll(1);
                Manager.Instance.ExcutueBrush(msg);
                Manager.Instance.ExcuteAnimation(msg);
                break;
        }
    }
    void OnReceiveMsg(object sender,msgEventArg msg)
    {
       
        message = msg.Message;
        isReceived = true;
    }
}
