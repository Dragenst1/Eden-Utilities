using System.Collections.Generic;
using UnityEngine;

namespace Eden.Utils
{
	public static class V1
	{
		public static bool InRange(float f, Vector2 range)
		{
			return InRange(f, range.x, range.y);
		}
		public static bool InRange(float f, float min, float max)
		{
			return f >= min && f <= max;
		}
		
		public static float SmoothLerp(float f, float n)
		{
			if (f != 0f && f != 0.5f && f != 1f)
			{
				if (f < 0.5f) { return SlerpLower(f, n) / 2f; }
				else { return SlerpUpper(f, n) / 2f + 0.5f; }
			}
			return f;
		}
		
		public static float SlerpUpper(float f, float n)
		{
			f = Mathf.Clamp01(f);
			n = Mathf.Clamp01(n);

			float n0 = Mathf.Pow(2 * n + 1, 2 * n + 1);
			return 1f - Mathf.Pow(1f - f, n0);
		}
		
		public static float SlerpLower(float f, float n)
		{
			f = Mathf.Clamp01(f);
			n = Mathf.Clamp01(n);

			float n0 = Mathf.Pow(2 * n + 1, 2 * n + 1);
			return Mathf.Pow(f, n0);
		}
		
		public static float SmoothMax(float a, float b, float k)
		{
			return SmoothMin(a, b, 0f - k);
		}

		public static float SmoothMin(float a, float b, float k)
		{
			if (k < 0f) { k = Mathf.Clamp(k, -2f, 0f); }
			else { k = Mathf.Clamp(k, 0f, 2f); }

			float h = Mathf.Min(1f, Mathf.Max(0, (b - a + k) / (2f * k)));
			return a * h + b * (1f - h) - h * k * (1f - h);
		}
	}
	
	public static class V2
	{
		public static float DistanceFromLine(Vector2 k, Vector2 pivot, float angle)
		{
			return DistanceFromLine(k.x, k.y, pivot.x, pivot.y, pivot.x + Mathf.Cos(angle), pivot.y + Mathf.Sin(angle));
		}
		public static float DistanceFromLine(Vector2 k, Vector2 start, Vector2 end)
		{
			return DistanceFromLine(k.x, k.y, start.x, start.y, end.x, end.y);
		}
		public static float DistanceFromLine(Vector2 k, float x1, float y1, float x2, float y2)
		{
			return DistanceFromLine(k.x, k.y, x1, y1, x2, y2);
		}
		public static float DistanceFromLine(float kx, float ky, float x1, float y1, float x2, float y2)
		{
			return Vector2.Distance(new Vector2(kx, ky), ClosestPointOnLine(kx, ky, x1, y1, x2, y2));
		}

		//0 yz, 1 xz, 2 xy
		public static Vector2 IsolateFromVector3(Vector3 v, int axis)
		{
			return new Vector2(axis == 0 ? v.y : v.x, axis == 2 ? v.y : v.z);
		}
		public static Vector2 IsolateFromVector3(Vector3 v, int axis, bool normalizeOutput)
		{
			Vector2 result = new Vector2(axis == 0 ? v.y : v.x, axis == 2 ? v.y : v.z);
			return normalizeOutput ? result.normalized : result;
		}
		public static Vector2 IsolateFromVector3(Vector3 v, int axis, out float other)
		{
			other = v[axis]; return new Vector2(axis == 0 ? v.y : v.x, axis == 2 ? v.y : v.z);
		}
		public static Vector2 IsolateFromVector3(Vector3 v, int axis, bool normalizeOutput, out float other)
		{
			Vector2 result = new Vector2(axis == 0 ? v.y : v.x, axis == 2 ? v.y : v.z);
			other = v[axis]; return normalizeOutput ? result.normalized : result;
		}

		public static Vector2 ClosestPointOnLine(Vector2 k, Vector2 pivot, float angle)
		{
			return ClosestPointOnLine(k.x, k.y, pivot.x, pivot.y, pivot.x + Mathf.Cos(angle), pivot.y + Mathf.Sin(angle));
		}
		public static Vector2 ClosestPointOnLine(Vector2 k, Vector2 start, Vector2 end)
		{
			return ClosestPointOnLine(k.x, k.y, start.x, start.y, end.x, end.y);
		}
		public static Vector2 ClosestPointOnLine(Vector2 k, float x1, float y1, float x2, float y2)
		{
			return ClosestPointOnLine(k.x, k.y, x1, y1, x2, y2);
		}
		public static Vector2 ClosestPointOnLine(float kx, float ky, float x1, float y1, float x2, float y2)
		{
			if (x1 == x2 && y1 == y2) { return new Vector2(x1, y1); }
			if (x1 == x2) { return new Vector2(x1, ky); }
			if (y1 == y2) { return new Vector2(kx, y1); }
			
			float m = (y2 - y1) / (x2 - x1);
			float x0 = (ky - y1 + m * x1 + 1 / m * kx) / (m + (1 / m));
			float y0 = (0f - 1 / m) * (x0 - kx) + ky;

			return new Vector2(x0, y0);
		}

		public static float DistanceFromSegment(Vector2 k, Vector2 p1, Vector2 p2)
		{
			return DistanceFromSegment(k.x, k.y, p1.x, p1.y, p2.x, p2.y);
		}
		public static float DistanceFromSegment(float kx, float ky, float x1, float y1, float x2, float y2)
		{
			Vector2 k = new Vector2(kx, ky);
			Vector2 p1 = new Vector2(x1, y1);
			Vector2 p2 = new Vector2(x2, y2);

			if (x1 == x2)
			{
				return Vector2.Distance(k, new Vector2(x1, ky));
			}
			if (y1 == y2)
			{
				return Vector2.Distance(k, new Vector2(kx, y1));
			}

			float m1 = (y2 - y1) / (x2 - x1);
			float m2 = 0 - (1 / m1);

			Vector2 pn = new Vector2(1f, m2);
			float de1 = DistanceFromLine(k, p1, pn + p1);
			float de2 = DistanceFromLine(k, p2, pn + p2);

			if (de1 + de2 > Vector2.Distance(p1, p2) + 0.002f) //outside segment bounds, distance should be from k to one of the end-points
			{
				return Mathf.Min(Vector2.Distance(p1, k), Vector2.Distance(p2, k));
			}
			return DistanceFromLine(k, p1, p2);
		}

		public static bool PointInTriangleStrong(Vector2 p, Vector2 a, Vector2 b, Vector2 c, int strength)
		{
			strength = Mathf.Clamp(strength, 1, 6);

			int total = 0;
			if (PointInTriangle(p, a, b, c)) total++;
			if (PointInTriangle(p, a, c, b)) total++;
			if (PointInTriangle(p, b, a, c)) total++;
			if (PointInTriangle(p, b, c, a)) total++;
			if (PointInTriangle(p, c, a, b)) total++;
			if (PointInTriangle(p, c, b, a)) total++;

			return total >= strength;
		}

		public static bool PointInTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
		{
			float s1 = c.y - a.y;
			float s2 = c.x - a.x;
			float s3 = b.y - a.y;
			float s4 = p.y - a.y;

			float w1 = (a.x * s1 + s4 * s2 - p.x * s1) / (s3 * s2 - (b.x - a.x) * s1);
			float w2 = (s4 - w1 * s3) / s1;
			return w1 >= 0f && w2 >= 0f && (w1 + w2) <= 1f;
		}
		
		public static bool InRange(Vector2 f, float min, float max)
		{
			return InRange(f, Vector2.one * min, Vector2.one * max);
		}
		public static bool InRange(Vector2 f, Vector2 min, float max)
		{
			return InRange(f, min, Vector2.one * max);
		}
		public static bool InRange(Vector2 f, float min, Vector2 max)
		{
			return InRange(f, Vector3.one * min, max);
		}
		public static bool InRange(Vector2 f, Vector2 min, Vector2 max)
		{
			return f.x >= min.x && f.x <= max.x && f.y >= min.y && f.y <= max.y;
		}
	}

	public static class V3
	{
		public static Vector3 FlattenPlane(Vector3 v, int axis) // <!> axis (only planes perpindicular to a single axis are supported/available)
		{
			return new Vector3(axis != 0 ? v.x : 0f, axis != 1 ? v.y : 0f, axis != 2 ? v.z : 0f);
		}

		public static float LateralDistance(Vector3 p1, Vector3 p2, Vector3 axis) //distance in 3 dimensions disregarding the specified axis
		{
			Vector3 mult = Vector3.one - axis.normalized();
			return Vector3.Distance(Vector3.Cross(p1, mult), Vector3.Cross(p2, mult));
		}
		
		public static float DistanceFromLine(Vector3 k, Vector3 v1, Vector3 v2)
		{
			return Vector3.Cross(k - v1, k - v2).magnitude / (v2 - v1).magnitude;
		}
		
		public static Vector3 MagnitudeMax(params Vector3[] elements) //returns the vector with the largest magnitude
		{
			float currentMag = Vector3.negativeInfinity.sqrMagnitude;
			int index = -1;
			
			for (int i = 0; i < elements.Length; i++)
			{
				if (currentMag < elements[i].sqrMagnitude)
				{
					currentMag = elements[i].sqrMagnitude;
					index = i;
				}
			}
			
			return elements[index];
		}
		
		public static Vector3 ElementMax(params Vector3[] elements) //returns a vector comprised of the largest of each axis from an array of vectors
		/*
			v(1,2,3) & v(2,2,2) returns v(2,2,3)
			v(1,2,3) & v(3,2,1) returns v(3,2,3)
			<?> see MagnitudeMax
		*/
		{
			Vector3 current = Vector3.negativeInfinity;
			int index = -1;
			
			for (int i = 0; i < elements.Length; i++)
			{
				current.x = Mathf.Max(current.x, elements[i].x);
				current.y = Mathf.Max(current.y, elements[i].y);
				current.z = Mathf.Max(current.z, elements[i].z);
			}
			
			return current;
			
		}
		
		public static Vector3 Clamp01(Vector3 f) //clamps each axis of a given vector to a value between 0 and 1
		{
			return new Vector3(Mathf.Clamp01(f.x), Mathf.Clamp01(f.y), Mathf.Clamp01(f.z));
		}
		
		public static Vector3 Clamp(Vector3 f, float min, float max) //clamps each axis of a given vector to a value between min and max
		{
			Vector3 range = max - min; Vector3 substitute = (f - min);
			Vector3 clamped Clamp01(new Vector3(substitute.x / range.x, substitute.y / range.y, substitute.z / range.z));
			clamped = new Vector3(clamped.x * range.x, clamped.y * range.y, clamped.z * range.z);
			return clamped + min;
		}
		
		public static bool InRange(Vector3 f, float min, float max)
		{
			return InRange(f, Vector3.one * min, Vector3.one * max);
		}
		public static bool InRange(Vector3 f, Vector3 min, float max)
		{
			return InRange(f, min, Vector3.one * max);
		}
		public static bool InRange(Vector3 f, float min, Vector3 max)
		{
			return InRange(f, Vector3.one * min, max);
		}
		public static bool InRange(Vector3 f, Vector3 min, Vector3 max)
		{
			return f.x >= min.x && f.x <= max.x && f.y >= min.y && f.y <= max.y && f.z >= min.z && f.z <= max.z;
		}
	}

	public static class V4
	{
		public static bool InRange(Vector4 f, float min, float max)
		{
			return InRange(f, Vector4.one * min, Vector4.one * max);
		}
		public static bool InRange(Vector4 f, Vector4 min, float max)
		{
			return InRange(f, min, Vector4.one * max);
		}
		public static bool InRange(Vector4 f, float min, Vector4 max)
		{
			return InRange(f, Vector4.one * min, max);
		}
		public static bool InRange(Vector4 f, Vector4 min, Vector4 max)
		{
			bool walk = f.x >= min.x && f.x <= max.x && f.y >= min.y && f.y <= max.y;
			walk = walk && f.z >= min.z && f.z <= max.z && f.w >= min.w && f.w <= max.w;
			return walk;
		}
	}

	public class TriVal //experimental 3-way boolean
	{
		public int value
		{
			get { value = Mathf.Clamp(value, 1, 3); return value; }
			set { this.value = Mathf.Clamp(value, 1, 3); }
		}

		public TriVal(int value)
		{
			this.value = Mathf.Clamp(value, 1, 3);
		}
	}

	public static class Bez2 //v2 bezier
	{
		public static Vector2 EvaluateCubic(Vector2 p1, Vector2 p2, Vector2 p3, float t)
		{
			return Vector2.Lerp(Vector2.Lerp(p1, p2, t), Vector2.Lerp(p2, p3, t), t);
		}

		public static Vector2 EvaluateQuadratic(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, float t)
		{
			Vector2 p5 = Vector2.Lerp(p1, p2, t);
			Vector2 p6 = Vector2.Lerp(p2, p3, t);
			Vector2 p7 = Vector2.Lerp(p3, p4, t);

			Vector2 p8 = Vector2.Lerp(p5, p6, t);
			Vector2 p9 = Vector2.Lerp(p6, p7, t);

			return Vector2.Lerp(p8, p9, t);
		}

		public static Vector2 EvaluateFromArray(Vector2[] points, float t)
		{
			if (points.Length == 0) { return Vector2.zero; }
			if (points.Length == 1) { return points[0]; }

			Vector2[] nest = points;

			for (int i = 0; i < points.Length; i++)
			{
				if (nest.Length - 2 == 0)
				{
					return Vector2.Lerp(nest[0], nest[1], t);
				}

				List<Vector2> l = new List<Vector2>();

				for (int j = 0; j < nest.Length - 1; j++)
				{
					l.Add(Vector2.Lerp(nest[j], nest[j + 1], t));
				}

				nest = l.ToArray();
			}

			return nest[0];
		}
	}

	public static class Bez3 //v3 bezier
	{
		public static Vector3 EvaluateCubic(Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			return Vector3.Lerp(Vector3.Lerp(p1, p2, t), Vector3.Lerp(p2, p3, t), t);
		}

		public static Vector3 EvaluateQuadratic(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
		{
			Vector3 p5 = Vector3.Lerp(p1, p2, t);
			Vector3 p6 = Vector3.Lerp(p2, p3, t);
			Vector3 p7 = Vector3.Lerp(p3, p4, t);

			Vector3 p8 = Vector3.Lerp(p5, p6, t);
			Vector3 p9 = Vector3.Lerp(p6, p7, t);

			return Vector3.Lerp(p8, p9, t);
		}

		public static Vector3 EvaluateFromArray(Vector3[] points, float t)
		{
			if (points.Length == 0) { return Vector3.zero; }
			if (points.Length == 1) { return points[0]; }

			Vector3[] nest = points;

			for (int i = 0; i < points.Length; i++)
			{
				if (nest.Length == 2)
				{
					return Vector3.Lerp(nest[0], nest[1], t);
				}

				List<Vector3> l = new List<Vector3>();

				for (int j = 0; j < nest.Length - 1; j++)
				{
					l.Add(Vector3.Lerp(nest[j], nest[j + 1], t));
				}

				nest = l.ToArray();
			}

			return nest[0];
		}
	}
}
