using UnityEngine;
using UnityEditor;

public static class ScriptableObjectUtility //��ʹ��ʱ�ᴴ��һ��  New(���洫����������).asset  ����Ŀ�ļ�����
{
    public static void CreateAsset<T>() where T : ScriptableObject
    {
        var asset = ScriptableObject.CreateInstance<T>();
        ProjectWindowUtil.CreateAsset(asset, "New" + typeof(T).Name + ".asset");
    }
}
