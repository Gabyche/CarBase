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
	
	// finds the corresponding visual wheel
	// correctly applies the transform
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
	
	public void FixedUpdate()
	{
		float brake=0;
		float motor=0;

		if (Input.GetAxis ("Vertical") < 0 && GoForward()) {
			 brake = - maxBrakeTorque * Input.GetAxis ("Vertical");
		}
		else {
			 motor = maxMotorTorque * Input.GetAxis ("Vertical");
			if ( GoRewind() ) {
				motor = motor/1.5f;
			}
		}

		float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
		
		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steering;
				axleInfo.rightWheel.steerAngle = steering;
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = motor;
				axleInfo.rightWheel.motorTorque = motor;
			}
			if (axleInfo.brake){
				axleInfo.leftWheel.brakeTorque = brake;
				axleInfo.rightWheel.brakeTorque = brake;
			}



			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);
			
		}
	}

	public bool GoForward() {
		AxleInfo axle = axleInfos [0];
		return (axle.leftWheel.rpm > 0);
	}

	public bool GoRewind() {
		return !GoForward();
	}
}