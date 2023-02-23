using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    List<GameObject> rooms = new List<GameObject>();
    int numberOfRooms = 0;
    GameObject player;
    void Start()
    {
        var Rooms = GameObject.FindGameObjectsWithTag("room");
        numberOfRooms = Rooms.Length;
        for (int i = 0; i < numberOfRooms; i++)
        {
            rooms.Add(GameObject.Find("room"+i));
        }

        player = GameObject.FindObjectOfType<Player>().gameObject;
    }

    void Update()
    {
        if(player.GetComponent<Player>().CollidingRoom != null)
        {

            player.GetComponent<Player>().CollidingRoom.transform.Find("front").gameObject.SetActive(false);

        }
        else
        {
            for (int i = 0; i < numberOfRooms; i++)
            {
                rooms[i].transform.Find("front").gameObject.SetActive(true);
            }
        }
    }
}
