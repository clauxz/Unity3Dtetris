using UnityEngine;
using System.Collections;

public class TouchScreenGestures : MonoBehaviour{

	#region Gesture event registration/unregistration

void OnEnable()
{
    Debug.Log( "Registering finger gesture events from C# script" );

    // register input events
    FingerGestures.OnFingerLongPress += FingerGestures_OnFingerLongPress;
    FingerGestures.OnFingerTap += FingerGestures_OnFingerTap;
    FingerGestures.OnFingerSwipe += FingerGestures_OnFingerSwipe;
    FingerGestures.OnFingerDragBegin += FingerGestures_OnFingerDragBegin;
    FingerGestures.OnFingerDragMove += FingerGestures_OnFingerDragMove;
    FingerGestures.OnFingerDragEnd += FingerGestures_OnFingerDragEnd; 
}
  
    void OnDisable()
    {
        // unregister finger gesture events
        FingerGestures.OnFingerLongPress -= FingerGestures_OnFingerLongPress;
        FingerGestures.OnFingerTap -= FingerGestures_OnFingerTap;
        FingerGestures.OnFingerSwipe -= FingerGestures_OnFingerSwipe;
        FingerGestures.OnFingerDragBegin -= FingerGestures_OnFingerDragBegin;
        FingerGestures.OnFingerDragMove -= FingerGestures_OnFingerDragMove;
        FingerGestures.OnFingerDragEnd -= FingerGestures_OnFingerDragEnd;
    }

    #endregion

    #region Reaction to gesture events

    void FingerGestures_OnFingerLongPress( int fingerIndex, Vector2 fingerPos )
    {
      
    }

    void FingerGestures_OnFingerTap( int fingerIndex, Vector2 fingerPos, int tapCount )
    {
     
    }

    // spin the yellow cube when swipping it
    void FingerGestures_OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {
       switch(direction)
		{
			case FingerGestures.SwipeDirection.Right:
				GameManager.instance.isSwipeRight=true;
			break;
			case FingerGestures.SwipeDirection.Left:
				GameManager.instance.isSwipeLeft=true;
			break;
			case FingerGestures.SwipeDirection.Up:
				GameManager.instance.isSwipeUp=true;
			break;
			case FingerGestures.SwipeDirection.Down:
				GameManager.instance.isSwipeDown=true;
			break;
		}
    }

    #region Drag & Drop Gesture

   

    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
        
    }

    void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
       
    }

    void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
     
    }

    #endregion

    #endregion

 
}
