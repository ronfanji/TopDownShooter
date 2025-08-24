using UnityEngine;
using System.Collections;

public class EnemyDash : MonoBehaviour
{
    private GameObject player;
    public EnemyState currState = EnemyState.Wander;
    private Transform target;
    public float range = 7.5f; // begin following range
    public float movementSpeed = 4f;
    public float wanderSpeed = 2f;

    private bool canDash = true;
    private bool isDashing = false;
    public float dashingPower = 15f;
    public float chargeTime = 1f;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 4f;
    private SpriteRenderer sprite;

    [SerializeField] Rigidbody2D enemyRb;

    // Wander logic
    private float targetX = 0f;
    private float targetY = 0f;
    private float timePassed = 0f;

    private bool hasLineOfSight = false;

    // saves concurrent player positional data only if there is line of sight directly to the player
    private Vector3 playerPosition;

    private bool isAwake = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        enemyRb = GetComponent<Rigidbody2D>();
        target = player.GetComponent<Transform>();
        playerPosition = player.transform.position;
        chooseLocation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || !isAwake)
        {
            return;
        }

        switch (currState)
        {
            case (EnemyState.Wander):
                timePassed += Time.deltaTime;
                Wander();
                break;
            case (EnemyState.Follow):
                Follow();
                break;
            case (EnemyState.Attack):
                StartCoroutine(Dash());
                break;

        }
        if (isPlayerInRange(range) && canDash && hasLineOfSight)
        {
            currState = EnemyState.Attack;
        }
        else if (isPlayerInRange(range) && hasLineOfSight)
        {
            currState = EnemyState.Follow;
        }
        else if (!isPlayerInRange(range) || !hasLineOfSight)
        {
            currState = EnemyState.Wander;
        }
    }

    void FixedUpdate()
    {
        // only runs when game object is awake
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
                playerPosition = player.transform.position;
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
        enemyRb.linearVelocity = direction * movementSpeed;
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

    IEnumerator Dash()
    {
        enemyRb.linearVelocity = new Vector2(0, 0);
        currState = EnemyState.Attack;
        isDashing = true;
        canDash = false;
        sprite.color = new Color(200, 0, 0);
        yield return new WaitForSeconds(chargeTime);
        Vector2 direction = (playerPosition - transform.position).normalized;
        enemyRb.linearVelocity = direction * dashingPower;

        yield return new WaitForSeconds(dashingTime);
        currState = EnemyState.Follow;
        isDashing = false;
        sprite.color = new Color(73 / 255f, 32 / 255f, 115 / 255f);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isAwake = true;
        }
    }

}