using UnityEngine;
using System.Collections;
using EventAggregation;

[RequireComponent(typeof(Animator))]
public class DoorActivatorMecanim : MonoBehaviour 
{
    private Animator DoorAnimator;
    private bool doorUnlock = false;

    public int levelnum;
    

	void Start()
	{
        DoorAnimator = GetComponent<Animator> ();
        EventAggregator.Subscribe<DoorUnlockedEvent>(OnDoorUnlock);

        if (levelnum <= PlayerPrefs.GetInt("level"))
            doorUnlock = true;
	}

    void OnDoorUnlock(IEventBase eventbase)
    {
        

        DoorUnlockedEvent doorlevel = eventbase as DoorUnlockedEvent;

        if (doorlevel.completeLevel == levelnum)
        {
            SoundDoorUnlocked soundunlock = new SoundDoorUnlocked();
            EventAggregator.Publish(soundunlock);
            doorUnlock = true;
        }
    }

    void OnTriggerEnter(Collider col)
	{
        if (doorUnlock)
        {
            OffAllAnimatorBool();
            DoorAnimator.SetBool("open", true);
            SoundDooropen sounddoor = new SoundDooropen();
            EventAggregator.Publish(sounddoor);
        }
	}
    void OnTriggerExit(Collider other)
    {
        OffAllAnimatorBool();
        DoorAnimator.SetBool("close", true);
    }

    void OffAllAnimatorBool()
    {
        DoorAnimator.SetBool("close", false);
        DoorAnimator.SetBool("open", false);
           
    }

}
