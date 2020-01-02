using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAtTime : MonoBehaviour
{
    public float time;
    
    void Start()
    {
        Destroy(gameObject, time);
    }
}
