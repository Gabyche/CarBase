using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[RequireComponent (typeof (CarBase))]
public class CarBaseController : MonoBehaviour {

	
	public float maxMotorTorque;
	public float maxBrakeTorque;
	public float maxSteeringAngle;

	public CarBase car;

	public void Start() {
		car = GetComponent<CarBase> ();
	}

		
	public void FixedUpdate()
	{
		
		car.Move (Motor (), Brake (), Steer ());
	}
	



	public float Steer () {
		return maxSteeringAngle * Horizontal();
	}

	public float Brake() {
		if (Braking()) {
			return - maxBrakeTorque * Vertical();
		}
		return 0;
	}

	public float Motor() {
		float motor;
		if (!Braking()) {
			motor = maxMotorTorque * Vertical();
			if ( car.GoRewind() ) {
				motor = motor/1.2f;
			}
			return motor;
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