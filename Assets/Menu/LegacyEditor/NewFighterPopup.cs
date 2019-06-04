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

    [SerializeField]
    private PopupErrorLabel errorLabel;

    public void OnConfirm()
    {
        if (!directoryName.Equals(""))
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
                        }
                        else DisplayError("Could not find an existing Fighter named " + templateFighter + ", are you sure you used the Directory name?");
                    }
                    else
                    {
                        FighterInfo newInfo = new FighterInfo();
                        ActionFile newActions = new ActionFile();
                        newInfo.display_name = directoryName;
                        newInfo.directory_name = directoryName;
                        newInfo.action_file_path = "fighter_actions.json";
                        newInfo.GenerateDefaultAttributes();
                        if (generateActions)
                        {
                            //TODO Clone default actions here
                        }
                        newActions.Save(Path.Combine(createdDir.FullName, "fighter_actions.json"));

                        if (useSprite)
                        {
                            DirectoryInfo spriteDir = Directory.CreateDirectory(Path.Combine(createdDir.FullName, "sprites"));
                            SpriteInfo spriteInfo = new SpriteInfo();
                            
                            spriteInfo.spriteDirectory = "sprites";
                            spriteInfo.default_sprite = "idle";
                            spriteInfo.costumeName = "default";
                            newInfo.sprite_info = spriteInfo;
                        }

                        if (useModel)
                        {
                            //TODO create model directory and stuff
                        }
                        newInfo.Save(Path.Combine(FileLoader.GetFighterDir(directoryName).FullName, "fighter_info.json"));
                        LegacyEditorData.instance.LoadNewFighter(newInfo);
                        Dispose();
                    }
                    FighterInfo newFighter = new FighterInfo();
                }
                else DisplayError("An issue occurred attempting to create the Fighter directory. Check your system permissions and try again.");

            }
            else DisplayError("A fighter already exists with that name");
        }
        else DisplayError("Fighter Directory cannot be empty");
    }

    public void Initialize()
    {
        NGUITools.SetActive(gameObject, true);
    }

    public void Dispose()
    {
        NGUITools.SetActive(gameObject, false);
    }

    public void DisplayError(string text)
    {
        errorLabel.SetErrorText(text);
        errorLabel.DisplayError();
    }
}
