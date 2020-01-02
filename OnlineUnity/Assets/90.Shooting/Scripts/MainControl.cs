using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainControl : MonoBehaviour
{
    static public int score = 0;
    static public int life = 3;
    public GUISkin skin;

    private void OnGUI()
    {
        GUI.skin = skin;
        Rect labelRect1 = new Rect(10.0f, 10.0f, 400.0f, 100.0f);
        GUI.Label(labelRect1, "Score : " + score);
        Rect labelRect2 = new Rect(10.0f, 50.0f, 400.0f, 100.0f);
        GUI.Label(labelRect2, "Life : " + life);
    }

    private void Update()
    {
        if (score > 20)
        {
            score = 0;
            SceneManager.LoadScene(1);
        }
    }
}
