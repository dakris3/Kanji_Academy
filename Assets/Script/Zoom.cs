using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Zoom : MonoBehaviour
{
    private Sprite sprite;
public Image images;

public void ButtonClick()
{
    GameObject clickedObj = EventSystem.current.currentSelectedGameObject;

    if (clickedObj != null)
    {
        Image currentImage = clickedObj.GetComponent<Image>();
        if (currentImage == null)
        {
            Debug.Log("Image not found");
            return;
        }

        images.sprite = currentImage.sprite;
    }
}

}
