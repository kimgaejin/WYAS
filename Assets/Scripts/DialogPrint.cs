using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using System;

public class DialogPattern
{
    // excel to json 파일들은 모두 string으로만 저장된다.
    public int item;
    public string frame;
    public string contents;
    public string font;
    public int fontSize;
    public int waitTime;
    public int retainTime;
}

public class DialogPrint : MonoBehaviour {

    private string filePath = "/Resources/DialogJson/";
    private string preFileName = "Dlg_";
    private string extension = ".json";
    private JsonData jsonData;

    private GameObject PlayerDialog;
    private GameObject EchoDialog;
    private GameObject DirectionDialog;

    private int progress = 0;

    private Transform PlayerTrans;
    private Transform DialogBox;

    private Coroutine print;

    private void Start()
    {
        PlayerDialog = GameObject.Find("WorldSpaceDialogCanvas").transform.GetChild(0).gameObject;
        if (PlayerDialog == null) Debug.Log("PlayerDialog is null");
        EchoDialog = GameObject.Find("ScreenSpaceDialogCanvas").transform.GetChild(0).gameObject;
        if (EchoDialog == null) Debug.Log("EchoDialog is null");
        DirectionDialog = GameObject.Find("ScreenSpaceDialogCanvas").transform.GetChild(1).gameObject;
        if (DirectionDialog == null) Debug.Log("DirectionDialog is null");

        PlayerDialog.SetActive(false);
        EchoDialog.SetActive(false);
        DirectionDialog.SetActive(false);

        PlayerTrans = GameObject.Find("Player").transform;
        DialogBox = GameObject.Find("DialogBox").transform;

        LoadDialog("stage1_1");
    }

    private void Update()
    {
        int ind = 0;
        foreach (Transform item in DialogBox)
        {
            bool isActive = item.gameObject.activeInHierarchy;
            float distantance = Mathf.Sqrt(Mathf.Pow(item.position.x - PlayerTrans.position.x, 2) +
                                           Mathf.Pow(item.position.y - PlayerTrans.position.y, 2));
            bool isAdjoin = distantance < 1 * item.localScale.x;

            if (isActive && isAdjoin)
            {
                try
                {
                    StopCoroutine(print);
                    PlayerDialog.SetActive(false);
                    EchoDialog.SetActive(false);
                    DirectionDialog.SetActive(false);
                }
                catch { }

                print = StartCoroutine(PrintDetail(ind));
                item.gameObject.SetActive(false);
            }

            ind++;
        }

        try
        {
            PlayerDialog.transform.position = PlayerTrans.position + new Vector3(2.5f, 2, 0);
        }
        catch { }
    }

    private IEnumerator PrintDetail(int TransformInd)
    {
        WaitForSeconds wait01 = new WaitForSeconds(0.1f);
        int minJsonInd =0;   // minJsonInd: json 상에서 해당 대화의 첫번째 인덱스
        int maxJsonInd =jsonData.Count;
        int jsonInd = 0;    // jsonInd: json 상에서 해당 대화의 진행중인 인덱스

        while (true)
        {
            // toFindItem: jsonData[jsonInd].item
            int toFindItem = -1;
            for (minJsonInd = 0; minJsonInd < maxJsonInd; minJsonInd++) {

                //
                toFindItem = System.Convert.ToInt32(jsonData[minJsonInd]["item"].ToString());
                if (toFindItem == TransformInd)
                {
                    jsonInd = minJsonInd;
                    break; 
                }
               
            }

            //Debug.Log("jsonInd " + jsonInd);

            while (toFindItem == TransformInd)
            {
                DialogPattern dlgPtn = new DialogPattern();

                dlgPtn.item = toFindItem;
                dlgPtn.fontSize = System.Convert.ToInt32(jsonData[jsonInd]["fontSize"].ToString());
                dlgPtn.waitTime = System.Convert.ToInt32(jsonData[jsonInd]["waitTime"].ToString());
                dlgPtn.retainTime = System.Convert.ToInt32(jsonData[jsonInd]["retainTime"].ToString());

                dlgPtn.frame = (string)jsonData[jsonInd]["frame"].ToString();
                dlgPtn.font = (string)jsonData[jsonInd]["font"].ToString();
                dlgPtn.contents = (string)jsonData[jsonInd]["contents"].ToString();

                for (int i = 0; i < dlgPtn.waitTime* 10; i++)
                {
                    yield return wait01;
                }

                GameObject DialogUI = null;
                try
                {
                    if (dlgPtn.frame == "PLAYER")
                    {
                        DialogUI = PlayerDialog;
                    }
                    else if (dlgPtn.frame == "ECHO")
                    {
                        DialogUI = EchoDialog;
                    }
                    else if (dlgPtn.frame == "DIRECTION")
                    {
                        DialogUI = DirectionDialog;
                    }
                    else
                    {
                        Debug.Log("frame이 잘못설정된 dialog가 있습니다.");
                    }

                    DialogUI.SetActive(true);

                    Text dlgText;
                    dlgText = DialogUI.transform.GetChild(0).GetChild(0).GetComponent<Text>();

                    dlgText.text = dlgPtn.contents;

                    
                    
                }
                catch { }

                for (int i = 0; i < dlgPtn.retainTime * 10; i++)
                {
                    yield return wait01;
                }

                try
                {
                    DialogUI.SetActive(false);
                }
                catch { }
                jsonInd++;

                try
                {
                    toFindItem = System.Convert.ToInt32(jsonData[jsonInd]["item"].ToString());
                }
                catch
                {
                    toFindItem = -1;
                    break;
                }
            }

            yield break;
        }
    }

    private void LoadDialog(string mapName)
    {
        mapName = "stage1_1";

        string fileName = filePath + preFileName + mapName + extension;
        Debug.Log(Application.dataPath+""+fileName);
        if (File.Exists(Application.dataPath + fileName))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + fileName);
            jsonData = JsonMapper.ToObject(jsonStr);
        }
        else
        {
            Debug.Log("LoadDialog is not exist");
        }
    }
}
