using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DinoController : MonoBehaviour
{
    public float minMouseDist;
    public float playerSpeed = 0.1f;
    public float ySortingOffset;
    private bool isFacingRight = true;
    private SpriteRenderer sprite;
    private Rigidbody2D rigidBody;



    // Use this for initialization
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        gameObject.AddComponent<CameraFollower>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float dist = Vector2.Distance(transform.position, mousePos);
        if (dist > minMouseDist)
        {
            Vector3 direction = Vector3.Normalize(mousePos - (Vector2)transform.position);
            Vector3 newPos = direction * playerSpeed * Time.deltaTime;
            //using a velocity hack to be able to use collisions with Translate
            //rigidBody.velocity = (newPos - transform.position) / Time.deltaTime;
            //rigidBody.MovePosition(direction * playerSpeed * Time.deltaTime);
            //rigidBody.transform.Translate(direction * playerSpeed * Time.deltaTime);
            transform.Translate(direction * playerSpeed * Time.deltaTime);
            if (direction.x > 0)
            {
                sprite.flipX = false;
            }
            else
            {
                sprite.flipX = true;
            }
        }
    }

    void LateUpdate()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -(int)((transform.position.y + ySortingOffset) * 100);
    }

    void OnColliderEnter2D(Collider2D col)
    {
        //Debug.Log("Something should be colliding with player.");
        //FriendlyAgent minion = col.gameObject.GetComponent<FriendlyAgent>();
        ////Debug.Log("Minion = " + minion);
        //if (minion != null && Time.time - timeOfLastAttack >= stats.secondsPerAttack)
        //{

        //    //Debug.Log("Minion taking damage from building.");
        //    minion.TakeDamage(stats.attackDamage);
        //    timeOfLastAttack = Time.time;
        //}
    }
}
