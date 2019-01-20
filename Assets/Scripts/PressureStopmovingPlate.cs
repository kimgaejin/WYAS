using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 발판을 밟는다면 p1에서 2초, p2에서 2초 대기하며 계속 움직이는 오브젝트를 활성화한다.
 * 발판에서 떨어진다면 오브젝트는 그 자리에서 기다리도록?
 * 
 */

public class PressureStopmovingPlate : ObjectProperty
{

    private SpriteRenderer spr;
    // Transforms
    private Transform movingObject;
    private Transform point1;
    private Transform point2;

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
    private bool action1 = false;
    private Coroutine moveToPos;


    /*
    void Start()
    {
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plateAnimator = transform.GetChild(0).GetComponent<Animator>();

        base.rangeX = spr.bounds.size.x / 2;
        base.mustFaced = false;

        movingObject = transform.GetChild(1).transform;
        point1 = transform.Find("point1").transform;
        point2 = transform.Find("point2").transform;
        try
        {
            Color p1C;
            p1C = point1.gameObject.GetComponent<SpriteRenderer>().color;
            p1C.a = 0;
            point1.gameObject.GetComponent<SpriteRenderer>().color = p1C;

            Color p2C;
            p2C = point2.gameObject.GetComponent<SpriteRenderer>().color;
            p2C.a = 0;
            point2.gameObject.GetComponent<SpriteRenderer>().color = p2C;
        }
        catch { }
        movingObject.position = point1.position;

        movingVector = Vector3.Normalize(point1.position - point2.position);
        onPlateSp = Resources.Load<Sprite>("Sprites/" + onPlateSpName);
        offPlateSp = Resources.Load<Sprite>("Sprites/" + offPlateSpName);
    }


    private void Update()
    {
        int layerMask = (1 << LayerMask.NameToLayer("PLAYER"))
                        | (1 << LayerMask.NameToLayer("OBJECT_1ST"));

        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position, new Vector2(.5f, .5f), 0.0f, layerMask, 0);

        isPushed = false;
        foreach (Collider2D col in colls)
        {
            isPushed = true;
        }

        if (hadPushed == false && isPushed == true)
        {
            try
            {
                StopCoroutine(moveToPos);
            }
            catch { }

            moveToPos = StartCoroutine(MoveToPoint());
            plateAnimator.SetBool("isPressed", true);
        }
        else if (hadPushed == true && isPushed == false)
        {
            try
            {
                StopCoroutine(moveToPos);
            }
            catch { }

            plateAnimator.SetBool("isPressed", false);
        }
        
        if (isPushed == true) hadPushed = true;
        else hadPushed = false;
    }

    private IEnumerator MoveToPoint()
    {
        Vector3 arrow = Vector3.zero;
        float xpp = 0;
        float ypp = 0; 

        WaitForSeconds waitTimeDelta = new WaitForSeconds(Time.deltaTime);
        WaitForSeconds wait200 = new WaitForSeconds(2.0f);

        arrow = point1.position - point2.position;
        if (action1 == false)
            arrow *= -1;

        while (true)
        {

            // rail 안에 있는 정상적인 상태
            bool inX = (movingObject.position.x  <= point1.position.x && point2.position.x  <= movingObject.position.x)
               || (movingObject.position.x <= point2.position.x && point1.position.x <= movingObject.position.x);
            bool inY = (movingObject.position.y  <= point1.position.y && point2.position.y  <= movingObject.position.y)
                    || (movingObject.position.y  <= point2.position.y && point1.position.y <= movingObject.position.y);

            if (!inX || !inY)
            {
                if (action1 == false)
                {
                    movingObject.position = point2.position;
                    //movingObject.transform.Translate(Vector3.zero, Space.Self);
                }
                else
                {
                    movingObject.position = point1.position;
                    //movingObject.transform.Translate(Vector3.zero, Space.Self);
                }

                action1 = !action1;
                arrow *= -1;
                yield return wait200;
            }

            movingObject.transform.Translate(arrow * movingSpeed * Time.deltaTime, Space.Self);
            yield return waitTimeDelta;
        }
    }
    */
}
