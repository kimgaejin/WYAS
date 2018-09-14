using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSave : MonoBehaviour {

    public static int[] Stage_Level = new int[10]; //스테이지레벨 배열
    // Use this for initialization
    void Start () {
        for (int i = 0; i < 5; i++) {
            Stage_Level[i] = PlayerPrefs.GetInt("Stage_Level" + i);
        }

    }
	
	// Update is called once per frame
	void Update () {
     
    }
}
