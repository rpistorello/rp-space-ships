using UnityEngine;
using System.Collections;

public class Pursuit : MonoBehaviour {
	public GameObject target;
	public float maxVelocity=3f;
	// Use this for initialization
	void Start () {
		if (gameObject.tag.Contains ("P1"))
			target = Game.Data.Player2;
		
		if (gameObject.tag.Contains ("P2"))
			target = Game.Data.Player1;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 steering_force = pursuitTarget (this.gameObject, target);
		GetComponent<Rigidbody2D>().AddForce( Vector2.ClampMagnitude (steering_force, maxVelocity) );
		transform.rotation = Quaternion.Euler(0,0,
		                                      Mathf.Atan2 (GetComponent<Rigidbody2D>().velocity.y, GetComponent<Rigidbody2D>().velocity.x)
		                                      * 180 / Mathf.PI );
		if (GetComponent<Rigidbody2D>().velocity.magnitude > maxVelocity)
			GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * Game.Data.maxVelocity;

		Game.Data.VerifyEdges (this.gameObject);

	}


	Vector2 pursuitTarget(GameObject current, GameObject evader) {
		Vector2 toEvader = evader.transform.position - current.transform.position;
		float relativeDir;
		relativeDir = Vector2.Dot( GetComponent<Rigidbody2D>().velocity.normalized,
		                          evader.GetComponent<Rigidbody2D>().velocity.normalized);
		//Vector2 direcao = target.rigidbody2D.velocity.normalized;

		if (Vector2.Dot (toEvader, GetComponent<Rigidbody2D>().velocity.normalized) > 0 && relativeDir < -0.95)
			return seek(current, evader.transform.position);
		
		float lookAheadTime = toEvader.magnitude / Game.Data.maxVelocity
			+ evader.GetComponent<Rigidbody2D>().velocity.magnitude;
		Vector3 newDirecado = evader.GetComponent<Rigidbody2D>().velocity.normalized * lookAheadTime;
		return seek(current, evader.transform.position + newDirecado);
	}

	Vector2 seek(GameObject current, Vector3 target) {
		
		Vector2 desiredVelocity = target - current.transform.position;

		desiredVelocity = desiredVelocity.normalized * maxVelocity;
		
		return desiredVelocity - current.GetComponent<Rigidbody2D>().velocity;
	}
}
