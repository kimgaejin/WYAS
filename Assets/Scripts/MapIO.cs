using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml;

public class MapInfo
{
    public string type;
    public double posX;
    public double posY;
    public double posZ;
}

public sealed class MapIO {

    public static string mapName = "NoNamed";

    public void SaveMapChip()
    {
        List<MapInfo> mapList = new List<MapInfo>();

        GameObject Map = GameObject.Find("Map");
        Transform Tiles = Map.transform.FindChild("Tiles");
        foreach (Transform tile in Tiles)
        {
            MapInfo elem = new MapInfo() ;
            elem.type = "Tiles";
            elem.posX = tile.position.x;
            elem.posY = tile.position.y;
            elem.posZ = tile.position.z;
            mapList.Add(elem);
        }
        Transform Background = Map.transform.FindChild("Background");
        foreach (Transform bg in Background)
        {
            MapInfo elem = new MapInfo();
            elem.type = "Background";
            elem.posX = bg.position.x;
            elem.posY = bg.position.y;
            elem.posZ = bg.position.z;
            mapList.Add(elem);
        }
        Transform Objects = Map.transform.FindChild("Objects");
        foreach (Transform obj in Objects)
        {
            MapInfo elem = new MapInfo();
            elem.type = "Objects";
            elem.posX = obj.position.x;
            elem.posY = obj.position.y;
            elem.posZ = obj.position.z;
            mapList.Add(elem);
        }

        Write(mapList, Application.dataPath + "/DSJ/Map/" + mapName + ".xml");
    }


    public static void Write(List<MapInfo> mapList, string filePath)
    {
        System.Xml.XmlDocument Document = new System.Xml.XmlDocument();
        XmlElement mapListElement = Document.CreateElement("mapList");
        Document.AppendChild(mapListElement);

        foreach (MapInfo item in mapList)
        {
            XmlElement itemElement = Document.CreateElement("Item");

            // 원소 추가
            itemElement.SetAttribute("type", item.type);
            itemElement.SetAttribute("posX", item.posX.ToString());
            itemElement.SetAttribute("posY", item.posY.ToString());
            itemElement.SetAttribute("posZ", item.posZ.ToString());

            mapListElement.AppendChild(itemElement);
        }

        Document.Save(filePath);
    }

    public static List<MapInfo> Read(string filePath)
    {
        XmlDocument Document = new XmlDocument();
        Document.Load(filePath);
        XmlElement itemListElement = Document["itemList"];

        List<MapInfo> itemList = new List<MapInfo>();

        foreach (XmlElement itemElem in itemListElement.ChildNodes)
        {
            MapInfo item = new MapInfo();
            item.type = itemElem.GetAttribute("type");
            item.posX = System.Convert.ToDouble(itemElem.GetAttribute("posX"));
            item.posY = System.Convert.ToDouble(itemElem.GetAttribute("posY"));
            item.posZ = System.Convert.ToDouble(itemElem.GetAttribute("posZ"));
            itemList.Add(item);
        }

        return itemList;
    }
}
