using UnityEngine;

public abstract class IDamageable : MonoBehaviour
{ 
    [SerializeField] protected float _health;
    [SerializeField] protected float healthMax; 

    void Start()
    {
        _health = healthMax;
    }

    public virtual void ApplyDamaged(float damage)
    {
        _health -= damage;
        if(_health > healthMax)
            _health = healthMax;
        if (_health <= 0)
        {
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

    public bool IsFullHealth()
    {
        return _health == healthMax;
    }
}

