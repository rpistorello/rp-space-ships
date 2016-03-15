using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {
	public float durationTime = 2f;
	public bool particleOnDeath = false;
	public GameObject particleObject;

	void Awake() {

	}

	void Start () {
	}

	void OnEnable() {

        InvokeRepeating("DestroySelf", durationTime, 10f);
	}

	void OnDisable() {

        CancelInvoke("DestroySelf");
	}
	public void DestroySelf() {
        CancelInvoke("DestroySelf");
		if(particleOnDeath) TrashMan.spawn(particleObject, transform.position, Quaternion.identity);
		TrashMan.despawn(gameObject);
	}

	public void ParticlesOn(){
		particleOnDeath = true;
	}
}
