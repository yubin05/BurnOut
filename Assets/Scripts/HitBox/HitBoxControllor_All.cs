using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// left and right hitbox superclass script
public class HitBoxControllor_All : MonoBehaviour
{
    // hit box controller
    protected BoxCollider2D colliderOfCollision;

    // subclass's object
    protected GameObject thisGameobject;
    protected SpriteRenderer object_render;

    // player object's script
    protected PlayerController player_script;

    // left and right hit box collider
    protected float offset_x_plus;
    protected float offset_x_minus;

    // this variable's initial value write subclass before base.Start()
    protected bool isRightDefaultValue;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        colliderOfCollision = gameObject.GetComponent<BoxCollider2D>();

        thisGameobject = transform.root.gameObject;
        object_render = thisGameobject.GetComponent<SpriteRenderer>();

        player_script = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        if (isRightDefaultValue) { offset_x_plus = colliderOfCollision.offset.x; }
        else { offset_x_plus = colliderOfCollision.offset.x * (-1); }
        offset_x_minus = offset_x_plus * (-1);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (isRightDefaultValue)
        {
            if (!object_render.flipX)
            {
                colliderOfCollision.offset = new Vector2(offset_x_plus, colliderOfCollision.offset.y);
            }   // right
            else
            {
                colliderOfCollision.offset = new Vector2(offset_x_minus, colliderOfCollision.offset.y);
            }   // left
        }
        else
        {
            if (object_render.flipX)
            {
                colliderOfCollision.offset = new Vector2(offset_x_plus, colliderOfCollision.offset.y);
            }   // right
            else
            {
                colliderOfCollision.offset = new Vector2(offset_x_minus, colliderOfCollision.offset.y);
            }   // left
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
