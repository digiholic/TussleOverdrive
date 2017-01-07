using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float Speed = 0f;
    private float movex = 0f;

    public Hitbox hitbox;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        float movex = Input.GetAxis("Horizontal");
        transform.Translate(movex*Speed*Time.deltaTime,0,0,Space.World);
        //GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(movex));
        if (Input.GetKeyDown(KeyCode.Z))
            hitbox.Activate();
    }

    void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in cols)
        {
            c.SendMessage("getLaunched");
        }
    }
}
