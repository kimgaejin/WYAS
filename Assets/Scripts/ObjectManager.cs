using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *  존재이유:
 *  1.오브젝트를 저장 및 관리하기 위해
 */
 // 오브젝트가 필요한거. 위치/범위/무엇을 할지(움직일지)/서로충돌여부?

public class ObjectManager : MonoBehaviour {

    // for management List
    private Transform objs; // GameObject인 Map의 하위 오브젝트 transform.
    private Transform curObj;
    private List<Transform> objList = new List<Transform>();  // 상호작용 가능한 obj 모음.
    private List<Transform> collideList = new List<Transform>();    // 충돌중인 obj 모음
   
    // setting
    private float gravityScale = 9.8f;

    // refernce form other objects
    

    private void Awake()
    {
        InitializeList();
    }

    private void Update()
    {
        TestMoveObjects();
    }

    public Transform GetCurObj()
    {
        return curObj;
    }

    private void TestMoveObjects()
    {

        foreach (Transform t in objList)
        {
           // t.position += Vector3.down * 0.01f;
        }
    }

    private void InitializeList()
    {
        objs = GameObject.Find("Map").transform.FindChild("Objects");

        foreach (Transform t in objs) {
            objList.Add(t);
        }
    }
}
