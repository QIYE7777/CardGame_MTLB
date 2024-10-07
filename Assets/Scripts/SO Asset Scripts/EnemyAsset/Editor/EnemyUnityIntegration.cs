using UnityEditor;

static class EnemyUnityIntegration
{
    [MenuItem("Assets/Create/EnemyAsset")]
    public static void CreateYourScriptableObject()
    {
        ScriptableObjectUtility.CreateAsset<EnemyAsset>();
    }
}
