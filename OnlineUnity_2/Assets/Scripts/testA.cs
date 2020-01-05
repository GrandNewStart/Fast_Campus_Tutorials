using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testA : MonoBehaviour
{
    public GameObject target = null;

    void Start()
    {
        //target.GetComponent<testB>().testMessage("asdf");
        target.SendMessage("testMessage", "Hello!", SendMessageOptions.DontRequireReceiver);
        gameObject.SendMessageUpwards("testMessage", "World!", SendMessageOptions.DontRequireReceiver);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
