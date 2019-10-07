using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemType item;
    public float bounceSpeed = 3f;
    //adjust this to change how high it goes
    public float height = 0.05f;

    Vector3 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        //get the objects current position and put it in a variable so we can access it later with less code
        Vector3 pos = transform.position;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * bounceSpeed) * height + pos.y;
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(startingPosition.x, newY, startingPosition.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GlobalEvents.ItemCollected(this, new ItemCollectedEventArgs(item));
            GlobalGameManager.player.ItemCollected(item);
            Destroy(gameObject);
        }
    }
}
