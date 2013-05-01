///////////////////////////////////////////////////////////
//
//   Author  : Alexander Orozco
//   Email   : alex@rozgo.com
//   License : Keep this notice around. Otherwise, enjoy!
//
///////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class Gestures : MonoBehaviour {
	
	// adjust accordingly in the inspector
	public Transform target;
	public float zoomNearLimit = 5;
	public float zoomFarLimit = 12;
	public float zoomScreenToWorldRatio = 3.0f;
	public float orbitScreenToWorldRatio = 1.0f;
	public float twistScreenToWorldRatio = 5.0f;
	
	// don't change these
	Vector3 orbitSpeed = Vector3.zero;
	float twistSpeed = 0;
	float distWeight;
	float zoomDistance;
	float zoomSpeed = 0;
	float lastf0f1Dist;
	
	void Update () {

		// one finger gestures
		if (iPhoneInput.touchCount == 1) {

			// finger data
			iPhoneTouch f0 = iPhoneInput.GetTouch(0);
			
			// finger delta
			Vector3 f0Delta = new Vector3(f0.deltaPosition.x, -f0.deltaPosition.y, 0);
			
			// if finger moving
			if (f0.phase == iPhoneTouchPhase.Moved) {
				
				// compute orbit speed
				orbitSpeed += (f0Delta + f0Delta * distWeight) * orbitScreenToWorldRatio * Time.deltaTime;
			}
		}
		
		// two fingers gestures
		else if (iPhoneInput.touchCount == 2) {
			
			// fingers data
			iPhoneTouch f0 = iPhoneInput.GetTouch(0);
			iPhoneTouch f1 = iPhoneInput.GetTouch(1);
			
			// fingers positions
			Vector3 f0Pos = new Vector3(f0.position.x, f0.position.y, 0);
			Vector3 f1Pos = new Vector3(f1.position.x, f1.position.y, 0);
			
			// fingers movements
			Vector3 f0Delta = new Vector3(f0.deltaPosition.x, f0.deltaPosition.y, 0);
			Vector3 f1Delta = new Vector3(f1.deltaPosition.x, f1.deltaPosition.y, 0);
			
			// fingers distance
			float f0f1Dist = Vector3.Distance(f0.position, f1.position);
			
			// if both fingers moving
			if (f0.phase == iPhoneTouchPhase.Moved && f1.phase == iPhoneTouchPhase.Moved) {
				
				// fingers moving direction
				Vector3 f0Dir = f0Delta.normalized;
				Vector3 f1Dir = f1Delta.normalized;
				
				// dot product of directions
				float dot = Vector3.Dot(f0Dir, f1Dir);
				
				// if fingers moving in opposite directions
				if (dot < -0.7f) {
					
					float pinchDelta = f0f1Dist - lastf0f1Dist;
					
					// if fingers move more than a threshold
					if (Mathf.Abs(pinchDelta) > 2) {
						
						// if pinch out, zoom in 
						if (f0f1Dist > lastf0f1Dist && zoomDistance > zoomNearLimit) {
							zoomSpeed += (pinchDelta + pinchDelta * distWeight) * Time.deltaTime * zoomScreenToWorldRatio;
						}
						
						// if pinch in, zoom out
						else if (f0f1Dist < lastf0f1Dist && zoomDistance < zoomFarLimit) {
							zoomSpeed += (pinchDelta + pinchDelta * distWeight) * Time.deltaTime * zoomScreenToWorldRatio;
						}
					}
					
					// detect twist
					if (f0Delta.magnitude > 2 && f1Delta.magnitude > 2) {
						
						// homemade algorithm works, but needs code review
						Vector3 fingersDir = (f1Pos - f0Pos).normalized;
						Vector3 twistNormal = Vector3.Cross(fingersDir, Vector3.forward);
						Vector3 twistAxis = Vector3.Cross(fingersDir, twistNormal);
						float averageDelta = (f0Delta.magnitude + f1Delta.magnitude) / 2;
						if (Vector3.Dot(f0Dir, twistNormal) > 0.7f) {
							twistSpeed =  twistAxis.z * averageDelta * Time.deltaTime * twistScreenToWorldRatio;
						}
						else if (Vector3.Dot(f0Dir, twistNormal) < -0.7f) {
							twistSpeed = -twistAxis.z * averageDelta * Time.deltaTime * twistScreenToWorldRatio;
						}
					}
				}
			}
			
			// record last distance, for delta distances
			lastf0f1Dist = f0f1Dist;
			
			// decelerate zoom speed
			zoomSpeed = zoomSpeed * (1 - Time.deltaTime * 10);
		}
		
		// no touching, or too many touches (we don't care about)
		else {
			
			// bounce to zoom limits
			if (zoomDistance < zoomNearLimit) {
				zoomSpeed += (zoomDistance - zoomNearLimit) * zoomScreenToWorldRatio;
			}
			else if (zoomDistance > zoomFarLimit) {
				zoomSpeed += (zoomDistance - zoomFarLimit) * zoomScreenToWorldRatio;
			}
			
			// or decelerate
			else {
				zoomSpeed = zoomSpeed * (1 - Time.deltaTime * 10);
			}
		}
		
		// decelerate orbit speed
		orbitSpeed = orbitSpeed * (1 - Time.deltaTime * 5);

		// decelerate twist speed
		twistSpeed = twistSpeed * (1 - Time.deltaTime * 5);

		// apply zoom
		transform.position += transform.forward * zoomSpeed * Time.deltaTime;
		zoomDistance = transform.position.magnitude;
		
		// apply orbit and twist
		transform.position = Vector3.zero;
		transform.localRotation *= Quaternion.Euler(orbitSpeed.y, orbitSpeed.x, twistSpeed);
		transform.position = -transform.forward * zoomDistance;
		
		// compensate for distance (ej. orbit slower when zoomed in; faster when out)
		distWeight = (zoomDistance - zoomNearLimit) / (zoomFarLimit - zoomNearLimit);
		distWeight = Mathf.Clamp01(distWeight);
	}
}
