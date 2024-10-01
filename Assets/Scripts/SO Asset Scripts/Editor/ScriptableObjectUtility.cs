using UnityEngine;
using UnityEditor;

public static class ScriptableObjectUtility //被使用时会创建一个  New(外面传进来的类型).asset  在项目文件夹中
{
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();
        ProjectWindowUtil.CreateAsset(asset, "New" + typeof(T).Name + ".asset");
    }
}
