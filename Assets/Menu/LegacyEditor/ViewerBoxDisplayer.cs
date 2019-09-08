using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerBoxDisplayer : MonoBehaviour
{
    public Rect boxRect;
    
    public BattleObject fighter;
    private SpriteHandler fighterSprite;
    [SerializeField] private Transform displayBox;
    private float depth;

    [SerializeField] private bool growLeft;
    [SerializeField] private bool growRight;
    [SerializeField] private bool growTop;
    [SerializeField] private bool growBottom;

    // Start is called before the first frame update
    void Start()
    {
        depth = transform.localPosition.z;
        fighterSprite = fighter.GetComponent<SpriteHandler>();
        
        //We set the "real" position of the box to the "real" center of the sprite, then set the rect center to the "pixel space" center of the sprite
        displayBox.transform.position = fighterSprite.getCenterPoint();
        boxRect.center = displayBox.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (growLeft){
            boxRect.xMin -= 1;
        }
        if (growRight){
            boxRect.xMax += 1;
        }
        if (growBottom){
            boxRect.yMin -= 1;
        }
        if (growTop){
            boxRect.yMax += 1;
        }
        SizeToOwner();
    }

    public void SizeToOwner()
    {
        float scale = fighter.GetFloatVar(TussleConstants.SpriteVariableNames.PIXELS_PER_UNIT);
        transform.localScale = new Vector3(1 / scale, 1 / scale, 1.0f);
        
        //float scale = 50;
        Vector3 newPosition = new Vector3(boxRect.center.x,boxRect.center.y,displayBox.transform.localPosition.z);
        displayBox.transform.localPosition = newPosition;
        displayBox.transform.localScale = new Vector3(boxRect.width, boxRect.height, 0.1f);
        
    }
}
