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
		if(!GameManager.instance.isGamePaused)
    	 	GameManager.instance.CurrentBlock.RotateBlock();
		
		
    }

    // spin the yellow cube when swipping it
    void FingerGestures_OnFingerSwipe( int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity )
    {
      if(direction== FingerGestures.SwipeDirection.Up)
		{
			GameManager.instance.CurrentBlock.RotateBlock();
		}
		
    }

    #region Drag & Drop Gesture

   

    void FingerGestures_OnFingerDragBegin( int fingerIndex, Vector2 fingerPos, Vector2 startPos )
    {
	
        GameManager.instance.isLongPress=true;
    }

    void FingerGestures_OnFingerDragMove( int fingerIndex, Vector2 fingerPos, Vector2 delta )
    {
       

		if(delta.x>0&&delta.y==0)
			GameManager.instance.isSwipeRight=true;
		else if(delta.x<0&&delta.y==0)
			GameManager.instance.isSwipeLeft=true;
	//	else if(delta.y>0&&delta.x==0)
	//		GameManager.instance.isSwipeUp=true;
		else if(delta.y<0&&delta.x==0)
			GameManager.instance.isSwipeDown=true;
		
    }

    void FingerGestures_OnFingerDragEnd( int fingerIndex, Vector2 fingerPos )
    {
     	GameManager.instance.isLongPress=false;
    }

    #endregion

    #endregion

 
}
