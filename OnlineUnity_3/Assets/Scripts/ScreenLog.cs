using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLog : SingletonMonoBehavior<ScreenLog>
{
    ArrayList LogList = new ArrayList();

    private void Start()
    {
        Application.logMessageReceived += CaptureLog;
    }

    void CaptureLog(string condition, string stackTrack, UnityEngine.LogType logType)
    {
        LogList.Add(condition);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LogList.Clear();
        }
    }

    private void OnGUI()
    {
        if (LogList != null && LogList.Count > 0)
        {
            foreach (string log in LogList)
            {
                GUILayout.Label(log);
            }
        }
    }
}
