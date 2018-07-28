using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class TileInfo
{
    public string type;
    public double posX;
    public double posY;
    public double posZ;
    public double rotationZ;

    public TileInfo(string in_type, double in_x, double in_y, double in_z, double in_rZ)
    {
        type = in_type;
        posX = in_x;
        posY = in_y;
        posZ = in_z;
        rotationZ = in_rZ;
    }
}

public class MapSaveNLoad : MonoBehaviour {

    public GameObject FloorTile01;
    public List<TileInfo> tileList = new List<TileInfo>();

    public string fileName = "stage1_1";
    private string filePath = "/Resources/Maps/";

    public void Start()
    {
        filePath += fileName;
        filePath += ".json";

       // SaveTileInfo();
        LoadTileInfo();
    }

    public void SaveTileInfo()
    {
        #region: add elements into
        //tileList.Add(new TileInfo());
        GameObject Map = GameObject.Find("Map");
        Transform Tiles = Map.transform.FindChild("Tiles");
        foreach (Transform tile in Tiles)
        {
            string[] nameArr = new string[2];
            nameArr = tile.name.Split(" "[0]);
            tileList.Add(new TileInfo(nameArr[0], tile.position.x, tile.position.y, tile.position.z, tile.rotation.ToEuler().z));
        }
        #endregion

        //JsonData infoJson = JsonMapper.ToJson(tileList);
        JsonData infoJson = JsonMapper.ToJson(tileList);
        File.WriteAllText(Application.dataPath + filePath, ("\"Tile\":" + infoJson.ToString()));

        Debug.Log("세이브 완료");
    }

    public void LoadTileInfo()
    {
        Debug.Log(Application.dataPath + filePath);
        if (File.Exists(Application.dataPath + filePath))
        {
            string jsonStr = File.ReadAllText(Application.dataPath + filePath);

            Debug.Log(jsonStr);

            JsonData tileData = JsonMapper.ToObject(jsonStr);
           
            for (int i = 0; i < tileData["Tile"].Count; i++)
            {
                //Debug.Log(tileData[i]["Tile"][0]["type"].ToString());
                Debug.Log(tileData["Tile"][0]["type"].ToString());
                string tileType = tileData["Tile"][i]["type"].ToString();
                float tileX = (float)(double)tileData["Tile"][i]["posX"];
                float tileY = (float)(double)tileData["Tile"][i]["posY"];
                float tileZ = (float)(double)tileData["Tile"][i]["posZ"];
                Vector3 tilePos = new Vector3(tileX, tileY, tileZ);
                if (tileType == "FloorTile01")
                {
                    Instantiate(FloorTile01,tilePos, Quaternion.identity);
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