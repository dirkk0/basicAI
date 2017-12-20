

using System;


using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class basicAI : MonoBehaviour {

		public UnityEngine.AI.NavMeshAgent agent;
		public ThirdPersonCharacter character;

		public enum State {
			PATROL,
			CHASE
		}

		public State state;
		private bool alive;

		// Variables for Patroling
		public GameObject[] waypoints;
		private int waypointInd = 1;
		public float patrolSpeed = 0.5f;

		// Variables for Chasing
		public float chaseSpeed = 1f;
		public GameObject target;

		// Use this for initialization
		void Start () {
			Debug.Log("yes");
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			character = GetComponent<ThirdPersonCharacter>();

			agent.updatePosition = true;
			agent.updateRotation = false;

			state = basicAI.State.PATROL;


		}

		IEnumerator FSM()
		{
			while (alive)
			{
				switch (state)
				{
				case State.PATROL:
					Patrol ();
					break;
				case State.CHASE:
					Chase ();
					break;


				}
				yield return null;
			}
		}

		void Patrol()
		{
			agent.speed = patrolSpeed;



			if (waypoints.Length > 0) {

				//				Debug.Log ( "i: " + waypointInd.ToString());
				//				Debug.Log ( "w: " + waypoints.Length);

				if (Vector3.Distance (this.transform.position, waypoints[waypointInd].transform.position) >= 2)
				{
					agent.SetDestination (waypoints [waypointInd].transform.position);
					character.Move (agent.desiredVelocity, false, false);
				}
				else if (Vector3.Distance (this.transform.position, waypoints[waypointInd].transform.position) <= 2)
				{
					waypointInd += 1;
					if (waypointInd >= waypoints.Length) {
						waypointInd = 0;
					}
					Debug.Log ( waypointInd);
				}
				else{
					character.Move (Vector3.zero, false, false);
				}			}

		}

		void Chase()
		{
			agent.speed = chaseSpeed;
			agent.SetDestination (target.transform.position);
			character.Move (agent.desiredVelocity, false, false);

		}

		void OnTriggerEnter (Collider coll)
		{
			if (coll.tag == "Player")
			{
				state = basicAI.State.CHASE;
				target = coll.gameObject;
			}
		}

		// Update is called once per frame
		void Update () {

			if (alive == false) {
				alive = true;

				// START FSM
				StartCoroutine("FSM");


			}

		}
	}

}

