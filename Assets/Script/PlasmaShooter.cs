﻿using System.Collections;
using UnityEngine;

public class PlasmaShooter : MonoBehaviour {

	public GameObject PlasmaShot;
	public Transform PlasmaSpawnPosition;
	public float ReloadTime;
	private bool _canFire = true;

	void ShootPlasma () {
		if (!_canFire) return;

		_canFire = false;
		StartCoroutine(ReloadTimer());
		TrashMan.spawn(PlasmaShot, PlasmaSpawnPosition.position, transform.rotation);
	}

	private IEnumerator ReloadTimer() {
		yield return new WaitForSeconds(ReloadTime);
		_canFire = true;
	}
}
