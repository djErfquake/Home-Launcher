using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Exit : MonoBehaviour, ISelectHandler
{
    public Image exitButton;
    public Image highlight;


    void ISelectHandler.OnSelect(BaseEventData eventData)
    {
        exitButton.color = highlight.color;
        highlight.gameObject.SetActive(false);
    }


    public void ExitApplication()
    {
        Application.Quit();
    }
}
