using UnityEngine;
using System.Collections;

public enum EnemyState { Wander, Follow, Still, Attack, Evade};

public class EnemyFollow : MonoBehaviour
{
    private GameObject player;
    public EnemyState currState = EnemyState.Wander;
    private EnemyState prevState;
    private Transform target;

    Rigidbody2D enemyRb;
    public float range = 10f; // begin following range q
    public float moveSpeed = 5f; // how fast enemy moves
    public float wanderSpeed = 3f;
    public float waitTime = 2f;

    // Wander logic
    private float targetX = 0f;
    private float targetY = 0f;

    private bool hasLineOfSight = false;
    private bool isTired = false;
    private float timePassed = 0f;
    private bool isAwake = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRb = GetComponent<Rigidbody2D>();
        target = player.GetComponent<Transform>();
        chooseLocation();
    }

    // Update is called once per frame
    void Update()
    {

        if (!isAwake) return;

        if (prevState == EnemyState.Follow && currState != EnemyState.Follow)
        {
            isTired = true;
        }
        prevState = currState;

        switch (currState)
        {
            case (EnemyState.Wander):
                timePassed += Time.deltaTime;
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Still):
                StartCoroutine(Still());
                break;
        }


        if (hasLineOfSight && isPlayerInRange(range))
        {
            currState = EnemyState.Follow;
            Debug.Log("In Range");
        }
        else if (isTired)
        {
            currState = EnemyState.Still;
        }
        else if (!hasLineOfSight || !isPlayerInRange(range))
        {
            Debug.Log("Too Far");
            currState = EnemyState.Wander;
        }
    }

    void FixedUpdate()
    {
        // if enemy isn't awake, don't move
        if (!isAwake) return;

        Vector2 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("SpawnedEnemy") || hit.collider.CompareTag("SpawnerTransform")) continue;  // skip self collider

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
    bool isPlayerInRange(float range)
    {
        return Vector2.Distance(transform.position, player.transform.position) <= range;
    }

    void Follow()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        enemyRb.linearVelocity = direction * moveSpeed;
    }
    void Wander()
    {
        float distance = Vector2.Distance(transform.position, new Vector2(targetX, targetY));
        if (distance < 0.1 || timePassed > 2f)
        {
            chooseLocation();
        }
        else
        {
            Vector2 targetPos = new Vector2(targetX, targetY);
            Vector2 currentPos = transform.position;
            Vector2 direction = (targetPos - currentPos).normalized;
            enemyRb.linearVelocity = direction * wanderSpeed;
        }
    }
    void chooseLocation()
    {
        targetX = Random.Range(-8.5f, 8.5f);
        targetY = Random.Range(-4.5f, 4.5f);
        timePassed = 0f;
    }

    IEnumerator Still()
    {
        enemyRb.linearVelocity = new Vector2(0, 0);
        yield return new WaitForSeconds(waitTime);
        isTired = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        isAwake = true;
    }
    

}