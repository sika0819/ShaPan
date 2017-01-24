using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System;
using DG.Tweening;
using System.Collections.Generic;

public class UIController : MonoBehaviour {
    Vector3 originPos;
    Vector3 endPos;
    public GameObject Title;
    public Image Fill;
    public Image ConsoleMenu;
    public Image ConsoleFill;
    public Image Theory;
    public Image TheoryFill;
    public Image Flow;
    public Image FlowFill;
    public Image BuildYouSelf;
    public Image BuildSelfFill;
    public GameObject MainMenu;
    public GameObject ConsoleMeanObj;//污水处理意义界面
    public GameObject PhysicBtn;
    public GameObject ChemicBtn;
    public GameObject BiologyBtn;
   
    public GameObject PhysicMenu;
    public GameObject ChemicMenu;
    public GameObject BiologyMenu;
    public GameObject FirstBtn;
    public GameObject SecondBtn;
    public GameObject ThirdBtn;
    public GameObject DirtyBtn;
    public GameObject BuildBtn;

    public GameObject FirstMenu;
    public GameObject SecondMenu;
    public GameObject ThirdMenu;
    public GameObject BuildYourselfMenu;
    public GameObject BackBtn;
    public delegate void OnSlideUp();
    public event OnSlideUp OnSlidedUp;
    public GameObject NowMenu {
        get {
            return _nowMenu;
        }set {
            if (_nowMenu != value)
            {
                _nowMenu = value;
                _nowMenu.SetActive(true);
                HideSecondBtn();
                if(_nowMenu!=MainMenu)
                _nowMenu.transform.DOLocalMoveX(0, 2).OnComplete(delegate() { BackBtn.SetActive(true); });
            }
        }
    }
    GameObject _nowMenu;
    MLTree<GameObject> MenuTree;
    public static UIController Instance {
        get {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIController>();
                if (_instance == null)
                {
                    GameObject container = new GameObject();
                    _instance = container.AddComponent<UIController>();
                }
            }
            return _instance;
        }
    }
    private static UIController _instance;
    // Use this for initialization
    void Start () {
        Title.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        Fill.rectTransform.sizeDelta = new Vector2(20,10);
        ConsoleMenu.rectTransform.sizeDelta = new Vector2(5,0);
        Theory.rectTransform.sizeDelta = new Vector2(5, 0);
        Flow.rectTransform.sizeDelta = new Vector2(5, 0);
        BuildYouSelf.rectTransform.sizeDelta = new Vector2(5, 0);
        ConsoleFill.rectTransform.sizeDelta = new Vector2(0, 0);
        TheoryFill.rectTransform.sizeDelta = new Vector2(0, 0);
        FlowFill.rectTransform.sizeDelta = new Vector2(0, 0);
        BuildSelfFill.rectTransform.sizeDelta = new Vector2(0, 0);
        ConsoleMeanObj.GetComponent<RectTransform>().localPosition=new Vector3(Screen.width,0);
        PhysicBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        ChemicBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        BiologyBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        FirstBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        SecondBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        ThirdBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        DirtyBtn.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 30);
        PhysicBtn.SetActive(false);
        ChemicBtn.SetActive(false);
        BiologyBtn.SetActive(false);
        FirstBtn.SetActive(false);
        SecondBtn.SetActive(false);
        ThirdBtn.SetActive(false);
        PhysicMenu.SetActive(false);
        BiologyMenu.SetActive(false);
        ChemicMenu.SetActive(false);
        FirstMenu.SetActive(false);
        SecondMenu.SetActive(false);
        ThirdMenu.SetActive(false);
        BuildYourselfMenu.SetActive(false);
        EventListener.GetListener(ConsoleFill.gameObject).OnSlideUp=OnShowConsoleMeaning;
        EventListener.GetListener(TheoryFill.gameObject).OnSlideUp = OnShowConsoleMethodBtn;
        EventListener.GetListener(FlowFill.gameObject).OnSlideUp = OnShowFlowBtn;
        EventListener.GetListener(BuildSelfFill.gameObject).OnSlideUp = OnShowBuildMenu;
        EventListener.GetListener(BackBtn).OnClick = OnBackMain;
        EventListener.GetListener(DirtyBtn).OnClick = OnDirtyWater;
        TreeNode<GameObject> menuRoot = new TreeNode<GameObject>(MainMenu);
        TreeNode<GameObject> consoleMenu = new TreeNode<GameObject>(ConsoleMeanObj);
        TreeNode<GameObject> physicMenu = new TreeNode<GameObject>(PhysicMenu);
        TreeNode<GameObject> biologyMenu = new TreeNode<GameObject>(BiologyMenu);
        TreeNode<GameObject> chemicMenu = new TreeNode<GameObject>(ChemicMenu);
        TreeNode<GameObject> firstMenu = new TreeNode<GameObject>(FirstMenu);
        TreeNode<GameObject> secondMenu = new TreeNode<GameObject>(SecondMenu);
        TreeNode<GameObject> thirdMenu = new TreeNode<GameObject>(ThirdMenu);
        TreeNode<GameObject> BuildMenu = new TreeNode<GameObject>(BuildYourselfMenu);
        MenuTree = new MLTree<GameObject>(menuRoot);
        MenuTree.AddChild(menuRoot,consoleMenu);
        MenuTree.AddChild(menuRoot, physicMenu);
        MenuTree.AddChild(menuRoot, biologyMenu);
        MenuTree.AddChild(menuRoot, chemicMenu);
        MenuTree.AddChild(menuRoot, firstMenu);
        MenuTree.AddChild(menuRoot, secondMenu);
        MenuTree.AddChild(menuRoot, thirdMenu);
        MenuTree.AddChild(menuRoot,BuildMenu);
        OnSlidedUp += OnSlideUpFunc;
        BackBtn.SetActive(false);
	}

   

    // Update is called once per frame
    void Update () {
        OnTouch();
     //   TouchMoved();
        TouchEnd();
        if (endPos.y - originPos.y > 0.5)
        {
            //Debug.Log("向上");
            originPos = endPos;
            OnSlidedUp();
        }
        if (endPos.x - originPos.x > 100) {
          //  Debug.Log("向右");
            originPos = endPos;
           // OnSlidedRight();
        }
	}
    bool OnTouch() {
        if (Input.GetMouseButtonDown(0))
        {
            originPos = Input.mousePosition;
            //Debug.Log(originPos);
            return true;
        }
        else if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                originPos = Input.GetTouch(0).position;
                return true;
            }
           
        }
        return false;
    }
   
    bool TouchEnd() {
        if (Input.GetMouseButtonUp(0))
        {
            endPos = Input.mousePosition;
            originPos = endPos;
          //  Debug.Log(endPos);
            return true;
        }
        else if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endPos = Input.GetTouch(0).position;
                return true;
            }
        }
        return false;
    }
    void OnSlideUpFunc() {
        //Debug.Log("向上");
        Title.transform.DOLocalMove(new Vector3(0,150,0),1);
        Fill.rectTransform.DOSizeDelta(new Vector2(1075, 10), 1).OnComplete(ShowLine);
    }
    void OnBackMain(GameObject go)
    {
        BackBtn.SetActive(false);
        if (NowMenu!=null&&NowMenu.GetComponent<RectTransform>().localPosition == Vector3.zero)
        {
            TreeNode<GameObject> nowMenuObj = new TreeNode<GameObject>(NowMenu);
            TreeNode<GameObject> uppderMenu = MenuTree.Parent(nowMenuObj);
            if (uppderMenu!= null)
            {
                uppderMenu.Data.transform.DOLocalMove(new Vector3(0, 0, 0), 2);
                nowMenuObj.Data.transform.DOLocalMoveX(Screen.width, 2);
                NowMenu = MainMenu;
            }
        }
    }
   
    void ShowLine() {
        ConsoleMenu.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowChuLiFill);
        Theory.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowYuanLiFill);
        Flow.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowLiuChengFill);
        BuildYouSelf.rectTransform.DOSizeDelta(new Vector2(5, 245), 1).OnComplete(ShowBuildYouSelfFill);
    }
    void ShowChuLiFill()
    {
        ConsoleFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void ShowYuanLiFill()
    {
        TheoryFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void ShowLiuChengFill()
    {
        FlowFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void ShowBuildYouSelfFill()
    {
        BuildSelfFill.rectTransform.DOSizeDelta(new Vector2(120, 120), 1);
    }
    void OnShowConsoleMethodBtn(GameObject go) {
        HideSecondBtn();
        PhysicBtn.SetActive(true);
        ChemicBtn.SetActive(true);
        BiologyBtn.SetActive(true);
        PhysicBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        ChemicBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        BiologyBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        EventListener.GetListener(PhysicBtn).OnClick = OnPhysicBtnClick;
        EventListener.GetListener(ChemicBtn).OnClick = OnChemicBtnClick;
        EventListener.GetListener(BiologyBtn).OnClick = OnBiologyBtnClick;
    }
   
    void OnShowFlowBtn(GameObject go)
    {
        HideSecondBtn();
        FirstBtn.SetActive(true);
        SecondBtn.SetActive(true);
        ThirdBtn.SetActive(true);
        FirstBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        SecondBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        ThirdBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        DirtyBtn.GetComponent<RectTransform>().DOSizeDelta(new Vector2(125, 30), 1);
        EventListener.GetListener(FirstBtn).OnClick = OnFirstClick;
        EventListener.GetListener(SecondBtn).OnClick = OnSecondClick;
        EventListener.GetListener(ThirdBtn).OnClick = OnThirdClick;
    }
    void OnPhysicBtnClick(GameObject go) {
        NowMenu = PhysicMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
    }
    void OnChemicBtnClick(GameObject go)
    {
        NowMenu = ChemicMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
    }
    void OnBiologyBtnClick(GameObject go)
    {
        NowMenu = BiologyMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
    }
    void OnFirstClick(GameObject go)
    {
        NowMenu = FirstMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
        MyClient.Instance.SendMessage(AreaName.WaterInArea);
    }
    void OnSecondClick(GameObject go)
    {
        NowMenu = SecondMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
        MyClient.Instance.SendMessage(AreaName.ConsoleArea);
    }
    void OnThirdClick(GameObject go)
    {
        NowMenu = ThirdMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
        MyClient.Instance.SendMessage(AreaName.CleanWaterOutArea);
    }
    void OnDirtyWater(GameObject go)
    {
        MyClient.Instance.SendMessage(AreaName.DirtyArea);
    }
    void OnShowConsoleMeaning(GameObject go) {
        NowMenu = ConsoleMeanObj;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
        MyClient.Instance.SendMessage(AreaName.All);
    }
    private void OnShowBuildMenu(GameObject go)
    {
        NowMenu = BuildYourselfMenu;
        MainMenu.transform.DOLocalMoveX(-Screen.width, 2);
    }
    void HideSecondBtn()
    {
        GameObject[] SecondBtns = GameObject.FindGameObjectsWithTag("SecondBtn");
        for (int iLoop = 0; iLoop < SecondBtns.Length; iLoop++)
        {
            SecondBtns[iLoop].GetComponent<RectTransform>().DOSizeDelta(new Vector2(0,0), 1).OnComplete(delegate() { SecondBtns[iLoop].SetActive(false); });
        }
    }
}
