using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineSample : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start 1: " + Time.time.ToString());
        StartCoroutine(TestCoroutine());
        Debug.Log("Start 2: " + Time.time.ToString());
    }

    IEnumerator TestCoroutine()
    {
        Debug.LogWarning("TestCoroutine 1: " + Time.time.ToString());
        yield return new WaitForSeconds(1.0f);
        Debug.LogWarning("TestCoroutine 2: " + Time.time.ToString());
        yield return new WaitForSeconds(2.0f);
        Debug.LogWarning("TestCoroutine 3: " + Time.time.ToString());
    }
}
