using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventsController : MonoBehaviour
{
    private Animator CharacterAnimator;
    public string eventName;
    public string stringParam;
    
    // Start is called before the first frame update
    void Start()
    {
        CharacterAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //TransitAnimaStateMachine(stringParam);
    }

    void TransitAnimaStateMachine(string Leg)
    {
        if (Leg != null)
        {          
            if (Leg == "Right")
                CharacterAnimator.SetTrigger("RightLeg");
            else if (Leg == "Left")
                CharacterAnimator.SetTrigger("LeftLeg");
            stringParam = null;
        }
    }
    
    public void transit(AnimationEvent animationEvent)
    {
        stringParam = animationEvent.stringParameter;            
    }
}
