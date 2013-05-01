using UnityEngine;
using System.Collections;

public class object_touch_manipulation_iOS : MonoBehaviour {
	
	public Transform target_1;
	public Transform target_2;
	
	private Transform camera_1;
	private Transform camera_2;
	
	public float rotate_xy_ratio = 1.0f;
	public float twist_ratio = 1.0f;
	public float pan_ratio = 1.0f;
	public float zoom_ratio = 1.0f;
	
	public Vector3 boundary=new Vector3(5.0f, 5.0f, 5.0f);
	
	Vector3 rotateSpeed = Vector3.zero;
	float twistSpeed = 0;
	float zoomSpeed = 0;
	Vector3 panSpeed = Vector3.zero;
	float lastf0f1Dist;
	float distWeight;
	
	private Vector3 default_pos;
	private Quaternion default_rot;
	
	public bool environmental=false;
	public float vertical_rot = 0.0f;

	// Use this for initialization
	void Start () {
		camera_1=target_1.parent;
		camera_2=target_2.parent;
		
		default_pos=target_1.localPosition;
		default_rot=target_1.rotation;
		
		//~ boundary=target_1.localPosition+boundary;
	}
	
	
	
	// Update is called once per frame
	void Update () {
		
		// one finger gestures
		if (Input.touchCount == 1) {

			// finger data
			//~ iPhoneTouch f0 = Input.GetTouch(0);
			Touch f0 = Input.GetTouch(0);
			
			// finger delta
			Vector3 f0Delta = new Vector3(f0.deltaPosition.y, -f0.deltaPosition.x, 0);
			
			
			
			// if finger moving
			if (f0.phase == TouchPhase.Moved) {
				
				// compute orbit speed
				rotateSpeed += (f0Delta + f0Delta * 1) * 1 * Time.deltaTime;
				
				if(environmental){
					vertical_rot-=f0.deltaPosition.y * Time.deltaTime * rotate_xy_ratio * 10.0f;
					if(vertical_rot>85 && vertical_rot<180) vertical_rot=85;
					if(vertical_rot<0.5f) vertical_rot=0.5f;
				}
			}
		}
		
		// two fingers gestures
		else if (Input.touchCount == 2) {
			
			// fingers data
			Touch f0 = Input.GetTouch(0);
			Touch f1 = Input.GetTouch(1);
			
			// fingers positions
			Vector3 f0Pos = new Vector3(f0.position.x, f0.position.y, 0);
			Vector3 f1Pos = new Vector3(f1.position.x, f1.position.y, 0);
			
			// fingers movements
			Vector3 f0Delta = new Vector3(f0.deltaPosition.x, f0.deltaPosition.y, 0);
			Vector3 f1Delta = new Vector3(f1.deltaPosition.x, f1.deltaPosition.y, 0);
			
			// fingers distance
			float f0f1Dist = Vector3.Distance(f0.position, f1.position);
		
			if (f0.phase == TouchPhase.Moved && f1.phase == TouchPhase.Moved) {
				
				// fingers moving direction
				Vector3 f0Dir = f0Delta.normalized;
				Vector3 f1Dir = f1Delta.normalized;
				//~ Vector3 f0Dir = new Vector3(0.0f, 1.0f, 0);
				//~ Vector3 f1Dir = new Vector3(0.0f, -1.0f, 0);
				
				// dot product of directions
				float dot = Vector3.Dot(f0Dir, f1Dir);
				
				// if fingers moving in opposite directions
				if (dot < -0.7f) {
					
					float pinchDelta = f0f1Dist - lastf0f1Dist;
					
					// if fingers move more than a threshold
					if (Mathf.Abs(pinchDelta) > 2) {
						
						// if pinch out, zoom in 
						if (f0f1Dist > lastf0f1Dist) {
							zoomSpeed += (pinchDelta + pinchDelta * 1) * Time.deltaTime * -50 * zoom_ratio;
						}
						
						// if pinch in, zoom out
						else if (f0f1Dist < lastf0f1Dist) {
							zoomSpeed += (pinchDelta + pinchDelta * 1) * Time.deltaTime * -50 * zoom_ratio;
						}
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
						twistSpeed =  twistAxis.z * averageDelta * Time.deltaTime * twist_ratio;
					}
					else if (Vector3.Dot(f0Dir, twistNormal) < -0.7f) {
						twistSpeed = -twistAxis.z * averageDelta * Time.deltaTime * twist_ratio;
					}
				}

				
				//calculate the angle between 2 vector
				float gradient = Vector3.Angle(f0Dir, f1Dir);
				
				// if they are more or less parellel
				if (gradient< 5.0f) {
					
					Vector3 f0DN=f0Delta.normalized;
					Vector3 f1DN=f1Delta.normalized;

					panSpeed=new Vector3((f0DN.x+f1DN.x)/2, (f0DN.y+f1DN.y)/2, 0) * pan_ratio * 0.03f;
					//~ print(panSpeed);

				}

			}

			// record last distance, for delta distances
			lastf0f1Dist = f0f1Dist;
			
			// decelerate zoom speed
			zoomSpeed = zoomSpeed * (1 - Time.deltaTime * 10);
		}
		
		// no touching, or too many touches (we don't care about)
		else {
				zoomSpeed = zoomSpeed * (1 - Time.deltaTime * 10);
		}
		
		
		if(target_1!=null){
			//rotate around xy axis
			target_1.transform.Rotate(Vector3.up * rotateSpeed.y * rotate_xy_ratio, Space.World);
			target_1.transform.Rotate(Vector3.right * rotateSpeed.x * rotate_xy_ratio, Space.World);
			
			//rotate around z-axis
			target_1.transform.Rotate(Vector3.forward * twistSpeed * twist_ratio, Space.World);
			
			//translate/zoom the object towards/away from the camera
			Vector3 relativePos = target_1.position - camera_1.position;
			Vector3 dir = Quaternion.LookRotation(relativePos)*Vector3.forward;
			dir = target_1.InverseTransformDirection(dir);
			target_1.Translate(dir * zoomSpeed *Time.deltaTime);
			//~ target_1.Translate(Vector3.forward * zoomSpeed *Time.deltaTime);
			
			//pan the object along the plane parallel to the camera plane
			//~ target_1.Translate(panSpeed, Space.World);
			target_1.position+=panSpeed;//(panSpeed, Space.World);
		}
		
		if(target_2!=null){
			//rotate around xy axis
			target_2.transform.Rotate(Vector3.up * rotateSpeed.y * rotate_xy_ratio, Space.World);
			target_2.transform.Rotate(Vector3.right * rotateSpeed.x * rotate_xy_ratio, Space.World);
			
			//rotate around z-axis
			target_2.transform.Rotate(Vector3.forward * twistSpeed * twist_ratio, Space.World);
			
			//translate/zoom the object towards/away from the camera
			Vector3 relativePos = target_2.position - camera_2.position;
			Vector3 dir = Quaternion.LookRotation(relativePos)*Vector3.forward;
			dir = target_1.InverseTransformDirection(dir);
			target_2.Translate(dir * zoomSpeed *Time.deltaTime);
			//~ target_2.Translate(Vector3.forward * zoomSpeed *Time.deltaTime);
			
			//pan the object along the plane parallel to the camera plane
			//~ target_2.Translate(panSpeed);
			target_2.position+=panSpeed;
		}
	
		// decelerate rotate speed
		rotateSpeed = rotateSpeed * (1 - Time.deltaTime * 5);
		
		// decelerate twist speed
		twistSpeed = twistSpeed * (1 - Time.deltaTime * 5);
		
		// decelerate pan speed
		panSpeed = panSpeed * (1 - Time.deltaTime * 5);
	}
	
	void Limit(){
		Vector3 pos=target_1.transform.localPosition;
		if(Mathf.Abs(pos.x)>boundary.x){
			if(pos.x>0)
				pos=new Vector3(boundary.x, pos.y, pos.z);
			else
				pos=new Vector3(-boundary.x, pos.y, pos.z);
		}
		if(Mathf.Abs(pos.y)>boundary.y){
			if(pos.y>0)
				pos=new Vector3(pos.x, boundary.y, pos.z);
			else
				pos=new Vector3(pos.x, -boundary.y, pos.z);
		}
		if(pos.z>default_pos.z+boundary.z	|| pos.z<default_pos.z-boundary.z){
			if(pos.z>default_pos.z+boundary.z)
				pos=new Vector3(pos.x, pos.y, default_pos.z+boundary.z);
			else if(pos.z<default_pos.z-boundary.z)
				pos=new Vector3(pos.x, pos.y, default_pos.z-boundary.z);
		}
		target_1.transform.localPosition=pos;
		target_2.transform.localPosition=pos;
		
		if(environmental) Env_Limit();
	}
	
	void Env_Limit(){
		//~ if(environmental){
			float y_rot=target_1.eulerAngles.y;
			float x_rot=0;
			float z_rot=0;

			float modifier=0;
			modifier=0.75f+Mathf.Cos(4 * y_rot * Mathf.Deg2Rad)*0.25f;
			
			if(y_rot>315 && y_rot<360 || y_rot>=0 && y_rot<=45){
				if(y_rot<180){
					x_rot=-modifier*vertical_rot;
					z_rot=360-(1.0f-modifier)*vertical_rot;
				}
				else{
					x_rot=-modifier*vertical_rot;
					z_rot=(1.0f-modifier)*vertical_rot;
				}
			}
			else if(y_rot>45 && y_rot<=135){
				if(y_rot<90){
					z_rot=-modifier*vertical_rot;
					x_rot=360-(1.0f-modifier)*vertical_rot;
				}
				else{
					z_rot=-modifier*vertical_rot;
					x_rot=(1.0f-modifier)*vertical_rot;
				}
			}
			else if(y_rot>135 && y_rot<=225){
				if(y_rot<180){
					x_rot=modifier*vertical_rot;
					z_rot=360-(1.0f-modifier)*vertical_rot;
				}
				else{
					x_rot=modifier*vertical_rot;
					z_rot=(1.0f-modifier)*vertical_rot;
				}
			}
			else if(y_rot>225 && y_rot<=315){
				if(y_rot<270){
					z_rot=modifier*vertical_rot;
					x_rot=(1.0f-modifier)*vertical_rot;
				}
				else{
					z_rot=modifier*vertical_rot;
					x_rot=360-(1.0f-modifier)*vertical_rot;
				}
			}
			
			target_1.rotation=Quaternion.Euler(x_rot, target_1.eulerAngles.y, z_rot);

			target_2.rotation=target_1.rotation;
		//~ }
	}
	
	void Reset_Scene(){
		target_1.localPosition=default_pos;
		target_1.rotation=default_rot;
		
		target_2.localPosition=default_pos;
		target_2.rotation=default_rot;
	}
}
