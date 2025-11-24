using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Pooling Settings")]
    public SimplePool pool; // Shared pool for projectiles and particles

    [Header("Prefabs")]
    public GameObject projectilePrefab; // For non-pooled comparison
    public GameObject particlePrefab;   // For non-pooled comparison

    [Header("Spawn Settings")]
    public Transform firePoint;
    public bool usePool = true;
    public bool spawnProjectile = true; // Toggle between projectile and particle

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (usePool)
            {
                var go = pool.Get(firePoint.position, firePoint.rotation);

                // Try to initialize as projectile or particle
                if (go.TryGetComponent<PooledProjectile>(out var proj))
                {
                    proj.Init(pool);
                }
                else if (go.TryGetComponent<PooledParticle>(out var particle))
                {
                    particle.Init(pool);
                }
            }
            else
            {
                if (spawnProjectile)
                {
                    var go = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                    Destroy(go, 2f);
                }
                else
                {
                    var go = Instantiate(particlePrefab, firePoint.position, firePoint.rotation);
                    // Particle will auto-disable after playing if Stop Action = Disable
                    Destroy(go, 5f); // Fallback cleanup
                }
            }
        }
    }
}