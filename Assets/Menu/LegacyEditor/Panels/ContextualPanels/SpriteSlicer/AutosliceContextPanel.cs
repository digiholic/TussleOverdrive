using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AutosliceContextPanel : ContextualPanelData
{
    //These are used as property binding for UI elements
    public string frameWidthString, frameHeightString;
    public string offsetXString, offsetYString;
    public string pivotXString, pivotYString;
    public string ppuString;
    public bool generateAnimations;

    public void OnModelChanged()
    {
        //If this is still active but we're not looking at the sprites editor, deactivate it
        if (editor.leftDropdown != "Sprites")
        {
            DeactivatePanel();
        }
    }

    public override void FireContextualPanelChange()
    {
        BroadcastMessage("OnContextualPanelChanged", SendMessageOptions.DontRequireReceiver);

        //After the broadcast, clear all the "dirty" bits
    }


    void ChangedImageDef(ImageDefinition def)
    {
        if (def != null)
        {
            frameWidthString = def.Width.ToString();
            frameHeightString = def.Height.ToString();
            offsetXString = def.OffsetX.ToString();
            offsetYString = def.OffsetY.ToString();
            pivotXString = def.Pivot.xPos.ToString();
            pivotYString = def.Pivot.yPos.ToString();
            ppuString = def.PixelsPerUnit.ToString();
        }
    }

    public void executeSlice()
    {
        if (editor.currentImageFile == null)
        {
            AutoSliceAll();
        } else
        {
            SliceOne(editor.currentImageFile);
        }
    }

    private void SliceOne(FileInfo spriteImage)
    {
        Texture2D cachedTextureFile = FileLoader.LoadTexture(spriteImage.FullName);

        int xIndex = 0;
        int yIndex = 0;
        int currentXOffset = 0;
        int currentYOffset = 0;

        int xSpacing = int.Parse(offsetXString);
        int ySpacing = int.Parse(offsetYString);
        int width = int.Parse(frameWidthString);
        int height = int.Parse(frameHeightString);
        float pivotX = float.Parse(pivotXString);
        float pivotY = float.Parse(pivotYString);

        //Pixels per unit must be at least 1.
        int pixelsPerUnit = Mathf.Max(1, int.Parse(ppuString));

        //If height or width is zero, use the full image size instead
        if (width == 0) width = cachedTextureFile.width;
        if (height == 0) height = cachedTextureFile.height;

        //Go through the image vertically
        while (currentYOffset <= cachedTextureFile.height - height)
        {
            //Go through the image horizontally
            while (currentXOffset <= cachedTextureFile.width - width)
            {
                //Create the image definition
                ImageDefinition imageDef = new ImageDefinition();
                imageDef.SpriteFileName = spriteImage.Name;
                imageDef.ImageName = Path.GetFileNameWithoutExtension(spriteImage.Name) + "_" + (xIndex + yIndex);
                imageDef.OffsetX = currentXOffset;
                imageDef.OffsetY = currentYOffset;
                imageDef.Width = width;
                imageDef.Height = height;
                imageDef.PixelsPerUnit = pixelsPerUnit;
                imageDef.Pivot = new AnchorPointData("Pivot", pivotX, pivotY, AnchorPointData.RELATIVE);

                editor.loadedFighter.sprite_info.imageDefinitions.Add(imageDef);

                //Advance the horizontal counter
                currentXOffset += width + xSpacing;
                xIndex++;
            }

            //Advance the vertical counter
            currentYOffset += height + ySpacing;
            yIndex++;
        }
        LegacyEditorData.ChangedFighterData();
    }

    private void AutoSliceAll()
    {
        foreach (FileInfo spriteImage in editor.loadedFighter.sprite_info.spriteFiles)
        {
            SliceOne(spriteImage);
        }
    }

    public override void RegisterListeners() {
        editor.CurrentImageDefinitionChangedEvent += ChangedImageDef;
    }

    public override void UnregisterListeners() {
        editor.CurrentImageDefinitionChangedEvent -= ChangedImageDef;
    }
}
