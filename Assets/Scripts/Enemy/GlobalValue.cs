using UnityEngine;
using System.Collections;

public class GlobalValue : MonoBehaviour {

	public static int MaxNumberOfEnemy;
	public static int NumberOfCurrentEnemy;
	public static int NumberOfCurrentHealPoint;
	// Use this for initialization
	void Start () {
		MaxNumberOfEnemy = 6;
		NumberOfCurrentEnemy = 0;
		NumberOfCurrentHealPoint = 6;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
