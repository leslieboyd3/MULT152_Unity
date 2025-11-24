using UnityEngine;

public class DamagePopupBootstrap : MonoBehaviour
{
    public DamagePopup damagePopupPrefab;

    void Awake()
    {
        if (damagePopupPrefab) DamagePopup.RegisterPrefab(damagePopupPrefab);
    }
}