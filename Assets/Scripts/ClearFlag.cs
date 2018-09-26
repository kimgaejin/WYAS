using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ClearFlag : MonoBehaviour {

    private Transform playerTrans;
    private Image fadeImg;

    private List<Transform> startList;
    private List<Transform> endList;
    private int stageLen;

    // ########################################### 지금 몇 챕터인지 받아오는게 필요
    private int curChapter = 0;

    private float fadeOutTime = 3.0f;
    private float fadeInTime = 3.0f;

    private GameObject fade;
    private Coroutine fadeOut;

    private void Start()
    {
        playerTrans = GameObject.Find("Player").transform;
        fade = GameObject.Find("Fade");
        if (fade == null)
        {
            fade = GameObject.Find("ScreenSpaceEffectCanvas").transform.GetChild(0).gameObject;
        }
        fadeImg =fade.transform.GetChild(0).GetComponent<Image>();
        if (fadeImg == null) Debug.Log("fadeImg is not exist");
        // ######################################## 지금 몇 챕터인지 받아오는게 필요
        string sceneName = EditorApplication.currentScene;
        int sceneNameLen = sceneName.Length;
        sceneName =sceneName.Substring(sceneNameLen - 3, 2);
        if (int.TryParse(sceneName, out curChapter) == false) {
            Debug.Log("curChpater: " + curChapter);
            curChapter = 0;
        }
        SetClearFlagChild();
        ClearInit();
    }

    private void SetClearFlagChild()
    {
        startList = new List<Transform>();
        endList = new List<Transform>();

        foreach (Transform item in transform.FindChild("StartPoint"))
        {
            startList.Add(item);
        }
        foreach (Transform item in transform.FindChild("EndPoint"))
        {
            endList.Add(item);
        }

        int srtLen = startList.Count;
        stageLen = endList.Count;
        if (srtLen == 0) Debug.Log("not exist start point at least one");
        if (srtLen != stageLen) Debug.Log("not equal startFlag size and endFlag");

        for (int i = 0; i < stageLen; i++)
        {
            endList[i].GetComponent<EndFlag>().SetSequence(i);
        }

    }

    private void ClearInit()
    // 이 Chapter에 처음 들어왔으니 stage 0을 깬 셈 칩니다.
    {
        try
        {
            playerTrans.position = startList[0].transform.position;
            
            Color color;
            color = fadeImg.color;
            color.a = 1.0f;
            fadeImg.color = color;
            
            try
            {
                StopCoroutine(fadeOut);
            }
            catch { }
            fadeOut = StartCoroutine(FadeOut(false));

            LevelSave.Stage_Level[curChapter] = 0;
            PlayerPrefs.SetInt("Stage_Level" + curChapter, LevelSave.Stage_Level[curChapter]);
        }
        catch
        {
            Debug.Log("ClearInit Something wrong");
        }
    }

    public void ClaerStage(int stage)
    // EndFlag가 호출합니다.
    {
        // if 클리어 이펙트가 있다면 이곳에
        try
        {
            StopCoroutine(fadeOut);
        }
        catch { }
        StartCoroutine(ClearStageCor(stage));

    }

    IEnumerator FadeOut(bool oper)
    {
        WaitForSeconds wait0025 = new WaitForSeconds(0.025f);
        float fadeTime;
        int sign;
        float inc;
        float finalColorA;
         
        // fadeOut a: 0->1
        if (oper == true)
        {
            fadeTime = fadeOutTime;
            sign = 1;
            finalColorA = 1;
        }
        // fadeIn a: 1->0
        else
        {
            fadeTime = fadeInTime;
            sign = -1;
            finalColorA = 0;
        }
        inc = (fadeTime * 40)/255.0f;

        Color fadeColor = fadeImg.color;

        while (true)
        {

            for (int i = 0; i < fadeTime * 40; i++)
            {

                try
                {
                    fadeColor.a = Mathf.Abs(finalColorA-1) + (float)i / 40* (sign * inc);
                    // Debug.Log("fadeColor.a: " + fadeColor.a);
                }
                catch
                {
                    break;
                }
                fadeImg.color = fadeColor;
                yield return wait0025;
            }

            fadeColor.a = finalColorA;
            fadeImg.color = fadeColor;
            yield break;
        }
    }

    IEnumerator ClearStageCor(int stage)
    {
        WaitForSeconds waitFadeIn = new WaitForSeconds(fadeInTime);
        WaitForSeconds wiatFadeOut = new WaitForSeconds(fadeOutTime);

        while (true)
        {
            try
            {
                StopCoroutine(fadeOut);
            }
            catch { }

            fadeOut = StartCoroutine(FadeOut(true));
            yield return waitFadeIn;

            LevelSave.Stage_Level[curChapter] = stage;
            PlayerPrefs.SetInt("Stage_Level" + curChapter, LevelSave.Stage_Level[curChapter]);

            // 페이드 아웃 이후

            if (stage == stageLen - 1)
            {
                // 다음 챕터로

                yield break;
            }

            //  플레이어 위치변환
            playerTrans.position = startList[stage + 1].transform.position;

            // 페이드 인
            try { StopCoroutine(fadeOut); }
            catch { }
            fadeOut = StartCoroutine(FadeOut(false));
            yield return wiatFadeOut;

            yield break;
        }
    }

}
