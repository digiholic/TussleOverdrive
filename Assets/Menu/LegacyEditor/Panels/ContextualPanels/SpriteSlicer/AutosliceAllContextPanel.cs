using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AutosliceAllContextPanel : ContextualPanelData
{
    public string frameWidthString, frameHeightString;
    public string offsetXString, offsetYString;
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

    public void AutoSliceAll()
    {
        foreach (FileInfo spriteImage in editor.loadedFighter.sprite_info.spriteFiles)
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
                    imageDef.PixelsPerUnit = 100;
                    Debug.Log(imageDef);
                    //TODO store this

                    //Advance the horizontal counter
                    currentXOffset += width + xSpacing;
                    xIndex++;
                }

                //Advance the vertical counter
                currentYOffset += height + ySpacing;
                yIndex++;
            }
            Debug.Log("Image contains " + xIndex + " subimages");
        }
    }

    public override void RegisterListeners() { }

    public override void UnregisterListeners() { }
}
