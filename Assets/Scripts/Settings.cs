using NiceJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("Components")]
    public Image backgroundImage;
    private List<Sprite> backgroundSprites;
    private int backgroundIndex = 0;
    private float backgroundWaitTime = 60000;
    public Image highlight;
    public Image exitButton;
    public GameObject iconPrefab;


    private Color defaultBackgroundColor;
    private Color defaultHightlightColor;


    private JsonNode settings;


    private void Start()
    {
        defaultBackgroundColor = backgroundImage.color;
        defaultHightlightColor = highlight.color;


        settings = ConfigHelper.Load(Application.streamingAssetsPath + "/settings.json");

        ApplySettings();
    }


    private void ApplySettings()
    {
        // set background image(s)
        if (settings.ContainsKey("background") && settings["background"].ContainsKey("images"))
        {
            float rotateBackgroundMinutes = settings["background"]["rotate-minutes"];
            backgroundWaitTime = rotateBackgroundMinutes * 60f;
            Debug.Log("Rotating background pictures every " + backgroundWaitTime + " seconds.");

            backgroundSprites = new List<Sprite>();
            JsonArray backgroundSettings = settings["background"]["images"] as JsonArray;
            for (int i = 0; i < backgroundSettings.Count; i++)
            {
                backgroundSprites.Add(ExhibitUtilities.LoadSpriteFromFile(backgroundSettings[i]));
            }

            backgroundSprites.ShuffleList();
            backgroundImage.sprite = backgroundSprites[0];
            backgroundImage.color = Color.white;

            StartCoroutine(NextBackgroundImage());
        }
        else
        {
            backgroundImage.color = defaultBackgroundColor;
        }

        // set highlight color
        Color highlightColor = defaultHightlightColor;
        ColorUtility.TryParseHtmlString(settings["highlight-color"], out highlightColor);
        highlight.color = highlightColor;
        exitButton.color = highlightColor;


        // set app icons
        JsonArray appSettings = settings["apps"] as JsonArray;
        for (int i = 0; i < appSettings.Count; i++)
        {
            GameObject iconObject = Instantiate(iconPrefab);
            iconObject.transform.SetParent(this.transform);

            Icon iconSettings = iconObject.GetComponent<Icon>();
            iconSettings.ChangeIcon(appSettings[i]["icon-image"]);
            iconSettings.SetLaunchPath(appSettings[i]["launch-path"]);
            iconSettings.highlight = highlight.GetComponent<RectTransform>();
            iconSettings.exitButton = exitButton;

            if (i == 0) { iconObject.GetComponent<Button>().Select(); }
        }
    }



    private IEnumerator NextBackgroundImage()
    {
        while (true)
        {
            backgroundIndex++; if (backgroundIndex >= backgroundSprites.Count) { backgroundIndex = 0; }
            backgroundImage.sprite = backgroundSprites[backgroundIndex];

            yield return new WaitForSeconds(backgroundWaitTime);
        }
    }

}
