using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class SpriteSheetDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private Image image;
    [SerializeField] private UIGridRenderer grid;

    private RectTransform rect;

    public float scrollFactor;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void UpdateSpriteFilePath(string newFilePath)
    {
        string fullSpriteFilePath = FileLoader.GetFighterSubFilePath(LegacyEditorController.current.workingDirectoryName, newFilePath);
        FileInfo newspriteFile = new FileInfo(fullSpriteFilePath);
        if (newspriteFile.Exists)
        {
            errorMessageText.gameObject.SetActive(false);
            image.enabled = true;
            grid.enabled = true;

            UpdateSprite(FileLoader.LoadTexture(fullSpriteFilePath));
        }
        else
        {
            errorMessageText.gameObject.SetActive(true);
            image.enabled = false;
            grid.enabled = false;

            errorMessageText.text = "Could not load Sprite Sheet At " + fullSpriteFilePath;
        }
    }

    private void UpdateSprite(Texture2D sourceTexture)
    {
        Sprite s = Sprite.Create(sourceTexture, new Rect(0, 0, sourceTexture.width, sourceTexture.height), new Vector2(0.5f,0.5f));
        image.sprite = s;
        rect.sizeDelta = new Vector2(sourceTexture.width, sourceTexture.height);
    }


    private bool dragging;
    private Vector3 lastScreenPoint;

    private void Update()
    {
        if (CentralPanel.isHovered)
        {
            if (Input.mouseScrollDelta.y != 0) zoomSheet(Input.mouseScrollDelta.y);

            if (Input.GetMouseButtonDown(0))
            {
                dragging = true;
                lastScreenPoint = Input.mousePosition;
            }
        }

        if (dragging && Input.GetMouseButton(0))
        {
            Vector2 currentPosition = rect.position;
            Vector3 diff = Input.mousePosition - lastScreenPoint;
            currentPosition.x += diff.x;
            currentPosition.y += diff.y;
            rect.position = currentPosition;
            lastScreenPoint = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }

    private void zoomSheet(float delta)
    {
        float xScale = rect.sizeDelta.x;
        float yScale = rect.sizeDelta.y;
        xScale = Mathf.Clamp(xScale + delta * scrollFactor, 10, 2000);
        yScale = Mathf.Clamp(yScale + delta * scrollFactor, 10, 2000);
        rect.sizeDelta = new Vector2(xScale, yScale);
    }
}
