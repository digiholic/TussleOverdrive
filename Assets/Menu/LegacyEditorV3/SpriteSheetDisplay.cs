using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class SpriteSheetDisplay : MonoBehaviour
{
    [SerializeField] private Camera displayCamera;
    [SerializeField] private TextMeshProUGUI errorMessageText;

    private SpriteRenderer spriteRenderer;
    private BoxCollider collider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider>();
    }

    public void UpdateSpriteFilePath(string newFilePath)
    {
        string fullSpriteFilePath = FileLoader.GetFighterSubFilePath(LegacyEditorController.current.workingDirectoryName, newFilePath);
        FileInfo newspriteFile = new FileInfo(fullSpriteFilePath);
        if (newspriteFile.Exists)
        {
            errorMessageText.gameObject.SetActive(false);
            spriteRenderer.enabled = true;
            collider.enabled = true;
            UpdateSprite(FileLoader.LoadTexture(fullSpriteFilePath));
        } else
        {
            errorMessageText.gameObject.SetActive(true);
            spriteRenderer.enabled = false;
            collider.enabled = false;
            errorMessageText.text = "Could not load Sprite Sheet At " + fullSpriteFilePath;
        }
    }

    private void UpdateSprite(Texture2D sourceTexture)
    {
        Sprite s = Sprite.Create(sourceTexture, new Rect(0, 0, sourceTexture.width, sourceTexture.height), new Vector2(0.5f,0.5f));
        spriteRenderer.sprite = s;
        collider.size = s.bounds.size;
    }

    private bool dragging;
    private Vector3 clickPoint;
    
    private void Update()
    {
        if (CentralPanel.isHovered)
        {
            //Zoom
            if (Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            {
                displayCamera.orthographicSize -= Input.mouseScrollDelta.y * 0.5f;
                displayCamera.orthographicSize = Mathf.Clamp(displayCamera.orthographicSize, 3, 50);
            }
        }

        Ray ray = displayCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject == gameObject)
            {
                //Drag
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 pixelSpace = new Vector2(hit.point.x * spriteRenderer.sprite.pixelsPerUnit, hit.point.y * spriteRenderer.sprite.pixelsPerUnit);
                    dragging = true;
                    clickPoint = hit.point - transform.localPosition;
                }
            }
        }

        if (dragging && Input.GetMouseButton(0))
        {
            Vector3 newPosition = displayCamera.ScreenToWorldPoint(Input.mousePosition) - clickPoint;
            transform.localPosition = new Vector3(newPosition.x,newPosition.y,transform.localPosition.z);
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
    }

    //Draw boxes over each frame

    //Highlight boxes that are selected
}
