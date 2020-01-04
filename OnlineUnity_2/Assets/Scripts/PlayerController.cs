using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Properties")]
    [Tooltip("Default Movement Speed")]
    public float walkSpeed = 2.0f;
    [Tooltip("Fast Movement Speed")]
    public float runSpeed = 3.5f;
    public float directionRotateSpeed = 100.0f;
    public float bodyRotateSpeed = 2.0f;
    [Range(0.01f, 5.0f)]
    public float accelerationSpeed = 0.1f;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController cc = null;
    private CollisionFlags cf = CollisionFlags.None;
    private float gravity = 9.8f;
    private float verticalSpeed = 0.0f;
    private bool movable = true;

    [Header("Animation Properties")]
    public AnimationClip idle = null;
    public AnimationClip walk = null;
    public AnimationClip run = null;
    public AnimationClip attack_1 = null;
    public AnimationClip attack_2 = null;
    public AnimationClip attack_3 = null;
    public AnimationClip attack_4 = null;
    private Animation anim = null;

    public enum PlayerState { none, idle, walk, run, attack, skill }
    public enum AttackState { attack_1, attack_2, attack_3, attack_4 }

    [Header("Player State")]
    public PlayerState state = PlayerState.none;
    public AttackState attackState = AttackState.attack_1;
    public bool nextAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animation>();
        anim.playAutomatically = false;
        anim.Stop();

        state = PlayerState.idle;
        anim[idle.name].wrapMode = WrapMode.Loop;
        anim[walk.name].wrapMode = WrapMode.Loop;
        anim[run.name].wrapMode = WrapMode.Loop;
        anim[attack_1.name].wrapMode = WrapMode.Once;
        anim[attack_2.name].wrapMode = WrapMode.Once;
        anim[attack_3.name].wrapMode = WrapMode.Once;
        anim[attack_4.name].wrapMode = WrapMode.Once;

        AddAnimationEvent(attack_1, "OnAttackAnimationFinish");
        AddAnimationEvent(attack_2, "OnAttackAnimationFinish");
        AddAnimationEvent(attack_3, "OnAttackAnimationFinish");
        AddAnimationEvent(attack_4, "OnAttackAnimationFinish");
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        BodyDirectionChange();
        ApplyGravity();
        AnimationControl();
        CheckState();
        InputControl();
    }

    void Move()
    {
        if (!movable) return;
    
        Transform cameraTransform = Camera.main.transform;
        Vector3 verticalMove = cameraTransform.TransformDirection(Vector3.forward);
        verticalMove.y = 0;
        Vector3 horizontalMove = new Vector3(verticalMove.z, 0.0f, -verticalMove.x);
        float moveV = Input.GetAxis("Vertical");
        float moveH = Input.GetAxis("Horizontal");
        Vector3 targetDirection = moveH * horizontalMove + moveV * verticalMove;

        moveDirection = Vector3.RotateTowards(moveDirection, targetDirection, directionRotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 1000.0f);
        moveDirection = moveDirection.normalized;

        float speed = walkSpeed;
        if (state == PlayerState.run)
            speed = runSpeed;

        Vector3 gravityVector = new Vector3(0.0f, verticalSpeed, 0.0f);

        Vector3 moveAmount = (moveDirection * speed * Time.deltaTime) + gravityVector;
        cf = cc.Move(moveAmount);
    }
    
    float GetAcceleration()
    {
        if (cc.velocity == Vector3.zero)
        {
            currentVelocity = Vector3.zero;
        }
        else
        {
            Vector3 goalVelocity = cc.velocity;
            goalVelocity.y = 0.0f;
            currentVelocity = Vector3.Lerp(currentVelocity, goalVelocity, accelerationSpeed * Time.fixedDeltaTime);
        }

        return currentVelocity.magnitude;
    }

    void BodyDirectionChange()
    {
        if(GetAcceleration() > 0.0f)
        {
            Vector3 newForward = cc.velocity;
            newForward.y = 0.0f;
            transform.forward = Vector3.Lerp(transform.forward, newForward, bodyRotateSpeed * Time.deltaTime);
        }
    }

    void AnimationPlay(AnimationClip clip)
    {
        anim.clip = clip;
        anim.CrossFade(clip.name);
    }

    void AnimationControl()
    {
        switch (state)
        {
            case PlayerState.idle:
                AnimationPlay(idle);
                break;
            case PlayerState.walk:
                AnimationPlay(walk);
                break;
            case PlayerState.run:
                AnimationPlay(run);
                break;
            case PlayerState.attack:
                AttackAnimationControl();
                break;
            case PlayerState.skill:

                break;

        }
    }

    void CheckState()
    {
        float currentSpeed = GetAcceleration();

        switch (state)
        {
            case PlayerState.idle:
                if (currentSpeed > 0.0f)
                    state = PlayerState.walk;
                break;
            case PlayerState.walk:
                if (currentSpeed > 0.5f)
                    state = PlayerState.run;
                else if (currentSpeed < 0.01f)
                    state = PlayerState.idle;
                break;
            case PlayerState.run:
                if (currentSpeed < 0.5f)
                    state = PlayerState.walk;
                if (currentSpeed < 0.01f)
                    state = PlayerState.idle;
                break;
            case PlayerState.attack:
                movable = false;
                break;
            case PlayerState.skill:
                movable = false;
                break;
        }
    }

    void InputControl()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (state != PlayerState.attack)
            {
                state = PlayerState.attack;
                attackState = AttackState.attack_1;
            }
            else
            {
                switch(attackState)
                {
                    case AttackState.attack_1:
                        if (anim[attack_1.name].normalizedTime > 0.1f)
                            nextAttack = true;
                        break;
                    case AttackState.attack_2:
                        if (anim[attack_2.name].normalizedTime > 0.1f)
                            nextAttack = true;
                        break;
                    case AttackState.attack_3:
                        if (anim[attack_3.name].normalizedTime > 0.1f)
                            nextAttack = true;
                        break;
                    case AttackState.attack_4:
                        if (anim[attack_4.name].normalizedTime > 0.1f)
                            nextAttack = true;
                        break;
                }
            }
        }
    }

    void OnAttackAnimationFinish()
    {
        if (nextAttack)
        {
            nextAttack = false;
            switch(attackState)
            {
                case AttackState.attack_1:
                    attackState = AttackState.attack_2;
                    break;
                case AttackState.attack_2:
                    attackState = AttackState.attack_3;
                    break;
                case AttackState.attack_3:
                    attackState = AttackState.attack_4;
                    break;
                case AttackState.attack_4:
                    attackState = AttackState.attack_1;
                    break;
            }
        }
        else
        {
            state = PlayerState.idle;
            attackState = AttackState.attack_1;
            movable = true;
        }
    }

    void AddAnimationEvent(AnimationClip clip, string functionName)
    {
        AnimationEvent newEvent = new AnimationEvent();
        newEvent.functionName = functionName;
        newEvent.time = clip.length - 0.1f;
        clip.AddEvent(newEvent);
    }

    void AttackAnimationControl()
    {
        switch (attackState)
        {
            case AttackState.attack_1:
                AnimationPlay(attack_1);
                break;
            case AttackState.attack_2:
                AnimationPlay(attack_2);
                break;
            case AttackState.attack_3:
                AnimationPlay(attack_3);
                break;
            case AttackState.attack_4:
                AnimationPlay(attack_4);
                break;
        }
    }

    void ApplyGravity()
    {
        if((cf & CollisionFlags.CollidedBelow) != 0)
        {
            verticalSpeed = 0.0f;
        }
        else
        {
            verticalSpeed = -gravity * Time.deltaTime;
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("current speed: " + GetAcceleration().ToString());
        GUILayout.Label("current velocity vector: " + cc.velocity.ToString());
        GUILayout.Label("current velocity manitude: " + cc.velocity.magnitude.ToString());
        GUILayout.Label("collision flag: " + cf.ToString());
    }
}