using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : ScriptableObject {

    public string sprite_name;
    public int sprite_rate = 1;
    public bool loop = true;
    public int length = 1;

    public int current_frame;
    protected int last_frame;

    protected AbstractFighter actor;
    

    public virtual void SetUp (AbstractFighter _actor) { //Replaces SetUp from TUSSLE 1.0
        last_frame = length;
        actor = _actor;
        actor.GetComponent<SpriteLoader>().ChangeSprite(sprite_name);
    }

    // Update is called once per frame
    public virtual void Update() {
        int sprite_number = Mathf.FloorToInt(current_frame / sprite_rate);
        if (sprite_rate < 0)
            sprite_number = Mathf.FloorToInt(current_frame / sprite_rate) - 1;
        actor.GetComponent<SpriteLoader>().ChangeSubimage(sprite_number,loop);
        if (current_frame >= last_frame)
        {
            this.OnLastFrame();
        }
	}

    public virtual void OnLastFrame()
    {
        //Destroy(this);
    }

    public virtual void LateUpdate() //This way the frame gets incremented after everything else
    {
        current_frame++;
    }

    public virtual void TearDown(Action new_action)
    {

    }

    public virtual void stateTransitions()
    {
        
    }

    public virtual void onClank()
    {

    }

    public virtual void onPrevail()
    {

    }
}
