using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryController : MonoBehaviour
{
    private void OnGUI()
    {
        float centerX = Screen.width * 0.5f;
        float centerY = Screen.height * 0.5f;
        Rect ButtonRect = new Rect(centerX - 100.0f, centerY - 100.0f, 200.0f, 200.0f);
        
        if (GUI.Button(ButtonRect, "You Win!!"))
        {
            SceneManager.LoadScene(0);
        }
    }
}
