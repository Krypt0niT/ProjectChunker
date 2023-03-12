using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    List<GameObject> rooms = new List<GameObject>();
    int numberOfRooms = 0;
    int numberOfElevators = 0;

    public bool BaseMoving = true;
    float baseSpeed = 1;
    Rigidbody2D rb;

    GameObject player;
    List<List<GameObject>> elevators = new List<List<GameObject>>(); 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        var Rooms = GameObject.FindGameObjectsWithTag("room");
        numberOfRooms = Rooms.Length;
        for (int i = 0; i < numberOfRooms; i++)
        {
            rooms.Add(GameObject.Find("room"+i));
        }
        var Elevators = GameObject.FindGameObjectsWithTag("elevator");
        numberOfElevators = Elevators.Length;
        for (int i = 0; i < numberOfElevators/2; i++)
        {
            List<GameObject> l = new List<GameObject>();
            GameObject el0 = GameObject.Find("door" + i + "-" + 0);
            GameObject el1 = GameObject.Find("door" + i + "-" + 1);
            l.Add(el0);
            l.Add(el1);

            elevators.Add(l);
        }

        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    void Update()
    {
        if(player.GetComponent<Player>().CollidingRoom != null)
        {

            ShowRoom(0.5f);

        }
 
        ElevatorCloseDoor();
        HideRooms(0.5f);
        BaseMove();
    }
    void BaseMove()
    {
        if (BaseMoving)
        {
            rb.velocity = new Vector2(baseSpeed,0);
        }
    }
    public void HideRooms(float time)
    {
        for (int i = 0; i < numberOfRooms; i++)
        {
            GameObject front = rooms[i].transform.Find("front").gameObject;
            if (front.GetComponent<SpriteRenderer>().color.a < 1 && rooms[i] != player.GetComponent<Player>().CollidingRoom)
            {
                front.GetComponent<SpriteRenderer>().color = new Color(
                    front.GetComponent<SpriteRenderer>().color.r,
                    front.GetComponent<SpriteRenderer>().color.g,
                    front.GetComponent<SpriteRenderer>().color.b,
                    front.GetComponent<SpriteRenderer>().color.a + (Time.deltaTime / time)
                    );
            }
        }
    }
    void ShowRoom(float time)
    {
        GameObject front = player.GetComponent<Player>().CollidingRoom.transform.Find("front").gameObject;
        if (front.GetComponent<SpriteRenderer>().color.a > 0)
        {
            front.GetComponent<SpriteRenderer>().color = new Color(
                front.GetComponent<SpriteRenderer>().color.r,
                front.GetComponent<SpriteRenderer>().color.g,
                front.GetComponent<SpriteRenderer>().color.b,
                front.GetComponent<SpriteRenderer>().color.a - (Time.deltaTime / time)
                );
        }
    }
    public void ElevatorOpenDoor()
    {
        GameObject frame0 = player.GetComponent<Player>().CollidingElevator.transform.Find("frame0").gameObject;
        GameObject frame1 = player.GetComponent<Player>().CollidingElevator.transform.Find("frame1").gameObject;
        if(frame0.transform.localPosition.x >= -0.75f && !player.GetComponent<Player>().usingElevator)
        {
            frame0.transform.localPosition = new Vector3(frame0.transform.localPosition.x - Time.deltaTime, frame0.transform.localPosition.y, frame0.transform.localPosition.z);
            frame1.transform.localPosition = new Vector3(frame1.transform.localPosition.x + Time.deltaTime, frame1.transform.localPosition.y, frame1.transform.localPosition.z);
           
        }
        if (frame0.transform.localPosition.x < -0.75f)
        {
            if (player.GetComponent<Player>().CollidingElevator != null)
            {
                
                frame0.transform.position = new Vector3(frame0.transform.position.x, frame0.transform.position.y, -0.5f);
                frame1.transform.position = new Vector3(frame1.transform.position.x, frame1.transform.position.y, -0.5f);
            }
        }
    

    }
    void ElevatorCloseDoor()
    {
        for (int i = 0; i < elevators.Count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject frame0 = elevators[i][j].transform.Find("frame0").gameObject;
                GameObject frame1 = elevators[i][j].transform.Find("frame1").gameObject;
                if (frame0.transform.localPosition.x < -0.25f)
                {
                    if (elevators[i][j] != player.GetComponent<Player>().CollidingElevator || 
                        (player.GetComponent<Player>().usingElevator && player.GetComponent<Player>().CollidingElevator.transform.position.x == player.transform.position.x))
                    {
                        frame0.transform.localPosition = new Vector3(frame0.transform.localPosition.x + Time.deltaTime, frame0.transform.localPosition.y, frame0.transform.localPosition.z);
                        frame1.transform.localPosition = new Vector3(frame1.transform.localPosition.x - Time.deltaTime, frame1.transform.localPosition.y, frame1.transform.localPosition.z);

                    }

                }
                if (elevators[i][j] == player.GetComponent<Player>().CollidingElevator)
                {
                    if (frame0.transform.localPosition.x > -0.25f && player.GetComponent<Player>().CollidingElevator != null && player.GetComponent<Player>().usingElevator)
                    {
                        string elevatorName = player.GetComponent<Player>().CollidingElevator.gameObject.name;
                        int elevatorIndex = Int32.Parse(elevatorName[4].ToString());
                        if (elevatorName[6] == '0')
                        {
                            player.transform.position = GameObject.Find("door" + elevatorIndex + "-" + "1").transform.position;
                            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);

                            

                            frame0.transform.position = new Vector3(frame0.transform.position.x, frame0.transform.position.y, 0.25f);
                            frame1.transform.position = new Vector3(frame1.transform.position.x, frame1.transform.position.y, 0.25f);
                            player.GetComponent<Player>().CollidingElevator = GameObject.Find("door" + elevatorIndex + "-" + "1");
                            player.GetComponent<Player>().usingElevator = false;
                        }
                        if (elevatorName[6] == '1')
                        {
                            player.transform.position = GameObject.Find("door" + elevatorIndex + "-" + "0").transform.position;
                            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);



                            frame0.transform.position = new Vector3(frame0.transform.position.x, frame0.transform.position.y, 0.25f);
                            frame1.transform.position = new Vector3(frame1.transform.position.x, frame1.transform.position.y, 0.25f);
                            player.GetComponent<Player>().CollidingElevator = GameObject.Find("door" + elevatorIndex + "-" + "0");
                            player.GetComponent<Player>().usingElevator = false;
                        }
                    }
                }
                

            }
        }
    }
}
