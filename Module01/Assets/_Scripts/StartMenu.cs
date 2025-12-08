using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene("_Scene_0");
    }

    public void QuitApplication()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    /*****
     
    public void ToggleMenu()
    {
        if (isMainMenuShowing)
        {
            isMainMenuShowing = false;
            canvasGroup.alpha = 0;
            Time.timeScale = 1;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        isMainMenuShowing = true;
        canvasGroup.alpha = 1;
        Time.timeScale = 0;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }

    *****/
}
