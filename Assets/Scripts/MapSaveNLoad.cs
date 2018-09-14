using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class ObjectDetailInfo
{
    public string key;
    public string type;
    public string [] spriteName;
    public double [] colorCode;
    public double [] posX;
    public double [] posY;
    public double [] posZ;
    public double [] scaleX;
    public double [] scaleY;
    public double [] rotationZ;
    public double [] value;
    public ObjectDetailInfo(string in_key, string in_type, string [] in_spriteName, double [] in_colorCode, double[] in_x, double[] in_y, double[] in_z
                            ,double [] in_scaleX, double [] in_scaleY, double [] in_rotationZ, double [] in_value)
    {
        key = in_key;
        type = in_type;
        spriteName = (string[])in_spriteName.Clone();
        colorCode = (double[])in_colorCode.Clone();
        posX = (double[])in_x.Clone();
        posY = (double[])in_y.Clone();
        posZ = (double[])in_z.Clone();
        scaleX = (double[])in_scaleX.Clone();
        scaleY = (double[])in_scaleY.Clone();
        rotationZ = (double[])in_rotationZ.Clone();
        value = (double[])in_value.Clone();
    }

}

public class MapSaveNLoad : MonoBehaviour {

    public List<ObjectDetailInfo> ObjectDetailInfoList = new List<ObjectDetailInfo>();

    public bool isSave = false;
    public bool isLoad = false;
    public string fileName = "stage1_1";
    private string filePath = "/Resources/Maps/";

    public void Start()
    {
        filePath += fileName;
        filePath += ".json";

        if (isSave) SaveTileInfo();
        if (isLoad) LoadTileInfo();
    }

    //===========================[SAVE]================

    public void SaveTileInfo()
    {
        string saveFile = "";
        saveFile += "{\"Map\":";

        #region: add elements into
        Transform Map = GameObject.Find("Map").transform;

        // json 특이사항: 배열 안에는 객체가 들어가야한다. 주의
        foreach (Transform category in Map)
        {
            foreach (Transform obj in category)
            {
                string key = category.name;

                string[] nameArr = new string[2];
                nameArr = obj.name.Split(" "[0]);

                int childSize = obj.GetChildCount();

                string[] sprName = new string[childSize + 1];
                double[] colorCode = new double[childSize + 1];
                double[] posX = new double[childSize + 1];
                double[] posY = new double[childSize + 1];
                double[] posZ = new double[childSize + 1];
                double[] scaleX = new double[childSize + 1];
                double[] scaleY = new double[childSize + 1];
                double[] rotationZ = new double[childSize + 1];
                double[] value = new double[0];

                try
                {
                    SpriteRenderer objSpr = obj.transform.GetComponent<SpriteRenderer>();
                    sprName[0] = objSpr.sprite.name;
                    Color objColor = objSpr.color;
                    colorCode[0] = 0.0;

                    colorCode[0] += (int)(objColor.r * 255);
                    colorCode[0] *= 0.001;
                   // colorCode[0] -= 0.001;

                    colorCode[0] += (int)(objColor.g*255) - 0.001;
                    colorCode[0] *= 0.001;
                  //  colorCode[0] -= 0.001;

                    colorCode[0] += (int)(objColor.b*255) - 0.001;
                    colorCode[0] *= 0.001;
                  //  colorCode[0] -= 0.001;

                    colorCode[0] += (int)(objColor.a*255) - 0.001;
                    colorCode[0] *= 0.001;
                   // colorCode[0] -= 0.001;
                }
                catch
                {
                    sprName[0] = "null";
                    colorCode[0] = 0.0f;
                }

                Vector3 pos = obj.transform.position;
                posX[0] = pos.x;
                posY[0] = pos.y;
                posZ[0] = pos.z;

                Vector3 scale = obj.transform.localScale;
                scaleX[0] = scale.x;
                scaleY[0] = scale.y;

                rotationZ[0] = obj.rotation.eulerAngles.z;

                for (int i = 1; i < childSize + 1; i++)
                {

                    Transform child = obj.GetChild(i - 1);
                    try
                    {
                        SpriteRenderer cObjSpr = child.transform.GetComponent<SpriteRenderer>();
                        if (cObjSpr == null) Debug.Log(nameArr[0].ToString()+" 스프라이트가 없었네");
                        sprName[i] = cObjSpr.sprite.name;

                        Color cObjColor = cObjSpr.color;
                        if (cObjColor == null) Debug.Log(nameArr[0].ToString() + " 컬러가 없었네");

                        colorCode[i] = 0.0;

                        colorCode[i] += (int)(cObjColor.r * 255);
                        colorCode[i] *= 0.001;
                       // colorCode[i] -= 0.001;

                        colorCode[i] += (int)(cObjColor.g * 255) - 0.001;
                        colorCode[i] *= 0.001;
                       // colorCode[i] -= 0.001;

                        colorCode[i] += (int)(cObjColor.b * 255) - 0.001;
                        colorCode[i] *= 0.001;
                        //colorCode[i] -= 0.001;

                        colorCode[i] += (int)(cObjColor.a * 255) - 0.001;
                        colorCode[i] *= 0.001;
                        //colorCode[i] -= 0.001;
                       // Debug.Log(nameArr[0].ToString() + "'s color" + colorCode[i]);
                    }
                    catch
                    {
                        sprName[i] = "null";
                        colorCode[i] = 0.0f;
                    }

                    Vector3 childPos = child.localPosition;
                    posX[i] = childPos.x;
                    posY[i] = childPos.y;
                    posZ[i] = childPos.z;

                    Vector3 childScale = child.localScale;
                    scaleX[i] = childScale.x;
                    scaleY[i] = childScale.y;

                    rotationZ[0] = child.rotation.eulerAngles.z;
                }
                ObjectDetailInfoList.Add(new ObjectDetailInfo(key, nameArr[0], sprName, colorCode, posX, posY, posZ, scaleX, scaleY, rotationZ, value));
            }

        }
        
        JsonData infoObjJson = JsonMapper.ToJson(ObjectDetailInfoList);
        saveFile += infoObjJson.ToString();
        saveFile += "}";
        ObjectDetailInfoList.Clear();
        
        File.WriteAllText(Application.dataPath + filePath, (saveFile));
        Debug.Log("세이브 완료");

        #endregion
       
    }

    public void LoadTileInfo()
    {
        Transform Map = GameObject.Find("Map").transform;

        if (File.Exists(Application.dataPath + filePath))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + filePath);

           // Debug.Log(jsonStr);

            JsonData jsonData = JsonMapper.ToObject(jsonStr);

            int categoryCount = jsonData["Map"].Count;

            for (int i = 0; i < categoryCount; i++)
            {
                string categoryName = jsonData["Map"][i]["key"].ToString();
                Transform tr_object;
                tr_object = Map.Find(categoryName);
                if (tr_object == null) tr_object = Map;

                string type = jsonData["Map"][i]["type"].ToString();

                GameObject obj = Resources.Load<GameObject>("Prefab/" + type);
                if (obj != null)
                {
                    float X = (float)(double)jsonData["Map"][i]["posX"][0];
                    float Y = (float)(double)jsonData["Map"][i]["posY"][0];
                    float Z = (float)(double)jsonData["Map"][i]["posZ"][0];
                    Vector3 Pos = new Vector3(X, Y, Z);
                    float scaleX = (float)(double)jsonData["Map"][i]["scaleX"][0];
                    float scaleY = (float)(double)jsonData["Map"][i]["scaleY"][0];
                    Vector3 scale = new Vector3(scaleX, scaleY, 0);
                    float rotationZ = (float)(double)jsonData["Map"][i]["rotationZ"][0];
                    string sprName = jsonData["Map"][i]["spriteName"][0].ToString();
                    double colorCode = (double)jsonData["Map"][i]["colorCode"][0];
                    Color objColor = new Color();

                  //  Debug.Log(type+" colorCode : "+ colorCode);

                    colorCode *= 1000;
                    objColor.a = (float)colorCode / 255;
                    colorCode -= (int)colorCode;

                    colorCode *= 1000;
                    objColor.b = (float)colorCode / 255;
                    colorCode -= (int)colorCode;

                    colorCode *= 1000;
                    objColor.g = (float)colorCode / 255;
                    colorCode -= (int)colorCode;

                    colorCode *= 1000;
                    objColor.r = (float)colorCode / 255;
                    colorCode -= (int)colorCode;

                    Sprite spr = Resources.Load<Sprite>("Sprites/" + sprName);
                     
                    Quaternion quat = new Quaternion();
                    quat.eulerAngles = new Vector3(0, 0, rotationZ);
                    GameObject insObj = Instantiate(obj, Pos, quat);

                    insObj.transform.localScale = scale;
                    if (sprName != "null")
                    {
                        SpriteRenderer insSpr = insObj.GetComponent<SpriteRenderer>();
                        insSpr.sprite = spr;
                        insSpr.color = objColor;
                    }

                    insObj.transform.parent = tr_object;

                    int childCount = jsonData["Map"][i]["posX"].Count;
                    for (int j = 1; j < childCount; j++) {
                        float cX = (float)(double)jsonData["Map"][i]["posX"][j];
                        float cY = (float)(double)jsonData["Map"][i]["posY"][j];
                        float cZ = (float)(double)jsonData["Map"][i]["posZ"][j];
                        Vector3 cPos = new Vector3(cX, cY, cZ);
                        float cScaleX = (float)(double)jsonData["Map"][i]["scaleX"][j];
                        float cScaleY = (float)(double)jsonData["Map"][i]["scaleY"][j];
                        Vector3 cScale = new Vector3(cScaleX, cScaleY, 0);
                        float cRotationZ = (float)(double)jsonData["Map"][i]["rotationZ"][j];
                        string cSprName = jsonData["Map"][i]["spriteName"][j].ToString();
                        
                        Sprite cSpr = Resources.Load<Sprite>("Sprites/" + cSprName);

                        Quaternion cQuat = new Quaternion();
                        quat.eulerAngles = new Vector3(0, 0, rotationZ);
                        //Transform childObj = insObj.transform.GetChild(j - 1);
                        Debug.Log("ObjectName: " + obj.name);
                        try
                        {
                            Transform childObj = insObj.transform.GetChild(j - 1);
                            childObj.localRotation = cQuat;

                            childObj.localScale = cScale;
                            if (cSprName != "null")
                            {
                                SpriteRenderer cInsSpr = childObj.GetComponent<SpriteRenderer>();
                                cInsSpr.sprite = cSpr;

                                double cColorCode = (double)jsonData["Map"][i]["colorCode"][j];
                                Color cObjColor = new Color();

                                cColorCode *= 1000;
                                cObjColor.a = (float)((int)cColorCode) / (float)255;
                                cColorCode -= (int)cColorCode;

                                cColorCode *= 1000;
                                cObjColor.b = (float)((int)cColorCode) / (float)255;
                                cColorCode -= (int)cColorCode;

                                cColorCode *= 1000;
                                cObjColor.g = (float)((int)cColorCode) / (float)255;
                                cColorCode -= (int)cColorCode;

                                cColorCode *= 1000;
                                cObjColor.r = (float)((int)cColorCode) / (float)255;
                                cColorCode -= (int)cColorCode;
                                cInsSpr.color = cObjColor;

                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    Debug.Log(type + "이 로드되지 못했습니다.");
                }
            }
         
            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("파일을 못찾았습니다.");
        }
    }
}