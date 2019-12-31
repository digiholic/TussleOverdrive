using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GhettoCSSRig : MonoBehaviour
{
    List<FighterInfo> fightersAvailable = new List<FighterInfo>();

    [SerializeField] InputBuffer input;

    void Awake()
    {
        if (input == null) input = GetComponent<InputBuffer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadFighterList();
    }

    // Update is called once per frame
    void Update()
    {
        if (input.GetKeyDown("Attack")) Debug.Log("Player Confirmed",gameObject);
    }

    private void InstantiateRow(){

    }

    private void LoadFighterList()
    {
        DirectoryInfo[] individualFighters = FileLoader.FighterDir.GetDirectories();
        foreach (DirectoryInfo fighterDir in individualFighters)
        {
            Debug.Log("Loading "+fighterDir.FullName);
            FighterInfo info = FighterInfo.LoadFighterInfoFile(fighterDir.Name);
            fightersAvailable.Add(info);
        }
    }
}
