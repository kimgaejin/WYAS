using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    private GameObject keypadCanvas;
    private Joystick joystick;

    private bool canMove;

    void Awake()
    {
        canMove = true;
        InitializeKeypad();
    }
    
    void Update()
    {
        Move();
    }

    public void Move()
    {
        if (canMove == false) return;

        float horizon;
        horizon = joystick.GetHorizontalValue();
        this.transform.position += horizon * Vector3.right;
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
