using UnityEngine;
using System.Collections;


public class MouseTrack : MonoBehaviour
{

    private Camera mainCam;
    private Vector3 mousePos;
    public GameObject bullet1; // bullet1 Prefab
    public GameObject bullet2; // bullet2 Prefab
    public Transform bulletTransform; // where bullet shoots from
    public bool canFire;
    public float switchCooldown = 2f;
    private bool switching = false;
    private float timer; // holds the current amount of time passed
    private BulletScript bs;
    private int bulletCounter = 0;
    private GameObject currBullet;

    [SerializeField] private UnityEngine.Rendering.Universal.Light2D light2D;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        bs = bullet1.GetComponent<BulletScript>();
        currBullet = bullet1;
    }

    // Update is called once per frame
    void Update()
    {

        // button used to change bullet 
        if (Input.GetMouseButton(1) && !switching)
        {
            StartCoroutine(SwitchBullet());
        }


        mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rotation = mousePos - transform.position;
        // Debug.Log(rotation);

        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
        light2D.transform.rotation = Quaternion.Euler(0, 0, rotZ + 270);

        if (!canFire)
        {
            timer += Time.deltaTime;
            if (timer > bs.fireRate)
            {
                canFire = true;
                timer = 0;
            }
        }

        if (Input.GetMouseButton(0) && canFire)
        {
            canFire = false;
            Instantiate(currBullet, bulletTransform.position, Quaternion.identity);
        }
    }

    IEnumerator SwitchBullet()
    {
        if (bulletCounter == 0)
        {
            bulletCounter = 1;
            bs = bullet2.GetComponent<BulletScript>();
            currBullet = bullet2;
        }
        else
        {
            bulletCounter = 0;
            bs = bullet1.GetComponent<BulletScript>();
            currBullet = bullet1;
        }
        switching = true;
        canFire = true;
        yield return new WaitForSeconds(switchCooldown);
        switching = false;
    }
}
