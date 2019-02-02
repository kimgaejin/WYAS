using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_instant_moving : LeverParent {


    [Header("" +
        "레버를 on 됐을 때 물체를 움직입니다." +
        "레버가 off 물체가 초기 위치로 돌아갑니다.")]


    private Coroutine moveToPos;
    private float movingSpeed = 1.0f;
    private int[] curPath;

    private void Start()
    {
        SetGPHReference();
        GetObjectAndPointArray();

        if (leverHandler == null) Debug.Log("leverHandler is null");
        SetSize();

        curPath = new int[objectCount];
        for (int pathInit = 0; pathInit < objectCount; pathInit++)
        {
            curPath[pathInit] = 0;
        }
    }

    public override void DoInteracting()
    {
        if (isSwtichOn)
        {
            leverAnimator.SetBool("isLeft", false);
            spr.sprite = offLeverSp;
        }
        else
        {
            leverAnimator.SetBool("isLeft", true);
            spr.sprite = onLeverSp;
        }

        isSwtichOn = !isSwtichOn;
        try
        {
            StopCoroutine(moveToPos);
        }
        catch { }

        moveToPos = StartCoroutine(MoveToPoint());     
        
    }

    private IEnumerator MoveToPoint()
    {
        Vector3 arrow = Vector3.zero;
        WaitForSeconds waitTimeDelta = new WaitForSeconds(Time.deltaTime);
        Transform movingObject;
        Transform point1;
        Transform point2;

        int intP1X = 0;
        int intP2X = 0;
        int intMX = 0;

        int intP1Y = 0;
        int intP2Y = 0;
        int intMY = 0;

        double xpp = 0;
        double ypp = 0;

        bool allObjectsArrivedLastPoint = false;

        if (isSwtichOn == true)
        {
            for (int i = 0; i < objectCount; i++)
            {
                if (curPath[i] <= 0) continue;
                curPath[i]--;
            }
        }
        else
        {
            for (int i = 0; i < objectCount; i++)
            {
                if (curPath[i] >= pointParent.GetChild(i).childCount - 1) continue;
                curPath[i]++;
            }
        }
        

        while (true)
        {
            allObjectsArrivedLastPoint = true;
            for (int i = 0; i < objectCount; i++)
            {
                

                if (isSwtichOn == true && curPath[i] + 1 >= pointParent.GetChild(i).childCount
                  ||isSwtichOn == false && curPath[i] - 1 < 0)
                {
                    continue;
                }
                allObjectsArrivedLastPoint = false;

                try
                {
                    movingObject = objectArray[i];
                    point1 = pointArray[i, curPath[i]];
                    if (isSwtichOn == true)
                        point2 = pointArray[i, curPath[i] + 1];
                    else 
                        point2 = pointArray[i, curPath[i] - 1];

                    arrow = point2.position - point1.position;

                    xpp = Mathf.Round(arrow.x * movingSpeed * Time.deltaTime * 10);
                    ypp = Mathf.Round(arrow.y * movingSpeed * Time.deltaTime * 10);

                    intP1X = (int)(Mathf.Round(point1.position.x * 10));
                    intP2X = (int)(Mathf.Round(point2.position.x * 10));
                    intMX = (int)(Mathf.Round(movingObject.position.x * 10));

                    intP1Y = (int)(Mathf.Round(point1.position.y * 10));
                    intP2Y = (int)(Mathf.Round(point2.position.y * 10));
                    intMY = (int)(Mathf.Round(movingObject.position.y * 10));

                    // point와 point 사이에 있는 정상적인 상태
                    bool inX = (intP2X <= intMX + xpp && intMX + xpp <= intP1X)
                             || (intP1X <= intMX + xpp && intMX + xpp <= intP2X);
                    bool inY = (intP2Y <= intMY + ypp && intMY + ypp <= intP1Y)
                            || (intP1Y <= intMY + ypp && intMY + ypp <= intP2Y);

                    if (!inX || !inY)
                    {
                        movingObject.position = point2.position;
                        if (isSwtichOn == true)
                            curPath[i]++;
                        else
                            curPath[i]--;

                    }
                    else
                    {
                        movingObject.transform.Translate(arrow * movingSpeed * Time.deltaTime, Space.World);
                    }
                }
                catch { }
            }
            if (allObjectsArrivedLastPoint)
            {
                yield break;
            }


            yield return waitTimeDelta;
        }

    }
}
