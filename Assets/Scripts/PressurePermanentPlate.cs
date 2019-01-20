using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePermanentPlate : ObjectProperty {

    private SpriteRenderer spr;
    // Transforms
    private Transform[] movingObjectArray;
    private Transform[] startPoint;
    private Transform[] endPoint;

    private int movingCount = 0;

    private Animator plateAnimator;
    private Sprite onPlateSp;
    private Sprite offPlateSp;
    private string offPlateSpName = "pressurePlate1";
    private string onPlateSpName = "pressurePlate2";

    // Variables
    private float movingSpeed = 1;
    private Vector3 movingVector;

    private bool hadPushed = false;
    private bool isPushed = false;
    private Coroutine moveToPos;


    void Start()
    {
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plateAnimator = transform.GetChild(0).GetComponent<Animator>();

        base.rangeX = spr.bounds.size.x / 2;
        base.mustFaced = false;

        GetObjectAndPointArray();

        onPlateSp = Resources.Load<Sprite>("Sprites/" + onPlateSpName);
        offPlateSp = Resources.Load<Sprite>("Sprites/" + offPlateSpName);
    }


    private void Update()
    {
        int layerMask = (1 << LayerMask.NameToLayer("PLAYER"))
                        | (1 << LayerMask.NameToLayer("OBJECT_1ST"));

        //Debug.Log("transform:position " + transform.position);
        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(.5f, .5f), 0.0f, layerMask, 0);

        isPushed = false;
        foreach (Collider2D col in colls)
        {
            isPushed = true;
        }

        // 새롭게 버튼밟는 상태
        if (isPushed == true)
        {
            try
            {
                StopCoroutine(moveToPos);
            }
            catch { }

            plateAnimator.SetBool("isPressed", true);
            moveToPos = StartCoroutine(MoveToPoint());
            spr.sprite = onPlateSp;

        }
    }

    private IEnumerator MoveToPoint()
    {
        Transform movingObject;
        Transform point1;
        Transform point2;

        Vector3 arrow = Vector3.zero;
        float xpp = 0;
        float ypp = 0;

        while (true)
        {
            for (int i = 0; i < movingCount; i++)
            {
                movingObject = movingObjectArray[i];
                point1 = startPoint[i];
                point2 = endPoint[i];

                arrow = point2.position - point1.position;
                xpp = arrow.x * movingSpeed * Time.deltaTime;
                ypp = arrow.y * movingSpeed * Time.deltaTime;

                bool inX = (movingObject.position.x + xpp <= point1.position.x && point2.position.x - xpp <= movingObject.position.x)
                    || (movingObject.position.x + xpp <= point2.position.x && point1.position.x - xpp <= movingObject.position.x);
                bool inY = ((movingObject.position.y + ypp <= point1.position.y && point2.position.y - ypp <= movingObject.position.y)
                        || (movingObject.position.y + ypp <= point2.position.y && point1.position.y - ypp <= movingObject.position.y));

                if (!inX || !inY)
                {
                    movingObject.position = point2.position;
                    yield break;
                }
                else
                {
                    movingObject.transform.Translate(arrow * movingSpeed * Time.deltaTime, Space.Self);
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void GetObjectAndPointArray()
    {
        /*
        // [link the movingObjects and p1, p2]
        Transform movingObjectsParent;
        movingObjectsParent = transform.FindChild("movingObjects");
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
        */
    }
}
