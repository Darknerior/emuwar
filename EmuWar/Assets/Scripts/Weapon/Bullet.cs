using UnityEngine;
using Interfaces;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour , IPoolable
{
    private Rigidbody rb;
    private Weapon weapon;
    private LayerMask mask;
    private float range;
    [SerializeField] private float timeOut;
    private int damage;
    private Vector3 startPos;
    private float clock;


    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Sets initial settings for the bullet to fire, and activates the gameobject.
    /// </summary>
    public void SetFactors(Weapon parent, int bulletDamage, float bulletRange ,Vector3 position, Vector3 forwardPos, float speed)
    {
        weapon = parent;
        damage = bulletDamage;
        transform.position = position + forwardPos.normalized;
        transform.forward = forwardPos;
        range = bulletRange;
        mask = (LayerMask)parent.gameObject.layer;
        gameObject.layer = mask;
        gameObject.SetActive(true);
        rb.AddForce(forwardPos * speed, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        startPos = transform.position;
        clock = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        while (Vector3.Distance(transform.position, startPos) < range && clock < timeOut)
        {
            clock += Time.fixedDeltaTime;
            return;
        }
        
        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.layer != mask && other.gameObject.TryGetComponent(out IDamageable health))
        {
            health.TakeDamage(damage);
        }
        ReturnToPool();
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }

}
