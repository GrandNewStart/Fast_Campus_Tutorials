using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public AudioSource shot;

    void Update()
    {
        float move = Input.GetAxisRaw("Horizontal");
        transform.Translate(new Vector3(-move, 0, 0) * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump"))
        {
            shot.Play();
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }


}
