using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class Loop : MonoBehaviour
{
    void Update()
    {
        Mod.Instance.Update();
    }
}
