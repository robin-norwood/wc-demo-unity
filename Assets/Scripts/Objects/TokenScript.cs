using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TokenScript : MonoBehaviour
{
    public bool followingMouse;
    public float floatHeight = 1f;
    public int boardLayer = 9;
    public int tokenLayer = 10;
    private int boardLayerMask;
    private int tokenLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        this.followingMouse = false;
        this.boardLayerMask = 1 << this.boardLayer;
        this.tokenLayerMask = 1 << this.tokenLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.followingMouse)
        {
            RaycastHit hit;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, this.boardLayerMask))
            { // Above a hex tile: center on the tile and hover
                Vector3 targetPosition = hit.collider.gameObject.transform.position;
                targetPosition.y += this.floatHeight;
                this.gameObject.transform.position = targetPosition;
            }
            else if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, ~this.tokenLayerMask))
            { // Above something else (ignore tokens): hover in place
                Vector3 targetPosition = hit.point;
                targetPosition.y += this.floatHeight;
                this.gameObject.transform.position = targetPosition;
            }

        }
    }

    void OnMouseDown()
    {
        // Pick up token
        this.followingMouse = true;
    }

    void OnMouseUp()
    {
        // Drop token
        this.followingMouse = false;
    }
}
