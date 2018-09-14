using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

/*
 * 3중 상속(혹은 이후를 생각하면 그 이후)까지 저장해야합니다.
 * 
 * 저장시]
 * 객체의 하위 하위 자식까지 모두 저장합니다.
 * 
 * 로드시]
 * 1. 큰 객체 생성
 * 2. 자식 객체 위치 조정
 * 3. if 자식 객체가 저장된 수보다 적다면, 찾아내 생성
 * 4. 자식 객체의 자식 객체 위치 조정
 * ...
 */

public class ObjectFile
{
    public string parentName;
    public string objectName;
    public string spriteName;
    public double colorCode;
    public double posX;
    public double posY;
    public double posZ;
    public double scaleX;
    public double scaleY;
    public double rotationZ;
    public ObjectFile [] child;
    public ObjectFile(string in_parentName, string in_objectName, string in_spriteName, double in_colorCode, double in_x, double in_y, double in_z
                      , double in_scaleX, double in_scaleY, double in_rotationZ, ObjectFile[] in_child)
    {
        parentName = in_parentName;
        objectName = in_objectName;
        spriteName = in_spriteName;
        colorCode = in_colorCode;
        posX = in_x;
        posY = in_y;
        posZ = in_z;
        scaleX = in_scaleX;
        scaleY = in_scaleY;
        rotationZ = in_rotationZ;
        child = (ObjectFile[])in_child.Clone();
        //rotationZ = (double[])in_rotationZ.Clone();
    }

}

public class MapSaveScript : MonoBehaviour
{

    public List<ObjectFile> ObjectFileList = new List<ObjectFile>();

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
        saveFile += "{\"key\":";

        #region: add elements into
        Transform Map = GameObject.Find("Map").transform;

        // json 특이사항: 배열 안에는 객체가 들어가야한다. 주의
        foreach (Transform category in Map)
        {
            foreach (Transform obj in category)
            {
                ObjectFileList.Add(ConvertToObjectFileFromHierarchy(obj));
            }

        }

        JsonData infoObjJson = JsonMapper.ToJson(ObjectFileList);
        saveFile += infoObjJson.ToString();
        saveFile += "}";
        ObjectFileList.Clear();

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

            JsonData jsonData = JsonMapper.ToObject(jsonStr);

            try
            {
                int i = 0;
                while (i < 1000)
                {
                    ConvertToObjectFormObjectFile(Map.FindChild(jsonData["key"][i]["parentName"].ToString()), null, ConverToObjectFileFromJSON(jsonData["key"][i]));
                    i++;
                }
                Debug.Log("로드하는 파일이 1000개를 넘어갑니다.");
            }
            catch { }
            

            Debug.Log("로드 완료");
        }
        else
        {
            Debug.Log("파일을 못찾았습니다.");
        }
    }

    public ObjectFile ConvertToObjectFileFromHierarchy(Transform item)
    {
        string in_parentName = item.parent.name;
        string in_objectName = item.name.Split(" "[0])[0];
        string in_spriteName;
        double in_colorCode;
        double in_posX;
        double in_posY;
        double in_posZ;
        double in_scaleX;
        double in_scaleY;
        double in_rotationZ;
        ObjectFile[] in_child;

        // save sprite & color
        try
        {
            SpriteRenderer item_spr = item.transform.GetComponent<SpriteRenderer>();
            in_spriteName = item_spr.sprite.name;
            Color itemColor = item_spr.color;
            in_colorCode = 0.0;

            in_colorCode += (int)(itemColor.r * 255);
            in_colorCode *= 0.001;

            in_colorCode += (int)(itemColor.g * 255) - 0.001;
            in_colorCode *= 0.001;

            in_colorCode += (int)(itemColor.b * 255) - 0.001;
            in_colorCode *= 0.001;

            in_colorCode += (int)(itemColor.a * 255) - 0.001;
            in_colorCode *= 0.001;
        }
        catch
        {
            in_spriteName = "null";
            in_colorCode = 0.0f;
        }

        // position & scale & rotation
        Vector3 item_position = item.transform.localPosition;
        in_posX = item_position.x;
        in_posY = item_position.y;
        in_posZ = item_position.z;

        Vector3 item_scale = item.transform.localScale;
        in_scaleX = item_scale.x;
        in_scaleY = item_scale.y;

        in_rotationZ = item.rotation.eulerAngles.z;

        int childSize = item.GetChildCount();
        in_child = new ObjectFile[childSize];
        for (int i = 0; i < childSize; i++)
        {
            Transform item_child = item.GetChild(i);
            in_child[i] = ConvertToObjectFileFromHierarchy(item_child);
        }
        ObjectFile convertedObject = new ObjectFile(in_parentName, in_objectName, in_spriteName, in_colorCode, in_posX, in_posY, in_posZ
                      , in_scaleX, in_scaleY, in_rotationZ, in_child);
        return convertedObject;
    }

    public ObjectFile ConverToObjectFileFromJSON(JsonData objJson)
    {
        string in_parentName = objJson["parentName"].ToString();
        string in_objectName = objJson["objectName"].ToString();
        string in_spriteName = objJson["spriteName"].ToString();
        double in_colorCode = (double)objJson["colorCode"];
        double in_posX = (double)objJson["posX"];
        double in_posY = (double)objJson["posY"];
        double in_posZ = (double)objJson["posZ"];
        double in_scaleX = (double)objJson["scaleX"];
        double in_scaleY = (double)objJson["scaleY"];
        double in_rotationZ = (double)objJson["rotationZ"];
        int childCount = objJson["child"].Count;
        ObjectFile[] in_child = new ObjectFile[childCount];
        for (int i = 0; i < childCount; i++)
        {
            in_child[i] = ConverToObjectFileFromJSON(objJson["child"][i]);
        }
        ObjectFile objFile = new ObjectFile(in_parentName, in_objectName, in_spriteName, in_colorCode, in_posX, in_posY, in_posZ
                      , in_scaleX, in_scaleY, in_rotationZ, in_child);
        return objFile;
    }

    public void ConvertToObjectFormObjectFile(Transform in_parent, Transform target, ObjectFile objFile)
    {
        // 정보입력
        string objectName = objFile.objectName;
        string parentName = objFile.parentName;
        float X = (float)objFile.posX;
        float Y = (float)objFile.posY;
        float Z = (float)objFile.posZ;
        Vector3 Pos = new Vector3(X, Y, Z);
        float scaleX = (float)objFile.scaleX;
        float scaleY = (float)objFile.scaleY;
        Vector3 scale = new Vector3(scaleX, scaleY, 0);
        float rotationZ = (float)objFile.rotationZ;
        string sprName = objFile.spriteName;
        double colorCode = (double)objFile.colorCode;

        Quaternion quat = new Quaternion();
        quat.eulerAngles = new Vector3(0, 0, rotationZ);

        SpriteRenderer target_spr = null;
        GameObject insObj = null;

        if (target == null)
        {
            try
            {
                GameObject objectPrefab = Resources.Load<GameObject>("Prefab/" + objectName);
                
                insObj = Instantiate(objectPrefab, Pos, quat);
                target_spr = insObj.GetComponent<SpriteRenderer>();
                if (in_parent != null)
                {
                    insObj.transform.parent = in_parent;
                }
            }
            catch
            {
                Debug.Log("맵 로드 도중 "+ parentName+"의 자식인" + objectName + " 의 prefab을 조정할 수 없습니다..");
            }
        }
        else
        {
            target_spr = target.GetComponent<SpriteRenderer>();
            insObj = target.gameObject;
            insObj.transform.position = Pos;
            insObj.transform.rotation= quat;
        }

        if (sprName != "null")
        {
            try
            {
                Sprite spr = Resources.Load<Sprite>("Sprites/" + sprName);
                Color objColor = new Color();
                target_spr.sprite = spr;
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

                target_spr.color = objColor;
            }
            catch { }
        }

        try
        {
            int i = 0;
            while (i < 100)
            {
                ConvertToObjectFormObjectFile(insObj.transform, insObj.transform.GetChild(i), objFile.child[i]);
                i++;
            }
        }
        catch { }


    }
}