using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class GhettoCSSRig : MonoBehaviour
{
    private static List<FighterInfo> fightersAvailable;

    [SerializeField] InputBuffer input;
    //[SerializeField] UIGrid grid;
    [SerializeField] TextMeshProUGUI offlineLabel;

    [SerializeField] GameObject FighterRowPrefab;

    public bool isActive = false;
    public bool isConfirmed = false;
    private List<GhettoCSSFighterRow> children = new List<GhettoCSSFighterRow>();
    private int currentSelected = 0;
    
    void Awake()
    {
        input = GetComponent<InputBuffer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (fightersAvailable == null) LoadFighterList();
    }

    // Update is called once per frame
    void Update()
    { 
        //Waiting state
        if (!isActive && !isConfirmed){
            if (input.GetKeyDown("Attack")){
                Activate();
            }
        }
        //Choosing state
        else if (!isConfirmed && isActive){
            //CANCEL
            if (input.GetKeyDown("Special")){
                Deactivate();
            }
            //DOWN
            if (input.GetAxis("Vertical") < -0.5f && input.GetAxisDelta("Vertical") < -0.5f) {
                children[currentSelected].isChosen = false;
                currentSelected = ((currentSelected+1)+children.Count) % children.Count;
                children[currentSelected].isChosen = true;
            }
            //UP
            if (input.GetAxis("Vertical") > 0.5f && input.GetAxisDelta("Vertical") > 0.5f) {
                children[currentSelected].isChosen = false;
                currentSelected = ((currentSelected-1)+children.Count) % children.Count;
                children[currentSelected].isChosen = true;
            }
            //CONFIRM
            if (input.GetKeyDown("Attack")){
                Confirm();
            }
        }
        //Confirmed state
        else if (isConfirmed){
            //CANCEL
            if (input.GetKeyDown("Special")){
                CancelConfirm();
            }
            if (input.GetKeyDown("Attack")){
                GhettoCSSManager.manager.CheckStart();
            }
        }
        
    }

    private void Activate(){
        offlineLabel.enabled = false;
        isActive = true;
        foreach (FighterInfo info in fightersAvailable){
            InstantiateRow(info);
        }
        children[currentSelected].isChosen = true;
        //grid.Reposition();
        GhettoCSSManager.manager.CheckReady();
    }

    private void Deactivate(){
        offlineLabel.enabled = true;
        isActive = false;
        children[currentSelected].isChosen = false;
        currentSelected = 0;
        
        foreach (GhettoCSSFighterRow child in children){
            Destroy(child.gameObject);
        }
        children.Clear();
        //grid.Reposition();
        GhettoCSSManager.manager.CheckReady();
    }

    private void Confirm(){
        for (int i=0;i<children.Count;i++){
            if (i==currentSelected){
                BattleLoader.current_loader.addFighter(children[i].fighter,input.player_num);
                
            } else {
                children[i].isHidden = true;
            }
            isConfirmed = true;
        }
        GhettoCSSManager.manager.CheckReady();
    }

    private void CancelConfirm(){
        for (int i=0;i<children.Count;i++){
            if (i==currentSelected){

            } else {
                children[i].isHidden = false;
            }
            isConfirmed = false;
        }
        GhettoCSSManager.manager.CheckReady();
    }
    private void InstantiateRow(FighterInfo info){
        //GameObject go = NGUITools.AddChild(grid.gameObject, FighterRowPrefab);
        GameObject go = Instantiate(FighterRowPrefab);
        TextMeshProUGUI label = go.GetComponent<TextMeshProUGUI>();
        label.text = info.displayName;
        GhettoCSSFighterRow fighterRow = go.GetComponent<GhettoCSSFighterRow>();
        fighterRow.fighter = info;
        children.Add(fighterRow);
    }

    private static void LoadFighterList()
    {
        fightersAvailable = new List<FighterInfo>();
        DirectoryInfo[] individualFighters = FileLoader.FighterDir.GetDirectories();
        foreach (DirectoryInfo fighterDir in individualFighters)
        {
            FighterInfo info = FighterInfo.LoadFighterInfoFile(fighterDir.Name);
            fightersAvailable.Add(info);
        }
    }
}
