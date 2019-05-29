using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SubactionDataDocumentationCreator : MonoBehaviour {
    
    public static void generateHtml()
    {
        Dictionary<SubactionType, List<SubactionDataDefault>> subactionsByCategory = new Dictionary<SubactionType, List<SubactionDataDefault>>();
        string htmlString = "";

        SubactionDataDefault[] data = Resources.LoadAll<SubactionDataDefault>("SubactionData");
        foreach (SubactionDataDefault sub in data)
        {
            if (!subactionsByCategory.ContainsKey(sub.subType))
                subactionsByCategory[sub.subType] = new List<SubactionDataDefault>();

            subactionsByCategory[sub.subType].Add(sub);
        }

        
        htmlString += "<html>";
        foreach(KeyValuePair<SubactionType,List<SubactionDataDefault>> subKey in subactionsByCategory){
            htmlString += "<h1>" + subKey.Key + "</h1>";
            htmlString += "<ul>";
            foreach(SubactionDataDefault subData in subKey.Value)
            {
                htmlString += "<li>";
                htmlString += "<b>"+subData.SubactionName+"</b> - ";
                htmlString += "<i>"+subData.Description+"</i>";
                htmlString += "<ul>";
                foreach (SubactionVarData varData in subData.arguments.GetItems())
                {
                    htmlString += "<li>";
                    htmlString += varData.name;
                    htmlString += "<ul>";
                    htmlString += "<li>" + varData.description + "</li>";
                    htmlString += "</ul>";
                    htmlString += "</li>";
                }
                htmlString += "</ul>";
                htmlString += "</li>";
            }
            htmlString += "</ul>";
        }
        htmlString += "</html>";

        //Sometimes the game tries to load while this is being written to, so it errors out. Some kinda permission thing. Skipping an auto save isn't really a problem, but it'd still be nice to know about it in case it's happening too often
        try
        {
            File.WriteAllText(Application.dataPath + "/Subaction-README.html", htmlString);
        }
        catch
        {
            Debug.LogWarning("Autosave of Subaction README failed");
        }
    }
}
