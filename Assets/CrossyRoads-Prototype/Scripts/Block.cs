using UnityEngine;
using RoadCrossing.Types;

public class Block : MonoBehaviour
{
    public string touchTargetTag = "Player";

    public TouchFunction[] touchFunctions;

    public int removeAfterTouches = 0;
    internal bool isRemovable = false;
    public AnimationClip hitAnimation;
    public Transform deathEffect;
    void Start()
    {
        if (removeAfterTouches > 0)
            isRemovable = true;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == touchTargetTag)
        {
            foreach (var touchFunction in touchFunctions)
            {
                if (touchFunction.functionName != string.Empty)
                {
                    if (touchFunction.targetTag == "TouchTarget")
                    {
                        other.SendMessage(touchFunction.functionName, transform);
                    }
                    else if (touchFunction.targetTag != string.Empty)    // Otherwise, apply the function on the target tag set in this touch function
                    {
                        GameObject.FindGameObjectWithTag(touchFunction.targetTag).SendMessage(touchFunction.functionName, touchFunction.functionParameter);
                    }
                }
            }


            if (GetComponent<Animation>() && hitAnimation)
            {
                // Stop the animation
                GetComponent<Animation>().Stop();

                GetComponent<Animation>().Play(hitAnimation.name);
            }


            if (isRemovable == true)
            {
                // Reduce the number of times this object was touched by the target
                removeAfterTouches--;

                if (removeAfterTouches <= 0)
                {
                    if (deathEffect)
                        Instantiate(deathEffect, transform.position, Quaternion.identity);

                    Destroy(gameObject);
                }
            }
        }
    }
}