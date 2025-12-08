using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // Enables use of UI classes like Text

public class ScoreCounter : MonoBehaviour
{
   // [Header("Dynamic")]
    public int score = 0;

    private Text uiText;

    // Start is called before the first frame update
    void Start()
    {
       uiText = GetComponent<Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        // Formats with comma separators and shows 0 if score is zero
        uiText.text = score.ToString("#,0");
    }
}
