using System.Collections;
using UnityEngine;

public class HommingMissileShooter : MonoBehaviour {

	public Transform Missile;
	public Transform MissileSpawnPosition;
	public float ReloadTime;
	private bool _canFire = true;

	void ShootHommingMissile () {
		if (!_canFire) return;

		_canFire = false;
		//StartCoroutine(ReloadTimer());
		Instantiate(Missile, MissileSpawnPosition.position, transform.rotation);
	}

	private IEnumerator ReloadTimer() {
		yield return new WaitForSeconds(ReloadTime);
		_canFire = true;
	}
}
