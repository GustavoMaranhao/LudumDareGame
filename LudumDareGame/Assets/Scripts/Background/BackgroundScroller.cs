using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;

    public Transform stageStartPosition;
    public Transform stageEndPosition;

    private Vector2 savedOffset;
    private Vector3 startPosition;
    private MeshRenderer spriteRenderer;
    private float timeForScroll;

    private bool isPlayerTouchingWall = false;

    void Start()
    {
        spriteRenderer = GetComponent<MeshRenderer>();

        startPosition = transform.position;
        savedOffset = spriteRenderer.sharedMaterial.GetTextureOffset("_MainTex");

        GlobalEvents.OnPlayerHitWall += PlayerReachedWall;
        GlobalEvents.OnPlayerLeaveWall += PlayerLeftWall;
    }

    private void OnDestroy()
    {
        GlobalEvents.OnPlayerHitWall -= PlayerReachedWall;
        GlobalEvents.OnPlayerLeaveWall -= PlayerLeftWall;
    }

    void PlayerReachedWall(object sender, System.EventArgs e)
    {
        isPlayerTouchingWall = true;

        WallEventArgs args = (WallEventArgs) e;
        if (args.tag == "EndWall")
        {
            GlobalGameManager.player.transform.position = stageStartPosition.position + Vector3.right*10;
        }
        if (args.tag == "StartWall")
        {
            GlobalGameManager.player.transform.position = stageEndPosition.position + Vector3.left*10;
        }
    }

    void PlayerLeftWall(object sender, System.EventArgs args)
    {
        isPlayerTouchingWall = false;        
    }

    void Update()
    {
        if (GlobalGameManager.player.isPlayerMoving && !isPlayerTouchingWall)
        {
            timeForScroll += Time.deltaTime * Mathf.Sign(Input.GetAxisRaw("Horizontal"));

            float x = Mathf.Repeat(timeForScroll * scrollSpeed * scrollSpeed, 1);
            Vector2 offset = new Vector2(x, savedOffset.y);
            spriteRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
        }
    }

    void OnDisable()
    {
        spriteRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
