using System;
using UnityEngine;


	public static class Geometry
	{
		public static float LinePlaneDistance(Vector3 linePoint, Vector3 lineVec, Vector3 planePoint, Vector3 planeNormal)
		{
			//calculate the distance between the linePoint and the line-plane intersection point
			float dotNumerator = Vector3.Dot((planePoint - linePoint), planeNormal);
			float dotDenominator = Vector3.Dot(lineVec, planeNormal);

			//line and plane are not parallel
			if(dotDenominator != 0f)
			{
				return dotNumerator / dotDenominator;
			}
			
			return 0;
		}

		//Note that the line is infinite, this is not a line-segment plane intersect
		public static Vector3 LinePlaneIntersect(Vector3 linePoint, Vector3 lineVec, Vector3 planePoint, Vector3 planeNormal)
		{
			float distance = LinePlaneDistance(linePoint, lineVec, planePoint, planeNormal);

			//line and plane are not parallel
			if(distance != 0f)
			{
				return linePoint + (lineVec * distance);	
			}

			return Vector3.zero;
		}
	}