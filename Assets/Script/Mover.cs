using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public Vector3 Offset;

// 	// Use this for initialization
// 	void Start () {
// 	
// 	}
	
	void Update () {
		Game.Data.VerifyEdges (this.gameObject);
		var dT = Time.deltaTime;
		transform.Translate(Offset * dT);
	}
}
