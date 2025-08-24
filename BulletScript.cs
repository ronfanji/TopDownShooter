using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D rb;
    public float bulletSpeed;
    public float lifetime = 2f;
    public float bulletDamage = 10f;
    public float fireRate = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        rb = GetComponent<Rigidbody2D>();
        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = -transform.position + mousePos;
        Vector3 rotation = transform.position - mousePos;

        rb.linearVelocity = new Vector2(direction.x, direction.y).normalized *  bulletSpeed;

        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot);

        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.transform.tag == "Enemy" || collider.transform.tag == "Spawner" || collider.transform.tag == "SpawnedEnemy")
        {
            Destroy(gameObject);
        }
        else if (collider.transform.tag == "Obstacle" || collider.transform.tag == "Light" || collider.transform.tag == "Bullet" || collider.transform.tag == "EnemyBullet")
        {
            // can add some kind of animation
            Destroy(gameObject);
        }
    }
}
