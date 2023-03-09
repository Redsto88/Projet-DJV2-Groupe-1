using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyBehaviour : ADamageable
{
    private List<Material> _materials = new List<Material>();
    private List<Color> _initMaterialsColor = new List<Color>();
    private List<Color> _initMaterialsEmissionColor = new List<Color>();
    [SerializeField] private AnimationCurve curve;

    public bool isTest = false;

    [Header("Stats")] 
    [SerializeField] protected float coolDown = 3f;
    

    public NavMeshAgent navMeshAgent;
    protected Transform _target;
    private float _timeSinceLastAttack;
    private Vector3 _velocity;
    
    public bool portalFlag;
    public bool attackFlag;
    public bool damageFlag;
    
    protected Animator animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int Color1 = Shader.PropertyToID("_Color");

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        foreach (var render in renderer)
        {
            foreach (var mat in render.materials)
            {
                _materials.Add(mat);
                _initMaterialsColor.Add(mat.GetColor(Color1));
                _initMaterialsEmissionColor.Add(mat.GetColor(EmissionColor));
            }
        }
        
        _health = healthMax;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        _target = PlayerController.Instance.transform;

        animator = GetComponentInChildren<Animator>();

        _timeSinceLastAttack = 0;
        
        portalFlag = false;
        attackFlag = false;
        damageFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (damageFlag)
        {
            navMeshAgent.destination = transform.position;
        }
        else
        {
            if (navMeshAgent.IsUnityNull()) return;
            print(navMeshAgent.path.status);
            for (int i = 0; i < navMeshAgent.path.corners.Length - 1; i++)
                Debug.DrawLine(navMeshAgent.path.corners[i], navMeshAgent.path.corners[i + 1], Color.red);

            if (!_target.IsUnityNull())
            {
                if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
                {
                    if (navMeshAgent.path.status == NavMeshPathStatus.PathComplete && !portalFlag && !attackFlag)
                    {
                        print("avance");
                        animator.SetBool(IsWalking, true);
                        navMeshAgent.destination = _target.position;
                    }
                    else
                    {
                        print("idle");
                        animator.SetBool(IsWalking, false);
                        navMeshAgent.destination = transform.position;
                    }
                }
                else
                {
                    navMeshAgent.destination = transform.position;
                    Attack();
                }
            }

            else if (_target.IsUnityNull() || navMeshAgent.path.status != NavMeshPathStatus.PathComplete)
            {
                animator.SetBool(IsWalking, false);
                navMeshAgent.destination = transform.position;
            }

            _timeSinceLastAttack += Time.deltaTime;
        }
    }

    private void Attack()
    {
        transform.LookAt(_target.transform);
        if (!attackFlag && _timeSinceLastAttack>coolDown)
        {
            attackFlag = true;
            animator.CrossFade("Attack_01", 0.1f);
            _timeSinceLastAttack = 0f;
        }
    }


    // IDamageable
    public override void ApplyDamage(float damage)
    {
        StartCoroutine(ColorCoroutine());
        base.ApplyDamage(damage);
        animator.CrossFade("Damage",0.2f);
        if (_health <= 0) RoomBehaviour.Instance.CountEnemyDeath();
    }

    IEnumerator ColorCoroutine()
    {
        print("color");
        const float duration = 0.5f;
        var timeLeft = duration;

        while (timeLeft > 0f)
        {
            var lerpValue = timeLeft > duration / 2f
                ? 2f * (1f - timeLeft / duration)
                : 2f * timeLeft / duration;


            for (var i = 0; i < _materials.Count; i += 1)
            {
                var material = _materials[i];
                
                material.SetColor(Color1, Color.Lerp(_initMaterialsColor[i], 2*Color.white, curve.Evaluate(duration - timeLeft)));
                material.SetColor(EmissionColor, Color.Lerp(_initMaterialsEmissionColor[i], 2*Color.white, curve.Evaluate(duration - timeLeft)));
            }

            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
