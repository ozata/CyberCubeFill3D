using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private bool horizontalCollider;
    private bool verticalCollider;

    public float moveSpeed = 5f;
    public Transform movePoint;

    public LayerMask whatStopsMovement;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }



    void MoveCharacter()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        CheckWalls();
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (!horizontalCollider)
            {
                movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0);
            }
            if (!verticalCollider)
            {
                movePoint.position += new Vector3(0, 0, Input.GetAxisRaw("Vertical"));
            }
        }
    }

    void CheckWalls()
    {
        horizontalCollider = Physics.CheckSphere(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.3f, whatStopsMovement);
        verticalCollider = Physics.CheckSphere(movePoint.position + new Vector3(0f, 0f, Input.GetAxisRaw("Vertical")), 0.3f, whatStopsMovement);
    }
}
