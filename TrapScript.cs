using UnityEngine;
using System.Collections;

public class TrapScript : MonoBehaviour
{

    private GameObject player;
    private bool hasLineOfSight;
    private bool enterRange;
    private bool startMove;
    private bool stopMoving;
    Rigidbody2D trapRb;
    public float moveSpeed = 5f;
    private Vector3 direction;
    [SerializeField] Transform directionTransform;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasLineOfSight = false;
        enterRange = false;
        stopMoving = false;
        player = GameObject.FindGameObjectWithTag("Player");
        trapRb = GetComponent<Rigidbody2D>();
        direction = (transform.position - directionTransform.transform.position).normalized;

    }

    // Update is called once per frame
    void Update()
    {

        if (enterRange && hasLineOfSight) // move in direction
        {
            startMove = true;
            Debug.Log("START MOVING");
        }
        if (startMove && !stopMoving)
        {
            trapRb.linearVelocity = direction * moveSpeed;
            Debug.Log("IS MOVING");
        }
        else if (stopMoving)
        {
            Destroy(gameObject);
        }
        
    }

    void FixedUpdate()
    {

        Vector2 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("SpawnedEnemy") || hit.collider.CompareTag("Obstacle")) continue;  // skip self collider
            if (hit.collider.CompareTag("Trap")) continue;

            if (hit.collider.CompareTag("Player"))
            {
                hasLineOfSight = true;
                Debug.DrawRay(transform.position, direction * distance, Color.green);
                break;
            }
            else
            {
                hasLineOfSight = false;
                Debug.DrawRay(transform.position, direction * distance, Color.red);
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        // trap triggers once something enters range
        if (collider.gameObject.CompareTag("Player") || collider.gameObject.CompareTag("Bullet"))
        {
            enterRange = true;
            Debug.Log("ENTERED RANGE");
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.CompareTag("Obstacle"))
        {
            stopMoving = true;
        }
        else if (collider.gameObject.CompareTag("Player"))
        {

        }
        else if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("SpawnedEnemy"))
        {
            
        }
        
    }

    private IEnumerator Trap()
    {

        yield return new WaitForSeconds(0f);
    }

}
