using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveAndLoad : MonoBehaviour
{
    // 玩家的游戏数据，包括角色数据，背包数据，任务数据等
    public GameData GameData;
    // 存档文件夹路径
    string SavePath = Application.persistentDataPath + "/Saves";

    public void Save()
    {
        // 如果没有存档文件夹，创建一个
        if (!Directory.Exists(SavePath))
        {
            Directory.CreateDirectory(SavePath);
        }
        // 用于存档文件二进制序列化
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(SavePath + "/Save.dat");

        var jsonData = JsonUtility.ToJson(GameData);
        formatter.Serialize(stream, jsonData);
        stream.Close();
    }

    public void Load()
    {
        // 用于存档文件二进制反序列化
        BinaryFormatter formatter = new BinaryFormatter();

        // 如果存档文件存在，读取存档文件
        if (File.Exists(SavePath + "/Save.dat"))
        {
            FileStream stream = new FileStream(SavePath + "/Save.dat");
            JsonUtility.FromJsonOverwrite((string) formatter.Deserialize(stream), GameData);
            stream.Close();
        }
        else
        {
            Debug.Log("存档文件不存在");
        }
    }
}