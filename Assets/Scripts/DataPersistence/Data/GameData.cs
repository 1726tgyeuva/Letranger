using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 可序列化的游戏数据
[System.Serializable]
public class GameData
{
    // 一些范例数据
    // TODO: 需要根据游戏的需求来修改
    public int Level;
    public int Coin;
    public string Name;
    public Vector3 Position;

    // 默认值
    // 当没有数据加载时，游戏将以这些值开始
    public GameData()
    {
        this.Level = 1;
        this.Coin = 0;
        this.Name = "Default";
        this.Position = Vector3.zero;
    }
}
