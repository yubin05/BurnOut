using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField] private MainCanvas mainCanvas;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        mainCanvas.Init();
    }
}
