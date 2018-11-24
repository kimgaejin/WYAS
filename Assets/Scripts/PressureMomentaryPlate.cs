using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureMomentaryPlate : ObjectProperty {

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
    private Coroutine moveToPos;


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

        // 새롭게 버튼밟는 상태
        if (hadPushed == false && isPushed == true)
        {
            try
            {
                StopCoroutine(moveToPos);
            }
            catch { }

            moveToPos = StartCoroutine(MoveToPoint(point2.position));
            plateAnimator.SetBool("isPressed", true);
            //spr.sprite = onPlateSp;

        }
        // 버튼 뗀 상태
        else if (hadPushed == true && isPushed == false)
        {
            try
            {
                StopCoroutine(moveToPos);
            }
            catch { }

            moveToPos = StartCoroutine(MoveToPoint(point1.position));
            plateAnimator.SetBool("isPressed", false);
            //spr.sprite = offPlateSp;
        }

        if (isPushed == true) hadPushed = true;
        else hadPushed = false;
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        Vector3 arrow = point1.position - point2.position;
        if (point2.position == point) arrow *= -1;
        float xpp = arrow.x * movingSpeed * Time.deltaTime;
        float ypp = arrow.y * movingSpeed * Time.deltaTime;

        while (true)
        {
            bool inX = (movingObject.position.x + xpp <= point1.position.x && point2.position.x - xpp <= movingObject.position.x)
                || (movingObject.position.x + xpp <= point2.position.x && point1.position.x - xpp <= movingObject.position.x);
            bool inY = ((movingObject.position.y + ypp <= point1.position.y && point2.position.y - ypp <= movingObject.position.y)
                    || (movingObject.position.y + ypp <= point2.position.y && point1.position.y - ypp <= movingObject.position.y));

            if (!inX || !inY)
            {
                movingObject.position = point;
                movingObject.transform.Translate(Vector3.zero, Space.Self);
                yield break;
            }

            movingObject.transform.Translate(arrow * movingSpeed * Time.deltaTime, Space.Self);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
