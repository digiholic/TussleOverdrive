using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentCollider : BattleComponent {
    public Vector3 center_offset;
    public Collider col;
    public float radius;
    public float height;

    private Rigidbody rigid;
    private float yDist;
    private float xDist;
    // Use this for initialization
	void Start () {
        col = GetComponent<Collider>();
        xDist = col.bounds.extents.x;
        yDist = col.bounds.extents.y;
        rigid = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	public override void ManualUpdate () {
        Debug.DrawRay(transform.position, -transform.up * yDist, Color.blue);
    }

    public void CheckForGround()
    {
        RaycastHit rayHit;

        if (Physics.Raycast(transform.position, -transform.up, out rayHit, yDist + 0.1f))
        {
            Platform plat = rayHit.collider.gameObject.GetComponent<Platform>();

            //Check if we're moving down relative to the ground. Float errors can sometimes cause issues so we compare with -0.01 instead of zero
            if (plat != null)
                //if ((rigid.velocity.y - rayHit.rigidbody.velocity.y) < -0.01f) //If we're falling into the ground
                SetVar("grounded", true);
        }
        else
            SetVar("grounded", false);

    }

    void OnCollisionEnter(Collision col)
    {
        Platform plat = col.gameObject.GetComponent<Platform>();
        //Angle the fighter to the platform, only if it's terrain and a platform.
        //Non-terrain platforms can cause collisions, but the fighter shouldn't align to them
        //or things might get wonky.
        if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && plat != null)
        {
            if (GetBoolVar("grounded"))
            {
                if (col.transform.eulerAngles.z < 41 || col.transform.eulerAngles.z > 319) //TODO un-hardcode this number
                {
                    Vector3 rot = transform.eulerAngles;
                    transform.eulerAngles = new Vector3(rot.x, rot.y, col.transform.eulerAngles.z);
                }
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        //Reset back to vertical if you leave the ground
        if (!GetBoolVar("grounded"))
        {
            Vector3 rot = transform.eulerAngles;
            transform.eulerAngles = new Vector3(rot.x, rot.y, 0);
        }
    }
}