using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubactionDataDocumentationCreator : MonoBehaviour {
    private Dictionary<SubactionType, List<SubactionData>> subactionsByCategory = new Dictionary<SubactionType, List<SubactionData>>();
    private string htmlString;

    // Use this for initialization
    void Start()
    {
        SubactionData[] data = Resources.LoadAll<SubactionData>("SubactionData");
        foreach (SubactionData sub in data)
        {
            if (!subactionsByCategory.ContainsKey(sub.subType))
                subactionsByCategory[sub.subType] = new List<SubactionData>();

            subactionsByCategory[sub.subType].Add(sub);
        }
        generateHtml();
    }

    void generateHtml()
    {
        htmlString += "<html>";
        foreach(KeyValuePair<SubactionType,List<SubactionData>> subKey in subactionsByCategory){
            htmlString += "<h1>" + subKey.Key + "</h1>";
            htmlString += "<ul>";
            foreach(SubactionData subData in subKey.Value)
            {
                htmlString += "<li>";
                htmlString += "<b>"+subData.SubactionName+"</b><br/>";
                htmlString += subData.Description;
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

        Debug.Log(htmlString);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
