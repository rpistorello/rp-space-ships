using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {
	public int currentLife = 15;
	public float DestroyTime = 1f;
	public float minInitialForce = 10;
	public float maxInitialForce = 40;
	public float minInitialTorque = -50;
	public float maxInitialTorque = 50;

	private Animator anim;
	private bool dead = false;
	private Rigidbody2D body;

    void Awake(){
        anim = GetComponent<Animator> ();
        body = GetComponent<Rigidbody2D>();
    }

	void Start() {
        resetValues();
	}

    void OnEnable() {
        resetValues();
    }
	void resetValues() {
        Debug.Log("reseted asteroid");
        body.velocity = Vector2.zero;
        body.angularVelocity = 0;
        body.AddTorque (Random.Range(minInitialTorque, maxInitialTorque));

        Vector2 force = new Vector2 (
                Random.Range(-minInitialForce, minInitialForce),
                Random.Range(-minInitialForce, minInitialForce));

        body.AddForce(force.normalized * Random.Range(minInitialForce, maxInitialForce));

        dead = false;
    }
	
	// Update is called once per frame
	void Update () {
		Game.Data.VerifyEdges (this.gameObject);
		if (currentLife <= 0 && !dead) {
			DestroySelf ();
		}
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
        TrashMan.despawn(gameObject);
	}

	void TakeDamage(int damage){
		currentLife-=damage;
	}
}
