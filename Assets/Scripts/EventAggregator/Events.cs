using UnityEngine;

namespace EventAggregation
{
    public class BallStartEvent : IEventBase
    {
        public Vector3 startPos;
        public Vector3 startVelocity;
    }
    public class BallsPlaneCollideEvent : IEventBase
    {
        public int planeNum;
        public int levelNum;
        public bool finalPlane;
    }
    public class BallCollideWallEvent : IEventBase { }
    public class BallsCollideWrongplaneEvent : IEventBase { }
    public class BallCollideFinalPlane : IEventBase { }

   
    public class DoorUnlockedEvent : IEventBase
    {
        public int completeLevel;
    }


    public class SoundFireEvent : IEventBase { }
    public class SoundBallCrash : IEventBase { }
    public class SoundDooropen : IEventBase { }
    public class SoundDoorUnlocked : IEventBase { }

    public class SpawnFpsPlayer : IEventBase
    {
        public Vector3 startPos;
    }
}