using UnityEngine;
using UnityEditor;

static class CardUnityIntegration
{
    [MenuItem("Assets/Create/CardAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<CardAsset>();
    }
}
