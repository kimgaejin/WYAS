using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Lever 
 *  - handler + GPH
 *  - dropObjectParent
 *  - pointParent
 */
public class Lever_create1 : ObjectProperty
{
    private Animator leverAnimator;
    // Transforms
    private Transform dropObjectParent;
    private Transform pointParent;
    private Transform[] point;

    // References
    SpriteRenderer spr;
    string onLeverSpName = "레버2";
    string offLeverSpName = "레버1";
    Sprite onLeverSp;
    Sprite offLeverSp;

    int objSize = 0;

    // Switchs
    private bool isOn = false;

    private void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        leverAnimator = GetComponent<Animator>();

        // handler의 크기입니다. Lever_moving이 아닙니다.
        base.rangeX = 0.7f;// + spr.bounds.size.x / 2;
        base.mustFaced = true;

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

        onLeverSp = Resources.Load<Sprite>("Sprites/" + onLeverSpName);
        offLeverSp = Resources.Load<Sprite>("Sprites/" + offLeverSpName);
    }

    override public void DoInteracting()
    {
        // on -> off
        if (isOn)
        {
            leverAnimator.SetBool("isLeft", false);
            //spr.sprite = offLeverSp;
        }
        // off -> on
        else
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
            leverAnimator.SetBool("isLeft", true);
            //spr.sprite = onLeverSp;
        }

        isOn = !isOn;
    }

    public override void SetSize()
    {
        spr = this.GetComponent<SpriteRenderer>();
        base.size = spr.bounds.size;
    }
}
