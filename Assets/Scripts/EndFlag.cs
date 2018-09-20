using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ClearFlag - EndPoint - EndFlag 에 들어가는 스크립트입니다.
 * 플레이어와 충돌시, 플레이어를 다음 StartFlag로 넘어가게합니다.
 * 그러기 위해서 자신이 몇 번째 EndFlag인지 알아야합니다.
 * 다음 StartFlag가 없다면, 다음 스테이지로 넘어가야합니다.
 * 
 * - 자신이 몇 번째 EndFlag인지 알기 위해서, ClearFlag에서 int SetSequence()를 통해 자신의 자식들에게 번호를 정해줍니다..
 * - 다음 StartFlag로 넘어갈땐 페이드아웃, 페이드인을 합니다.
 * 
 *  
        //Stage_Level[0] == 2 는 1스테이지에 2까지 깻다는 뜻 
        //바로 몇스테이지 깻는지 인자를 전달 받을수 잇는경우 
        //스테이지 i-k를 깻다고 하면 (배열 편의상 0부터 시작)
        LevelSave.Stage_Level[i] == k; // i+1 번째 레벨에 k 스테이지 까지 깬걸 저장
        PlayerPrefs.SetInt("Stage_Level" + i, LevelSave.Stage_Level[i]); // 레지스트리에 저장
 */

public class EndFlag : MonoBehaviour {

    private int stage;
    private Vector3 nextStartPos;
    private bool isThrough = false;

    private ClearFlag clearFlag;

    private void Start()
    {
        clearFlag = transform.parent.parent.GetComponent<ClearFlag>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌");
        if (other.transform.tag == "PLAYER")
        {
            ReachThisPoint();
        }
    }
    
    public void SetSequence(int in_stage)
    {
        stage = in_stage;
    }

    private void ReachThisPoint()
    {
        if (isThrough == true)
            return;

        Debug.Log("알려도라");
        clearFlag.ClaerStage(stage);
        isThrough = true;
    }

}
