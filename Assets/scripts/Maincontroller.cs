using UnityEngine;

public class Maincontroller : MonoBehaviour
{
    private float initialPosition;
    private bool isDragging = false;
    private Rigidbody2D rb;
    public int skinNumber;
    public overallController ov;
    [HideInInspector]
    public Transform posToSpawn;
    [HideInInspector]
    public bool isSpawner = false;
    [SerializeField]
    private GameObject[] newBall;
    private bool gameOver = false;
    public AudioSource collisonSound; 

    void Start()
    {
        ov = FindAnyObjectByType<overallController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        collisonSound = GameObject.FindGameObjectWithTag("Finish").GetComponent<AudioSource>();
        initialPosition = 1.85f;
        if (transform.childCount > 0 && gameObject.transform.position.y >= 1.83f) 
        {
            Transform firstChild = this.transform.GetChild(0);
            firstChild.gameObject.SetActive(true);
        }
    }
    void Update()
    {
        // Check for touch input on mobile
        if (Input.touchCount > 0 && !ov.gmOver && !ov.paused)
        {
            if(gameObject.transform.position.y >= initialPosition-0.5f && gameObject.transform.position.y < initialPosition + 0.5f)
            {
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isDragging = true;
                        rb.isKinematic = true;
                        Vector3 touchPos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                        transform.position = new Vector3(touchPos.x, initialPosition, 0f);
                        break;

                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            // Move ball horizontally based on touch x position horizontally in the place 
                            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));
                            transform.position = new Vector3(touchPosition.x, initialPosition, 0f);
                        }
                        break;
                    case TouchPhase.Stationary:
                        if (isDragging)
                        {
                            rb.gravityScale = 0f;
                        }
                        break;
                    case TouchPhase.Ended:
                        if (isDragging)
                        {
                            isDragging = false;
                            rb.isKinematic = false;
                            rb.gravityScale = 0.3f;
                            if (transform.childCount > 0)
                            {
                                Transform firstChild = this.transform.GetChild(0);
                                firstChild.gameObject.SetActive(false);
                            }
                        }
                        break;
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (gameOver && gameObject.transform.position.y >= 0.29f)
        {
            Debug.Log("Game Over ");
            ov.gameOver();
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("skin" + skinNumber.ToString()))
        {
            if(!isSpawner && gameObject.GetInstanceID() < collision.gameObject.GetInstanceID())
            {
                if (skinNumber != 0) { ov.score += 5 * skinNumber; }
                else ov.score += 1;
                collisonSound.Play();
                posToSpawn = gameObject.transform;
                if(skinNumber != 9)
                {
                    GameObject g = Instantiate(newBall[skinNumber+1], posToSpawn.position, Quaternion.identity);
                    if (g.transform.childCount > 0)
                    {
                        Transform firstChild = g.transform.GetChild(0);
                        firstChild.gameObject.SetActive(false);
                    }

                    g.GetComponent<Rigidbody2D>().gravityScale = 0.4f;
                }
                else
                {
                    int p = Random.Range(0, 6);
                    GameObject g = Instantiate(newBall[p], posToSpawn.position, Quaternion.identity);
                    g.GetComponent<Rigidbody2D>().gravityScale = 0.4f;
                }
                isSpawner = true;
            }
            Destroy(this.gameObject);//I am learning android app development in kotlin
            Destroy(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("newball instantiated");
            instantiateNewBall();
        }
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Debug.Log("respawn called");
            Invoke("makeTrue", 0.7f);
        }
    }
    void instantiateNewBall()
    {
        GameObject obj = Instantiate(newBall[ov.getRandomInt()], new Vector3(-0.3f, initialPosition, 0f), Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().gravityScale = 0f;
    }
    void makeTrue() 
    {
        this.gameOver = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            Invoke("makeTrue", 1.2f);
        }
    }
}
