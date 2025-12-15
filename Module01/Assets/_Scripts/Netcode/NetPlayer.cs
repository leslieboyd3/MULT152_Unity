using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class NetPlayer : NetworkBehaviour
{
    public float moveSpeed = 4f;
    CharacterController cc;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    void update()
    {
        if (!IsOwner) return;
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        var wish = new Vector3(h, 0, v).normalized * moveSpeed;

        if (wish != Vector3.zero)
        {
            MoveServerRPC(wish * Time.deltaTime);
        }
    }

    [ServerRpc]
    void MoveServerRPC(Vector3 delta)
    {
        if (cc) cc.Move(transform.TransformDirection(delta));
        else transform.position += transform.TransformDirection(delta);
    }
}