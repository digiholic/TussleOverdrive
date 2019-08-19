using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewerBoxDisplayer : MonoBehaviour
{

    [SerializeField] private Vector2 center;
    [SerializeField] private Vector2 size;
    [SerializeField] private BattleObject fighter;

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
        //float scale = 50;
        transform.localPosition = new Vector3(center.x / scale, center.y / scale, depth);
        transform.localScale = new Vector3(size.x / scale, size.y / scale, 1.0f);
    }
}
