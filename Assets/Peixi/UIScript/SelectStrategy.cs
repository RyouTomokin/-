using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectStrategy : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public Text detailText;
    public string detail;
    Button button;

    private void Start()
    {
        button = GetComponent<Button>();
    }
    private void OnMouseDown()
    {
        print("Select"); 
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        detailText.enabled = true;
        detailText.text = detail;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        detailText.enabled = false;
        detailText.text = "";
    }
}
