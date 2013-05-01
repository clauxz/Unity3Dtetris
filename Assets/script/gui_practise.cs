using UnityEngine;
using System.Collections;

public class gui_practise : MonoBehaviour {

	private Transform label_time;
	private Transform label_difference;
	private Transform label_num;
	private Transform label_error;
	private Transform label_message;
	private Transform gui_box;
	
	private Transform button_restart;
	private Transform button_reset;
	private Transform button_prev;
	private Transform button_next;
	
	public bool using_mouse=false;
	
	public float time_given=0;
	public int total_difference=0;
	//~ private float time_completed=0.0f;
	private int score=0;
	private int reset_count=0;
	private int click_count=0;
	private int error_click=0;
	private bool level_end=false;
	private bool level_started=false;
	private bool[] error_array= new bool[5];
	//~ private Transform[] button = new Transform[3];
	private Transform label_instruct;
	
	public string next_level="";
	
	private string language = "english";
	
	//~ public bool record_data = true;
	
	private GameObject next_level_indicator;
	

// Use this for initialization
	void Start () {
		//~ print(Application.loadedLevel.ToString());
		
		GameObject language_indicator=GameObject.FindWithTag("language_indicator");
		if(language_indicator!=null)
			language=language_indicator.GetComponent<language_indicator>().Get_Language();
		
		label_time=transform.Find("time_left");
		label_difference=transform.Find("difference_left");
		label_error=transform.Find("error_click");
		label_num=transform.Find("num_click");
		label_message=transform.Find("message");
		gui_box=transform.Find("box");
		
		button_restart=transform.Find("button_restart");
		button_reset=transform.Find("button_reset");
		button_prev=transform.Find("button_prev");
		button_next=transform.Find("button_next");
		
		button_restart.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
		button_reset.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
		button_prev.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
		button_next.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
		
		button_restart.gameObject.AddComponent<button>();
		button_reset.gameObject.AddComponent<button>();
		button_prev.gameObject.AddComponent<button>();
		button_next.gameObject.AddComponent<button>();
		
		label_instruct=transform.Find("instruction");
		
		
		if(language=="english"){
			//~ button_restart.guiText.text="";
			//~ button_prev.guiText.text="Main Menu";
			label_difference.guiText.text="Error Left: "+(total_difference)+"    Error Found: 0";
			button_next.guiText.text="Skip";
			button_reset.guiText.text="Reset";
			
			if(label_instruct!=null) label_instruct.guiText.text="If you finish your practice, please press next button to start the trial level";
		}
		else if(language=="japanese"){
			button_prev.guiText.text="メニュー";
			button_next.guiText.text="スキップ";
			button_reset.guiText.text="リセット";
			
			if(label_instruct!=null) label_instruct.guiText.text="練習が終わったら、次へボタンを押して下さい。間違い探しの練習に進みます。";
			
			label_time.guiText.text="残り時間: "+(time_given).ToString("f1")+"秒";
			label_difference.guiText.text="残りの違い: "+total_difference+"    違いを見つけた: 0";
			label_num.guiText.text="クリックの回数: 0";
			label_error.guiText.text="エラーダブルクリック: 0";
		}
		
		
			
		//~ label_time.guiText.text="Time Left: "+time_given.ToString("f1")+"sec";
		//~ label_difference.guiText.text="Error Left: "+total_difference+"    Error Found: "+score;
		//~ label_num.guiText.text="Number of Click: "+click_count;
		//~ label_error.guiText.text="Number of Error Click: "+error_click;
		
		
		for(int i=0; i<5; i++) error_array[i]=false;
		
		StartCoroutine(GUI_Update());
		StartCoroutine(Indicator_Update());
		
		camera_1=GameObject.Find("Cameras/Camera_1");
		camera_2=GameObject.Find("Cameras/Camera_2");
		
		next_level_indicator = Instantiate(Resources.Load("next_level_indicator")) as GameObject;
		if(next_level_indicator!=null){
			next_level_indicator.GetComponent<next_level_indicator>().Set_Level(next_level);
			next_level_indicator.GetComponent<next_level_indicator>().Set_Error_Count("2");
			DontDestroyOnLoad(next_level_indicator);
		}
	}
	
	IEnumerator GUI_Update(){
		//~ while(!level_started){
			//~ if(language=="english"){
				//~ label_message.guiText.text="There are "+total_difference+" differences in this level. Find them All!\nGame start in "+(2.5f-Time.timeSinceLevelLoad).ToString("f1")+"sec";
			//~ }
			//~ else if(language=="japanese"){
				//~ label_message.guiText.text="このレベル"+total_difference+"の違いがあります. それらをすべて見つけてください!\n"+(2.5f-Time.timeSinceLevelLoad).ToString("f1")+"でゲームスタート";
			//~ }
			//~ Rect rect_box=label_message.guiText.GetScreenRect ();
			//~ gui_box.guiTexture.pixelInset=new Rect(-rect_box.width/2-10, -rect_box.height/2-10, rect_box.width+20, rect_box.height+20);
			//~ if(2.5f-Time.timeSinceLevelLoad<=0.15) level_started=true;
			//~ yield return new WaitForSeconds (0.1f);
		//~ }
		level_started=true;
		StartCoroutine(Start_Message());
		while(!level_end){
			if(language=="english"){
				label_time.guiText.text="Time Left: "+(time_given-Time.timeSinceLevelLoad+1.5f).ToString("f1")+"sec";
				label_difference.guiText.text="Error Left: "+(total_difference-score)+"    Error Found: "+score;
				label_num.guiText.text="Number of Click: "+click_count;
				label_error.guiText.text="Number of Error Click: "+error_click;
			}
			else if(language=="japanese"){
				label_time.guiText.text="残り時間: "+(time_given-Time.timeSinceLevelLoad+1.5f).ToString("f1")+"秒";
				label_difference.guiText.text="間違い残り:"+(total_difference-score)+"個    違いを見つけた: "+score;
				label_num.guiText.text="クリックの回数: "+click_count;
				label_error.guiText.text="エラーダブルクリック: "+error_click;
			}
			yield return new WaitForSeconds (0.1f);
		}
		
		gui_box.guiTexture.enabled=true;
		if(score==total_difference){
			if(language=="english"){
				label_message.guiText.text="Level Cleared\nPress next button to proceed";
				label_difference.guiText.text="Error Left: 0    Error Found: "+total_difference;
			}
			else if(language=="japanese"){
				label_message.guiText.text="レベルクリア!\n次へボタンを押すと、次に進みます。";
				label_difference.guiText.text="間違い残り:0個    違いを見つけた: "+total_difference;
			}
			Rect rect_box=label_message.guiText.GetScreenRect ();
			gui_box.guiTexture.pixelInset=new Rect(-rect_box.width/2-10, -rect_box.height/2-10, rect_box.width+20, rect_box.height+20);
		}
		else {
			if(language=="english"){
				button_next.guiText.text="Next";
				label_time.guiText.text="Time Left: 0.0sec";
				label_message.guiText.text="Game Over\nPress next button to proceed";
			}
			else if(language=="japanese"){
				button_next.guiText.text="次へ";
				label_time.guiText.text="残り時間: 0.0秒";
				label_message.guiText.text="Game Over\n次のボタン押し続けると";
			}
			Rect rect_box=label_message.guiText.GetScreenRect ();
			gui_box.guiTexture.pixelInset=new Rect(-rect_box.width/2-10, -rect_box.height/2-10, rect_box.width+20, rect_box.height+20);
		}
	}
	
	
	
	IEnumerator Start_Message(){
		if(language=="english"){
			label_message.guiText.text="Game started!";
		}
		else if(language=="japanese"){
			label_message.guiText.text="ゲームを開始!";
		}
		Rect rect_box=label_message.guiText.GetScreenRect ();
		gui_box.guiTexture.pixelInset=new Rect(-rect_box.width/2-10, -rect_box.height/2-10, rect_box.width+20, rect_box.height+20);
		yield return new WaitForSeconds (1.0f);
		label_message.guiText.text="";
		gui_box.guiTexture.enabled=false;
	}
	
	private GameObject[] object_indicators;
	private float dis;
	public float scale_modifier=10.0f;
	
	IEnumerator Indicator_Update(){
		while(true){
			object_indicators = GameObject.FindGameObjectsWithTag("indicator");
			foreach (GameObject ind in object_indicators) {
				temp_camera=camera_1.GetComponent("Camera") as Camera;
				dis=Vector3.Distance(ind.transform.parent.transform.position, camera_1.transform.position);
				
				Vector3 temp=ind.transform.parent.transform.localScale;
				float size=(temp[0]+temp[1]+temp[2])/3;
				
				Vector3 screenPos = temp_camera.WorldToViewportPoint(ind.transform.parent.transform.position);
				ind.transform.position=new Vector3(screenPos.x, screenPos.y, 0);
				float scale=128*(scale_modifier/dis)*size;
				ind.guiTexture.pixelInset=new Rect(screenPos.x-scale/2, screenPos.y-scale/2, scale, scale);
			}
			//~ if(level_end) break;
			yield return null;
		}
		//~ object_indicators = GameObject.FindGameObjectsWithTag("indicator");
		//~ foreach (GameObject ind in object_indicators) Destroy(ind);
	}
	
	
	//~ bool touch_state=false;
	
	private bool touched_1=false;
	private bool touched_2=false;
	
	// Update is called once per frame
	void Update () {
		if(!level_end && level_started){
			if(!using_mouse){
				//~ if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began){
				if(iPhoneInput.touchCount==1 && !touched_1){
					touched_1=true;
					if ((Time.time - doubleTapStart) < 1.2f){
						OnSelectDifference(iPhoneInput.GetTouch(0).position);
						doubleTapStart=-1;
					}
					else{
						doubleTapStart = Time.time;
					}
				}
				else if(iPhoneInput.touchCount!=1) touched_1=false;
			}
			else if(using_mouse){
				if(Input.GetMouseButtonDown(0)){
					print("mousedown");
					if ((Time.time - doubleClickStart) < 0.5f){
						OnSelectDifference(Input.mousePosition);
						doubleClickStart=-1;
					}
					else{
						doubleClickStart = Time.time;
					}
				}
			}

			
			if(time_given-Time.timeSinceLevelLoad<=0) {
				level_end=true;
				Show_All_Errors();
			}
			
			//~ print(Time.timeSinceLevelLoad);
			//~ string temp=(Time.timeSinceLevelLoad-2.5f).ToString("f1");
					//~ print("skip: "+temp);
		}
		
		if(using_mouse){
			if(Input.GetMouseButtonDown(0)){
				Vector2 pos=Input.mousePosition;
				Test_Button_Pressed(pos);
			}			
		}
		else{
			//~ if(iPhoneInput.touchCount==1 && iPhoneInput.GetTouch(0).phase==iPhoneTouchPhase.Began){
			if(iPhoneInput.touchCount==1 && !touched_2){
				touched_2=true;
				iPhoneTouch touch = iPhoneInput.GetTouch(0);
				Test_Button_Pressed(touch.position);
			}
			else if(iPhoneInput.touchCount!=1) touched_2=false;
		}
		
		//~ if(iPhoneInput.touchCount==1 && !touch_state){
		if(iPhoneInput.touchCount==1){
			//~ touch_state=true;
			iPhoneTouch touch = iPhoneInput.GetTouch(0);
			if (button_restart.guiText.HitTest( touch.position )){
				button_restart.guiText.material.color=Color.green;
			}
			else if (button_reset.guiText.HitTest( touch.position )){
				button_reset.guiText.material.color=Color.green;
			}
			else if (button_prev.guiText.HitTest( touch.position )){
				button_prev.guiText.material.color=Color.green;
			}
			else if (button_next.guiText.HitTest( touch.position )){
				button_next.guiText.material.color=Color.green;
			}
			else{
				button_restart.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
				button_reset.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
				button_next.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
				button_prev.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
			}
		}
		//~ else{
			//~ button_reset.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
			//~ button_restart.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
			//~ button_next.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
			//~ button_prev.guiText.material.color=new Color(0.5f, 0.5f, 0.5f, 0.5f);
		//~ }
		
		
		if(score==total_difference && !level_end) {
			//~ time_completed=(Time.timeSinceLevelLoad-1.5f);
			level_end=true;
			if(language=="english") button_next.guiText.text="Next";
			else if(language=="japanese") button_next.guiText.text="次へ";
		}
	}
	
	//test if a button has been pressed
	void Test_Button_Pressed(Vector2 pos){
		//~ if (button_restart.guiText.HitTest( pos )){
			//~ button_restart.guiText.material.color=Color.red;
			//~ StartCoroutine(Load_Next_Level(Application.loadedLevelName));
			//~ if(score==total_difference) Record("completed");
			//~ else Record("skipped");
		//~ }
		if (button_reset.guiText.HitTest( pos )){
			button_reset.guiText.material.color=Color.red;
			reset_count+=1;
			gameObject.SendMessage("Reset_Scene");
		}
		else if (button_prev.guiText.HitTest( pos )){
			button_prev.guiText.material.color=Color.red;
			StartCoroutine(Load_Next_Level(1)); //main_menu
			if(score==total_difference) Record("completed");
			else Record("skipped");
		}
		else if (button_next.guiText.HitTest( pos )){
			button_next.guiText.material.color=Color.red;
			if(next_level!="") {
				if(next_level=="main_menu")
					StartCoroutine(Load_Next_Level(1)); //main_menu
				else
					StartCoroutine(Load_Next_Level(2)); //next_level
				if(score==total_difference) Record("completed");
				else Record("skipped");
			}
			else {
				//~ StartCoroutine(Load_Next_Level(2)); //game_finish
				if(score==total_difference) Record("completed");
				else Record("skipped");
			}
		}
	}
	
	void Record(string status){
		//~ float temp=0.0f;
		//~ if(record_data){
			//~ ScoreManager scoreMagr = gameObject.AddComponent<ScoreManager>();
			//~ scoreMagr.enabled=false;
			
			//~ object_mouse_control omc = gameObject.GetComponent<object_mouse_control>() as object_mouse_control;
				
			//~ if(scoreMagr != null){
				//~ if(status=="completed")
					//~ scoreMagr.SaveScore(Application.loadedLevel.ToString(), click_count.ToString(), time_completed.ToString("f1"), error_array, reset_count.ToString(), status, omc.GetFlick(), omc.GetZoomIn(), omc.GetZoomOut());
				//~ else if(status=="skipped"){
					//~ temp=(Time.timeSinceLevelLoad-2.5f);
					//~ if(temp<0.0f) temp=0.0f;
					//~ scoreMagr.SaveScore(Application.loadedLevel.ToString(), click_count.ToString(), temp.ToString("f1"), error_array, reset_count.ToString(), status, omc.GetFlick(), omc.GetZoomIn(), omc.GetZoomOut());
				//~ }
			//~ }
		//~ }
	}
	
	private GameObject camera_1;
	private GameObject camera_2;
	//~ GameObject temp_camera;
	Camera temp_camera;
	
	private Ray ray; 
	private RaycastHit hit;
	
	private float doubleTapStart=0f;
	private float doubleClickStart=0f;
	
	public Transform indicator;
	private Transform instance_indicator;
	
	

	void OnSelectDifference(Vector3 position){
		click_count+=1;
		if(position.x<Screen.width/2)
			temp_camera=camera_1.GetComponent("Camera") as Camera;
		else
			temp_camera=camera_2.GetComponent("Camera") as Camera;

		
		ray = temp_camera.ScreenPointToRay(position);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
			if(hit.collider.tag=="difference"){
				score+=1;
				hit.collider.gameObject.tag="Untagged";
				instance_indicator=Instantiate(indicator, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
				instance_indicator.parent=hit.collider.gameObject.transform;
				if(int.Parse(hit.collider.gameObject.name)-1<5)
					error_array[int.Parse(hit.collider.gameObject.name)-1]=true;
			}
			else {
				error_click+=1;
				//~ print("error_click wrong object");
			}
		}
		else {
			error_click+=1;
			//~ print("error_click_not hitting anything");
		}
	}
	
	void Show_All_Errors(){
		GameObject[] difference_objects = GameObject.FindGameObjectsWithTag("difference");
		foreach (GameObject dif_object in difference_objects) {
			dif_object.tag="Untagged";
			instance_indicator=Instantiate(indicator, new Vector3(0, 0, 0), Quaternion.identity) as Transform;
			instance_indicator.parent=dif_object.transform;
			instance_indicator.guiTexture.color= new Color(1.0f, 0.0f, 0.0f, 0.8f);
		}
	}
	
	IEnumerator Load_Next_Level(int level){
		yield return new WaitForSeconds (0.1f);
		//~ Application.LoadLevel(level);
		
		if(level==1){
			//~ print(level+"!");
			Destroy(next_level_indicator);
			Application.LoadLevel("main_menu");
		}
		else if(level==2){
			//~ print(level+"!");
			//~ Destroy(next_level_indicator);
			//~ Application.LoadLevel("game_finished_screen");
		//~ }
		//~ else{
			Application.LoadLevel("inter_level");
		}
	}
	
	
}
