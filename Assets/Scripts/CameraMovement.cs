using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float tempFrame = 60;

    private Transform player;
    private Vector3 rangeOfChange;

    private Vector3 startCameraPosition;
    private Vector3 endCameraPosition;
    private Vector3 vel;
    private Vector3 acc;

    // CONSTANTS
    private Vector3 increasePositionX;
    private Vector3 increasePositionY;
    private Vector3 increaseVelocityX;
    private Vector3 increaseVelocityY;
    private Vector3 increaseAccelationX;
    private Vector3 increaseAccelationY;

    private void Awake()
    {
        startCameraPosition = this.transform.position;
        endCameraPosition = startCameraPosition;
        vel = Vector3.zero;

        increasePositionX = new Vector3(8.5f, 0, 0);
        increasePositionY = new Vector3(0, 6.5f, 0);
        increaseVelocityX = new Vector3(1, 0, 0);
        increaseVelocityY = new Vector3(0, 1, 0);
        increaseAccelationX = new Vector3(1, 0, 0);
        increaseAccelationY = new Vector3(0, 1, 0);
    }

    private void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 distance = player.position - endCameraPosition;

        if (distance.x < -4.25f)    // 플레이어가 왼쪽
        {
            endCameraPosition -= increasePositionX;
           
        }
        else if (distance.x > 4.25f)    // PLAYER IS RIGHT
        {
            endCameraPosition += increasePositionX;
        }
        else if (distance.y > 2.75f)    // PLAYER IS TOP
        {
            endCameraPosition += increasePositionY;
        }
        else if (distance.y < -3.75f)    // PLAYER IS BOTTOM
        {
            endCameraPosition -= increasePositionY;
        }
        MoveToEnd();

        this.transform.position += vel * Time.deltaTime;
       // Debug.Log("VELOCITY: " + vel);
    }

    private void MoveToEnd() // 플레이어 근처 카메라위치로 이동
    {
        Vector3 moveArrow = endCameraPosition - this.transform.position;
       // Debug.Log("MOVEARROW: "+moveArrow);
        //Debug.Log("end: " + endCameraPosition +" camera  "+this.transform.position) ;
        if (vel.x * moveArrow.x < 0)
        {
            vel.x = 0;
        }
        if (vel.y * moveArrow.y < 0)
        {
            vel.y = 0;
        }

        vel = moveArrow;
    }
    /*
    public float tempFrame = 60;

    private Vector3 StandardPosition;
    private float cameraInitialVelocity = 1.0f;
    private Vector3 vel;
    private Vector3 acc;

    private float intervalPosX = 850 / 100.0f;
    private float intervalPosY = 650 / 100.0f;
    // 카메라가 간격을 건너간 수
    public int crossX = 0;
    public int crossY = 0;


    private void Awake() {
        StandardPosition = this.GetComponent<Transform>().position;
    }

    private void Update() {
        cameraMovement();
    }

    private void cameraMovement()
    {
        //if (isAdjusting == true) return;

        Vector3 distance = CheckDistanceToPlayer();
        Debug.Log(distance);

        if (Mathf.Abs(distance.x) > intervalPosX/2)
        {
            // 카메라-플레이어이므로 카메라가 크다면, 플레이어가 뒤로간 것.
            if (distance.x < 0) CameraAccelation(1,0);
            else CameraAccelation(-1,0);
        }

        if (Mathf.Abs(distance.y) > intervalPosY/2)
        {
            if (distance.y < 0) CameraAccelation(0, 1);
            else CameraAccelation(0, -1);
        }

        //Debug.Log("Distance" + distance);

        if ((vel.x*(vel.x+acc.x)) < 0 )
        {
            acc = acc + new Vector3(-acc.x, 0, 0);
            vel = vel + new Vector3(-vel.x, 0, 0);
            //transform.position = new Vector3(crossX * intervalPosX, transform.position.y, 0) + StandardPosition;
            
        }
        else if ((vel.y * (vel.y + acc.y)) < 0)
        {
            acc = acc + new Vector3(0, -acc.y, 0);
            vel = vel + new Vector3(0, -vel.y, 0);
           // transform.position = new Vector3(transform.position.x, crossY * intervalPosY, 0) + StandardPosition;
        }
        else
        {
            vel = vel + acc;
            transform.Translate(vel * Time.deltaTime);
        }

    }

    private void CameraAccelation(int Inc_crossX, int Inc_crossY)
    {
        crossX += Inc_crossX;
        crossY += Inc_crossY;

        if (Inc_crossX != 0)
        {
            acc = acc + new Vector3(-acc.x, 0, 0);
            vel = vel + new Vector3(-vel.x, 0, 0);

            cameraInitialVelocity = Mathf.Abs(transform.position.x-intervalPosX*crossX) * 2 ;
            if (Inc_crossX == 1)
            {
               // acc = new Vector3(-cameraInitialVelocity * Time.deltaTime, 0, 0);
                acc += new Vector3(-cameraInitialVelocity * Time.deltaTime, 0, 0);
                vel += Vector3.right * cameraInitialVelocity;

            }
            if (Inc_crossX == -1)
            {
                //  acc = new Vector3(-cameraInitialVelocity / Time.deltaTime, 0, 0);
                acc += new Vector3(cameraInitialVelocity * Time.deltaTime, 0, 0);
                vel += Vector3.left * cameraInitialVelocity;
            }
           
        }
        if (Inc_crossY != 0)
        {
            acc = acc + new Vector3(0, -acc.y, 0);
            vel = vel + new Vector3( 0, -vel.y, 0);

            cameraInitialVelocity = Mathf.Abs(transform.position.y - intervalPosY * crossY) * 2;
            if (Inc_crossY == 1)
            {
                acc += new Vector3(0, -cameraInitialVelocity * Time.deltaTime, 0);
                vel += Vector3.up * cameraInitialVelocity ;
            }
            if (Inc_crossY == -1)
            {
                acc += new Vector3(0, cameraInitialVelocity * Time.deltaTime, 0);
                vel += Vector3.down * cameraInitialVelocity ;
            }
        }

       

        //acc *= Time.deltaTime;
    }
    
    private Vector3 CheckDistanceToPlayer()
    {
        Vector3 distance = Vector3.zero;
        Transform player = GameObject.Find("Player").GetComponent<Transform>();

        distance = StandardPosition + new Vector3(intervalPosX * crossX, intervalPosY * crossY, 0) - player.position;
        return distance;
    }
    */
}
