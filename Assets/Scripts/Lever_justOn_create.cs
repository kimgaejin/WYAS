using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever_justOn_create : LeverParent {

    // * 레버를 on 됐을 때 오브젝트를 pointParent1의 각 point에서 드랍합니다
    // * 레버가 off 되어도 반응이 없습니다.
    // * 드랍하는 오브젝트가 낙사로 사라졌을 경우 레버를 on 시키면 다시 드랍합니다.
    // * 오브젝트가 2개가 되는 일은 없습니다.

    Transform dropPoint;

    private void Start()
    {
        SetGPHReference();
        GetObjectAndPointArray();

        if (leverHandler == null) Debug.Log("leverHandler is null");
        SetSize();

        foreach (Transform obj in objectParent)
        {
            obj.gameObject.SetActive(false);
        }

        try
        {
            dropPoint = pointParent.GetChild(0);
        }
        catch { Destroy(this); };
    }

    override public void DoInteracting()
    {
        // on -> off
        if (isSwtichOn)
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

                foreach (Transform obj in objectParent)
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
                    foreach (Transform pt in dropPoint)
                    {
                        objectParent.GetChild(ind).rotation = Quaternion.identity;
                        objectParent.GetChild(ind).position = pt.position;
                        objectParent.GetChild(ind).gameObject.SetActive(true);
                        ind++;
                    }
                }
            }
            catch { }
            leverAnimator.SetBool("isLeft", true);
        }

        isSwtichOn = !isSwtichOn;
    }

}
