using UnityEngine;
using System.Collections;

public class EnemyRanged : MonoBehaviour
{

    private GameObject player;
    public EnemyState currState = EnemyState.Wander;
    private Transform target;
    public Transform bulletTransform;
    public GameObject bullet;
    public float range = 12f;
    public float escapeRange = 3f;
    public float dashingPower = 10f;
    public float dashingTime = 1.5f;
    public float dashingCooldown = 1f;
    public float shootingCooldown = 3f;
    private bool canDash = true;
    private bool isDashing = false;
    private bool canFire = true;
    private bool initiateAttack = false;
    private bool hasLineOfSight = false;
    private bool isAwake = false;
    private float timer = 0f;
    private EnemyBulletScript bs;


    private Rigidbody2D enemyRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRb = GetComponent<Rigidbody2D>();
        target = player.GetComponent<Transform>();
        bs = bullet.GetComponent<EnemyBulletScript>();
    }

    // make like a flamethrower-esque ranged attack so transformation can be like a spit monster
    // have ability to dash back when player gets too close

    // Update is called once per frame
    void Update()
    {


        switch (currState)
        {
            case (EnemyState.Wander):
                //timePassed += Time.deltaTime;
                //Wander();
                break;
            case (EnemyState.Follow):
                //Follow();
                break;
            case (EnemyState.Attack):

                break;

        }
        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > bs.fireRate)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (hasLineOfSight && isPlayerInRange(range))
        {
            Vector3 rotation = player.transform.position - transform.position;
            // Debug.Log(rotation);

            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, rotZ + 33);

            //if (initiateAttack && canFire)
            if (canFire)
            {
                Debug.Log("Fire");
                canFire = false;
                Instantiate(bullet, bulletTransform.position, Quaternion.identity);
            }

        }

        if (isDashing) return;

        
        if (isPlayerInRange(escapeRange) && canDash && hasLineOfSight)
        {
            Debug.Log("Dash");
            StartCoroutine(Evade());
        }
    }

    void FixedUpdate()
    {
        // if enemy isn't awake, don't move
        // if (!isAwake) return;

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


    IEnumerator ShootBullets()
    {
        initiateAttack = true;
        yield return new WaitForSeconds(shootingCooldown);
        initiateAttack = false;
    }
    IEnumerator Evade()
    {
        enemyRb.linearVelocity = new Vector2(0, 0);
        isDashing = true;
        canDash = false;

        Vector2 direction = (transform.position - player.transform.position).normalized;
        enemyRb.linearVelocity = direction * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
}
