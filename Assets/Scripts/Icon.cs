using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Diagnostics;
using UnityEngine.UI;

public class Icon : MonoBehaviour, ISelectHandler
{
    public Image image;

    public RectTransform highlight;
    public Image exitButton;
    

    // C:/<Path To Steam>/Steam.exe -start -bigpicture
    public string applicationCommand = "";


    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        DOTween.KillAll();

        exitButton.color = Color.white;
        highlight.gameObject.SetActive(true);
        highlight.SetParent(transform);
        highlight.sizeDelta = new Vector2(240, 10);
        highlight.anchoredPosition = new Vector2(highlight.anchoredPosition.x, -180);
        
        highlight.DOAnchorPosX(0, 0.4f);
    }


    public void ChangeIcon(string iconPath)
    {
        if (iconPath != "")
        {
            if (!iconPath.StartsWith("C:")) { iconPath = Application.streamingAssetsPath + "/Icons/" + iconPath; }

            image.sprite = ExhibitUtilities.LoadSpriteFromFile(iconPath);
            image.color = Color.white;
        }
    }

    public void SetLaunchPath(string launchPath)
    {
        if (launchPath != "")
        {
            applicationCommand = launchPath;
        }
    }


    public void LaunchApplication()
    {
        if (applicationCommand != "")
        {
            Process.Start(applicationCommand);
        }
    }
}
