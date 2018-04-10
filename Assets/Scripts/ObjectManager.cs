using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class ObjectManager : MonoBehaviour {

    private Transform objs; // GameObject인 Map의 하위 오브젝트 transform.
    private List<Transform> objList = new List<Transform>();  // 상호작용한 obj 모음.

    private void Awake()
    {
        InitializeList();
    }

    private void Update()
    {
        TestMoveObjects();
    }

    private void TestMoveObjects()
    {

        foreach (Transform t in objList)
        {
            t.position += Vector3.right * 0.01f;
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
