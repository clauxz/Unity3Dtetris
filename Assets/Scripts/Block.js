var block : String[];
var playable = true;


private var blockMatrix : boolean[,];
private var fallSpeed : float;
private var yPosition : int;
private var xPosition : int;
private var size : int;
private var halfSize : int;
private var halfSizeFloat : float;
private var dropped = false;
private var material : Material;

function Start () {
	// Sanity checking
	size = block.Length;
	var width = block[0].Length;
	if (size < 2) {
		Debug.LogError ("Blocks must have at least two lines");
		return;
	}
	if (width != size) {
		Debug.LogError ("Block width and height must be the same");
		return;
	}
	if (size > Manager.use.maxBlockSize) {
		Debug.LogError ("Blocks must not be larger than " + Manager.use.maxBlockSize);
		return;
	}
	for (i = 1; i < size; i++) {
		if (block[i].Length != block[i-1].Length) {
			Debug.LogError ("All lines in the block must be the same length");
			return;
		}
	}
	
	halfSize = (size/2);
	halfSizeFloat = (size*.5); // halfSize is an integer for the array, but we need a float for positioning the on-screen cubes (for odd sizes)
	
	// Convert block string array from the inspector to a boolean 2D array for easier usage
	blockMatrix = new boolean[size, size];
	for (y = 0; y < size; y++) {
		for (x = 0; x < size; x++) {
			if (block[y][x] == "1"[0]) {
				blockMatrix[x, y] = true;
				var block = Instantiate(Manager.use.cube, Vector3((x-halfSizeFloat)*.1, ((size-y)+halfSizeFloat-size)*.1, 0.0), Quaternion.identity) as Transform;
				block.parent = transform;
				block.renderer.material = material;
			}
		}
	}
	
	// For blocks with even sizes, we just add 0, but odd sizes need .5 added to the position to work right
	transform.position.x = (Manager.use.FieldWidth()/2 + (size%2 == 0? 0.0 : .5));
	xPosition = transform.position.x - halfSizeFloat;
	yPosition = Manager.use.FieldHeight() - 1;
	transform.position.y = yPosition - halfSizeFloat;
	fallSpeed = Manager.use.blockNormalSpeed;
	
	// Check to see if this block would overlap existing blocks, in which case the game is over
	if (Manager.use.CheckBlock (blockMatrix, xPosition, yPosition)) {
		Manager.use.GameOver();
		return;
	}
	
	CheckInput();
	yield Delay( (1.0 / Manager.use.blockNormalSpeed) * 2.0 );
	Fall();
}

// This is used instead of WaitForSeconds, so that the delay can be cut short if player hits the drop button
function Delay (time : float) {
	var t = 0.0;
	//Debug.Log(time);
	while (t <= Manager.use.delayTime && !dropped) {
		t += Time.deltaTime;	
		yield;
	}
}

function Fall () {
	while (playable) {
		// Check to see if block would collide if moved down one row
		yPosition--;
		if (Manager.use.CheckBlock (blockMatrix, xPosition, yPosition)) {
			Manager.use.SetBlock (blockMatrix, xPosition, yPosition+1, material);
			Destroy(gameObject);
			break;
		}
		
		// Make on-screen block fall down 1 square
		// Also serves as a delay...if you want old-fashioned square-by-square movement, replace this with yield WaitForSeconds
		for (var i : float = yPosition+1; i > yPosition; i -= Time.deltaTime*fallSpeed) {
			transform.position.y = i - halfSizeFloat;
			yield;
		}
	}
}

function CheckInput () {
	while (playable) {
		
		//var input = Input.GetAxis("Horizontal");
		if (Input.GetKey(KeyCode.LeftArrow)||Input.GetKey(KeyCode.A)) {
			yield MoveHorizontal(-1);
		}
		else if (Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.W)) {
			yield MoveHorizontal(1);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W)) {
			RotateBlock();
		}
		
		if (Input.GetKey(KeyCode.DownArrow)||Input.GetKey(KeyCode.S)) {
			fallSpeed = Manager.use.blockDropSpeed;
			//dropped = true;
			//Manager.use.score += 5;
			//break;	// Break out of while loop, so the coroutine stops (we don't care about input anymore)
		}
		else
		{
			fallSpeed = Manager.use.blockNormalSpeed ;
			
		}
		
		//Change the next block
		if(Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Space))
		{
			Manager.use.ChangeNextBlock();
		}
		
		//Get speed down ou up just in Manual game
		if(Manager.use.gameKind == "Manually")
		{
			if(Input.GetKeyDown(KeyCode.KeypadPlus))
			{
				Manager.use.blockNormalSpeed += .5;
				Manager.use.delayTime -= Manager.use.delayTime * 0.4;
			}
			if(Input.GetKeyDown(KeyCode.KeypadMinus) && Manager.use.blockNormalSpeed > 2.0)
			{
				Manager.use.blockNormalSpeed -= .5;
				Manager.use.delayTime += Manager.use.delayTime * 0.4;
			}
		}
		
		yield;
	}
}

function MoveHorizontal (dir : int) {
	// Check to see if block could be moved in the desired direction
	if (!Manager.use.CheckBlock (blockMatrix, xPosition + dir, yPosition)) {
		transform.position.x += dir;
		xPosition += dir;
		yield WaitForSeconds (Manager.use.blockMoveDelay);
	}
}

function RotateBlock () {
	// Rotate matrix 90° to the right and store the results in a temporary matrix
	var tempMatrix = new boolean[size, size];
	for (y = 0; y < size; y++) {
		for (x = 0; x < size; x++) {
			tempMatrix[y, x] = blockMatrix[x, (size-1)-y];
		}
	}
	
	// If the rotated block doesn't overlap existing blocks, copy the rotated matrix back and rotate on-screen block to match
	if (!Manager.use.CheckBlock (tempMatrix, xPosition, yPosition)) {
		System.Array.Copy (tempMatrix, blockMatrix, size*size);
		transform.Rotate (Vector3.forward * -90.0);
	}
}

function SetMaterial(mat : Material){
	material = mat;
}
