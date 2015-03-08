using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class AxleInfo {
	public WheelCollider leftWheel;
	public WheelCollider rightWheel;
	public bool brake;
	public bool motor;
	public bool steering;
}

public class CarBase :MonoBehaviour {

	public float maxMotorTorque;
	public float maxBrakeTorque;
	public float maxSteeringAngle;
	

	public List<AxleInfo> axleInfos; 
	public Rigidbody car;

	private Quaternion cylRotationRight =  Quaternion.AngleAxis (270, Vector3.right); // only for cylinder wheel not for 3d model
	private Quaternion cylRotationUp =  Quaternion.AngleAxis (90, Vector3.forward); // only for cylinder wheel not for 3d model
	
	public void Move(float accel, float braking, float steering) {
		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = Steer(steering);
				axleInfo.rightWheel.steerAngle = Steer(steering);
			}
			if (axleInfo.motor) {
				axleInfo.leftWheel.motorTorque = Motor(accel);
				axleInfo.rightWheel.motorTorque = Motor(accel);
			}
			if (axleInfo.brake){
				axleInfo.leftWheel.brakeTorque = Brake(braking);
				axleInfo.rightWheel.brakeTorque = Brake(braking);
			}
			
			ApplyLocalPositionToVisuals(axleInfo.leftWheel);
			ApplyLocalPositionToVisuals(axleInfo.rightWheel);

			SpeedCounter();
			
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

	public float Steer(float steering) {
		return maxSteeringAngle * steering;
	}

	public float Brake(float braking) {
		return maxBrakeTorque * braking;
	}

	public float Motor(float accel) {
		return maxMotorTorque * accel;
	}

	public bool GoForward() {
		AxleInfo axle = axleInfos [0];
		return (axle.leftWheel.rpm > 0);
	}

	public bool GoRewind() {
		return !GoForward();
	}

	public float Speed() {
		float meterPerSecond = car.velocity.magnitude ;
		float kilometerPerHour = meterPerSecond * 3600f / 1000f;
		return kilometerPerHour;
	}

	public Text speedDisplay;

	public void SpeedCounter() {
		speedDisplay.text = Mathf.Round(Speed ()) + " km/h";
	}


}
