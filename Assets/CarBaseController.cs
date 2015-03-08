using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool brake;
	public bool motor;
	public bool steering;
}

public class CarBaseController : MonoBehaviour {
	public List<AxleInfo> axleInfos; 
	public float maxMotorTorque;
	public float maxBrakeTorque;
	public float maxSteeringAngle;
	public Rigidbody voiture;

	private Quaternion cylRotationRight =  Quaternion.AngleAxis (270, Vector3.right);
	private Quaternion cylRotationUp =  Quaternion.AngleAxis (90, Vector3.forward);

	public void FixedUpdate()
	{
		
		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = Steer();
				axleInfo.rightWheel.steerAngle = Steer();
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = Motor();
				axleInfo.rightWheel.motorTorque = Motor();
			}
			if (axleInfo.brake){
				axleInfo.leftWheel.brakeTorque = Brake();
				axleInfo.rightWheel.brakeTorque = Brake();
			}

			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
			
		}
	}

	public void ApplyLocalPositionToVisuals(WheelCollider collider)
	{
		if (collider.transform.childCount == 0) {
			return;
		}
		
		Transform visualWheel = collider.transform.GetChild(0);
		
		Vector3 position;
		Quaternion rotation;
		collider.GetWorldPose(out position, out rotation);
		
		visualWheel.transform.position = position;
		visualWheel.transform.rotation = rotation*cylRotationRight*cylRotationUp;
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
			if ( GoRewind() ) {
				motor = motor/1.2f;
			}
			return motor;
		}
		return 0;
	}

	public bool Braking() {
		return Vertical() < 0 && GoForward();
	}

	public bool GoForward() {
		AxleInfo axle = axleInfos [0];
		return (axle.leftWheel.rpm > 0);
	}
	
	public bool GoRewind() {
		return !GoForward();
	}
	
	public float Vertical() {
		return Input.GetAxis ("Vertical");
	}
	
	public float Horizontal() {
		return Input.GetAxis ("Horizontal");
	}
}