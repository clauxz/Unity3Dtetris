using UnityEngine;
using System.Collections;

public class object_mouse_control : MonoBehaviour {

	public Transform target_1;
	public Transform target_2;
	
	private Transform camera_1;
	//~ private Transform camera_2;

	public bool environmental=false;
	//~ private Transform sub_1;
	//~ private Transform sub_2;
	
	public float mouse_sensitivity=25.0f;
	public float zoom_ratio=2.0f;
	
	public Vector3 boundary=new Vector3(5.0f, 5.0f, 5.0f);
	
	private float rot_y;
	private float rot_x;
	private float rot_z;
	private float tran_x;
	private float tran_y;
	private float tran_z;
	
	private Vector3 default_pos;
	private Quaternion default_rot;
	
	//Added by Sergio
	private bool sergio_ClickedMouse1 = false;
	private bool sergio_ZoomIn = false;
	private bool sergio_ZoomOut = false;
	private bool sergio_Pan = false;
	private int sergio_FlickCount = 0;
	private int sergio_ZoomInCount = 0;
	private int sergio_ZoomOutCount = 0;
	private int sergio_PanCount = 0;
	//Ended
	//~ private int last_zoom_dir = 0;
	
	// Use this for initialization
	void Start () {
		camera_1=target_1.parent;
		//~ camera_2=target_2.parent;
		
		default_pos=target_1.localPosition;
		default_rot=target_1.rotation;
		
		//~ if(environmental){
			//~ sub_1=target_1.transform.Find("sub");
			//~ sub_2=target_2.transform.Find("sub");
		//~ }
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(1)){
			rot_y = -Input.GetAxis("Mouse X") * mouse_sensitivity * 10.0f ; //define wanted y-axis rotation
			rot_x = Input.GetAxis("Mouse Y") * mouse_sensitivity * 10.0f ; //define wanted x-axis rotation
			rotateSpeed = new Vector3(rot_x, rot_y, 0);
			//Added by sergio
			sergio_ClickedMouse1 = true;	
			if(sergio_Pan){
				sergio_Pan = false;
				sergio_PanCount++;
			}			
			//Ended
		}
		else if(Input.GetButton("Shift")){ 
			tran_x = Input.GetAxis("Mouse X") * mouse_sensitivity * 0.01f ;
			tran_y = Input.GetAxis("Mouse Y") * mouse_sensitivity * 0.01f ;
			panSpeed=new Vector3(tran_x, tran_y, 0);
			//Added by sergio
			sergio_Pan = true;			
			if(sergio_ClickedMouse1){
				sergio_ClickedMouse1 = false;
				sergio_FlickCount++;
			}
			//Ended
		}	
		//Added by sergio
		else{
			if(sergio_ClickedMouse1){
				sergio_ClickedMouse1 = false;
				sergio_FlickCount++;
			}
			if(sergio_Pan){
				sergio_Pan = false;
				sergio_PanCount++;
			}
		}
		
		float wheel = Input.GetAxisRaw("Mouse ScrollWheel");
		zoomSpeed += wheel * -zoom_ratio; //Song Tan's ZoomIn ZoomOut 
		
		if(Mathf.Abs(zoomSpeed) > 0.1){
			if(zoomSpeed > 0.0){
				sergio_ZoomIn = false;
				if(!sergio_ZoomOut){
					sergio_ZoomOut = true;
					sergio_ZoomOutCount++;
					//~ print("zoom out "+sergio_ZoomOutCount);
					//~ last_zoom_dir=1;
				}
			}
			else if(zoomSpeed < 0.0){
				sergio_ZoomOut = false;
				if(!sergio_ZoomIn){
					sergio_ZoomIn = true;
					sergio_ZoomInCount++;
					//~ print("zoom in "+sergio_ZoomInCount);
					//~ last_zoom_dir=-1;
				}
			}
		}
		else{
			sergio_ZoomIn = false;
			sergio_ZoomOut = false;
			//~ if(sergio_ZoomIn){
				//~ sergio_ZoomIn = false;
				//~ if(last_zoom_dir!=1) sergio_ZoomInCount++;
			//~ }
			//~ if(sergio_ZoomOut){
				//~ sergio_ZoomOut = false;
				//~ if(last_zoom_dir!=-1) sergio_ZoomOutCount++;
			//~ }
		}
		//Ended
		
		//~ print(sergio_FlickCount+"  "+sergio_ZoomInCount+"  "+sergio_ZoomOutCount+"  "+sergio_PanCount);
		//~ print(zoomSpeed);
		
		if(environmental){
			vertical_rot-=rot_x * Time.deltaTime * mouse_sensitivity/25;
			if(vertical_rot>85 && vertical_rot<180) vertical_rot=85;
			if(vertical_rot<0.5f) vertical_rot=0.5f;
		}
		
		Limit();
		
		target_1.Rotate(Vector3.right * Time.deltaTime * rotateSpeed.x, Space.World);
		target_1.Rotate(Vector3.up * Time.deltaTime * rotateSpeed.y, Space.World);
		target_1.position+=panSpeed;
		
		Vector3 relativePos = target_1.position - camera_1.position;
		Vector3 dir = Quaternion.LookRotation(relativePos)*Vector3.forward;
		dir = target_1.InverseTransformDirection(dir);
		target_1.Translate(dir * zoomSpeed *Time.deltaTime);
		//~ target_1.Translate(Vector3.forward * zoomSpeed * 0.05f, Space.World);
		
		target_2.rotation=target_1.rotation;
		target_2.position=target_1.position;
		
		rotateSpeed = rotateSpeed * (1 - Time.deltaTime * 5);
		panSpeed = panSpeed * (1 - Time.deltaTime * 5);
		zoomSpeed = zoomSpeed * (1 - Time.deltaTime * 5);
		
		// decelerate all movement
		rotateSpeed = rotateSpeed * (1 - Time.deltaTime * 5);
		panSpeed = panSpeed * (1 - Time.deltaTime * 5);
		zoomSpeed = zoomSpeed * (1 - Time.deltaTime * 5);
		
		if(environmental) Env_Limit();
	}
	

	private Vector3 panSpeed = Vector3.zero;
	private Vector3 rotateSpeed = Vector3.zero;
	private float zoomSpeed = 0.0f;
	
	float vertical_rot=10;
	
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

			target_2.rotation=Quaternion.Euler(x_rot, target_2.eulerAngles.y, z_rot);
		//~ }
	}
	
	void Reset_Scene(){
		target_1.localPosition=default_pos;
		target_1.rotation=default_rot;
		
		target_2.localPosition=default_pos;
		target_2.rotation=default_rot;
	}

	void OnDrawGizmos(){
		//~ Quaternion dir_up=target_1.rotation;
		//~ Quaternion dir_cam=Quaternion.LookRotation(target_1.position-Vector3.zero) ;

		
		//~ Vector3 v_left =target_1.rotation*Quaternion.Euler(90, 0, 0)*target_1.TransformDirection(Vector3.forward);
		//~ Vector3 v_right = Quaternion.Euler(0, 25, 0)*transform.rotation*Vector3.forward;
		
		//~ Vector3 v_up=dir_up*target_1.TransformDirection(Transform.up);
		//~ Vector3 v_cam=dir_cam*target_1.InverseTransformDirection(target_1.Find("sketch").forward);
		
		//~ if(!obstacle_left)
			//~ Debug.DrawRay(transform.position, v_left * 2, Color.green);
		//~ else
			//~ Debug.DrawRay(target_1.position, v_up *5, Color.red);
			//~ Debug.DrawRay(target_1.position, v_cam *5, Color.green);
		

	}
	
	//Added By Sergio
	public int GetFlick()
	{
		return sergio_FlickCount;
	}
	
	public int GetZoomIn()
	{
		return sergio_ZoomInCount;
	}
	
	public int GetZoomOut()
	{
		return	sergio_ZoomOutCount;
	}	

	public int GetPan()
	{
		return	sergio_PanCount;
	}
	//End
}
