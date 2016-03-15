using UnityEngine;
using System.Collections;

public class SpaceshipMovement : MonoBehaviour {

	public Transform frontPos;
	public int currentLife;
	public int energy, maxEnergy = 100;
	public ParticleSystem part;

	public GameObject special;

	public string shotTag;
	public string specialTag;

	public SpriteRenderer healthBar;
	private Vector3 healthScale;

	void Start () {
		currentLife = Game.Data.maxLife;
		healthScale = healthBar.transform.localScale;
		UpdateHealth ();
	}
	
	// Update is called once per frame
	void Update () {
		Game.Data.VerifyEdges (this.gameObject);
		healthBar.transform.position = transform.position;
		if (currentLife <= 0){
			if(gameObject.tag == "P1"){
				Game.Data.SetEnd("Player 2");
			}else{
				Game.Data.SetEnd("Player 1");
			}
		}
	}



	void Trust () {
		part.enableEmission = true;
		Vector2 direction = frontPos.position - transform.position;
		GetComponent<Rigidbody2D>().AddForce (direction.normalized * Game.Data.trustForce);
		if (GetComponent<Rigidbody2D>().velocity.magnitude > Game.Data.maxVelocity)
						GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * Game.Data.maxVelocity;

	}

	void Rotate (float rotation){
		//transform.rotation = Quaternion.RotateTowards (transform.rotation, angleToRotate, Game.Data.rotationSpeed * Time.deltaTime);
		transform.rotation = transform.rotation * Quaternion.Euler (0, 0,
		                                                            rotation * Game.Data.rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter2D(Collider2D obstacle){
		string tag = obstacle.gameObject.tag;
		Debug.Log("tag: " + tag);
		if (tag == shotTag || tag == specialTag) return;

		if (tag == "Asteroid") {
			//obstacle.SendMessage ("TakeDamage", 50, SendMessageOptions.DontRequireReceiver);
			//TakeDamage (Game.Data.asteroidDamage);
			return;
		}
		if (tag.Contains ("shot")){ 
			obstacle.SendMessage ("ParticlesOn", SendMessageOptions.DontRequireReceiver);
			obstacle.SendMessage (Game.Data.DestroySelfMsg, SendMessageOptions.DontRequireReceiver);
			TakeDamage (Game.Data.shotDamage);
			return;
		}
		if (tag.Contains ("special")){ 
			obstacle.SendMessage ("ParticlesOn", SendMessageOptions.DontRequireReceiver);
			obstacle.SendMessage (Game.Data.DestroySelfMsg, SendMessageOptions.DontRequireReceiver);
			TakeDamage (Game.Data.specialDamage);
			return;
		}
	}

	void TakeDamage(int damage){
		currentLife-=damage;
		UpdateHealth ();
	}

	void UpdateHealth(){
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - currentLife * 0.01f);

		healthBar.transform.localScale = new Vector3(healthScale.x * currentLife * 0.01f, healthScale.y, healthScale.z);
		if (currentLife <= 0) {
			healthBar.transform.localScale = new Vector3(0, healthScale.y, healthScale.z);
		}
	}

	public void DestroySelf() {

	}

	void StopParticles(){
		part.enableEmission = false;
	}

}
