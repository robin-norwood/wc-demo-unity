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
            Vector3 targetPosition;

            if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, this.boardLayerMask))
            { // Above a hex tile: center on the tile and hover
                targetPosition = hit.collider.gameObject.transform.position;
            }
            else if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, ~this.tokenLayerMask))
            { // Above something else (ignore tokens): hover in place
                targetPosition = hit.point;
            }
            else
            { // Just follow the mouse
                // FIXME: This isn't right...mousePosition isn't the same as world
                // ...but in practice, I think the goal should be to have the table fill the entire view,
                // so the mouse is always over it, and this code path never gets hit.
                Debug.Log("MOUSE: " + Input.mousePosition);
                targetPosition = Input.mousePosition;
            }

            targetPosition.y += this.floatHeight;
            this.gameObject.transform.position = targetPosition;
            Debug.Log("TARGET: " + targetPosition);
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
        // FIXME: Sometimes this glitches, and the token falls through the table, or goes to the wrong place.

        this.followingMouse = false;
    }
}
