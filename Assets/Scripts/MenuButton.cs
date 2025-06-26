using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI targetTxt;

    // 클릭했을 때 발생하는 이벤트
    [SerializeField] private UnityEvent onClick;

    // 터치 포인터가 영역 안을 클릭했을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("들어옴");
        targetTxt.color = Color.red;
    }
    // 터치 포인터가 영역 밖으로 나갔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("나감");
        targetTxt.color = Color.white;
    }

    // 영역 안을 터치했을 때
    public void OnPointerDown(PointerEventData eventData)
    {
    }
    // 영역 안을 터치하고 떼었을 때
    public void OnPointerUp(PointerEventData eventData)
    {
        onClick?.Invoke();
    }

    private void OnDisable()
    {
        targetTxt.color = Color.white;
    }
}
