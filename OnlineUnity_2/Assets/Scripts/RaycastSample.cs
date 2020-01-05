using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSample : MonoBehaviour
{
    public GUISkin skin = null;
    private Vector3 pickPosition = Vector3.zero;

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit = new RaycastHit();
        /*
        if (Physics.Raycast(ray, out raycastHit))
        {
            pickPosition = raycastHit.point;
        }*/

        int backgroundLayer = LayerMask.NameToLayer("background");
        int pickLayer = 1 << backgroundLayer;

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, pickLayer))
        {
            pickPosition = raycastHit.point;
        }
    }

    private void OnGUI()
    {
        GUI.color = Color.black;
        GUI.skin = skin;
        GUILayout.Label("Point : " + pickPosition.ToString());
    }
}
