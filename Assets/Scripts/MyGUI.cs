using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyGUI : MonoBehaviour {
    public GameObject UiCanvas;
    public GameObject DreamCanvas;

    public GameObject ProgressStatePanel;
    public GameObject SetPanel;
    public GameObject Chapter1Panel;


    public Button GameStartButton;
    public Button DeveloperButton;
    public Button ExitButton;
    //UiCanvas

    public Button ProgressStateButton;
    public Button SetStateButton;
    //DreamCanvas

    public Button Chapter1Button;
    public Button Chapter2Button;
    public Button Chapter3Button;
    public Button Chapter4Button;
    public Button Chapter5Button;
    public Button PSExitButton;
    //ProgressStatePanel

    public Button SetButton1;
    public Button SetButton2;
    public Button SetButton3;
    public Button SetExitButton;
    //SetPanel

    public Button Chapter1_1Button;
    public Button Chapter1_2Button;
    public Button Chapter1_3Button;
    public Button Chapter1_4Button;
    public Button Chapter1_5Button;
    public Button C1ExitButton;
    //Chapter1Panel



    // Use this for initialization
    void Start()
    {
        UiCanvas.SetActive(true);
        DreamCanvas.SetActive(false);
        ProgressStatePanel.SetActive(false);
        SetPanel.SetActive(false);
        Chapter1Panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        UiCanvas.SetActive(false);
        //현재 진행중인 챕터로 가야하는 거 구현해야됨
    }

    public void Developer()
    {
        UnityEngine.Debug.Log("Dream");
        UiCanvas.SetActive(false);
        DreamCanvas.SetActive(true);
        //일단 이버튼을 꿈으로 가는 것으로 해놓음 추후 변경해야됨
    }

    public void ProgressState()
    {
        UnityEngine.Debug.Log("ProgressState");
        ProgressStatePanel.SetActive(true);
    }
    public void Chapter1()
    {
        UnityEngine.Debug.Log("1");
        Chapter1Panel.SetActive(true);
    }
    /*
    public void Chapter2()
    {
        Chapter2Panel.SetActive(true);
    }
    public void Chapter3()
    {
        Chapter3Panel.SetActive(true);
    }
    public void Chapter4()
    {
        Chapter4Panel.SetActive(true);
    }
    public void Chapter5()
    {
        Chapter5Panel.SetActive(true);
    }
    */
    public void PSExit()
    {
        UnityEngine.Debug.Log("PSExit");
        ProgressStatePanel.SetActive(false);
    }

    public void Chapter1_1()
    {
        UnityEngine.Debug.Log("1_1");
        //해당 챕터로 가는 코드 구현
    }
    public void Chapter1_2()
    {
        UnityEngine.Debug.Log("1_2");
        //해당 챕터로 가는 코드 구현
    }
    public void Chapter1_3()
    {
        UnityEngine.Debug.Log("1_3");
        ///해당 챕터로 가는 코드 구현
    }
    public void Chapter1_4()
    {
        UnityEngine.Debug.Log("1_4");
        //해당 챕터로 가는 코드 구현
    }
    public void Chapter1_5()
    {
        UnityEngine.Debug.Log("1_5");
        //해당 챕터로 가는 코드 구현
    }

    public void C1Exit()
    {
        UnityEngine.Debug.Log("C1Exit");
        Chapter1Panel.SetActive(false);
    }

    public void Set()
    {
        UnityEngine.Debug.Log("Set");
        SetPanel.SetActive(true);
    }
    public void Set1()
    {
        UnityEngine.Debug.Log("Set1");
        //해당 설정버튼해당하는 코드
    }
    public void Set2()
    {
        UnityEngine.Debug.Log("Set2");
        //해당 설정버튼해당하는 코드
    }
    public void Set3()
    {
        UnityEngine.Debug.Log("Set3");
        //해당 설정버튼해당하는 코드
    }

    public void SetExit()
    {
        UnityEngine.Debug.Log("SetExit");
        SetPanel.SetActive(false);
    }
}
