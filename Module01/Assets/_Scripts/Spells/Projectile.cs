using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float lifeSeconds = 5f;
    public LayerMask hitMask = ~0; // everything by default
    public GameObject hitVfx;
    public AudioClip hitSfx;
    [Range(0f, 1f)] public float sfxVolume = 0.8f;

    Rigidbody _rb;
    float _timer;
    Transform _source; // optional: who fired

    public void Init(Transform source, Vector3 velocity, int dmg, float life)
    {
        _source = source;
        damage = dmg;
        lifeSeconds = life;

        if (!_rb) _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = velocity;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        var col = GetComponent<Collider>();
        col.isTrigger = false;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= lifeSeconds) Destroy(gameObject);
    }

    void OnCollisionEnter(Collision c)
    {
        // prevent hitting the caster immediately
        if (_source && c.transform == _source) return;

        // Try damage
        var hp = c.transform.GetComponentInParent<HealthComponent>();
        if (hp != null && !hp.IsDead)
        {
            hp.Damage(Mathf.Max(1, damage));
            DamagePopup.Spawn(hp.transform.position + Vector3.up * 1.6f, damage);
        }

        if (hitVfx) Instantiate(hitVfx, c.contacts[0].point, Quaternion.identity);
        if (hitSfx) AudioSource.PlayClipAtPoint(hitSfx, c.contacts[0].point, sfxVolume);

        Destroy(gameObject);
    }
}