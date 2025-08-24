using UnityEngine;
using System;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{

    private GameObject player;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform enemyParent;
    public Transform northSpawn;
    public Transform eastSpawn;
    public Transform westSpawn;
    public Transform southSpawn;
    public float enemySpawnTime = 5f;
    public float spawnRange = 7f;
    private float timeUntilEnemySpawn;
    public bool inRange = false;
    private GameObject[] enemies;
    public int maxNumberEnemies = 5;

    private bool hasLineOfSight = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemies = new GameObject[maxNumberEnemies];
        timeUntilEnemySpawn = enemySpawnTime-0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(inRange && hasLineOfSight)
            SpawnLoop();
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
            if (hit.collider.gameObject == gameObject || hit.collider.CompareTag("SpawnerTransform")) continue;  // skip self collider

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
    private void SpawnLoop()
    {
        timeUntilEnemySpawn += Time.deltaTime;
        if(timeUntilEnemySpawn >= enemySpawnTime){
            Spawn();
            timeUntilEnemySpawn = 0f;
            // enemySpawnTime = Random.Range(0.75f, 2.5f); // to randomize spawn times
        }
    }
    private void Spawn()
    {

        int index = UnityEngine.Random.Range(0, enemyPrefabs.Length); // to determine which enemy to spawn, is randomized

        enemies = GameObject.FindGameObjectsWithTag("SpawnedEnemy");
        Debug.Log(enemies.Length);
        if (enemies.Length > maxNumberEnemies) return;

        GameObject enemyToSpawn = enemyPrefabs[0];
        Transform spawnLocation;

        Vector3 rotation = player.transform.position - transform.position;

        if (Math.Abs(rotation.x) > Math.Abs(rotation.y))
        {
            if (rotation.x < 0){
                spawnLocation = westSpawn;
            }
            else{
                spawnLocation = eastSpawn;
            }
        }
        else
        {
            if (rotation.y < 0){
                spawnLocation = southSpawn;
            }
            else{
                spawnLocation = northSpawn;
            }
        }

        GameObject spawnedEnemy = Instantiate(enemyToSpawn, spawnLocation.position, Quaternion.identity);

    }
}
