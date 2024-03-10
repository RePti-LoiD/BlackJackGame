using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        print(Enum.Parse(typeof(BJStepState), "GetCard"));
    }
}