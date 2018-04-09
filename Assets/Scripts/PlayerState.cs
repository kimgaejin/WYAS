using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour {

    private GameObject keypadCanvas;
    private Joystick joystick;

    void Awake()
    {
        joystick = null;
        keypadCanvas = null;

        keypadCanvas = GameObject.Find("KeypadCanvas");
        if (keypadCanvas == null) Debug.Log("error: can't not find keypadCanvas at player");

        joystick = keypadCanvas.transform.FindChild("Joystick").GetComponent<Joystick>();
        if (joystick == null) Debug.Log("error: can't not find joystick at player");
    }
    
    void Update()
    {
        float horizon, vertical;
        horizon = joystick.GetHorizontalValue();
        vertical = joystick.GetVerticalValue();
        this.transform.position += horizon * Vector3.right;
        this.transform.position += vertical * Vector3.up;
    }

    public void move()
    {

    }
    
}
