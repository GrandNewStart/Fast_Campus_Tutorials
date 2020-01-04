using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Properties")]
    public GameObject target = null;

    public enum POVState {FIRST, SECOND, THIRD}
    public POVState camState = POVState.THIRD;

    [Header("3rd person POV")]
    public float distance = 5.0f;
    public float height = 1.5f;
    public float heightDamping = 2.0f;
    public float rotatetionDamping = 3.0f;

    [Header("2nd person POV")]
    public float rotateSpeed = 10.0f;

    [Header("1st person POV")]
    public float sensitivityX = 5.0f;
    public float sensitivityY = 5.0f;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;
    public Transform firstCamera = null;

    void ThirdView()
    {
        float targetRotationAngle = target.transform.eulerAngles.y;
        float targetHeight = target.transform.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, targetRotationAngle, rotatetionDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, targetHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0f, currentRotationAngle, 0f);

        transform.position = target.transform.position;
        transform.position -= currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(target.transform);
    }

    void SecondView()
    {
        transform.RotateAround(target.transform.position, Vector3.up, rotateSpeed * Time.deltaTime);
        transform.LookAt(target.transform);
    }

    void FirstView()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX = transform.localEulerAngles.y + mouseX * sensitivityX;
        rotationY = rotationY + mouseY * sensitivityY;

        rotationX = (rotationX > 180.0f) ? rotationX - 360.0f : rotationX;
        rotationY = (rotationY > 180.0f) ? rotationY - 360.0f : rotationY;

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0f);
        transform.position = firstCamera.position;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        if (target == null) return;

        switch (camState)
        {
            case POVState.FIRST:
                FirstView();
                break;
            case POVState.SECOND:
                SecondView();
                break;
            case POVState.THIRD:
                ThirdView();
                break;
        }
    }
}
