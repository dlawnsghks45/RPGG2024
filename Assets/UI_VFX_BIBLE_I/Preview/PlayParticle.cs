using UnityEngine;
using System.Collections;

public class PlayParticle : MonoBehaviour {
	public GameObject []myParticles;
	int ShowParticle;
	bool tip;
	bool rotate;
	// Use this for initialization
	void Start () 
	{

		
		ShowParticle = 0;
		if(myParticles.Length != 0)
			myParticles[ShowParticle].SetActive(true);
		tip = false;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			prevParticle();
		}
		else if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			nextParticle();
		}
	}
	

	
	void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width * 0.7f, 0f, Screen.width * 0.05f, Screen.width * 0.05f), ">"))
		{
			nextParticle();
		}		
		if(myParticles.Length != 0)
		{
			if(myParticles[ShowParticle] != null)
				GUI.Box(new Rect(Screen.width * 0.3f, 0f, Screen.width * 0.4f, Screen.width * 0.05f), "NO. : " + ShowParticle.ToString() + "\n" + myParticles[ShowParticle].name);
			else
				GUI.Box(new Rect(Screen.width * 0.3f, 0f, Screen.width * 0.4f, Screen.width * 0.05f), "NO. : " + ShowParticle.ToString() + "\n東西不見啦!!");
		}
		else
		{
			tip = true;
		}
		
			
		if(GUI.Button(new Rect(Screen.width * 0.25f, 0f, Screen.width * 0.05f, Screen.width * 0.05f), "<"))
		{
			prevParticle();
		}
		
		if(GUI.Button(new Rect(Screen.width * 0.95f, 0f, Screen.width * 0.05f, Screen.width * 0.05f), "?"))
		{
			tip = !tip;
		}
				
		if(tip)
		{
			if(GUI.Button(new Rect(Screen.width * 0.0f, Screen.width * 0.1f, Screen.width * 1f, Screen.width * 0.3f), "Press < and > to see the next one!"))
			{
				tip = !tip;
			}
			
		}
	}
	void nextParticle()
	{
		if(myParticles[ShowParticle] != null)
			myParticles[ShowParticle].SetActive(false);
		ShowParticle++;
		if(ShowParticle >= myParticles.Length)
			ShowParticle = 0;
		if(myParticles[ShowParticle] != null)
			myParticles[ShowParticle].SetActive(true);
	}
	void prevParticle()
	{
		if(myParticles[ShowParticle] != null)
			myParticles[ShowParticle].SetActive(false);
		ShowParticle--;
		if(ShowParticle < 0)
			ShowParticle = myParticles.Length -1;
		if(myParticles[ShowParticle] != null)
			myParticles[ShowParticle].SetActive(true);
	}	
	
	
	
}
