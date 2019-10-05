using UnityEngine;

public class WallsTrigger : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            GlobalEvents.PlayerHitWall(this, null);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            GlobalEvents.PlayerLeaveWall(this, null);
        }
    }
}
