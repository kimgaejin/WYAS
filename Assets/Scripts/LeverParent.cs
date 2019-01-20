using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LeverParent : ObjectProperty {

    // * LeverParent; 레버들의 부모인 추상클래스
    // * 몇몇 특정 초기화 및 공통기능을 관리한다.
    
    protected Transform leverHandler;
    protected Transform objectParent;
    protected Transform pointParent;
    protected Animator leverAnimator;

    protected Transform[] objectArray;
    protected Transform[,] pointArray;
    protected int objectCount = 0;

    protected SpriteRenderer spr;
    protected Sprite onLeverSp;
    protected Sprite offLeverSp;

    protected string onLeverSpName = "레버2";
    protected string offLeverSpName = "레버1";

    protected bool isSwtichOn = false;

    protected void GetObjectAndPointArray()
    {
        leverHandler = transform.Find("leverHandler");
        objectParent = transform.Find("objectParent");
        pointParent = transform.Find("pointParent");

        // * moving/create 등... 오브젝트에 관련된 동작일 때, 해당하는 오브젝트들을 모두 objectArray로 넣어준다.

        Debug.Log("objectParent'sName: " + objectParent.transform.name);

        objectCount = objectParent.childCount;
        
        objectArray = new Transform[objectCount];
        for (int i = 0; i < objectCount; i++)
        {
            objectArray[i] = objectParent.GetChild(i);
        }

        // * point들의 transform을 2차원 배열 pointArray에 저장한다.

        int pointPackSize = 0;
        int pointSize = 0;

        pointPackSize = pointParent.childCount;
        if (pointPackSize > 0) pointSize = pointParent.GetChild(0).childCount;

        pointArray = new Transform[pointPackSize,pointSize];

        for (int i = 0; i < pointPackSize; i++)
        {
            Transform thisPointPack = pointParent.GetChild(i);

            int j = 0;
            foreach (Transform item in thisPointPack)
            {
                // * point의 spr 투명도 조절을 한다. (투명하게)
                Color pColor;
                pColor = item.gameObject.GetComponent<SpriteRenderer>().color;
                pColor.a = 0;
                item.gameObject.GetComponent<SpriteRenderer>().color = pColor;

                pointArray[i,j] = item;
                j++;
            }
        }

        // * 모든 오브젝트들의 초기 위치를 첫번째 pointPack의 첫 point로 바꾼다.
        for (int i = 0; i < objectCount; i++)
        {
            objectArray[i].transform.position = pointArray[i,0].position;
        }
    }

    public override void SetSize()
    {
        if (leverHandler == null
            || objectParent == null
            || pointParent == null)
        {
            leverHandler = transform.Find("leverHandler");
            objectParent = transform.Find("objectParent");
            pointParent = transform.Find("pointParent");
        }

        spr = leverHandler.GetComponent<SpriteRenderer>();
        base.size = spr.bounds.size;

        // handler의 크기입니다. Lever_moving이 아닙니다.
        base.rangeX = 0.7f;// + spr.bounds.size.x / 2;
        base.mustFaced = true;

        leverAnimator = leverHandler.GetComponent<Animator>();
    }

    protected void SetGPHReference()
    {
        spr = GetComponent<SpriteRenderer>();
        leverAnimator = GetComponent<Animator>();
        onLeverSp = Resources.Load<Sprite>("Sprites/" + onLeverSpName);
        offLeverSp = Resources.Load<Sprite>("Sprites/" + offLeverSpName);
    }
}
