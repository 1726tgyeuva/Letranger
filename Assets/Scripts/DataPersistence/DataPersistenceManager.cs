using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

// 游戏数据管理器
// 用于管理游戏数据的存储和加载操作
// 用于把加载的游戏数据传递给其他脚本
public class DataPersistenceManager : MonoBehaviour
{
    // 用于在 Unity 编辑器中编辑储存设置
    // 调试版本使用
    [Header("存档设置")]
    [SerializeField] private string _fileName = "save";
    [SerializeField] private bool _useEncryption = true;

    // 游戏数据
    private GameData _gameData;
    // 数据对象列表
    private List<IDataPersistence> _dataPersistenceObjects;
    // 存档数据处理器
    private SaveHandler _saveHandler;

    // 单例模式
    // 确保全局只有一个游戏数据管理器
    // 可全局访问 但只能内部修改
    public static DataPersistenceManager instance { get; private set; }

    // Awake()在Start()之前调用
    // 用于对象自身初始化
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("在场景中发现了多个游戏数据管理器");
        }
        instance = this;
    }

    // Start()在Awake()之后调用
    // 用于对象之间的初始化
    void Start()
    {
        // 初始化文档数据处理器
        this._saveHandler = new SaveHandler(Application.persistentDataPath, _fileName, _useEncryption);

        // 初始化数据对象列表
        this._dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    // 初始化游戏数据
    public void NewGame()
    {
        this._gameData = new GameData();
    }

    public void LoadGame()
    {
        // 从文件中加载数据
        this._gameData = _saveHandler.Load();

        // 如果没有数据被加载，则跳过
        if (this._gameData == null)
        {
            Debug.Log("没有数据被加载，取消");
            NewGame();
        }

        // 将加载的数据传递给其他脚本
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(_gameData);
        }
    }

    public void SaveGame()
    {
        // 将数据传递给其他脚本
        foreach (IDataPersistence dataPersistenceObj in _dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(_gameData);
        }

        // 将数据保存到文件
        _saveHandler.Save(_gameData);
    }

    // 在游戏退出时确认是否存档
    private void OnApplicationQuit()
    {
        bool result = EditorUtility.DisplayDialog("退出保存", "即将退出游戏，是否保存？", "是", "否");
        if (!result)
            return;
        SaveGame();
    }

    // 查找所有的数据对象并返回列表
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> _dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(_dataPersistenceObjects);
    }
}
