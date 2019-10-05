using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed;
    public float tileSizeZ;

    private Vector2 savedOffset;
    private Vector3 startPosition;
    private MeshRenderer spriteRenderer;
    private float timeForScroll;

    private bool isPlayerTouchingWall = false;
    private bool bSpeedUpdated = false;

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

    void PlayerReachedWall(object sender, System.EventArgs args)
    {
        isPlayerTouchingWall = true;
    }

    void PlayerLeftWall(object sender, System.EventArgs args)
    {
        isPlayerTouchingWall = false;
        bSpeedUpdated = false;
    }

    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            timeForScroll += Time.deltaTime * Mathf.Sign(Input.GetAxisRaw("Horizontal"));
        }

        float x = Mathf.Repeat(timeForScroll * scrollSpeed * scrollSpeed, 1);
        Vector2 offset = new Vector2(x, savedOffset.y);
        spriteRenderer.sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

    void OnDisable()
    {
        spriteRenderer.sharedMaterial.SetTextureOffset("_MainTex", savedOffset);
    }
}
