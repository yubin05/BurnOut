using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ActionButton : MonoBehaviour
{
    protected Button m_button;

    protected void Awake()
    {
        m_button = GetComponent<Button>();
    }
    // temp
    protected void Start()
    {
        Init();
    }

    protected abstract void Init();
}
