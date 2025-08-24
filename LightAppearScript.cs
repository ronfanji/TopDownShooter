using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class LightAppearScript : MonoBehaviour
{
    private GameObject player;
    private Transform target;
    private SpriteRenderer sprite;
    public float targetIntensity = 1f;
    public float fadeDuration = 2f;

    [SerializeField] private Light2D light2D;
    private float initialIntensity = 0f;
    private float elapsedTime = 0f;
    private bool fadeIn = false;
    [SerializeField] private LayerMask layerMask;
    private Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        target = player.GetComponent<Transform>();
        light2D.intensity = 0f;
        color = sprite.color;
        color.a = 0f;
        sprite.color = color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, player.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);


        if (hit.collider.CompareTag("Player") && !fadeIn)
        {
            StartCoroutine(FadeInLight());
            Debug.Log("in");
            Debug.DrawRay(transform.position, direction * distance, Color.green);
        }
        else if (!hit.collider.CompareTag("Player") && fadeIn)
        {
            StartCoroutine(FadeOutLight());
            Debug.Log("out");
            Debug.DrawRay(transform.position, direction * distance, Color.red);
        }
        
    }
    IEnumerator FadeInLight()
    {
        while (elapsedTime < fadeDuration && light2D.intensity < 0.99f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            light2D.intensity = Mathf.Lerp(initialIntensity, targetIntensity, t);
            color.a = t;
            sprite.color = color;
            Debug.Log(light2D.intensity);
            yield return null;
        }

        light2D.intensity = targetIntensity; // ensure exact value at end
        elapsedTime = 0f;
        fadeIn = true;
    }
    IEnumerator FadeOutLight()
    {
        while (elapsedTime < fadeDuration && light2D.intensity > 0.01f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            light2D.intensity = Mathf.Lerp(targetIntensity, 0, t);
            color.a = 1-t;
            sprite.color = color;
            yield return null;
        }

        light2D.intensity = 0; // ensure exact value at end
        elapsedTime = 0f;
        fadeIn = false;
    }
}
