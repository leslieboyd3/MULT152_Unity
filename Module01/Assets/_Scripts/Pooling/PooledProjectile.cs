using UnityEngine;

public class PooledProjectile : MonoBehaviour
{
    public float speed = 20f;
    public float life = 2f;
    SimplePool pool;
    float t;
    ParticleSystem ps;

    public void Init(SimplePool p){ pool = p; ps.GetComponent<ParticleSystem>();ps.Play(); t = 0f; }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        t += Time.deltaTime;
        if (t >= life) pool.Return(gameObject);

        if (!ps.IsAlive(true))
        {
            pool.Return(gameObject);
        }
    }

    void OnCollisionEnter(Collision _){
        pool.Return(gameObject);
    }
}