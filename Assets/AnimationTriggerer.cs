using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerer : MonoBehaviour {
    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void SetSizeToggle()
    {
        if (anim != null)
            anim.SetTrigger("SizeToggle");
    }

    void SetHover()
    {
        if (anim != null)
            anim.SetBool("Hover", true);
    }

    void UnsetHover()
    {
        if (anim != null)
            anim.SetBool("Hover", false);
    }
}
