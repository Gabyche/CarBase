using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (CarBase))]
public class CarBaseController : MonoBehaviour {
	

	public CarBase car;

	public void Start() {
		car = GetComponent<CarBase> ();
	}

		
	public void FixedUpdate()
	{
		
		car.Move (Accel (), Brake (), Steer ());
	}
	



	public float Steer () {
		return Horizontal();
	}

	public float Brake() {
		if (Braking()) {
			return - Vertical();
		}
		return 0;
	}

	public float Accel() {
		if (!Braking()) {
			return Vertical();
		}
		return 0;
	}

	public bool Braking() {
		return Vertical() < 0 && car.GoForward ();
	}


	public float Vertical() {
		return Input.GetAxis ("Vertical");
	}
	
	public float Horizontal() {
		return Input.GetAxis ("Horizontal");
	}
}