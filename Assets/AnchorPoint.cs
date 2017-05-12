using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour {
    private BattleObject parent;

	// Use this for initialization
	void Start () {
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

    void MoveAnchor(Vector2 motion)
    {
        transform.Translate(new Vector3(motion.x, motion.y));
    }
}
