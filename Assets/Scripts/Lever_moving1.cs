using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_moving1 : ObjectProperty {

    // Transforms
    private Transform movingObject;
    private Transform point1;
    private Transform point2;

    // References
    SpriteRenderer spr;
    string onLeverSpName = "레버2";
    string offLeverSpName = "레버1";
    Sprite onLeverSp;
    Sprite offLeverSp;

    // Variables
    private float movingSpeed = 1;
    private Vector3 movingVector;

    // Switchs
    private bool isOn = false;
    private Coroutine moveToPos;

    private void Start()
    {
        // handler의 크기입니다. Lever_moving이 아닙니다.
        base.rangeX = 0.7f + spr.bounds.size.x / 2;
        base.mustFaced = true;

        movingObject = transform.parent.transform.GetChild(1).transform;
        point1 = transform.parent.transform.FindChild("point1").transform;
        point2 = transform.parent.transform.FindChild("point2").transform;
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
        movingVector = Vector3.Normalize(point1.position-point2.position);

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
            moveToPos = StartCoroutine(MoveToPoint(point1.position));
            spr.sprite = offLeverSp;
        }
        else
        {
            moveToPos = StartCoroutine(MoveToPoint(point2.position));
            spr.sprite = onLeverSp;
        }

        isOn = !isOn;
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        Vector3 arrow = point1.position - point2.position;
        if (point2.position == point) arrow *= -1;
        float xpp = arrow.x * movingSpeed * Time.deltaTime;
        float ypp = arrow.y * movingSpeed * Time.deltaTime;


        while (true)
        {
            bool inX = (movingObject.position.x+ xpp <= point1.position.x && point2.position.x- xpp <= movingObject.position.x)
                || (movingObject.position.x+ xpp <= point2.position.x && point1.position.x- xpp <= movingObject.position.x);
            bool inY = ((movingObject.position.y+ ypp <= point1.position.y && point2.position.y- ypp <= movingObject.position.y)
                    || (movingObject.position.y+ ypp <= point2.position.y && point1.position.y- ypp <= movingObject.position.y));

            if (!inX || !inY)
            {
                movingObject.position = point;
                movingObject.transform.Translate(Vector3.zero, Space.Self);
                yield break;
            }

            movingObject.transform.Translate(arrow* movingSpeed *Time.deltaTime, Space.Self);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    public override void SetSize()
    {
        spr = this.GetComponent<SpriteRenderer>();
        base.size = spr.bounds.size;
    }
}
