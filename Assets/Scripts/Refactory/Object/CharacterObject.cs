using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : EntityObject
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField] private Animator animator;
    public Animator Animator => animator;

    public Rigidbody2D Rigidbody2D { get; protected set; }

    public bool IsJump { get; set; }

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public FSM FSM { get; protected set; }

    public override void Init(Data data)
    {
        base.Init(data);

        var character = data as Character;
        character.Init();

        FSM = new FSM(this);

        IsJump = false;
    }
}
