using UnityEngine;
using UnityEngine.UI;

public class ToggleImage : MonoBehaviour
{
    public Toggle toggle;
    public Sprite image1;
    public Sprite image2;

    private bool isImage1Active = true;

    public void SwitchToggleImage()
    {
        if (isImage1Active)
        {
            toggle.image.sprite = image2;
        }
        else
        {
            toggle.image.sprite = image1;
        }

        isImage1Active = !isImage1Active;
    }

    public void SetImage1Active()
    {
        if (!isImage1Active)
        {
            toggle.image.sprite = image1;
            isImage1Active = true;
        }
    }

    public void SetImage2Active()
    {
        if (isImage1Active)
        {
            toggle.image.sprite = image2;
            isImage1Active = false;
        }
    }

    public bool Image1isActive()
    {
        return isImage1Active;
    }

}
