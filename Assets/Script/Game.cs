using UnityEngine;

public class Game : MonoBehaviour {

	public static Game Data;
	public string WasHitMsg = "WasHit";
	public string DestroySelfMsg = "DestroySelf";
	public int Score = 0;
	public int Lives = 3;

	public GameObject Asteroid;
	public int asteroidDamage = 20;
	public int shotDamage = 15;
	public int specialDamage = 35;

	public GameObject Player1;
	public GameObject Player2;
	public int maxLife = 100;
	public float trustForce = 10f;
	public float maxVelocity = 10f;
	public float rotationSpeed = 60f;


	public Transform boundaryBL;
	public Transform boundaryTR;

	public TextMesh GUITutorialP1;
	public TextMesh GUITutorialP2;
	public TextMesh GUIGameOver;

	private bool _newGame;
	public void Start() {
		GUIGameOver.text = "    Sudden Death!\n" +
			"Press Enter to start";
		GUITutorialP1.text = "-Player 1- \n" +
				"Movement: W,A and D\n" +
				"Plasma Shot: C\n" +
				"HommingMissile(1 load): V";
		GUITutorialP2.text = "-Player 2- \n" +
			"Movement: Up,Left and Right arrows\n" +
				"Plasma Shot: N\n" +
				"HommingMissile(1 load): M";
		InvokeRepeating("AsteroidSpawn", 5f, 5f);
		Time.timeScale = 0f;
		_newGame = true;
	}

	void Awake () {
		Data = this;
	}

	void Update(){
		if (Time.timeScale == 0f) {
			if (Input.GetKeyDown (KeyCode.Return)){
				if(_newGame){
					Time.timeScale = 1.0f;
					GUIGameOver.gameObject.SetActive (false);
					GUITutorialP1.gameObject.SetActive (false);
					GUITutorialP2.gameObject.SetActive (false);
				}
				else Application.LoadLevel("GameScene");

			}
			return;
		}

		//Player1
		if (Input.GetKey (KeyCode.W))
			Player1.SendMessage ("Trust",SendMessageOptions.DontRequireReceiver);
				else
			Player1.SendMessage ("StopParticles",SendMessageOptions.DontRequireReceiver);

		if (Input.GetKey (KeyCode.A)) 
			Player1.SendMessage ("Rotate",  1,SendMessageOptions.DontRequireReceiver);
		if (Input.GetKey (KeyCode.D)) 
			Player1.SendMessage ("Rotate",  -1,SendMessageOptions.DontRequireReceiver);
		if (Input.GetKey (KeyCode.C))
			Player1.SendMessage ("ShootPlasma",SendMessageOptions.DontRequireReceiver);
		if (Input.GetKey (KeyCode.V))
			Player1.SendMessage ("ShootHommingMissile",SendMessageOptions.DontRequireReceiver);


		//Player2
		if (Input.GetKey (KeyCode.UpArrow))
			Player2.SendMessage ("Trust",SendMessageOptions.DontRequireReceiver);
		else
			Player2.SendMessage ("StopParticles",SendMessageOptions.DontRequireReceiver);
		
		if (Input.GetKey (KeyCode.LeftArrow)) 
			Player2.SendMessage ("Rotate",  1,SendMessageOptions.DontRequireReceiver);
		if (Input.GetKey (KeyCode.RightArrow)) 
			Player2.SendMessage ("Rotate",  -1,SendMessageOptions.DontRequireReceiver);
		if (Input.GetKey (KeyCode.N))
			Player2.SendMessage ("ShootPlasma",SendMessageOptions.DontRequireReceiver);
		if (Input.GetKey (KeyCode.M))
			Player2.SendMessage ("ShootHommingMissile",SendMessageOptions.DontRequireReceiver);

		if (Lives > 0)return;




		/*GUIGameOver.gameObject.SetActive (true);
		Time.timeScale = 0;
		GameObject.FindGameObjectWithTag ("Player").SendMessage(
								DestroySelfMsg,
								SendMessageOptions.DontRequireReceiver);
		*/
	}

	public void SetEnd(string player){
		GUIGameOver.gameObject.SetActive (true);
		GUIGameOver.text = "     " +player + " wins! \n" +
			"Press Enter to Reset";
		_newGame = false;
		Time.timeScale = 0f;
	}

	public void IncreaseScore(int scoreBonus) {


	}

	public void AsteroidSpawn() {

		Vector3 newSpawnPos = new Vector3(boundaryBL.position.x, boundaryBL.position.y, 0);
		Instantiate(Asteroid, newSpawnPos, Quaternion.Euler(0,0, Random.Range(0,359)));
		Debug.Log ("Asteroid spawned");
		//Player1.SendMessage ("Rotate",  -1, SendMessageOptions.DontRequireReceiver);
	}

	public void DecreaseLife(int lifeReduction){
		Lives -= lifeReduction;
	}

	public void VerifyEdges(GameObject obj){
		if (obj.transform.position.x < boundaryBL.position.x)
						obj.transform.position = new Vector3 (boundaryTR.position.x,
			                                      obj.transform.position.y, 
			                                      obj.transform.position.z);
		if (obj.transform.position.x > boundaryTR.position.x)
			obj.transform.position = new Vector3 (boundaryBL.position.x,
			                                      obj.transform.position.y, 
			                                      obj.transform.position.z);
		if (obj.transform.position.y < boundaryBL.position.y)
			obj.transform.position = new Vector3 (obj.transform.position.x,
			                                      boundaryTR.position.y,
			                                      obj.transform.position.z);
		if (obj.transform.position.y > boundaryTR.position.y)
			obj.transform.position = new Vector3 (obj.transform.position.x,
			                                      boundaryBL.position.y,
			                                      obj.transform.position.z);
	}

}
