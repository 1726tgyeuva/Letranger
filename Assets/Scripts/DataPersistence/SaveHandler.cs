using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

// 存档处理器
public class SaveHandler
{
    private string _savePath = "";
    private string _fileName = "";
    private bool _useEncryption = false;
    private readonly string _fileExtension = ".dat";
    private readonly string encryptionKey = "Letranger";

    public SaveHandler(string savePath, string fileName, bool useEncryption)
    {
        this._savePath = savePath;
        this._fileName = fileName;
        this._useEncryption = useEncryption;
    }

    public GameData Load()
    {
        // 用 Path.Combine 去适应不同的操作系统
        string fullPath = Path.Combine(_savePath, _fileName + _fileExtension);
        GameData loadedData = null;

        if (File.Exists(fullPath))
        {
            try
            {
                // 从文件中读取（加密/未加密的）序列化数据
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // 如果数据加密了要解密
                if (_useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // 从 Json 中反序列化数据到 C# 对象
                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("读档时发生错误: " + fullPath + "\n" + e);
            }
        }
        return loadedData;
    }

    public void Save(GameData data)
    {
        // 用 Path.Combine 去适应不同的操作系统
        string fullPath = Path.Combine(_savePath, _fileName + _fileExtension);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            // 序列化数据到 Json
            string dataToSave = JsonUtility.ToJson(data);

            // 是否需要加密数据
            if (_useEncryption)
            {
                dataToSave = EncryptDecrypt(dataToSave);
            }

            // 写入数据到文件
            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("存档时发生错误: " + fullPath + "\n" + e);
        }
    }

    // XOR 加密/解密
    private string EncryptDecrypt(string data)
    {
        string encryptedData = "";
        for (int i = 0; i < data.Length; i++)
        {
            encryptedData += (char) (data[i] ^ encryptionKey[i % encryptionKey.Length]);
        }
        return encryptedData;
    }
}
