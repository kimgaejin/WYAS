using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ui : MonoBehaviour {
    public GameObject Start_Screen_Canvas;
    public GameObject Dream_Control_Center_Canvas;
    public GameObject Portal_Canvas;
    public GameObject Progression_Canvas;


  
    public GameObject Diary_Panel;
    public GameObject Chater1_Content_Panel;
    public GameObject Chater2_Content_Panel;
    public GameObject Chater3_Content_Panel;
    public GameObject Chater4_Content_Panel;


    public Button Start_Game_Button;
    public Button Diary_Button;
    public Button Diary_Exit_Button;

    public Button Charter1_Button;
    public Button Charter2_Button;
    public Button Charter3_Button;
    public Button Charter4_Button;

    public Button Portal_Button;
    public Button Exit_Portal_Button;

    public Button Progression_Exit_Button;


    // Use this for initialization
    void Start () {
        Start_Screen_Canvas.SetActive(true);
        Dream_Control_Center_Canvas.SetActive(false);
        Portal_Canvas.SetActive(false);
      //  Set_Up_Canvas.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGame() {
        Start_Screen_Canvas.SetActive(false);
        Dream_Control_Center_Canvas.SetActive(true);
        Portal_Canvas.SetActive(false);
       // Set_Up_Canvas.SetActive(false);

    }

    public void Exit_Game()
    {


    }

    public void Diary()
    {
        Start_Screen_Canvas.SetActive(false);
        Dream_Control_Center_Canvas.SetActive(true);
        Portal_Canvas.SetActive(false);
       // Set_Up_Canvas.SetActive(false);

        Diary_Panel.SetActive(true);

    }
    public void Diary_Exit() {
        Diary_Panel.SetActive(false);
    }

    public void Set_Up()
    {


    }

    public void Exit_Set_Up()
    {


    }

    public void Diary_Chapter1()
    {
        Chater1_Content_Panel.SetActive(true);
        Chater2_Content_Panel.SetActive(false);
        Chater3_Content_Panel.SetActive(false);
        Chater4_Content_Panel.SetActive(false);


    }

    public void Diary_Chapter2()
    {
        Chater1_Content_Panel.SetActive(false);
        Chater2_Content_Panel.SetActive(true);
        Chater3_Content_Panel.SetActive(false);
        Chater4_Content_Panel.SetActive(false);

    }

    public void Diary_Chapter3()
    {
        Chater1_Content_Panel.SetActive(false);
        Chater2_Content_Panel.SetActive(false);
        Chater3_Content_Panel.SetActive(true);
        Chater4_Content_Panel.SetActive(false);

    }

    public void Diary_Chapter4()
    {
        Chater1_Content_Panel.SetActive(false);
        Chater2_Content_Panel.SetActive(false);
        Chater3_Content_Panel.SetActive(false);
        Chater4_Content_Panel.SetActive(true);

    }
    public void Portal()
    {
        Start_Screen_Canvas.SetActive(false);
        Dream_Control_Center_Canvas.SetActive(false);
        Portal_Canvas.SetActive(true);
        //  Set_Up_Canvas.SetActive(false);


    }
    public void Exit_Portal()
    {
        Start_Screen_Canvas.SetActive(false);
        Dream_Control_Center_Canvas.SetActive(true);
        Portal_Canvas.SetActive(false);
        // Set_Up_Canvas.SetActive(false);

    }

    public void Enter()
    {
        SceneManager.LoadScene("main");
    }

    public void Progreesion()
    {
        Progression_Canvas.SetActive(true);
    }

    public void Progression_Exit()
    {
        Progression_Canvas.SetActive(false);

    }
}
