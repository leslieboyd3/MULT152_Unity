using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplePicker : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject basketPrefab;

    public int numBaskets = 3;
    public float basketBottomY = -14f;
    public float basketSpacingY = 2f;

    public List<GameObject> basketList;

    void Start()
    {
        basketList = new List<GameObject>();

        for (int i=0; i < numBaskets; i++)
        {
            GameObject tBasketGO = Instantiate<GameObject>(basketPrefab);

            Vector3 pos = Vector3.zero;

            pos.y = basketBottomY + (basketSpacingY * i);

            tBasketGO.transform.position = pos;

            basketList.Add(tBasketGO);
        }
        
    }

    public void AppleMissed()
    {
        // Destroy all of the falling Apples
        GameObject[] appleArray = GameObject.FindGameObjectsWithTag("Apple");

        foreach (GameObject tempGO in appleArray)
        {
            Destroy(tempGO);
        }

        // Destroy all of the falling GoldenApples
        GameObject[] goldenAppleArray = GameObject.FindGameObjectsWithTag("GoldenApple");

        foreach (GameObject tempGO in goldenAppleArray)
        {
            Destroy(tempGO);
        }
        // Destroy all of the falling PoisonApples
        GameObject[]poisonAppleArray = GameObject.FindGameObjectsWithTag("PoisonApple");

        foreach (GameObject tempGO in poisonAppleArray)
        {
            Destroy(tempGO);
        }

        // Destroy one of the baskets
        // Get the index of the last Basket in basketList
        int basketIndex = basketList.Count - 1;

        // Get a reference to that Basket GameObject
        GameObject basketGO = basketList[basketIndex];

        // Remove the Basket from the list and destroy the GameObject
        basketList.RemoveAt(basketIndex);
        Destroy(basketGO);

        // If there are no Baskets left, restart the game
        if (basketList.Count == 0)
        {
            SceneManager.LoadScene("_Game_Over");
        }
    }

}
