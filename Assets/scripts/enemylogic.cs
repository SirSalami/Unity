﻿using UnityEngine;
using System.Collections;

public class enemylogic : MonoBehaviour {
	
	public GameObject player; // drag the player here
	float pathfindingtimer;
	public float evadedistance;
	bool evade;
	GameObject evadetarget;
	float spawntimer;
	public int hitpoints = 1;
	public GameObject explosion;
	Vector3 myposition;
	Rigidbody mybody;
	Vector3 evadevariance;
	float mouseinputtime;
	
	// Use this for initialization
	void Start () {
		
		evadedistance = 5f;
		
		spawntimer += Time.deltaTime;
		player = GameObject.FindGameObjectWithTag("Player");
		transform.LookAt(player.transform.position);
		//rigidbody.AddForce(transform.forward * 100.0f, ForceMode.Impulse);
		
		//evadevariance = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
	
	}
	
	void Update () {
		
		myposition = transform.position;
		
		if (collider.enabled == false)
		{
			spawntimer += Time.deltaTime;
			if (spawntimer > 1.0f)
			{
				collider.enabled = true;
			}
		}
		
		Debug.DrawRay(myposition, rigidbody.velocity.normalized * evadedistance, Color.blue, 0.1f);
		Debug.DrawRay(myposition, transform.forward * evadedistance, Color.white, 0.1f);
		
		//transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		if (player && !evade)
		{
	    	transform.LookAt(player.transform.position + player.rigidbody.velocity);
		}

		//evasion state		
		pathfindingtimer += Time.deltaTime;
		if (pathfindingtimer > 0.2f)
		{
			
			
			Debug.DrawRay(myposition, (transform.forward*-1) * evadedistance, Color.red, 0.1f);
			Debug.DrawRay(myposition, transform.up * evadedistance, Color.red, 0.1f);
			Debug.DrawRay(myposition, (transform.up*-1) * evadedistance, Color.red, 0.1f);
			
			int layermask = 1<<12 | 1<<8 | 1<<13 ;
			RaycastHit hit;
			
	        Collider[] hitColliders = Physics.OverlapSphere(myposition, evadedistance, layermask);
			if (hitColliders.Length > 0)
			{
				evade = true;
				float nearestobstacledistance = Mathf.Infinity;
				foreach(Collider obstacle in hitColliders)
				{
					if (Vector3.Distance(myposition, obstacle.transform.position) < nearestobstacledistance)
					{
						nearestobstacledistance = Vector3.Distance(myposition, obstacle.transform.position);
						evadetarget = obstacle.gameObject;
						Vector3 direction = myposition - evadetarget.transform.position;
						//print (Vector3.Angle(transform.position, evadetarget.transform.position));
						Debug.DrawLine(myposition, evadetarget.transform.position, Color.yellow, 0.33f);
						transform.rotation = Quaternion.LookRotation(direction);
					}
				}
			}
			else if (evade)
			{
				evade = false;
				evadetarget = null;
				evadevariance = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
			}

			pathfindingtimer = 0.0f;
		}
		
		/*
		//missile targeting
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			Vector3 mousepos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 75f));
			if (Vector3.Distance(mousepos, myposition) < 5.0f)
			{

					player.GetComponent<playerlogic>().homingtargets.Add(gameObject);

			}
		}
		*/

		
	}
	
	public float movementspeed = 10;
	
	void FixedUpdate() {
		mybody = rigidbody;

		if (mybody.velocity.magnitude > movementspeed)
		{
			mybody.AddForce(mybody.velocity * -movementspeed);
			//print (rigidbody.velocity.magnitude);
		}
		else
		{
			//handle evasion velocity, or just move toward player
			if (evade)
			{
					mybody.AddForce((transform.forward) * (movementspeed*0.5f), ForceMode.Impulse);
				
			}
			else
			{
				mybody.AddForce(transform.forward * movementspeed, ForceMode.Impulse);
			}

		}
	}
	
    void OnCollisionEnter(Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal*-5, Color.white, 1.0f);
			hitpoints--;
        }
		if (hitpoints <= 0)
		{
			//print (controlscript.score);
			player.GetComponent<playerlogic>().score += 100;
			GameObject explosionclone = Instantiate(explosion, myposition, transform.rotation) as GameObject;
			Destroy(explosionclone, 2.0f);
			Destroy(gameObject);
		}
		//print ("ping");
    }
}
