using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

class TussleAssetPostProcessor : AssetPostprocessor
{   
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        bool subactionDataChanged = false;

        //Check if any of the items in Resources/SubactionData/ changed. If so, mark that they have so we can re-generate their documentation
        foreach (string str in importedAssets)
        {
            if (str.Contains("Resources/SubactionData/")) subactionDataChanged = true;
        }
        foreach (string str in deletedAssets)
        {
            if (str.Contains("Resources/SubactionData/")) subactionDataChanged = true;
        }

        foreach (string str in movedAssets)
        {
            if (str.Contains("Resources/SubactionData/")) subactionDataChanged = true;
        }

        if (subactionDataChanged) SubactionDataDocumentationCreator.generateHtml();
    }
}