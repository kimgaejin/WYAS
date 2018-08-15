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

        movingObject.position = point1.position;
        movingVector = Vector3.Normalize(point1.position-point2.position);

    }

    override public void DoInteracting()
    {
        Debug.Log("레버와의 상호작용");

        try
        {
            StopCoroutine(moveToPos);
        }
        catch { }

        if (isOn)
        {
            moveToPos = StartCoroutine(MoveToPoint(point1.position));
        }
        else
        {
            moveToPos = StartCoroutine(MoveToPoint(point2.position));
        }

        isOn = !isOn;
    }

    private IEnumerator MoveToPoint(Vector3 point)
    {
        Vector3 arrow = point1.position - point2.position;
        if (point2.position == point) arrow *= -1;

        while (true)
        {
            bool inX = (movingObject.position.x <= point1.position.x && point2.position.x <= movingObject.position.x)
                || (movingObject.position.x <= point2.position.x && point1.position.x <= movingObject.position.x);
            bool inY = ((movingObject.position.y <= point1.position.y && point2.position.y <= movingObject.position.y)
                    || (movingObject.position.y <= point2.position.y && point1.position.y <= movingObject.position.y));

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
