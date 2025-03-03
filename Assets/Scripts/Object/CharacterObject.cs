using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ImmunitySystem))]
[RequireComponent(typeof(DebuffSystem))]
public class CharacterObject : EntityObject
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    public SpriteRenderer SpriteRenderer => spriteRenderer;

    [SerializeField] protected Animator animator;
    public Animator Animator => animator;

    [SerializeField] protected Transform attackNode;
    public Transform AttackNode => attackNode;

    [SerializeField] protected Transform headBarNode;
    public Transform HeadBarNode => headBarNode;

    [SerializeField] protected Transform floorCheckNode;
    public Transform FloorCheckNode => floorCheckNode;

    [SerializeField] protected Transform backNode;  // 등
    public Transform BackNode => backNode;

    public MotionHandler MotionHandler { get; protected set; }
    
    public Rigidbody2D Rigidbody2D { get; protected set; }

    public ImmunitySystem ImmunitySystem { get; protected set; }
    public DebuffSystem DebuffSystem { get; protected set; }

    public HeadBarObject HeadBarObject { get; set; }  // 대상이 가지고 있는 헤드바 오브젝트

    protected virtual void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        MotionHandler = animator.transform.GetComponent<MotionHandler>();
        ImmunitySystem = GetComponent<ImmunitySystem>();
        DebuffSystem = GetComponent<DebuffSystem>();
    }

    public FSM FSM { get; protected set; }

    public override void Init(Data data)
    {
        base.Init(data);

        // 캐릭터 데이터 초기화
        var character = data as Character;
        character.Init(this);

        // 상태 머신 초기화
        FSM = new FSM(this);

        // 애니메이터 모션 핸들러 초기화 및 이벤트 추가
        MotionHandler.Init();
        MotionHandler.DeathEvent += () => 
        {
            ImmunitySystem.StopImmunity();
            character.RemoveData();
        };

        // 무적 시스템 초기화 및 이벤트 추가
        Color color = SpriteRenderer.color;
        float originAlpha = color.a; float setAlpha = 0.5f;

        ImmunitySystem.Init();
        ImmunitySystem.OnStartImmunity += () => 
        {
            color.a = setAlpha;
            SpriteRenderer.color = color;
        };
        ImmunitySystem.OnStopImmunity += () =>
        {
            color.a = originAlpha;
            SpriteRenderer.color = color;
        };

        DebuffSystem.Init();

        HeadBarObject = null;
    }

    protected virtual void Update()
    {
        var character = data as Character;
        character.AttackDelayTime += Time.deltaTime;
    }

    // 가만히 있습니다.
    public virtual void OnIdle()
    {
        FSM.ChangeState(new IdleState(this));
    }

    // 이동합니다.
    public virtual void OnMove(Character.MoveDirectionXs moveDirectionX)
    {
        FSM.ChangeState(new MoveState(this));

        var character = data as Character;
        character.MoveDirectionX = moveDirectionX;
        transform.Translate(new Vector3((int)moveDirectionX*character.BasicStat.MoveSpeed*Time.deltaTime, 0, 0));
    }

    // 점프합니다.
    public virtual void OnJump()
    {
        FSM.ChangeState(new JumpState(this));

        var character = data as Character;
        Rigidbody2D.AddForce(transform.up * character.BasicStat.JumpPower, ForceMode2D.Impulse);
    }

    // 공격합니다.
    public virtual void OnAttack()
    {
        var character = data as Character;
        if (character.AttackDelayTime >= 1f/character.BasicStat.AttackSpeed)
        {
            FSM.ChangeState(new AttackState(this));
            character.AttackDelayTime = 0f;
        }
    }

    // 피격 당합니다.
    public virtual void OnHit(int attackPower)
    {
        // 디버프 적용
        if (DebuffSystem.IsDebuff) attackPower = (int)(attackPower * 1.5f);

        var character = data as Character;
        character.CurrentHp = Mathf.Max(0, character.CurrentHp-attackPower);

        if (character.CurrentHp <= 0)
        {
            OnDeath();
        }
        else
        {
            FSM.ChangeState(new HitState(this));
            ImmunitySystem.StartImmunity(character.BasicStat.StaggerDuration);
        }

        // 데미지 폰트 소환
        GameApplication.Instance.GameController.DamageFontController.Spawn<DamageFont, DamageFontObject>(this, 60001, attackPower);
    }

    // 죽습니다.
    public virtual void OnDeath()
    {
        var character = data as Character;
        character.CurrentHp = 0;
        
        FSM.ChangeState(new DeathState(this));
        ImmunitySystem.StartImmunity(); // 죽는 동안은 무적 처리하도록 설정
    }
}
