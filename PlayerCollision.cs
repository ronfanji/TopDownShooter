using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    Vector2 startPos;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] public GameObject circlePrefab;
    [SerializeField] public GameObject squarePrefab;
    [SerializeField] public GameObject trianglePrefab;

    private GameObject currentPlayer;
    private int playerCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        //currentPlayer = Instantiate(circlePrefab, transform.position, Quaternion.identity);
        //startPos = currentPlayer.transform.position;
        playerCounter = 0;
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(GetComponent<Collider>().gameObject);
            StartCoroutine(Die());
            // if (playerCounter == 0)
            // {
            //     SwitchPlayer(squarePrefab);
            // }
            // else if (playerCounter == 1)
            // {
            //     SwitchPlayer(trianglePrefab);
            // }
            // else
            // {
            //     StartCoroutine(Die());
            // }
        }
        if (collision.collider.CompareTag("Bullet"))
        {
            BulletScript bulletScript = collision.collider.GetComponent<BulletScript>();
            // if(bulletScript != null){
            //     totalHealth -= bulletScript.bulletDamage;
            // }
        }
    }

    IEnumerator Die()
    {
        Debug.Log("hit");
        Destroy(currentPlayer);
        yield return new WaitForSeconds(0);
        Respawn();
    }
    void Respawn()
    {
        currentPlayer.transform.position = startPos;
    }

    void SwitchPlayer(GameObject newPrefab)
    {
        if (currentPlayer != null)
        {
            Vector3 lastPos = currentPlayer.transform.position;
            Destroy(currentPlayer);
            currentPlayer = Instantiate(newPrefab, lastPos, Quaternion.identity);
            playerCounter += 1;
        }
    }
}
