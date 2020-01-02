using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public GameObject bullet;
    public GameObject explosion;
    public AudioSource boom;
    private bool start = true;

    void Update()
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);

        if (transform.position.y < -50.0f)
        {
            regen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bullet")
        {
            boom.Play();
            MainControl.score++;
            Instantiate(explosion, transform.position, Quaternion.identity);
            regen();
            Destroy(other.gameObject);
        }
        if (other.tag == "Player")
        {
            MainControl.life--;
        }
    }

    void regen()
    {
        transform.position = new Vector3(Random.Range(-60.0f, 60.0f), 50.0f, 0.0f);
    }
}