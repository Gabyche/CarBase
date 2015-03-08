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

public class CarBase :MonoBehaviour {

	public List<AxleInfo> axleInfos; 
	public Rigidbody car;

	private Quaternion cylRotationRight =  Quaternion.AngleAxis (270, Vector3.right); // only for cylinder wheel not for 3d model
	private Quaternion cylRotationUp =  Quaternion.AngleAxis (90, Vector3.forward); // only for cylinder wheel not for 3d model
	
	public void Move(float motor, float brake, float steer) {
		foreach (AxleInfo axleInfo in axleInfos) {
			if (axleInfo.steering) {
				axleInfo.leftWheel.steerAngle = steer;
				axleInfo.rightWheel.steerAngle = steer;
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

	public bool GoForward() {
		AxleInfo axle = axleInfos [0];
		return (axle.leftWheel.rpm > 0);
	}
	
	public bool GoRewind() {
		return !GoForward();
	}

}
