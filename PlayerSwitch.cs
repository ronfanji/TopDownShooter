using UnityEngine;

public class PlayerSwitch : MonoBehaviour
{

    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spr;
    [SerializeField] BulletScript bulletScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
