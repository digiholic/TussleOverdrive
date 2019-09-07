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
    // Start is called before the first frame update
    void Start()
    {
        depth = transform.localPosition.z;
        fighterSprite = fighter.GetComponent<SpriteHandler>();
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
        displayBox.transform.position = fighterSprite.getCenterPoint();
        boxRect.center = displayBox.transform.localPosition;
        displayBox.transform.localScale = new Vector3(boxRect.width, boxRect.height, 0.1f);
    }
}
