using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventAggregation;
using UniRx;

public class ProgressController : MonoBehaviour
{

    private class PlaneInfo {public int planeNum; public int levelNum; public bool finalPlane; };
    private List<PlaneInfo> progressList = new List<PlaneInfo>();

    public bool clearPlayerPrefs;

    public Text scoreText;
    public IntReactiveProperty score = new IntReactiveProperty(0);

    private void Awake()
    {
        EventAggregator.Subscribe<BallsPlaneCollideEvent>(OnNewCollideWithPlane);
        EventAggregator.Subscribe<BallCollideWallEvent>(OnBallFail);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (clearPlayerPrefs)
            PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 0);

        if (!PlayerPrefs.HasKey("score"))
            PlayerPrefs.SetInt("score", 0);

        score.SubscribeToText(scoreText);
        score.Value = PlayerPrefs.GetInt("score");

    }

    void OnNewCollideWithPlane(IEventBase eventBase)
    {

        BallsPlaneCollideEvent ballscollide = eventBase as BallsPlaneCollideEvent;

        PlaneInfo planeinfo = new PlaneInfo()
        {
            planeNum = ballscollide.planeNum,
            levelNum = ballscollide.levelNum,
            finalPlane = ballscollide.finalPlane
        };

        progressList.Add(planeinfo);
        CheckForFailOrWin();
    }

    void CheckForFailOrWin()
    {
        int count = 0;
        foreach(PlaneInfo i in progressList)
        {
            if (count == i.planeNum)
            {
                if(i.finalPlane)
                  {

                    AddProgressInPlayerPrefs(i.levelNum);

                    DoorUnlockedEvent doorunlock = new DoorUnlockedEvent()
                    {
                        completeLevel = i.levelNum
                    };
                    EventAggregator.Publish(doorunlock);

                    BallCollideFinalPlane finalendfly = new BallCollideFinalPlane();
                    EventAggregator.Publish(finalendfly);
                }
                count++;

                PlayerPrefs.SetInt("score", PlayerPrefs.GetInt("score") + 100);
                score.Value = PlayerPrefs.GetInt("score");

                continue;
            }
            else
            {
                progressList.Clear();
                BallsCollideWrongplaneEvent wrongplaneevent = new BallsCollideWrongplaneEvent();
                EventAggregator.Publish(wrongplaneevent);
                break;
            }
        }

    }
    void OnBallFail(IEventBase eventbase)
    {
        progressList.Clear();
    }

    void AddProgressInPlayerPrefs(int level)
    {
        PlayerPrefs.SetInt("level", level);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
