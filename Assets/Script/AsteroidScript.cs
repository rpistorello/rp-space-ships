using UnityEngine;
using System.Collections;

public class AsteroidScript : MonoBehaviour {
	public int currentLife = 50;
	public float DestroyTime = 1f;
	public float maxInitialForce = 10;
	public float minInitialForce = 40;
	public float minInitialTorque = -50;
	public float maxInitialTorque = 50;

	private Animator anim;
	private bool dead;
	// Use this for initialization
	void Start () {
		dead = false;
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		Game.Data.VerifyEdges (this.gameObject);
		if (currentLife <= 0 && !dead) {
			DestroySelf ();
		}
	}
	
	void Awake(){
		//transform.position = new Vector3 (Game.Data.boundaryBL.position.x,0,0);
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		GetComponent<Rigidbody2D>().angularVelocity = 0;
		GetComponent<Rigidbody2D>().AddTorque (
			Random.Range(minInitialTorque, maxInitialTorque));

		Vector2 force = new Vector2 (
			Random.Range(-10, 10),
			Random.Range(-10, 10));

		GetComponent<Rigidbody2D>().AddForce(
			force.normalized * Random.Range(minInitialForce, maxInitialForce)
			);

	}
	void OnTriggerEnter2D(Collider2D obstacle){
		string tag = obstacle.gameObject.tag;
		Debug.Log("tag: " + tag);
		if (dead) return;
		if (tag.Contains ("shot")){ 
			obstacle.SendMessage ("ParticlesOn", SendMessageOptions.DontRequireReceiver);
			obstacle.SendMessage (Game.Data.DestroySelfMsg, SendMessageOptions.DontRequireReceiver);
			TakeDamage (Game.Data.shotDamage);
			return;
		}
		if (tag.Contains ("special")){ 
			obstacle.SendMessage ("ParticlesOn", SendMessageOptions.DontRequireReceiver);
			obstacle.SendMessage (Game.Data.DestroySelfMsg, SendMessageOptions.DontRequireReceiver);
			DestroySelf();
			return;
		}
		if (tag == "P1" || tag == "P2" ) {
			DestroySelf();
			obstacle.SendMessage ("TakeDamage", Game.Data.asteroidDamage, SendMessageOptions.DontRequireReceiver);
		}
	}
	public void DestroySelf() {
		anim.Play ("Destroy");
		dead = true;
		StartCoroutine(DestroyTimer ());
		
	}
	private IEnumerator DestroyTimer() {
		yield return new WaitForSeconds(DestroyTime);
		Destroy(gameObject);
	}

	void TakeDamage(int damage){
		currentLife-=damage;
	}
}
