using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask whatStopsMovement;

    private bool leftHorizontalCollider;
    private bool rightHorizontalCollider;
    private bool upVerticalCollider;
    private bool downVerticalCollider;


    private bool movingLeft;
    private bool movingRight;
    private bool movingUp;
    private bool movingDown;
    private bool resetMovingFlags;

    private bool canMoveLeft;
    private bool canMoveUp;
    private bool canMoveDown;
    private bool canMoveRight;

    private Vector3 cubeGenerationPoint;
    private Vector3 startPosition;
    private Vector3 endPosition;

    private float x;
    private float y;
    private float z;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        resetMovingFlags = false;

        canMoveLeft = true;
        canMoveRight = true;
        canMoveUp = true;
        canMoveDown = true;

        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;

        cubeGenerationPoint= transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Swipe();
        MoveCharacter();
        GenerateCubeLogic();
    }

    void MoveCharacter()
    {
        CheckWalls();
        CheckPlayerMovement();
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
        {
            if (movingLeft && canMoveLeft)
            {
                movePoint.position += new Vector3(-1, 0, 0);
            }
            if (movingRight && canMoveRight)
            {
                movePoint.position += new Vector3(1, 0, 0);
            }
            if (movingUp && canMoveUp)
            {
                movePoint.position += new Vector3(0, 0, 1);
            }
            if (movingDown && canMoveDown)
            {
                movePoint.position += new Vector3(0, 0, -1);
            }
        }
    }


    // The cubes that player will generate after moving
    void GenerateCubeLogic(){
        if(transform.position.x == x && transform.position.z >= z + 0.6F){
            GenerateCube();
            z = z + 1F;
            cubeGenerationPoint = new Vector3(x, y, z);

        } else if(transform.position.x == x && transform.position.z <= z - 0.6F){
            GenerateCube();
            z = z - 1F;
            cubeGenerationPoint = new Vector3(x, y, z);

        } else if(transform.position.x <= x - 0.6F && transform.position.z == z){
            GenerateCube();
            x = x - 1F;
            cubeGenerationPoint = new Vector3(x, y, z);

        } else if(transform.position.x >= x + 0.6F && transform.position.z == z){
            GenerateCube();
            x = x + 1F;
            cubeGenerationPoint = new Vector3(x, y, z);
        }
    }

    void GenerateCube() {
        ObjectPooler.Instance.SpawnFromPool("Cube", cubeGenerationPoint);
    }

    void CheckWalls()
    {
        leftHorizontalCollider = Physics.CheckSphere(movePoint.position + new Vector3(-1, 0, 0), 0.3f, whatStopsMovement);
        rightHorizontalCollider = Physics.CheckSphere(movePoint.position + new Vector3(1, 0, 0), 0.3f, whatStopsMovement);
        upVerticalCollider = Physics.CheckSphere(movePoint.position + new Vector3(0, 0, 1), 0.3f, whatStopsMovement);
        downVerticalCollider = Physics.CheckSphere(movePoint.position + new Vector3(0, 0, -1), 0.3f, whatStopsMovement);
    }

    void CheckPlayerMovement()
    {
        if (leftHorizontalCollider)
        {
            canMoveLeft = false;
            movingLeft = false;
            canMoveRight = true;
            canMoveUp = true;
            canMoveDown = true;
        }
        if (rightHorizontalCollider)
        {
            canMoveRight = false;
            movingRight = false;
            canMoveLeft = true;
            canMoveUp = true;
            canMoveDown = true;
        }
        if (upVerticalCollider)
        {
            canMoveUp = false;
            movingUp = false;
            canMoveDown = true;
            canMoveLeft = true;
            canMoveRight = true;
        }
        if (downVerticalCollider)
        {
            canMoveDown = false;
            movingDown = false;
            canMoveUp = true;
            canMoveLeft = true;
            canMoveRight = true;
        }
    }


    //inside class
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    public void Swipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButtonUp(0))
        {
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && !movingDown)
            {
                movingLeft = false;
                movingRight = false;
                movingUp = true;
                movingDown = false;
            }
            //swipe down
            if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f && !movingUp)
            {
                movingLeft = false;
                movingRight = false;
                movingUp = false;
                movingDown = true;
            }
            //swipe left
            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && !movingRight)
            {
                movingLeft = true;
                movingRight = false;
                movingUp = false;
                movingDown = false;
            }
            //swipe right
            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f && !movingLeft)
            {
                movingLeft = false;
                movingRight = true;
                movingUp = false;
                movingDown = false;
            }
        }
    }
}
