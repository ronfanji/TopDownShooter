using UnityEngine;

public class KeyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool hasKey = false;

    private Rigidbody2D rb;
    private PolygonCollider2D pc;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            // activate key boolean on player or something
            Destroy(gameObject);
            hasKey = true;
            Debug.Log("Got Key!");
        }
    }
}
