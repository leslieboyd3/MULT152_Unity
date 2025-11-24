
using UnityEngine;

public class PooledParticle : MonoBehaviour
{
    SimplePool pool;
    ParticleSystem ps;

    public void Init(SimplePool p)
    {
        pool = p;
        ps = GetComponent<ParticleSystem>();
        ps.Play();
    }

    void Update()
    {
        if (!ps.IsAlive(true))
        {
            pool.Return(gameObject);
        }
    }
}