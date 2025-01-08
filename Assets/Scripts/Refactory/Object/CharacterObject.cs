using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : EntityObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    protected MotionHandler motionHandler;
    public MotionHandler MotionHandler => motionHandler;
    
    public Rigidbody2D Rigidbody2D { get; protected set; }

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        motionHandler = animator.transform.GetComponent<MotionHandler>();
    }

    public FSM FSM { get; protected set; }

    public override void Init(Data data)
    {
        base.Init(data);

        var character = data as Character;
        character.Init();

        FSM = new FSM(this);
        motionHandler.Init();
    }
}
