using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    static DamagePopup _prefab;
    static Transform _poolRoot;

    [SerializeField] private TMP_Text text;
    [SerializeField] private float riseSpeed = 1f;
    [SerializeField] private float life = 0.75f;

    float _t;

    public static void Spawn(Vector3 worldPos, int dmg)
    {
        if (!_prefab)
        {
            // Load from Resources or assign in a bootstrap. For simplicity:
            Debug.LogWarning("DamagePopup prefab not assigned. Create PF_DamagePopup and assign via DamagePopupBootstrap or Resources.");
            return;
        }

        var go = Instantiate(_prefab.gameObject, worldPos, Quaternion.identity);
        var dp = go.GetComponent<DamagePopup>();
        dp.Setup(dmg);
    }

    public void Setup(int dmg)
    {
        if (text) text.text = dmg.ToString();
        _t = 0f;
    }

    void Update()
    {
        // billboard to camera
        var cam = Camera.main;
        if (cam) transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);

        transform.position += Vector3.up * (riseSpeed * Time.deltaTime);
        _t += Time.deltaTime;
        if (_t >= life) Destroy(gameObject);
    }

    // Utility to register the prefab at startup (call once)
    public static void RegisterPrefab(DamagePopup prefab)
    {
        _prefab = prefab;
        if (!_poolRoot)
        {
            _poolRoot = new GameObject("DamagePopupPool").transform;
            Object.DontDestroyOnLoad(_poolRoot.gameObject);
        }
    }
}