using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipHealthBar : MonoBehaviour {

	public Scrollbar HealthBar;
	public float Health = 100;
	public Slider Stamina;

	public void damage(float value){
		Health -= value;
		HealthBar.size = Health / 100f;

		Stamina.value = Health / 100f;
	}	
}
