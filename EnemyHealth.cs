using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float totalHealth = 50f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (totalHealth <= 0f)
        {
            Destroy(gameObject);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("Bullet"))
        {
            BulletScript bulletScript = collision.collider.GetComponent<BulletScript>();
            if(bulletScript != null)
                totalHealth -= bulletScript.bulletDamage;
        }
    }
}
