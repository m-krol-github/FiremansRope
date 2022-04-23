using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Rope : MonoBehaviour
{
	public GameObject ropeSegmentPrefab;

	List<GameObject> ropeSegments = new List<GameObject>();

	public bool isIncreasing { get; set; }
	public bool isDecreasing { get; set; }

	public Rigidbody connectedObject;
	public float maxRopeSegmentLength = 1.0f;
	public float ropeSpeed = 4.0f;
	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();

		ResetLength();

	}

	public void ResetLength()
	{

		foreach (GameObject segment in ropeSegments)
		{
			Destroy(segment);

		}

		ropeSegments = new List<GameObject>();

		isDecreasing = false;
		isIncreasing = false;

		CreateRopeSegment();

	}

	public void CreateRopeSegment()
	{
		GameObject segment = (GameObject)Instantiate(ropeSegmentPrefab, this.transform.position, Quaternion.identity);

		segment.transform.SetParent(this.transform, true);

		Rigidbody segmentBody = segment.GetComponent<Rigidbody>();

		SpringJoint segmentJoint = segment.GetComponent<SpringJoint>();

		if (segmentBody == null || segmentJoint == null)
		{
			Debug.Log("Rope segment not created");
			return;
		}

		ropeSegments.Insert(0, segment);

		if (ropeSegments.Count == 1)
		{
			SpringJoint connectedObjectJoint = connectedObject.GetComponent<SpringJoint>();

			connectedObjectJoint.connectedBody = segmentBody;
			connectedObjectJoint.minDistance = 0.1f;

			segmentJoint.maxDistance = maxRopeSegmentLength;
		}
		else
		{
			GameObject nextSegment = ropeSegments[1];

			SpringJoint nextSegmentJoint = nextSegment.GetComponent<SpringJoint>();

			nextSegmentJoint.connectedBody = segmentBody;
			segmentJoint.minDistance = 0.0f;
		}
		segmentJoint.connectedBody = this.GetComponent<Rigidbody>();
	}

	public void RemoveRopeSegment()
	{
		if (ropeSegments.Count < 2)
		{
			return;
		}

		GameObject topSegment = ropeSegments[0];
		GameObject nextSegment = ropeSegments[1];

		SpringJoint nextSegmentJoint = nextSegment.GetComponent<SpringJoint>();

		nextSegmentJoint.connectedBody = this.GetComponent<Rigidbody>();

		ropeSegments.RemoveAt(0);
		Destroy(topSegment);

	}

	void Update()
	{
		if (lineRenderer != null)
		{
			lineRenderer.positionCount = ropeSegments.Count + 2;

			lineRenderer.SetPosition(0, this.transform.position);

			for (int i = 0; i < ropeSegments.Count; i++)
			{
				lineRenderer.SetPosition(i + 1, ropeSegments[i].transform.position);
			}

			SpringJoint connectedObjectJoint = connectedObject.GetComponent<SpringJoint>();
			lineRenderer.SetPosition( ropeSegments.Count + 1, connectedObject.transform.TransformPoint(connectedObjectJoint.anchor)
			);
		}

		
	}
}