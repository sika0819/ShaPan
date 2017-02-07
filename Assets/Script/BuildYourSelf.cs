using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class BuildYourSelf : MonoBehaviour {
    public Text TextArea;
    public GameObject sureBtn;
    int order;
    public GameObject[] buildBtn;
    Color[] originColor;
    Dictionary<int,KeyValuePair<GameObject, bool>> orderList;
    public Text nowOrder {
        get {
            return _orderText;
        }set {
            if (_orderText!=value) {
                _orderText = value;
                order++;
            }
        }
    }
    Text _orderText;
    // Use this for initialization
    void Awake () {
       // buildBtn = GameObject.FindGameObjectsWithTag("BuildBtn");
        orderList = new Dictionary<int, KeyValuePair<GameObject, bool>>();
        originColor = new Color[buildBtn.Length];
        for (int iLoop = 0; iLoop < buildBtn.Length; iLoop++)
        {
            orderList.Add(iLoop+1,new KeyValuePair<GameObject, bool>(buildBtn[iLoop],true));
            originColor[iLoop] = buildBtn[iLoop].GetComponent<Image>().color;
            EventListener.GetListener(buildBtn[iLoop]).OnDraging += OnBuild;
            EventListener.GetListener(buildBtn[iLoop]).OnClick += OnBuild;
        }
        EventListener.GetListener(sureBtn).OnClick += OnSure;
        TextArea.text = "";
    //    Debug.Log(orderList.Count);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnBuild(GameObject go) {
        if (orderList.Count > 0)
        {
            for (int iLoop = 0; iLoop < orderList.Count; iLoop++)
            {
                if (orderList[iLoop+1].Key.name == go.name && orderList[iLoop+1].Value)
                {
                    nowOrder = go.GetComponentInChildren<Text>();
                    if (nowOrder)
                        nowOrder.text = order.ToString();
                    orderList[iLoop+1] = new KeyValuePair<GameObject, bool>(nowOrder.transform.parent.gameObject, false);
                    Image clickedImage = orderList[iLoop + 1].Key.GetComponent<Image>();
                    if (clickedImage)
                    {
                        clickedImage.DOColor(Color.white, 1);
                    }
                    MyClient.Instance.SendMessage(go.name);
                }
            }
        }
    }
    public void Clear() {
        order = 0;
        nowOrder = null;
        if (orderList.Count > 0)
        {
            for (int iLoop = 0; iLoop < orderList.Count; iLoop++)
            {
                Image building = orderList[iLoop+1].Key.GetComponent<Image>();
                if (building)
                {
                    building.color = originColor[iLoop];
                }
                Text child = orderList[iLoop+1].Key.GetComponentInChildren<Text>();
                if (child)
                    child.text = "";
                orderList[iLoop+1] = new KeyValuePair<GameObject, bool>(orderList[iLoop+1].Key, true);
            }
        }
    }
    void OnSure(GameObject go)
    {
        for (int iLoop = 0; iLoop < orderList.Count; iLoop++)
        {
            Text errorText=orderList[iLoop+1].Key.GetComponentInChildren<Text>();
          //  Debug.Log(errorText.text);

            if (errorText)
            {
                if (errorText.text != (iLoop + 1).ToString() || orderList[iLoop+1].Value)
                {
                    Image errorImage = orderList[iLoop+1].Key.GetComponent<Image>();
                    if (errorImage)
                    {
                        errorImage.DOColor(new Color(1, 0, 0, 1), 1);
                    }
                    errorText.text = (iLoop + 1).ToString();
                    TextArea.text = "失败！";
                }
            }
        }
    }
}
