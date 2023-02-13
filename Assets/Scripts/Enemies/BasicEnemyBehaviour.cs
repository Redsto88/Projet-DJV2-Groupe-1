using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyBehaviour : MonoBehaviour, IDamageable
{
    private List<Material> _materials = new List<Material>();
    private List<Color> _initMaterialsColor = new List<Color>();
    [SerializeField] private AnimationCurve curve;

    public bool isTest = false;
    
    [Header("Stats")] 
    [SerializeField] protected float healthMax = 30;
    protected float _health;

    public NavMeshAgent navMeshAgent;
    protected Transform _target;
    
    public bool portalFlag;
    public bool attackFlag;
    
    private Animator _animator;
    private static readonly int IsWalking = Animator.StringToHash("isWalking");

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Renderer[] renderer = GetComponentsInChildren<Renderer>();
        foreach (var render in renderer)
        {
            foreach (var mat in render.materials)
            {
                _materials.Add(mat);
                _initMaterialsColor.Add(mat.color);
            }
        }
        
        _health = healthMax;
        
        navMeshAgent = GetComponent<NavMeshAgent>();
        _target = PlayerController.Instance.transform;

        _animator = GetComponentInChildren<Animator>();
        
        portalFlag = false;
        attackFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_target.IsUnityNull() && navMeshAgent.path.status == NavMeshPathStatus.PathComplete && !portalFlag && !attackFlag)
        {
            if ((transform.position - _target.position).magnitude > navMeshAgent.stoppingDistance)
            {
                _animator.SetBool(IsWalking, true);
                navMeshAgent.destination = _target.position;
            }
            else
            {
                navMeshAgent.destination = transform.position;
                Attack();
            }
        }
        
        else if (_target.IsUnityNull() || navMeshAgent.path.status != NavMeshPathStatus.PathComplete)
        {
            _animator.SetBool(IsWalking, false);
            navMeshAgent.destination = transform.position;
        }
    }

    private void Attack()
    {
        print("attack");
        if (!attackFlag)
        {
            attackFlag = true;
            _animator.CrossFade("Attack_01", 0.1f);
        }
    }


    // IDamageable
    public void ApplyDamaged(float damage)
    {
        StartCoroutine(ColorCoroutine());
        _health -= damage;

        if (_health <= 0)
        {
            if (RoomBehaviour.Instance != null)
            {
                RoomBehaviour.Instance.CountEnemyDeath();
            }
            
            Destroy(gameObject);
        }
    }

    public float GetHealth()
    {
        return _health;
    }

    public float GetHealthMax()
    {
        return healthMax;
    }


    IEnumerator ColorCoroutine()
    {
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

                material.color = Color.Lerp(_initMaterialsColor[i], 10*Color.white, curve.Evaluate(duration - timeLeft));
            }

            timeLeft -= Time.deltaTime;
            yield return null;
        }
    }
}
