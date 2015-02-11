using UnityEngine;
using System.Collections;

public class GenerateMap : MonoBehaviour {

	// Use this for initialization
	private Transform MH;

	// Prefabs
	public GameObject BG;

	//container coordinate and BG instantiate
	private Vector2 []CornerCamera;
	private float BgX,BgY,CamX,CamY;
	private Vector2 [] BG_Index;
	private GameObject[] BG_Object;
	private bool ok;

	void Start () {
		// Setting up the reference.
		BG_Object = new GameObject[4];
		BgX = BG.renderer.bounds.size.x;
		BgY = BG.renderer.bounds.size.y;
		CamX = BgX / 2;
		CamY = BgY / 2; 
		CornerCamera = new Vector2[4];
		BG_Index = new Vector2[4];
		for (int i=0; i<4; i++)
			BG_Index [i] = new Vector2(-1,-1);
	}
	
	// Update is called once per frame
	void Update () {
		CornerCamera[0] = calculateCoordinate(transform.position.x - CamX, transform.position.y - CamY);
		CornerCamera[1] = calculateCoordinate(transform.position.x + CamX, transform.position.y - CamY);
		CornerCamera[2] = calculateCoordinate(transform.position.x - CamX, transform.position.y + CamY);
		CornerCamera[3] = calculateCoordinate(transform.position.x + CamX, transform.position.y + CamY);
		// destroy background when it's out of camera
		for (int i=0; i<4; i++) 
			if (BG_Index[i].x!=-1)
			{
				ok = true;
				for (int j=0;j<4;j++)
					if (CornerCamera[j]==BG_Index[i]) ok = false;
				if (ok) {
					BG_Index[i] = new Vector2(-1,-1);
					Destroy (BG_Object[i]);
				}
			}
		// instantiate new background within camera
		for (int i=0; i<4; i++) 
		{
			ok = true;

			for (int j=0; j<4; j++)
				if (CornerCamera[i]==BG_Index[j]) ok = false;

			if (ok){
				for (int j=0; j<4; j++)
					if (BG_Index[j].x==-1)
					{
						BG_Index[j] = CornerCamera[i];
						//Debug.Log(" toado"+BG_Index[j].x+BG_Index[j].y);
						BG_Object[j] = Instantiate (BG,new Vector3(BG_Index[j].x*BgX, BG_Index[j].y*BgY, 0.001953125f), Quaternion.identity) as GameObject;
						break;
					}
			}
		}

	}

	//calculate witch contains coordinate(x,y);
	public Vector2 calculateCoordinate(float x, float y){
		int i = 0, j = 0;
		while (!((x>=(BgX*i-BgX/2))&&(x<=(BgX*i+BgX/2)))) i = i+1;
		while (!((y>=(BgY*j-BgY/2))&&(y<=(BgY*j+BgY/2)))) j = j+1;
		return (new Vector2 (i, j));
	}

}
