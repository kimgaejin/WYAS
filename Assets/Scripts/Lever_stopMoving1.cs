using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 레버를 누른다면 p1에서 2초, p2에서 2초 대기하면서 움직이는 레버
 * 레버를 멈춘다면 딱 그자리에서 멈추는걸로?
 * 
 */



 // 앗 지금은 버튼이 필요해서 버튼먼저 작업.



public class Lever_stopMoving1 : MonoBehaviour {
   /* // Transforms
    private Transform[] movingObjectArray;
    private Transform[] startPoint;
    private Transform[] endPoint;
    private Animator leverAnimator;

    // References
    SpriteRenderer spr;
    string onLeverSpName = "레버2";
    string offLeverSpName = "레버1";
    Sprite onLeverSp;
    Sprite offLeverSp;

    // Variables
    private float movingSpeed = 1;
    private int movingCount;

    // Switchs
    private bool isOn = false;
    private bool toP2 = true;
    private Coroutine moveToPos;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        leverAnimator = GetComponent<Animator>();

        // handler의 크기입니다. Lever_moving이 아닙니다.
        base.rangeX = 0.7f;// + spr.bounds.size.x / 2;
        base.mustFaced = true;

        // [link the movingObjects and p1, p2]
        Transform movingObjectsParent;
        movingObjectsParent = transform.parent.Find("movingObjects");
        movingCount = movingObjectsParent.childCount;
        movingObjectArray = new Transform[movingCount];
        for (int i = 0; i < movingCount; i++)
        {
            movingObjectArray[i] = movingObjectsParent.GetChild(i);
        }

        int point1Count;
        int point2Count;
        Transform point1Parent;
        Transform point2Parent;
        point1Parent = transform.parent.Find("point1Parent");
        point2Parent = transform.parent.Find("point2Parent");
        point1Count = point1Parent.childCount;
        point2Count = point2Parent.childCount;

        // exception
        if (point1Count != point2Count)
        {
            Debug.Log("not match point1Count and p2 in Lever_moving1  ");
        }
        if (movingCount != point1Count)
        {
            Debug.Log("not match movingCount and p1 in Lever_moving1  " + movingCount.ToString() + " , " + point1Count.ToString());
        }

        startPoint = new Transform[point1Count];
        endPoint = new Transform[point2Count];
        for (int i = 0; i < point1Count; i++)
        {
            startPoint[i] = point1Parent.GetChild(i);
            endPoint[i] = point2Parent.GetChild(i);
        }


        // [hide the points' graphics]

        int repeat = startPoint.Length;
        for (int i = 0; i < repeat; i++)
        {
            Color p1C;
            p1C = startPoint[i].gameObject.GetComponent<SpriteRenderer>().color;
            p1C.a = 0;
            startPoint[i].gameObject.GetComponent<SpriteRenderer>().color = p1C;
        }
        repeat = endPoint.Length;
        for (int i = 0; i < repeat; i++)
        {
            Color p1C;
            p1C = endPoint[i].gameObject.GetComponent<SpriteRenderer>().color;
            p1C.a = 0;
            endPoint[i].gameObject.GetComponent<SpriteRenderer>().color = p1C;
        }


        // [set the movingObjects' init position]
        repeat = movingObjectArray.Length;
        for (int i = 0; i < repeat; i++)
        {
            movingObjectArray[i].position = startPoint[i].position;
        }

        onLeverSp = Resources.Load<Sprite>("Sprites/" + onLeverSpName);
        offLeverSp = Resources.Load<Sprite>("Sprites/" + offLeverSpName);
    }

    override public void DoInteracting()
    {
        try
        {
            StopCoroutine(moveToPos);
        }
        catch { }

        if (isOn)
        {
            moveToPos = StartCoroutine(MoveToPoint(false));
            leverAnimator.SetBool("isLeft", false);
            spr.sprite = offLeverSp;
        }
        else
        {
            moveToPos = StartCoroutine(MoveToPoint(true));
            leverAnimator.SetBool("isLeft", true);
            spr.sprite = onLeverSp;
        }

        isOn = !isOn;
    }

    private IEnumerator MoveToPoint(bool turnOn)
    {
        Vector3 arrow;
        float xpp;
        float ypp;

        Transform movingObject;
        Transform point1;
        Transform point2;

        while (true)
        {
            for (int i = 0; i < movingCount; i++)
            {
                movingObject = movingObjectArray[i];
                point1 = startPoint[i];
                point2 = endPoint[i];

                //Debug.Log("pos2: " + point2.position.ToString() + " pos1: " + point1.position.ToString());
                arrow = point2.position - point1.position;
                if (turnOn == false) arrow *= -1;
                xpp = arrow.x * movingSpeed * Time.deltaTime;
                ypp = arrow.y * movingSpeed * Time.deltaTime;

                Vector3 movepointPositionVector = new Vector3(point2.position.x - point1.position.x, point2.position.y - point1.position.y, 0);
                Vector3 objectPositionVector = new Vector3(movingObject.position.x - point1.position.x, movingObject.position.y - point1.position.y, 0);

                bool inX = objectPositionVector.x * movepointPositionVector.x < 0 || Mathf.Abs(movepointPositionVector.x) < Mathf.Abs(objectPositionVector.x);
                bool inY = objectPositionVector.y * movepointPositionVector.y < 0 || Mathf.Abs(movepointPositionVector.y) < Mathf.Abs(objectPositionVector.y);

                // Debug.Log("object, movePoint vector: " + objectPositionVector.ToString() + " , " + movepointPositionVector.ToString());

                //Debug.Log("inX,Y: " + inX.ToString() + inX.ToString());
                if (inX == true || inY == true)
                {
                    // 움직이는 블록이 라인(p1 - p2 사이) 를 벗어나면 멈춤.
                    if (turnOn == true) movingObject.position = point2.position;
                    else movingObject.position = point1.position;
                    movingObject.transform.Translate(Vector3.zero, Space.Self);
                    yield break;
                }
                else
                {
                    // 이전에 Space.Self 여서 안됐었음.
                    movingObject.transform.Translate(arrow * movingSpeed * Time.deltaTime, Space.World);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public override void SetSize()
    {
        spr = this.GetComponent<SpriteRenderer>();
        base.size = spr.bounds.size;
    }*/
}
