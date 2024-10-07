using UnityEngine;

public class DontDestroyOnLoadBh : Singleton<DontDestroyOnLoadBh>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}