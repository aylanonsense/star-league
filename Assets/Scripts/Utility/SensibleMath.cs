using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public static class SensibleMath
	{
		public static float Range(float min = 0.00f, float max = 1.00f)
		{
			return Mathf.Clamp(Random.Range(min, max), min, Mathf.Max(min, max - 0.0001f));
		}

		public static int Range(int min = 0, int max = 1)
		{
			return Random.Range(min, max + 1);
		}
	}
}
