using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureCreatePlate : ObjectProperty {

    private SpriteRenderer spr;

    private Transform dropObjectParent;
    private Transform pointParent;
    private Transform[] point;

    private Animator plateAnimator;
    private Sprite onPlateSp;
    private Sprite offPlateSp;
    private string offPlateSpName = "pressurePlate1";
    private string onPlateSpName = "pressurePlate2";

    private bool hadPushed = false;
    private bool isPushed = false;
    private bool action1 = false;
    private Coroutine moveToPos;

    int objSize = 0;


    void Start()
    {
        spr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        plateAnimator = transform.GetChild(0).GetComponent<Animator>();

        base.rangeX = spr.bounds.size.x / 2;
        base.mustFaced = false;

        // dropObjectParent의 자식들 모두 가져오고, 배열할당, SetActive(false);
        dropObjectParent = transform.parent.transform.GetChild(1);
        objSize = dropObjectParent.GetChildCount();

        foreach (Transform obj in dropObjectParent)
        {
            obj.gameObject.SetActive(false);
        }

        // pointParent의 자식들 모두 가져오고, 배열할당, 색 0;
        pointParent = transform.parent.transform.GetChild(2);
        int pointSize = pointParent.GetChildCount();
        point = new Transform[pointSize];

        try
        {
            int ind = 0;
            foreach (Transform pt in pointParent)
            {
                point[ind] = pt;
                Color pC;
                pC = pt.gameObject.GetComponent<SpriteRenderer>().color;
                pC.a = 0;
                pt.gameObject.GetComponent<SpriteRenderer>().color = pC;
            }
        }
        catch { }

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

                bool flag = true;

                foreach (Transform obj in dropObjectParent)
                {
                    if (obj.gameObject.activeSelf == true)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag == true)
                {
                    int ind = 0;
                    foreach (Transform pt in pointParent)
                    {
                        dropObjectParent.GetChild(ind).rotation = Quaternion.identity;
                        dropObjectParent.GetChild(ind).position = pt.position;
                        dropObjectParent.GetChild(ind).gameObject.SetActive(true);
                        ind++;
                    }
                }
            }
            catch { }
            plateAnimator.SetBool("isPressed", true);
        }
        else if (hadPushed == true && isPushed == false)
        {
            plateAnimator.SetBool("isPressed", false);
        }

        if (isPushed == true) hadPushed = true;
        else hadPushed = false;
    }

}
