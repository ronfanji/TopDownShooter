using UnityEngine;

public class SceneTriggerScript : MonoBehaviour
{

    private CircleCollider2D cc;
    private Rigidbody2D rb;
    public GameObject key;
    private KeyScript ks;
    private bool keyPicked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ks = key.GetComponent<KeyScript>();
        cc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        keyPicked = ks.hasKey;
        if (keyPicked)
        {
            Debug.Log("can leave");
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && keyPicked)
        {
            Debug.Log("Escape");
        }
    }

}
