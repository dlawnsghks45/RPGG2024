﻿using UnityEngine;
using System.Collections;

public class Rotate2 : MonoBehaviour
{
	Vector3 angle;

	void Start()
	{
		angle = transform.eulerAngles;
	}

	void Update()
	{
		angle.y += Time.deltaTime * 100;
		transform.eulerAngles = angle;
	}

}
