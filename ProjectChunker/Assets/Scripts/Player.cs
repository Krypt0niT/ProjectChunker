using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Vector2 moveDirection;
    Save save;
    Rigidbody2D rb;
    public float speed = 10;
    public float jumpForce = 10;
    bool grounded = false;  
    Transform WeaponHolder;
    public GameObject CollidingRoom = null;
    public GameObject CollidingElevator = null;
    public bool usingElevator = false;
    // Start is called before the first frame update
    void Start()
    {
        save = GameObject.Find("Manager").GetComponent<Save>();
        rb = GetComponent<Rigidbody2D>();
        WeaponHolder = GameObject.Find("WeaponHolder").transform;
    }

    // Update is called once per frame
 
    private void Update()
    {
        PlayerMove();
        weapon();
        elevatorInteraction();
        if (Input.GetKey(KeyCode.X))
        {
            usingElevator = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "room")
        {
            CollidingRoom = collision.gameObject;
        }
        if (collision.gameObject.tag == "elevator")
        {
            CollidingElevator = collision.gameObject;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "room")
        {
            CollidingRoom = null;
            GameObject.FindObjectOfType<Base>().HideRooms(0.5f);
        }
        if (collision.gameObject.tag == "elevator")
        {
            CollidingElevator = null;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
    void PlayerMove()
    {
        //podmienky
        if (usingElevator) { return; }
        float x;
        float y = 0;

        if (Input.GetKey(save.player.moveLeft) && !Input.GetKey(save.player.moveRight)) { x = -1; }
        else if (!Input.GetKey(save.player.moveLeft) && Input.GetKey(save.player.moveRight)) { x = 1; }
        else { x = 0; }

        if (Input.GetKeyDown(save.player.jump) && grounded)
        {
            y = jumpForce;


        }
        
        moveDirection = new Vector2(x * speed, rb.velocity.y + y);
        rb.velocity = moveDirection;

    }
    void elevatorInteraction()
    {
        if (Input.GetKeyDown(save.player.use))
        {
            if (CollidingElevator != null)
            {
                usingElevator = true;
            }
        }
        if (usingElevator)
        {
            walkTo(CollidingElevator.transform.position.x);

            GameObject frame0 = CollidingElevator.transform.Find("frame0").gameObject;
            GameObject frame1 = CollidingElevator.transform.Find("frame1").gameObject;
            frame0.transform.position = new Vector3(frame0.transform.position.x, frame0.transform.position.y, -1.6f);
            frame1.transform.position = new Vector3(frame1.transform.position.x, frame1.transform.position.y, -1.6f);
        }

        if (CollidingElevator != null)
        {
            GameObject.FindObjectOfType<Base>().ElevatorOpenDoor();
        }
    }
    public void walkTo(float positionX)
    {
        if( positionX > transform.position.x)
        {
            if (positionX - transform.position.x < 0.125)
            {
                transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
                return;
            }
            moveDirection = new Vector2(1 * speed, rb.velocity.y);
            rb.velocity = moveDirection;
        }
        else if (positionX < transform.position.x)
        {
            if (transform.position.x - positionX < 0.125)
            {
                transform.position = new Vector3(positionX, transform.position.y, transform.position.z);
                return;
            }
            moveDirection = new Vector2(-1 * speed, rb.velocity.y);
            rb.velocity = moveDirection;
            
        }



    }

    void weapon()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(new Vector3(WeaponHolder.position.x, WeaponHolder.position.y, WeaponHolder.position.z));
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WeaponHolder.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
