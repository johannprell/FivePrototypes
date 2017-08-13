using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VomitFaceMovement : MonoBehaviour 
{
	/* --- MEMBER VARIABLES --- */
	private Transform _player;
	private NavMeshAgent _nav;

	[SerializeField]
	private float _speedMin = 1.5f;
	[SerializeField]
	private float _speedMax = 3.5f;

	/* --- UNITY METHODS --- */
	void Start () 
	{
		_player = GameObject.FindWithTag("Player").transform;
		_nav = GetComponent<NavMeshAgent>();

		_nav.speed = UnityEngine.Random.Range(_speedMin, _speedMax);
	}
	
	void Update () 
	{
		_nav.SetDestination(_player.position);
	}

	/* --- CUSTOM METHODS --- */
}
