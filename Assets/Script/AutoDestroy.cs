using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
	public float durationTime = 2f;
	public bool particleOnDeath = false;
	public GameObject particleObject;
	// Use this for initialization
	void Start () {
		InvokeRepeating("DestroySelf", durationTime, 10f);
	}
	
	public void DestroySelf() {
		if(particleOnDeath) Instantiate(particleObject, transform.position, Quaternion.identity);
		Destroy(gameObject);

	}

	public void ParticlesOn(){
		particleOnDeath = true;
	}
}
