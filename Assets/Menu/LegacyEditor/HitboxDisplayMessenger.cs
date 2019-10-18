using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDisplayMessenger : LegacyEditorWidget
{
    
    [SerializeField] private ViewerBoxDisplayer viewerBoxPrefab;
    [SerializeField] private BattleObject fighterViewerObject;
    [SerializeField] private Camera viewerCamera;

    private ViewerBoxDisplayer viewerBoxInstance;

    //This is used to track when the subaction has actually changed, rather than just updated
    private SubactionData lastData;

    private void displayHitbox(SubactionData data){
        viewerBoxInstance = GameObject.Instantiate<ViewerBoxDisplayer>(viewerBoxPrefab);
        viewerBoxInstance.viewerCamera = viewerCamera;
        viewerBoxInstance.fighter = fighterViewerObject;
        viewerBoxInstance.createHitboxSubData = data;

        viewerBoxInstance.transform.SetParent(fighterViewerObject.transform.parent,false);
        viewerBoxInstance.BroadcastMessage("SetCamera",viewerCamera);

        //Link it to the proper createHitbox action
    }

    private void disposeHitbox(){
        if (viewerBoxInstance != null) Destroy(viewerBoxInstance.gameObject);
    }

    private void ChangedSubaction(SubactionData data){
        //If it's the same subaction, it means only the data inside it has changed. The internal components will handle this.
        if (data != lastData){
            disposeHitbox(); //We always throw out the old one before we make a new one
            if (data?.SubactionName == "CreateHitbox"){
                displayHitbox(data);
            }
        }
        lastData = data;
    }

    public override void RegisterListeners()
    {
        editor.CurrentSubactionChangedEvent += ChangedSubaction;
    }

    public override void UnregisterListeners()
    {
        editor.CurrentSubactionChangedEvent -= ChangedSubaction;
    }
}
