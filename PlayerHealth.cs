using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float totalHealth = 100;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(totalHealth);
        if (totalHealth < 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameObject.SetActive(true);
            GameManager.Instance.StartGame();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Enemy") || collision.collider.CompareTag("SpawnedEnemy") || collision.collider.CompareTag("Spawner"))
        {
            totalHealth -= 20;
        }
        else if (collision.collider.CompareTag("EnemyBullet"))
        {
            EnemyBulletScript bulletScript = collision.collider.GetComponent<EnemyBulletScript>();
            if(bulletScript != null)
                totalHealth -= bulletScript.bulletDamage;
        }
    }
}
