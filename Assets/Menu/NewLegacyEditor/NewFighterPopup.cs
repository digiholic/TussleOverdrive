using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class NewFighterPopup : MonoBehaviour
{
    public string directoryName;
    public bool useSprite;
    public bool useModel;
    public bool generateActions;
    public string templateFighter;

    public void OnConfirm()
    {
        DirectoryInfo newDir = FileLoader.GetFighterDir(directoryName);
        if (!newDir.Exists)
        {
            DirectoryInfo createdDir = Directory.CreateDirectory(newDir.FullName);
            //Permission issues can possibly cause this directory to not exist even after we just made it so we gotta check again
            if (createdDir.Exists)
            {
                //Copy the template fighter if there is one
                if (templateFighter != null && templateFighter != "")
                {
                    FighterInfo templateInfo = FighterInfo.LoadFighterInfoFile(templateFighter);
                    if (templateInfo != null)
                    {
                        //TODO Copy fighter template
                    } else DisplayError("Could not find an existing Fighter named " + templateFighter + ", are you sure you used the Directory name?");
                } else
                {
                    FighterInfo newInfo = new FighterInfo();
                    newInfo.display_name = directoryName;
                    
                    if (generateActions)
                    {
                        //TODO Clone default actions here
                    } else
                    {
                        newInfo.action_file_path = "actions.json";
                    }

                    if (useSprite)
                    {
                        DirectoryInfo spriteDir = Directory.CreateDirectory(Path.Combine(createdDir.FullName, "sprites"));
                        File.Create(Path.Combine(spriteDir.FullName, "sprites.json")).Dispose();
                    }

                    if (useModel)
                    {
                        //TODO create model directory and stuff
                    }
                    newInfo.Save(Path.Combine(FileLoader.GetFighterDir(directoryName).FullName,"fighter_info.json"));
                    Debug.Log("Done");
                }
                FighterInfo newFighter = new FighterInfo();
            } else DisplayError("An issue occurred attempting to create the Fighter directory. Check your system permissions and try again.");
            
        }
        else DisplayError("A fighter already exists with that name");
    }

    public void DisplayError(string text)
    {
        Debug.LogError(text);
    }
}
