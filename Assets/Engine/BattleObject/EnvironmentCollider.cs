using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentCollider : BattleComponent {
    public Vector3 center_offset;
    public Collider col;
    public float radius;
    public float height;

    //private Rigidbody rigid;
    private float yDist;
    private float xDist;

    private CharacterController controller;

    private bool checkCollisions = true;
    // Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        //col = GetComponent<Collider>();
        controller = GetComponent<CharacterController>();
            
        xDist = controller.bounds.extents.x;
        yDist = controller.bounds.extents.y;
        //If there's a center offset, we need to make sure to add it back in.
        //Signs are inverted, higher centers (negative) require longer yDist
        yDist -= controller.center.y;
        //rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 pos = transform.position;
        Vector3 leftPos = new Vector3(pos.x - xDist, pos.y, pos.z);
        Vector3 rightPos = new Vector3(pos.x + xDist, pos.y, pos.z);
        Debug.DrawRay(leftPos, -transform.up * yDist, Color.red);
        Debug.DrawRay(rightPos, -transform.up * yDist, Color.blue);
        Debug.DrawRay(pos, (-transform.up * (yDist+0.3f)), Color.green);

    }

    public override void ManualUpdate()
    {
        checkCollisions = true;
        if (!Physics.Raycast(transform.position, -Vector3.up, yDist + 0.3f)) //If we're way off base
        {
            SetVar("grounded", false);
        }
    }

    public void CheckForGround()
    {
        Vector3 pos = transform.position;
        Vector3 leftPos = new Vector3(pos.x - xDist, pos.y, pos.z);
        Vector3 rightPos = new Vector3(pos.x + xDist, pos.y, pos.z);

        SetVar("grounded", false);
        if (controller.isGrounded) SetVar("grounded", true);
        else
        {
            RaycastHit rayHit;
            if (Physics.Raycast(pos,-Vector3.up, out rayHit, yDist + 0.3f))
            {
                if (rayHit.collider.gameObject.GetComponent<Platform>() != null)
                {
                    RaycastHit slopeHit;
                    if (Physics.Raycast(leftPos, -Vector3.up, out slopeHit, yDist+0.1f))
                    {
                        if (slopeHit.collider.gameObject.GetComponent<Platform>() != null)
                        {
                            if (battleObject.GetYSpeed() <= 0.0f) //If we're going down
                            {
                                SetVar("grounded", true);
                                transform.position = new Vector3(rayHit.point.x, rayHit.point.y + controller.height / 2, rayHit.point.z);
                            }
                        }
                    }
                    else if (Physics.Raycast(rightPos, -Vector3.up, out slopeHit, yDist + 0.1f))
                    {
                        if (slopeHit.collider.gameObject.GetComponent<Platform>() != null)
                        {
                            if (battleObject.GetYSpeed() <= 0.0f) //If we're going down
                            {
                                SetVar("grounded", true);
                                transform.position = new Vector3(rayHit.point.x, rayHit.point.y + controller.height / 2, rayHit.point.z);
                            }
                        }
                    }
                }
            }
            
        }
    }

    /*
    void OnCollisionEnter(Collision col)
    {
        Platform plat = col.gameObject.GetComponent<Platform>();
        //Angle the fighter to the platform, only if it's terrain and a platform.
        //Non-terrain platforms can cause collisions, but the fighter shouldn't align to them
        //or things might get wonky.

        if (col.gameObject.layer == LayerMask.NameToLayer("Terrain") && plat != null)
        {
            if (GetFloatVar("elasticity") > 0.0f)
            {
                Vector3 point = col.contacts[0].point;
                Vector3 dir = -col.contacts[0].normal;
                point -= dir;
                RaycastHit hit;
                if (col.collider.Raycast( new Ray(point,dir), out hit, 2))
                {
                    Vector3 normal = hit.normal;
                    Debug.Log(normal);
                    float angle = Vector3.Angle(-transform.forward, normal);
                    Debug.Log(angle);
                    //Vector3 reflectAngle = Vector3.Reflect(rigid.velocity, normal);
                    //rigid.velocity = reflectAngle * GetFloatVar("elasticity");
                }
            }
            else if (GetBoolVar("grounded"))
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
    */
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //This flag makes sure that the 
        if (!checkCollisions) return;
        checkCollisions = false;

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Terrain"))
        {
            MotionHandler mot = battleObject.GetMotionHandler();
            if (GetFloatVar("elasticity") > 0.0f)
            {
                battleObject.SendMessage("WallBounce", hit);
            }
            else if (GetBoolVar("grounded"))
            {

                if (hit.transform.eulerAngles.z < 41 || hit.transform.eulerAngles.z > 319) //TODO un-hardcode this number
                {
                    SendMessage("UnRotate");
                    SendMessage("RotateSprite", GetIntVar("facing") * hit.transform.eulerAngles.z);
                    //Vector3 rot = transform.eulerAngles;
                    //transform.eulerAngles = new Vector3(rot.x, rot.y, hit.transform.eulerAngles.z);
                }
            }
        }
    }
}