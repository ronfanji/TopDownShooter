using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{

    [SerializeField] public GameObject spawner;
    private EnemySpawner enemySpawner;
    private SpriteRenderer spr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemySpawner = spawner.GetComponent<EnemySpawner>();
        spr = spawner.GetComponent<SpriteRenderer>();
        spr.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            enemySpawner.inRange = true;
            spr.color = Color.red;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            enemySpawner.inRange = false;
            spr.color = Color.blue;
        }
    }
}
