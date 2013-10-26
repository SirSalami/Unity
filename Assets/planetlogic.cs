﻿using UnityEngine;
using System.Collections;

public class planetlogic : MonoBehaviour {
	
	public GameObject target; //target to orbit around
	public GameObject player;
	public GameObject enemy; //is set in editor
	public bool trigger;
	public float rotationspeed;
	public float orbitspeed;
	public float mydistance;
	public float maxhealth;
	public float health;
	public GameObject eruption;

	// Use this for initialization
	void Start () {
		
		rigidbody.freezeRotation = true;
		
		player = GameObject.Find("player");
		
		rotationspeed = Random.Range(20.0f, 75.0f);
		orbitspeed = target.transform.localScale.x*Random.Range(0.2f, 1.0f);
        //rigidbody.AddTorque(Vector3.forward * -100);
		if (Random.Range(0, 2) < 1)
		{
			rotationspeed*=-1;
		}
		if (Random.Range(0, 2) < 1)
		{
			orbitspeed*=-1;
		}
		
		transform.localScale *= Random.Range(1.0f, 3.5f);

		mydistance = Vector3.Distance(target.transform.position+new Vector3(0,0,0), transform.position);
		
		maxhealth = 50;
		health = 50;
		
		eruption.transform.localScale = transform.localScale;
	}
	
	void Update() {
		
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		
		if (health < maxhealth)
		{
			health += Time.deltaTime;
		}

	    // rotation
	    transform.Rotate(Vector3.forward * rotationspeed * Time.deltaTime);
	 
	    // orbit
	    transform.RotateAround (target.transform.position, Vector3.forward, orbitspeed * Time.deltaTime);
		
		//check if player is in range of sun to spawn. move this to sun at somepoint so it's called less
		//print (Vector3.Distance(target.transform.position+new Vector3(0,0,-60), player.transform.position));
		float playerdistance = Vector3.Distance(target.transform.position, player.transform.position);

			if (renderer.isVisible || playerdistance < mydistance)
			{
				trigger = true;
			}
			else
			{
				trigger = false;
			}
		
		//spawn enemies
		//print (timer);
		if (Time.frameCount%100 == 0 && trigger)
		{
			Instantiate(enemy, new Vector3(transform.position.x, transform.position.y, 0), transform.rotation);
		}
	}
	
    void OnCollisionEnter(Collision collision) {
		
		if (health < 0)
		{
			Destroy(gameObject, 0.25f);	
		}
		else
		{
			eruption.particleSystem.emissionRate = maxhealth - health;
			health -= collision.transform.localScale.x*2;
		}

	}
	
}
