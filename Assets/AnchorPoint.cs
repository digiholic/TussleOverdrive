using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour {
    private BattleObject parent;

	// Use this for initialization
	void Awake () {
        parent = transform.parent.gameObject.GetComponent<BattleObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SnapAnchorToPoint(Vector3 snapPoint)
    {
        int facingDir = parent.GetAbstractFighter().facing;
        snapPoint.x = snapPoint.x - (transform.localPosition.x * facingDir);
        snapPoint.y = snapPoint.y - transform.localPosition.y;
        snapPoint.z = parent.transform.localPosition.z;
        parent.transform.localPosition = snapPoint;
    }

    public void MoveAnchor(Vector2 motion)
    {
        transform.Translate(new Vector3(motion.x, motion.y));
    }

    public void MoveAnchorPixel(int centerx, int centery)
    {
        float pixelsPerUnit = parent.GetSpriteHandler().pixelsPerUnit;
        transform.localPosition = new Vector3(centerx / pixelsPerUnit, centery / pixelsPerUnit, 0.0f);
    }
}
