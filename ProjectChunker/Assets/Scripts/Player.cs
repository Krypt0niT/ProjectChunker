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
    bool jumpReset = true;
    Transform WeaponHolder;
    // Start is called before the first frame update
    void Start()
    {
        save = GameObject.Find("Manager").GetComponent<Save>();
        rb = GetComponent<Rigidbody2D>();
        WeaponHolder = GameObject.Find("WeaponHolder").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMove();
    }
    private void Update()
    {
        weapon();
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
            jumpReset = true;
        }
    }
    void PlayerMove()
    {
        float x;

        float y = 0;

        if (Input.GetKey(save.player.moveLeft) && !Input.GetKey(save.player.moveRight)) { x = -1; }
        else if (!Input.GetKey(save.player.moveLeft) && Input.GetKey(save.player.moveRight)) { x = 1; }
        else { x = 0; }

        if (Input.GetKey(save.player.jump) && grounded && jumpReset)
        {
            y = jumpForce;
            jumpReset = false;


        }
        moveDirection = new Vector2(x * speed, rb.velocity.y + y);
        rb.velocity = moveDirection;

    }
    void weapon()
    {
        var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(new Vector3(WeaponHolder.position.x, WeaponHolder.position.y, WeaponHolder.position.z));
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        WeaponHolder.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
