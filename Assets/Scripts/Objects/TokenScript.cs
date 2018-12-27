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

    private Quaternion faceDown = Quaternion.Euler(90, 0, 0);
    private Quaternion faceUp = Quaternion.Euler(-90, 0, 0);
    private Quaternion kindaFaceUp = Quaternion.Euler(-70, -60, 0);

    public Quaternion targetRotation;
    public float rotationSpeed = 750f;

    // Start is called before the first frame update
    void Start()
    {
        this.followingMouse = false;
        this.boardLayerMask = 1 << this.boardLayer;
        this.tokenLayerMask = 1 << this.tokenLayer;

        this.targetPosition = this.gameObject.transform.position;
        this.targetRotation = this.kindaFaceUp;
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
                this.targetPosition = hit.collider.gameObject.transform.position;
                targetPosition.y += this.floatHeight;
                this.velocity = Vector3.zero;
                this.targetRotation = this.kindaFaceUp;
            }
            else if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, ~this.tokenLayerMask))
            { // Above something else (ignore tokens): hover in place
                this.targetPosition = hit.point;
                targetPosition.y += this.floatHeight;
                this.velocity = Vector3.zero;
                if (hit.collider.tag == "FU")
                { // FIXME: Basing this off of tag seems hacky to me
                    this.targetRotation = this.faceUp;
                }
                else if (hit.collider.tag == "FD")
                {
                    this.targetRotation = this.faceDown;
                }
            }

            if (this.gameObject.transform.position != this.targetPosition)
            { // FIXME: Need to add a fudge factor w/distance so this doesn't get called too much?
                this.gameObject.transform.position = Vector3.SmoothDamp(this.gameObject.transform.position, this.targetPosition, ref this.velocity, this.smoothTime);
            }

            if (this.gameObject.transform.rotation != this.targetRotation)
            { // FIXME: Need to add fudge factor on rotation too?
                this.gameObject.transform.rotation = Quaternion.RotateTowards(transform.rotation, this.targetRotation, this.rotationSpeed * Time.deltaTime);
            }
        }
    }

    void OnMouseDown()
    {
        // Pick up token
        GetComponent<Rigidbody>().useGravity = false;
        this.followingMouse = true;
        this.targetRotation = this.kindaFaceUp;
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
