using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;

public class TussleScriptParser : MonoBehaviour
{
    [SerializeField] private TextAsset tussleScriptDocument;

    [SerializeField] private ActionFile testActionFile;

    // Start is called before the first frame update
    void Start()
    {
        if (tussleScriptDocument != null)
        {
            testActionFile = new ActionFile();
            ParseActionScript(tussleScriptDocument.text, testActionFile, false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static DynamicAction workingAction = null;
    private static string workingSubGroupName = null;
    
    /// <summary>
    /// Parse a string representation of an Action Script
    /// </summary>
    /// <param name="actionScript">the action script to parse</param>
    /// <returns>-1 on error, 1 on success</returns>
    public static int ParseActionScript(string actionScript, ActionFile outputFile,bool overwrite)
    {
        //Reset the working vars
        workingAction = null;
        workingSubGroupName = null;

        StringReader reader = new StringReader(actionScript);

        string line;
        int lineNumber = 0;
        
        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim(); //Remove the tabs. We ain't python.
            if (line.Length == 0){
                lineNumber++;
                continue; //Skip empty lines
            }

            string tokenValue;
            //Process Start Token
            if ((tokenValue = GetTokenVal(line, ActionTokens.Start)) != null)
            {
                if (workingAction == null)
                {
                    workingAction = new DynamicAction(tokenValue);
                    //Debug.Log(string.Format("Creating new action with name {0}", tokenValue));
                }
                else
                {
                    return throwException("Attempting to create a new action before finishing the old one", lineNumber);
                }
            }
            
            //Process End Token
            else if ((tokenValue = GetTokenVal(line, ActionTokens.End, 0)) != null)
            {
                //TODO Finish the action here
                //Debug.Log("Ending Action Definition");
                outputFile.Add(workingAction,overwrite);
                
                workingAction = null;
            }
            
            if (workingAction != null){
                ProcessActionSection(line,workingAction,lineNumber);
            }

            lineNumber++;
        }

        return SUCCESS;
    }

    /// <summary>
    /// In this parsing section, we can assume the action is set and we are adding properties or subactions to it. This means we don't need to null check everything
    /// </summary>
    /// <param name="line">The full text line to parse</param>
    /// <param name="workingAction">The DynamicAction currently being processed. Guaranteed not null.</param>
    private static void ProcessActionSection(string line, DynamicAction workingAction, int lineNumber)
    {
        string token;
        //Process Properties Tokens
        if ((token = GetTokenVal(line, ActionTokens.Properties.Length)) != null)
        {
            int lengthNo = 1;
            if (int.TryParse(token, out lengthNo))
                workingAction.length = lengthNo;
            else throwException(string.Format("Could not parse int for property {0} - {1}", ActionTokens.Properties.Length, token), lineNumber);
        }
        else if ((token = GetTokenVal(line, ActionTokens.Properties.Animation)) != null)
        {
            workingAction.animationName = token;
        }
        else if ((token = GetTokenVal(line, ActionTokens.Properties.ExitAction)) != null)
        {
            workingAction.exit_action = token;
        }
        
        //Process Group
        else if ((token = GetTokenVal(line, ActionTokens.GroupStart)) != null)
        {
            //Debug.Log("Starting SubGroup "+token);
            workingSubGroupName = token;
        }
        else if ((token = GetTokenVal(line, ActionTokens.GroupEnd,0)) != null)
        {
            //Debug.Log("Ending SubGroup");
            workingSubGroupName = null;
        }

        //If we haven't closed this subGroup by now, all that's left is a Subaction Definition
        else if (workingSubGroupName != null){
            SubactionData subData = processSubactionLine(line,lineNumber);
            if (subData != null){
                workingAction.AddSubaction(workingSubGroupName,subData);
            }
        }
    }
    /// <summary>
    /// Given a line containing the definition of a SubactionData, return that SubactionData as an object.
    /// This uses reflection to get the Subactions, so the spelling of the SubName is key.
    /// </summary>
    /// <param name="line">The line being parsed</param>
    /// <returns>A proper SubactionData object, ready for injecting into a Dynamic Action</returns>
    private static SubactionData processSubactionLine(string line, int lineNumber){
        string[] startParenSplit = line.Split('(');

        //This string should contain EXACTLY one parenthesis pair. Any more or less and you fucked up your script, dawg
        if (startParenSplit.Length != 2 || !line.EndsWith(")")){
            //Debug.Log(startParenSplit.Length);
            //Debug.Log(line.EndsWith(")"));
            throwException("Incorrect parenthesis in subDataLine: "+line,lineNumber);
            return null;
        }
        string subDataName = startParenSplit[0];
        string argList = startParenSplit[1].Substring(0,startParenSplit[1].Length-1);
        //Debug.Log(string.Format("PARSING SUBACTION {0}\n\tArguments: {1}",subDataName,argList));


        SubactionDataDefault subDataDef = Resources.Load<SubactionDataDefault>("SubactionData/"+subDataName);
        if (subDataDef != null){
            SubactionData subData = subDataDef.CreateSubactionData();
            string[] splitArgs = new string[0];
            if (argList.Length > 0){
                splitArgs = argList.Split(',');
            }
            if (subDataDef.scriptArgNames.Count != splitArgs.Length){
                throwException(string.Format("Subaction {0} has {1} arguments, expecting {2}",subDataName,splitArgs.Length,subDataDef.scriptArgNames.Count),lineNumber);
                return null;
            }
            for (int i=0;i<splitArgs.Length;i++){
                string argumentName = subDataDef.scriptArgNames[i];
                
                SubactionSource source = SubactionSource.CONSTANT;
                if (splitArgs[i].StartsWith("owner.")){
                    source = SubactionSource.OWNER;
                    splitArgs[i] = splitArgs[i].Substring("owner.".Length);
                }else if (splitArgs[i].StartsWith("action.")){
                    source = SubactionSource.ACTION;
                    splitArgs[i] = splitArgs[i].Substring("action.".Length);
                }

                SubactionVarType varType = subDataDef.arguments[argumentName].type;

                SubactionVarData argVarData = new SubactionVarData(argumentName,source,varType,splitArgs[i]);
                subData.arguments[argumentName] = argVarData;
            }
            return subData;
        } else {
            throwException("No Subaction found with name "+subDataName,lineNumber);
        }
        return null;
    }


    /// <summary>
    /// Checks the given line for the presence of a token, strips the token out and returns only the value.
    /// If the token is not present in the line, it will return null
    /// </summary>
    /// <param name="line">The string to parse</param>
    /// <param name="token">The token to look for</param>
    /// <param name="lengthAdjust">How many more or less characters to get from the line before getting the value. For example, if there is a space between the token and the value, you would keep the default value of 1</param>
    /// <returns></returns>
    private static string GetTokenVal(string line, string token, int lengthAdjust = 1)
    {
        if (line.StartsWith(token))
        {
            return line.Substring(token.Length + lengthAdjust);
        }
        else
        {
            return null;
        }
    }

    private static int throwException(string error, int line)
    {
        Debug.LogError(string.Format("Error parsing line {0}\n\t{1}", line, error));
        return PARSEERROR;
    }


    #region static definitions
    private class ActionTokens
    {
        public static string Start = "Action";
        public static string End = "EndAction";
        public static string GroupStart = "StartGroup";
        public static string GroupEnd = "EndGroup";

        public class Properties
        {
            public static string Length = "Length";
            public static string Animation = "Animation";
            public static string ExitAction = "ExitAction";
        }
    }

    private static int PARSEERROR = -1;
    private static int SUCCESS = 1;
    #endregion
}
