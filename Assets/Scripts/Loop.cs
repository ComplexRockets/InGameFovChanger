using Assets.Scripts;
using UnityEngine;

public class Loop : MonoBehaviour // a static object that's always active and updates the mod
{
    void Update() 
    {
        Mod.Instance.Update();
    }
}
