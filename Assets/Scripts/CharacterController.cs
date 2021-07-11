using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float rotateSpeed = 35f, runSpeed = 5f;
    [SerializeField] private float firstRotationY, currentRotationY;
    [SerializeField] private GameObject playerVisual;
    private bool canMove = false;

    private Vector3 firstTouchPos, currentTouchPos, distanceTouchPos;

    [SerializeField] private GameObject woodObject,collectableWoodObject;
    [SerializeField] private Transform woodGeneratePos, woodGroundGeneratePoint,rayPoint;

    List<GameObject> WoodObjects = new List<GameObject>();
    private Vector3 playerStartPos;
    private bool isCanRotate = false;
    private bool isCreateWood = false;


    // In this script, we determined our player type in the game. We change it manually. !!!
    public enum CharacterType
    {
        Player, Enemy
    }
    public CharacterType characterType;
    void Start()
    {
        playerStartPos = transform.position;
    }

    // Game states are constantly checked in the update.
    void Update()
    {
        switch (GameManager.instance.CurrentGameState)
        {
            case GameManager.GameState.Prepare:
                PrepareGame();
                break;
            case GameManager.GameState.MainGame:
                Move();
                PlayerInput();

                break;
            case GameManager.GameState.FinishGame:
                
                break;
        }
    }

    private void PrepareGame()
    {
        transform.position = playerStartPos;
        playerVisual.SetActive(true);
        if (WoodObjects.Count > 0)
        {
            for (int i = 0; i < WoodObjects.Count; i++)
            {

                Destroy(WoodObjects[i]);
                WoodObjects.RemoveAt(i);
                i--;
            }
        }
    }

    // Player rotated according to inputs
    #region PlayerInput 

    private void PlayerInput()
    {
        
        if (characterType == CharacterType.Player)
        {

            #region MouseInput
            if (Input.GetMouseButtonDown(0))
            {

                firstTouchPos = MousePosition();
                firstRotationY = transform.rotation.eulerAngles.y;
                isCanRotate = true;
            }
            if (Input.GetMouseButton(0))
            {
                if (isCanRotate)
                {
                    currentTouchPos = MousePosition();
                    distanceTouchPos = currentTouchPos - firstTouchPos;
                    if (distanceTouchPos.x > 0f)
                    {
                        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
                    }
                    else if (distanceTouchPos.x < 0f)
                    {
                        transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);

                    }
                    currentRotationY = transform.rotation.eulerAngles.y;
                    checkRotation(firstRotationY, currentRotationY);
                    if (Mathf.Abs(currentRotationY - firstRotationY) >= 90f)
                    {
                        resetTouchData();
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                resetTouchData();
            }
            #endregion


            #region TouchInput
            /*
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    firstTouchPos = touch.position;
                    firstRotationY = transform.rotation.eulerAngles.y;
                    isCanRotate = true;
                }
                if (touch.phase == TouchPhase.Stationary)
                {

                }
                if (touch.phase == TouchPhase.Moved)
                {
                    if (isCanRotate)
                    {
                        currentTouchPos = touch.position;
                        distanceTouchPos = currentTouchPos - firstTouchPos;
                        if (distanceTouchPos.x > 0f)
                        {
                            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
                        }
                        else if (distanceTouchPos.x < 0f)
                        {
                            transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);

                        }
                        currentRotationY = transform.rotation.eulerAngles.y;
                        checkRotation(firstRotationY, currentRotationY);
                        if (Mathf.Abs(currentRotationY - firstRotationY) >= 90f)
                        {
                            resetTouchData();
                        }
                    }
                }
                if (touch.phase == TouchPhase.Ended)
                {
                    resetTouchData();
                }
            }
            */
            #endregion

        }
    }
    #endregion
    private void checkRotation(float firstRotY, float currentRotY)
    {

        if ((firstRotY >= 0 && firstRotY <= 90) && (currentRotY>=270&& currentRotY<=360))
        {
            this.currentRotationY = Mathf.Abs(360-currentRotY);
        }
    }
    private void resetTouchData()
    {
        isCanRotate = false;
        firstTouchPos = Vector3.zero;
        currentTouchPos = Vector3.zero;
        distanceTouchPos = Vector3.zero;
    }
    private Vector3 MousePosition()
    {
         return cam.ScreenToViewportPoint(Input.mousePosition);
    }
    private void Move()
    {
        if (GameManager.instance.CurrentGameState == GameManager.GameState.MainGame && playerVisual.activeInHierarchy)
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }
        if (canMove)
        {
            transform.Translate(Vector3.forward * runSpeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.CurrentGameState == GameManager.GameState.MainGame)
        {
            if (other.CompareTag("FinishArea"))
            {
                GameManager.instance.CurrentGameState = GameManager.GameState.FinishGame;
            }
            if (other.CompareTag("CollectableWood"))
            {
                Vector3 temp = woodGeneratePos.position;
                temp.y += WoodObjects.Count * 0.1f;
                Quaternion rot = Quaternion.Euler(new Vector3(0, 0, 0));
                GameObject createdWoodObject = Instantiate(woodObject, temp, Quaternion.identity, woodGeneratePos);
                createdWoodObject.transform.localEulerAngles = new Vector3(0, 0, 0);
                WoodObjects.Add(createdWoodObject);
                other.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.CurrentGameState == GameManager.GameState.MainGame)
        {
            if (other.CompareTag("Ground") || other.CompareTag("Wood"))
            {
                CreateWoodGround(true);
            }
        }
    }
    private bool CheckRaycastHit()
    {
        LayerMask mask = ~(1 << LayerMask.NameToLayer("IgnoreLayer"));
        if (Physics.Raycast(rayPoint.position,-Vector3.up,out RaycastHit hit,200f,mask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
   private void CreateWoodGround(bool canCreateWood)
    {
        isCreateWood = canCreateWood;
        if (isCreateWood && !CheckRaycastHit())
        {
            if (WoodObjects.Count > 0)
            {
                Destroy(WoodObjects[WoodObjects.Count - 1]);
                WoodObjects.RemoveAt(WoodObjects.Count - 1);
                GameObject go = Instantiate(woodObject, woodGroundGeneratePoint.position, Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0));
                go.AddComponent<BoxCollider>();
            }
            else
            {
                Vector3 temp = transform.position + (transform.forward * 5);
                transform.DOJump(temp , 5, 1, 1, false).OnComplete(() => CheckForDeath());
            }
            isCreateWood = false;
        }
    }
    void CheckForDeath()
    {
        if (!CheckRaycastHit())
        {
            if (characterType == CharacterType.Player)
            {
                GameManager.instance.CurrentGameState = GameManager.GameState.FinishGame;
            }
            playerVisual.SetActive(false);
        }
    }
}
