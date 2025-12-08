using UnityEngine;

public class PoisonApple : MonoBehaviour
{
    public static float bottomY = -20f;

    void Update()
    {
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);

            // don't call AppleMissed for PoionApples as player wants to avoid them
            // Get a reference to the ApplePicker component of Main Camera
            //ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();

            // Call the public AppleMissed() method of apScript
            // apScript.AppleMissed();
        }
    }
}
