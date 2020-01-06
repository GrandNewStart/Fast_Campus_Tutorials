using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class GoblinController : MonoBehaviour
{
    public enum goblinState { none, idle, patrol, wait, move, attack, damage, die };

    private goblinState state = goblinState.none;
    [Header("Basic Properties")]
    public float speed = 1.0f;
    public GameObject target = null;

    private Animation anim = null;
    [Header("Animation Clips")]
    public AnimationClip idle = null;
    public AnimationClip move = null;
    public AnimationClip attack = null;
    public AnimationClip damage = null;
    public AnimationClip die = null;

    [Header("Combat Properties")]
    public int HP = 100;
    public float attackRange = 1.5f;
    public GameObject damageEffect = null;
    public GameObject DieEffect = null;
    private Tweener effectTweener = null;
    private SkinnedMeshRenderer skinnedMeshRenderer = null;


    void OnAttackFinish()
    {
        Debug.Log("Attack Animation Finished");
    }
    void OnDamageFinish()
    {
        Debug.Log("Damage Animation Finished");
    }
    void OnDieFinish()
    {
        Debug.Log("Die Animation Finished");
        //Instantiate(DieEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void AddAnimationEvent(AnimationClip clip, string functionName)
    {
        AnimationEvent newEvent = new AnimationEvent();
        newEvent.functionName = functionName;
        newEvent.time = clip.length - 0.1f;
        clip.AddEvent(newEvent);
    }

    void CheckState()
    {
        switch (state)
        {
            case goblinState.idle:
                IdleUpdate();
                break;
            case goblinState.move:
            case goblinState.patrol:
                MoveUpdate();
                break;
            case goblinState.attack:
                AttackUpdate();
                break;
        }
    }

    void IdleUpdate()
    {
        if(target == null)
        {
            Vector3 randomTarget = new Vector3(transform.position.x + Random.Range(-10.0f, 10.0f),
                transform.position.y + 1000.0f,
                transform.position.z + Random.Range(-10.0f, 10.0f));

            Ray ray = new Ray(randomTarget, Vector3.down);
            RaycastHit raycastHit = new RaycastHit();
            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity))
            {
                randomTarget.y = raycastHit.point.y;
            }

            target.transform.position = randomTarget;
            state = goblinState.patrol;
        }
        else
        {
            state = goblinState.move;
        }
    }

    void MoveUpdate()
    {
        Vector3 diff = Vector3.zero;
        Vector3 lookAtPositon = Vector3.zero;

        switch (state)
        {
            case goblinState.patrol:
                if (target.transform.position != Vector3.zero)
                {
                    diff = target.transform.position - transform.position;

                    if (diff.magnitude > attackRange)
                    {
                        StartCoroutine(WaitUpdate());
                        return;
                    }

                    lookAtPositon = new Vector3(target.transform.position.x,
                        transform.position.y,
                        target.transform.position.z);
                }
                break;
            case goblinState.move:
                if (target != null)
                {
                    diff = target.transform.position - transform.position;

                    if (diff.magnitude < attackRange)
                    {
                        state = goblinState.attack;
                        return;
                    }

                    lookAtPositon = new Vector3(target.transform.position.x,
                        transform.position.y,
                        target.transform.position.z);
                }
                break;
        }

        Vector3 direction = diff.normalized;
        direction = new Vector3(direction.x, 0, direction.z);

        Vector3 moveAmount = direction * speed * Time.deltaTime;

        transform.Translate(moveAmount, Space.World);
        transform.LookAt(lookAtPositon);
    }

    IEnumerator WaitUpdate()
    {
        state = goblinState.wait;
        float waitTime = Random.Range(1.0f, 3.0f);
        yield return new WaitForSeconds(waitTime);
        state = goblinState.idle;
    }

    void OnSetTarget(GameObject obj)
    {
        target = obj;
        state = goblinState.move;
    }

    void AttackUpdate()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);
        if(distance > attackRange + 0.5f)
        {
            state = goblinState.move;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player Attack"))
        {
            HP -= 10;
            if (HP > 0)
            {
                anim.CrossFade(damage.name);
                //Instantiate(damageEffect, other.transform.position, Quaternion.identity);
                DamageTweenEffect();
            }
            else
                state = goblinState.die;
        }
    }

    void DamageTweenEffect()
    {
        if(effectTweener != null && !effectTweener.isComplete)
        {
            return;
        }

        Color colorTo = Color.red;
        effectTweener = HOTween.To(skinnedMeshRenderer.material, 0.2f, new TweenParms().Prop("color", colorTo)
            .Loops(1, LoopType.Yoyo)
            .OnStepComplete(OnDamageTweenFinish));
    }

    void OnDamageTweenFinish()
    {
        skinnedMeshRenderer.material.color = Color.white;
    }

    void AnimationControl()
    {
        switch (state) {
            case goblinState.wait:
            case goblinState.idle:
                anim.CrossFade(idle.name);
                break;
            case goblinState.patrol:
            case goblinState.move:
                anim.CrossFade(move.name);
                break;
            case goblinState.attack:
                anim.CrossFade(attack.name);
                break;
            case goblinState.die:
                anim.CrossFade(die.name);
                break;
        }
    }

    private void Start()
    {
        state = goblinState.idle;
        anim = GetComponent<Animation>();

        anim[idle.name].wrapMode = WrapMode.Loop;
        anim[move.name].wrapMode = WrapMode.Loop;
        anim[attack.name].wrapMode = WrapMode.Once;
        anim[damage.name].wrapMode = WrapMode.Once;
        anim[damage.name].layer = 10;
        anim[die.name].wrapMode = WrapMode.Once;
        anim[die.name].layer = 10;

        AddAnimationEvent(attack, "OnAttackFinish");
        AddAnimationEvent(damage, "OnDamageFinish");
        AddAnimationEvent(die, "OnDieFinish");

        skinnedMeshRenderer = transform.Find("body").GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        CheckState();
        AnimationControl();
    }
}
