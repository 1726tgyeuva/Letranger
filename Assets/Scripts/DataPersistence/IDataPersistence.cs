using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 游戏对象的数据导入导出接口
public interface IDataPersistence
{
    void LoadData(GameData data);

    // 不需要 ref 关键词
    // 在 C# 中，非基本类型会自动以引用的方式传递
    void SaveData(GameData data);
}
