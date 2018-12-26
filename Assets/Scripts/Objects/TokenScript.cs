using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TokenScript : MonoBehaviour
{
    public bool followingMouse;
    public float floatHeight = 2f;

    public int boardLayer = 9;
    public int tokenLayer = 10;
    private int boardLayerMask;
    private int tokenLayerMask;

    public Vector3 targetPosition;
    public Vector3 velocity = Vector3.zero;
    public float smoothTime = .01f;

    // Start is called before the first frame update
    void Start()
    {
        this.followingMouse = false;
        this.boardLayerMask = 1 << this.boardLayer;
        this.tokenLayerMask = 1 << this.tokenLayer;

        this.targetPosition = this.gameObject.transform.position;
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
                // TODO: Make sure token is "face up"
                this.targetPosition = hit.collider.gameObject.transform.position;
                targetPosition.y += this.floatHeight;
                this.velocity = Vector3.zero;
            }
            else if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, ~this.tokenLayerMask))
            { // Above something else (ignore tokens): hover in place
                this.targetPosition = hit.point;
                targetPosition.y += this.floatHeight;
                this.velocity = Vector3.zero;
            }

            if (this.gameObject.transform.position != this.targetPosition)
            {
                this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, this.targetPosition, ref this.velocity, this.smoothTime);
            }
        }
    }

    void OnMouseDown()
    {
        // Pick up token
        GetComponent<Rigidbody>().useGravity = false;
        this.followingMouse = true;
    }

    void OnMouseUp()
    {
        // Drop token
        // FIXME: Sometimes this glitches, and the token falls through the table, or goes to the wrong place.
        // I think disabling rotation on the Token GO's RigidBody and expanding the table size "fixed" this.
        GetComponent<Rigidbody>().useGravity = true;
        this.gameObject.transform.position = this.targetPosition;
        this.followingMouse = false;
    }
}
