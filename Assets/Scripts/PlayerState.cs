using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    // references of objects and scripts and components
    private GameObject keypadCanvas;
    private Joystick joystick;
    private Rigidbody2D rigid;

    // setting
    private float horizonSpeed = 0.05f;

    // bit switch
    private bool isFix;
    private bool canMove;

    void Awake()
    {
        InitializeBitSwitch();
        InitializeComponent();
        InitializeKeypad();
    }
    
    void Update()
    {
        if (isFix == false) { rigid.velocity = 0; }
        else { 
            Move();
        }
        
        
        
    }

    private void Move()
    {
        if (canMove == false) return;

        float horizon;
        horizon = joystick.GetHorizontalValue();
        this.transform.position += horizon * horizonSpeed * Vector3.right;

    }

    private void InitializeBitSwitch()
    {
        isFix = false;
        canMove = true;
    }

    private void InitializeComponent()
    {
        rigid = this.GameObject.GetComponent<Rigidbody2D>();
    }

    private void InitializeKeypad()
    {
        joystick = null;
        keypadCanvas = null;

        keypadCanvas = GameObject.Find("KeypadCanvas");
        if (keypadCanvas == null) Debug.Log("error: can't not find keypadCanvas at player");

        joystick = keypadCanvas.transform.FindChild("Joystick").GetComponent<Joystick>();
        if (joystick == null) Debug.Log("error: can't not find joystick at player");
    }
    
}
