using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public GameObject icon;

    public void OnDeselect(BaseEventData eventData)
    {
        icon.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        icon.SetActive(true);
    }
}
