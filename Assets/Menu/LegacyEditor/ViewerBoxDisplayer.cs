using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerBoxDisplayer : MonoBehaviour
{

    public Vector2 center;
    public Vector2 size;
    public BattleObject fighter;
    [SerializeField] private Transform displayBox;
    private float depth;
    // Start is called before the first frame update
    void Start()
    {
        depth = transform.localPosition.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        SizeToOwner();
    }

    public void SizeToOwner()
    {
        float scale = fighter.GetFloatVar(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT);
        transform.localScale = new Vector3(1 / scale, 1 / scale, 1.0f);
        
        //float scale = 50;
        displayBox.transform.localPosition = new Vector3(center.x, center.y, 0.0f);
        displayBox.transform.localScale = new Vector3(size.x, size.y, 0.1f);
    }
}
