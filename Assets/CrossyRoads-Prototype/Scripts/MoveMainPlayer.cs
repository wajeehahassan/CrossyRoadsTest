using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMainPlayer : MonoBehaviour
{

    internal int index = 0;
    public Transform moveButtonsObject;
    public bool swipeControls = false;
    internal Vector3 swipeStart;
    internal Vector3 swipeEnd;
    public float swipeDistance = 10;
    public float swipeTimeout = 1;
    internal float swipeTimeoutCount;
    // Start is called before the first frame update
    void Start()
    {
      
        if (swipeControls == true && moveButtonsObject) moveButtonsObject.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (swipeControls == true)
        {
            if (swipeTimeoutCount > 0) swipeTimeoutCount -= Time.deltaTime;

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    swipeStart = touch.position;
                    swipeEnd = touch.position;

                    swipeTimeoutCount = swipeTimeout;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    swipeEnd = touch.position;
                }

                if (touch.phase == TouchPhase.Ended && swipeTimeoutCount > 0)
                {
                    if ((swipeStart.x - swipeEnd.x) > swipeDistance && (swipeStart.y - swipeEnd.y) < -swipeDistance) //Swipe left
                    {
                        MovePlayer("left");
                    }
                    else if ((swipeStart.x - swipeEnd.x) < -swipeDistance && (swipeStart.y - swipeEnd.y) > swipeDistance) //Swipe right
                    {
                        MovePlayer("right");
                    }
                    else if ((swipeStart.y - swipeEnd.y) < -swipeDistance && (swipeStart.x - swipeEnd.x) < -swipeDistance) //Swipe up
                    {
                        MovePlayer("forward");
                    }
                    else if ((swipeStart.y - swipeEnd.y) > swipeDistance && (swipeStart.x - swipeEnd.x) > swipeDistance) //Swipe down
                    {
                        MovePlayer("backward");
                    }
                }
            }
        }

    }

 

    void MovePlayer(string moveDirection)
    {
        if (GameController.Instance.playerObjects[GameController.Instance.currentPlayer] && GameController.Instance.playerObjects[GameController.Instance.currentPlayer].gameObject.activeSelf == true) GameController.Instance.playerObjects[GameController.Instance.currentPlayer].SendMessage("Move", moveDirection);
        else if (GameController.Instance.respawnObject && GameController.Instance.respawnObject.gameObject.activeSelf == true) GameController.Instance.respawnObject.SendMessage("Move", moveDirection);
    }



    void SetPlayerSpeed(float setValue)
    {
        if (GameController.Instance.playerObjects[GameController.Instance.currentPlayer] && GameController.Instance.playerObjects[GameController.Instance.currentPlayer].gameObject.activeSelf == true) GameController.Instance.playerObjects[GameController.Instance.currentPlayer].SendMessage("SetPlayerSpeed", setValue);
        else if (GameController.Instance.respawnObject && GameController.Instance.respawnObject.gameObject.activeSelf == true) GameController.Instance.respawnObject.SendMessage("SetPlayerSpeed", setValue);
    }



}