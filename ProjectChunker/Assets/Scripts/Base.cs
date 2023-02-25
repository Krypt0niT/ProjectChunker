using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    List<GameObject> rooms = new List<GameObject>();
    int numberOfRooms = 0;
    int numberOfElevators = 0;
    GameObject player;
    List<List<GameObject>> elevators = new List<List<GameObject>>(); 
    void Start()
    {
        var Rooms = GameObject.FindGameObjectsWithTag("room");
        numberOfRooms = Rooms.Length;
        for (int i = 0; i < numberOfRooms; i++)
        {
            rooms.Add(GameObject.Find("room"+i));
        }
        var Elevators = GameObject.FindGameObjectsWithTag("elevator");
        numberOfElevators = Elevators.Length;
        for (int i = 0; i < numberOfElevators; i++)
        {
            GameObject el0 = GameObject.Find("door" + i + "-" + 0);
            GameObject el1 = GameObject.Find("door" + i + "-" + 1);
            

            //elevators.Add(new List<GameObject>().Add(new List<GameObject>() = el0,el1));
        }//GameObject.Find("room" + i)

        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    void Update()
    {
        if(player.GetComponent<Player>().CollidingRoom != null)
        {

            ShowRoom(0.5f);

        }
        HideRooms(0.5f);
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
}
