using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour
{
	Rigidbody rb;

	float floatDelay;

	[SerializeField]
	bool floatToSleep = false;

	[SerializeField]
	float submergenceOffset = 0.5f;

	[SerializeField, Min(0.1f)]
	float submergenceRange = 1f;

	[SerializeField, Min(0f)]
	float buoyancy = 1f;

	[SerializeField, Range(0f, 10f)]
	float waterDrag = 1f;

	[SerializeField]
	LayerMask waterMask = 0;

	[SerializeField]
	Vector3 buoyancyOffset = Vector3.zero;

	float submergence;

	Vector3 gravity;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
	}

	void FixedUpdate()
	{
		if (floatToSleep)
        {
			if (rb.IsSleeping())
			{
				floatDelay = 0f;
				return;
			}

			if (rb.velocity.sqrMagnitude < 0.0001f)
			{
				floatDelay += Time.deltaTime;
				if (floatDelay >= 1f)
				{
					return;
				}
			}
			else
			{
				floatDelay = 0f;
			}
		}

		gravity = CustomGravity.GetGravity(rb.position);
		if (submergence > 0f)
		{
			float drag = Mathf.Max(0f, 1f - waterDrag * submergence * Time.deltaTime);
			rb.velocity *= drag;
			rb.angularVelocity *= drag;
			rb.AddForceAtPosition(gravity * -(buoyancy * submergence), transform.TransformPoint(buoyancyOffset), ForceMode.Acceleration);
			submergence = 0f;
		}
		rb.AddForce(gravity, ForceMode.Acceleration);
	}

	void OnTriggerEnter(Collider other)
	{
		if ((waterMask & (1 << other.gameObject.layer)) != 0)
		{
			EvaluateSubmergence();
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (!rb.IsSleeping() && (waterMask & (1 << other.gameObject.layer)) != 0)
		{
			EvaluateSubmergence();
		}
	}

	void EvaluateSubmergence()
	{
		Vector3 upAxis = -gravity.normalized;
		if (Physics.Raycast(rb.position + upAxis * submergenceOffset, -upAxis, out RaycastHit hit, submergenceRange + 1f, waterMask, QueryTriggerInteraction.Collide))
		{
			submergence = 1f - hit.distance / submergenceRange;
		}
		else
		{
			submergence = 1f;
		}
	}
}
