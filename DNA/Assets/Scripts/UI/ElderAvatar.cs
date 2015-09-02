﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameInventory;

[RequireComponent (typeof (Image))]
public class ElderAvatar : MBRefs, IPoolable {

	ElderItem elder;
	bool trackHealth = false;

	Image image = null;
	Image Image {
		get {
			if (image == null) {
				image = GetComponent<Image> ();
			}
			return image;
		}
	}

	public void Init (ElderItem elder) {
		this.elder = elder;
		//Image.fillAmount = elder.HealthManager.Health;		
	}

	IEnumerator CoTrackHealth () {
		//int blink = 0;
		//int blinkSpeed = 10;
		while (trackHealth) {
			if (elder != null) {
				/*Image.fillAmount = elder.HealthManager.Health;
				if (elder.HealthManager.Sick) {
					blink ++;
					if (blink < blinkSpeed) {
						Image.color = Color.red;
					} else {
						Image.color = Color.white;
					}
					if (blink > blinkSpeed*2) {
						blink = 0;
					}
				} else {
					Image.color = Color.white;
				}*/
			}
			yield return null;
		}
	}

	public void OnEnable () {
		StartCoroutine (CoTrackHealth ());
	}

	public void OnPoolCreate () {
		trackHealth = true;
	}

	public void OnPoolDestroy () {
		trackHealth = false;
	}
}