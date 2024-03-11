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
    public int damage = 1;
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
    /// <param name="parent"></param>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    /// <param name="forwardPos"></param>
    /// <param name="speed"></param>
    public void SetFactors(Weapon parent, int damage, float range ,Vector3 position, Vector3 forwardPos, float speed)
    {
        weapon = parent;
        this.damage = damage;
        transform.position = position;
        transform.forward = forwardPos;
        this.range = range;
        mask = (LayerMask)parent.gameObject.layer;
        this.gameObject.layer = mask;
        gameObject.SetActive(true);
        //rb.AddRelativeForce(forwardPos * speed, ForceMode.Impulse);
        rb.AddForce(forwardPos * speed, ForceMode.Impulse);
    }

    private void OnEnable()
    {
        startPos = transform.position;
        clock = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        while (Vector3.Distance(transform.position, startPos) < range && clock < timeOut)
        {
            clock += Time.deltaTime;
            return;
        }
        
        ReturnToPool();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == mask)
        {
            return;
        }
        if (other.gameObject.TryGetComponent(out IHaveHealth health))
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
