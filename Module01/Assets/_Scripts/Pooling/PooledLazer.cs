
using UnityEngine;

public class PooledLaser : MonoBehaviour
{
    SimplePool pool;
    ParticleSystem[] particles;
    float life = 2f;
    float t;

    public void Init(SimplePool p)
    {
        pool = p;
        t = 0f;
        if (particles == null)
            particles = GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
            ps.Play();
    }

    void Update()
    {
        t += Time.deltaTime;
        if (t >= life)
            pool.Return(gameObject);
    }
}