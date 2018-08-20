using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    // 유저가 어디까지 진행했는지를 저장. ( 스테이지와 세이브포인트? )
    // 진행최대스테이지, 현재진행중스테이지, 현재진행중세이브포인트

    private int userClearStage = 0;
    private int userProcessingStage = 1;    // 최대 clearStage보다 1노ㅍ다.
    private Vector3 userSavePointVec3;

}
