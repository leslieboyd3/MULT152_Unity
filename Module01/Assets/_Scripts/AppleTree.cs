using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Inscribed")]
    // Prefab for instantiating apples
    public GameObject applePrefab;
    public GameObject goldenApplePrefab;
    public GameObject poisonApplePrefab;

    // Speed at which the AppleTree moves
    public float speed = 1f;

    // Distance where the AppleTree turns around
    public float leftAndRightEdge = 10f;

    // Seconds between apple instantiations
    public float changeDirChance = 0.1f;

    public float appleDropDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Start dropping apples
        Invoke("DropApple", appleDropDelay);
    }

    void DropApple()
    {
        float appleChooser = Random.Range(0f, 9f);

        if (appleChooser < 1.0f)
        {
            GameObject apple = Instantiate<GameObject>(goldenApplePrefab);

            apple.transform.position = transform.position;
        }
        else if (appleChooser > 7.0f)
        {
            GameObject apple = Instantiate<GameObject>(poisonApplePrefab);

            apple.transform.position = transform.position;
        }
        else
        {
            GameObject apple = Instantiate<GameObject>(applePrefab);

            apple.transform.position = transform.position;
        }


            Invoke("DropApple", appleDropDelay);
    }

    // Update is called once per frame
    void Update()
    {
        // Basic Movement
        Vector3 pos = transform.position;
        pos.x += speed * Time.deltaTime;
        transform.position = pos;

        // Changing Directions   
        if (pos.x < -leftAndRightEdge)
        {
            speed = Mathf.Abs(speed); // Move right
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed); // Move left
        } 
    }

    void FixedUpdate()
    {
        // Random direction changes are now time-based due to FixedUpdate() 
        if (Random.value < changeDirChance)
        {
            speed *= -1;    // Change direction
        }
    }
}
