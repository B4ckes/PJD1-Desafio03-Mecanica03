using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    public int currentTouchID;
    
    private Rigidbody2D rigidBody;
    private Collider2D objectCollider;

    void Awake()
    {
        this.rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        this.objectCollider = this.gameObject.GetComponent<Collider2D>();
    }

    void Update()
    {
        this.verifyTouchesAndSetCurrentTouchID();
    }

    void FixedUpdate()
    {
        this.moveObject();
    }

    void moveObject() {
        if (this.currentTouchID > -1) {
            Touch currentTouch = Input.GetTouch(this.currentTouchID);

            if (currentTouch.phase == TouchPhase.Moved) {
                Vector3 worldPosition = Camera.current.ScreenToWorldPoint(new Vector3(currentTouch.position.x, currentTouch.position.y, 10f));

                this.gameObject.transform.position = worldPosition;
            }
        }
    }

    void verifyTouchesAndSetCurrentTouchID() {
        if (Input.touchCount > 0) {
            Touch[] touches = Input.touches;

            foreach (Touch touch in touches) {
                Vector3 touchPosition = Camera.current.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));

                if (touch.phase == TouchPhase.Began && this.isTouchingInside(touchPosition)) {
                    this.currentTouchID = touch.fingerId;
                    this.rigidBody.gravityScale = 0;
                } else if (touch.fingerId == this.currentTouchID && touch.phase == TouchPhase.Ended) {
                    this.currentTouchID = -1;
                    this.rigidBody.gravityScale = 1;
                }
            }
        }
    }

    bool isTouchingInside(Vector3 initialTouchPosition) {
        return this.objectCollider.bounds.Contains(initialTouchPosition);
    }
}
