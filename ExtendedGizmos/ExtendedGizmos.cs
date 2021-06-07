using UnityEngine;

public struct TransformToWorldSpaceParameters
{
	private bool? _positionRelative, _rotationRelative, _scaleRelative;

	public bool positionRelative {
		get {
			return _positionRelative ?? true;
		}

		set
		{
			_positionRelative = value;
		}
	}

	public bool rotationRelative
	{
		get
		{
			return _rotationRelative ?? true;
		}

		set
		{
			_rotationRelative = value;
		}
	}

	public bool scaleRelative
	{
		get
		{
			return _scaleRelative ?? false;
		}

		set
		{
			_scaleRelative = value;
		}
	}
}

public class WorldSpace
{
	private Transform _transform;
	private TransformToWorldSpaceParameters transformToWorldSpaceParameters;
	// OR
	private WorldSpace _parent;

	private Vector3 position = Vector3.zero;
	private Quaternion rotation = Quaternion.identity;
	private Vector3 scale = Vector3.one;

	#region Constructors

	public WorldSpace()
	{

	}

	#region With Parent

	public WorldSpace(WorldSpace parent, Vector3 center)
	{
		_parent = parent;
		this.position = center;
	}

	public WorldSpace(WorldSpace parent, Vector3 center, Quaternion rotation)
	{
		_parent = parent;
		this.position = center;
		this.rotation = rotation;
	}

	public WorldSpace(WorldSpace parent, Vector3 center, Vector3 rotation)
	{
		_parent = parent;
		this.position = center;
		this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
	}

	public WorldSpace(WorldSpace parent, Vector3 center, Quaternion rotation, Vector3 scale)
	{
		_parent = parent;
		this.position = center;
		this.rotation = rotation;
		this.scale = scale;
	}

	public WorldSpace(WorldSpace parent, Vector3 center, Vector3 rotation, Vector3 scale)
	{
		_parent = parent;
		this.position = center;
		this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
		this.scale = scale;
	}

	#endregion

	#region Without Parent

	public WorldSpace(Vector3 center)
	{
		this.position = center;
	}

	public WorldSpace(Vector3 center, Quaternion rotation)
	{
		this.position = center;
		this.rotation = rotation;
	}

	public WorldSpace(Vector3 center, Vector3 rotation)
	{
		this.position = center;
		this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
	}

	public WorldSpace(Vector3 center, Quaternion rotation, Vector3 scale)
	{
		this.position = center;
		this.rotation = rotation;
		this.scale = scale;
	}

	public WorldSpace(Vector3 center, Vector3 rotation, Vector3 scale)
	{
		this.position = center;
		this.rotation = Quaternion.Euler(rotation.x, rotation.y, rotation.z);
		this.scale = scale;
	}

	#endregion

	#endregion

	#region Methods

	public void Link(Transform transform, TransformToWorldSpaceParameters transformToWorldSpaceParameters = default)
	{
		this._transform = transform;
		this.transformToWorldSpaceParameters = transformToWorldSpaceParameters;
	}

	public Vector3 GetCoordinates(Vector3 position)
	{
		if (this._transform != null)
		{
			return (this.transformToWorldSpaceParameters.positionRelative ? this._transform.position : this.position) + (this.transformToWorldSpaceParameters.rotationRelative ? this._transform.rotation : this.rotation) * (this.transformToWorldSpaceParameters.scaleRelative ? new Vector3(this._transform.localScale.x * position.x, this._transform.localScale.y * position.y, this._transform.localScale.z * position.z) : new Vector3(this.scale.x * position.x, this.scale.y * position.y, this.scale.z * position.z));
		}
		else if (this._parent != null)
		{
			return this._parent.GetCoordinates(this.position + this.rotation * new Vector3(position.x * this.scale.x, position.y * this.scale.y, position.z * this.scale.z));
		}
		else
		{
			return this.position + this.rotation * new Vector3(position.x * this.scale.x, position.y * this.scale.y, position.z * this.scale.z);
		}
	}

	#endregion
}

public class ExtendedGizmos
{
	#region Core

	private static void GetCubePoints(Vector3 center, Vector3 size, Quaternion rotation, out Vector3[] points)
	{
		points = new Vector3[8];

		for (int y = -1; y <= 1; y += 2)
		{
			for (int x = -1; x <= 1; x += 2)
			{
				for (int z = -1; z <= 1; z += 2)
				{
					int i = (x + 1) + (y + 1) * 2 + (z + 1) / 2;

					points[i] = center + rotation * new Vector3(x * (size.x / 2), y * (size.y / 2), z * (size.z / 2));
				}
			}
		}
	}

	private static void GetCubePoints(WorldSpace worldSpace, Vector3 center, Vector3 size, Quaternion rotation, out Vector3[] points)
	{
		GetCubePoints(center, size, rotation, out points);

		for (int i = 0; i < points.Length; i++)
		{
			points[i] = worldSpace.GetCoordinates(points[i]);
		}
	}

	private static void GetOvalPoints(Vector3 center, Vector2 size, Quaternion rotation, int lineNumber, out Vector3[] points)
	{
		points = new Vector3[lineNumber];

		for (int lineI = 0; lineI < lineNumber; lineI++)
		{
			float angle = (Mathf.PI * 2) / lineNumber * lineI;
			points[lineI] = center + rotation * new Vector3(Mathf.Cos(angle) * size.x / 2, 0, Mathf.Sin(angle) * size.y / 2);
		}
	}

	private static void GetOvalPoints(WorldSpace worldSpace, Vector3 center, Vector2 size, Quaternion rotation, int lineNumber, out Vector3[] points)
	{
		GetOvalPoints(center, size, rotation, lineNumber, out points);

		for (int i = 0; i < points.Length; i++)
		{
			points[i] = worldSpace.GetCoordinates(points[i]);
		}
	}

	#endregion

	#region 2D

	#region DrawOval
	public static void DrawOval(Vector3 center, Vector2 size, Quaternion rotation = default, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;
		Vector3[] points;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		int numberOfLines = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.x * size.x + size.y * size.y) / 2) / 0.25f);
		GetOvalPoints(center, size, rotation, numberOfLines, out points);

		for (int i = 0; i < points.Length; i++)
		{
			Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]); 
		}
		
		Gizmos.color = currentGizmosColor;
	}

	public static void DrawOval(WorldSpace worldSpace, Vector3 center, Vector2 size, Quaternion rotation = default, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;
		Vector3[] points;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		int numberOfLines = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.x * size.x + size.y * size.y) / 2) / 0.25f);
		GetOvalPoints(worldSpace, center, size, rotation, numberOfLines, out points);

		for (int i = 0; i < points.Length; i++)
		{
			Gizmos.DrawLine(points[i], points[(i + 1) % points.Length]);
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawOval(Vector3 center, Vector2 size, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawOval(center, size, internRotation, color);
	}

	public static void DrawOval(WorldSpace worldSpace, Vector3 center, Vector2 size, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawOval(worldSpace, center, size, internRotation, color);
	}

	#endregion

	#region DrawCircle

	public static void DrawCircle(Vector3 center, float radius, Quaternion rotation = default, Color color = default)
	{
		DrawOval(center, new Vector2(radius * 2, radius * 2), rotation, color);
	}

	public static void DrawCircle(WorldSpace worldSpace, Vector3 center, float radius, Quaternion rotation = default, Color color = default)
	{
		DrawOval(worldSpace, center, new Vector2(radius * 2, radius * 2), rotation, color);
	}

	public static void DrawCircle(Vector3 center, float radius, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawOval(center, new Vector2(radius * 2, radius * 2), internRotation, color);
	}

	public static void DrawCircle(WorldSpace worldSpace, Vector3 center, float radius, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawOval(worldSpace, center, new Vector2(radius * 2, radius * 2), internRotation, color);
	}

	#endregion

	#endregion

	#region 3D

	#region DrawWireCube

	public static void DrawWireCube(Vector3 center, Vector3 size, Quaternion rotation = default, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;
		Vector3[] points; // Size: 8

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}
		
		GetCubePoints(center, size, rotation, out points);

		for (int i = 0; i < 4; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 4]);
		}

		for (int y = 0; y <= 1; y++)
		{
			for (int x = 0; x <= 2; x += 2)
			{
				Gizmos.DrawLine(points[x + y * 4], points[x + 1 + y * 4]);
			}

			for (int z = 0; z <= 1; z++)
			{
				Gizmos.DrawLine(points[z + y * 4], points[z + 2 + y * 4]);
			}
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawWireCube(WorldSpace worldSpace, Vector3 center, Vector3 size, Quaternion rotation = default, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;
		Vector3[] points; // Size: 8

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		GetCubePoints(worldSpace, center, size, rotation, out points);

		for (int i = 0; i < 4; i++)
		{
			Gizmos.DrawLine(points[i], points[i + 4]);
		}

		for (int y = 0; y <= 1; y++)
		{
			for (int x = 0; x <= 2; x += 2)
			{
				Gizmos.DrawLine(points[x + y * 4], points[x + 1 + y * 4]);
			}

			for (int z = 0; z <= 1; z++)
			{
				Gizmos.DrawLine(points[z + y * 4], points[z + 2 + y * 4]);
			}
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawWireCube(Vector3 center, Vector3 size, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawWireCube(center, size, internRotation, color);
	}

	public static void DrawWireCube(WorldSpace worldSpace, Vector3 center, Vector3 size, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawWireCube(worldSpace, center, size, internRotation, color);
	}

	#endregion

	#region DrawWireSphere

	public static void DrawWireSphere(Vector3 center, float radius, Quaternion rotation = default, Color color = default)
	{
		DrawWireEllipse(center, Vector3.one * radius * 2, rotation, color);
	}

	public static void DrawWireSphere(WorldSpace worldSpace, Vector3 center, float radius, Quaternion rotation = default, Color color = default)
	{
		DrawWireEllipse(worldSpace, center, Vector3.one * radius * 2, rotation, color);
	}

	public static void DrawWireSphere(Vector3 center, float radius, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawWireEllipse(center, Vector3.one * radius * 2, internRotation, color);
	}

	public static void DrawWireSphere(WorldSpace worldSpace, Vector3 center, float radius, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawWireEllipse(worldSpace, center, Vector3.one * radius * 2, internRotation, color);
	}

	#endregion

	#region DrawWireEllipse

	public static void DrawWireEllipse(Vector3 center, Vector3 size, Quaternion rotation = default, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;
		Vector3[] pointsX, pointsY, pointsZ;
		int numberOfLinesX = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.y * size.y + size.z * size.z) / 2) / 0.25f), numberOfLinesY = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.x * size.x + size.z * size.z) / 2) / 0.25f), numberOfLinesZ = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.x * size.x + size.y * size.y) / 2) / 0.25f);

		GetOvalPoints(Vector3.zero, new Vector2(size.y, size.z), Quaternion.Euler(0, 0, 90), numberOfLinesX, out pointsX);
		GetOvalPoints(Vector3.zero, new Vector2(size.x, size.z), Quaternion.identity, numberOfLinesY, out pointsY);
		GetOvalPoints(Vector3.zero, new Vector2(size.x, size.y), Quaternion.Euler(90, 0, 0), numberOfLinesZ, out pointsZ);

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		for (int i = 0; i < pointsX.Length; i++)
		{
			Gizmos.DrawLine(center + rotation * pointsX[i], center + rotation * pointsX[(i + 1) % pointsX.Length]);
		}

		for (int i = 0; i < pointsY.Length; i++)
		{
			Gizmos.DrawLine(center + rotation * pointsY[i], center + rotation * pointsY[(i + 1) % pointsY.Length]);
		}

		for (int i = 0; i < pointsZ.Length; i++)
		{
			Gizmos.DrawLine(center + rotation * pointsZ[i], center + rotation * pointsZ[(i + 1) % pointsZ.Length]);
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawWireEllipse(WorldSpace worldSpace, Vector3 center, Vector3 size, Quaternion rotation = default, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;
		Vector3[] pointsX, pointsY, pointsZ;
		int numberOfLinesX = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.y * size.y + size.z * size.z) / 2) / 0.25f), numberOfLinesY = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.x * size.x + size.z * size.z) / 2) / 0.25f), numberOfLinesZ = Mathf.CeilToInt(2 * Mathf.PI * Mathf.Sqrt((size.x * size.x + size.y * size.y) / 2) / 0.25f);

		GetOvalPoints(Vector3.zero, new Vector2(size.y, size.z), Quaternion.Euler(0, 0, 90), numberOfLinesX, out pointsX);
		GetOvalPoints(Vector3.zero, new Vector2(size.x, size.z), Quaternion.identity, numberOfLinesY, out pointsY);
		GetOvalPoints(Vector3.zero, new Vector2(size.x, size.y), Quaternion.Euler(90, 0, 0), numberOfLinesZ, out pointsZ);

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		for (int i = 0; i < pointsX.Length; i++)
		{
			Gizmos.DrawLine(worldSpace.GetCoordinates(center + rotation * pointsX[i]), worldSpace.GetCoordinates(center + rotation * pointsX[(i + 1) % pointsX.Length]));
		}

		for (int i = 0; i < pointsY.Length; i++)
		{
			Gizmos.DrawLine(worldSpace.GetCoordinates(center + rotation * pointsY[i]), worldSpace.GetCoordinates(center + rotation * pointsY[(i + 1) % pointsY.Length]));
		}

		for (int i = 0; i < pointsZ.Length; i++)
		{
			Gizmos.DrawLine(worldSpace.GetCoordinates(center + rotation * pointsZ[i]), worldSpace.GetCoordinates(center + rotation * pointsZ[(i + 1) % pointsZ.Length]));
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawWireEllipse(Vector3 center, Vector3 size, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawWireEllipse(center, size, internRotation, color);
	}

	public static void DrawWireEllipse(WorldSpace worldSpace, Vector3 center, Vector3 size, Vector3 rotation, Color color = default)
	{
		Quaternion internRotation = Quaternion.Euler(rotation);

		DrawWireEllipse(worldSpace, center, size, internRotation, color);
	}

	#endregion

	#endregion

	#region Misc

	#region DrawLine
	public static void DrawLine(Vector3 from, Vector3 to, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		Gizmos.DrawLine(from, to);

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawLine(WorldSpace worldSpace, Vector3 from, Vector3 to, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		Gizmos.DrawLine(worldSpace.GetCoordinates(from), worldSpace.GetCoordinates(to));

		Gizmos.color = currentGizmosColor;
	}

	#endregion

	#region DrawRay
	public static void DrawRay(Vector3 from, Vector3 direction, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		Gizmos.DrawLine(from, from + direction);

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawRay(WorldSpace worldSpace, Vector3 from, Vector3 direction, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		Gizmos.DrawLine(worldSpace.GetCoordinates(from), worldSpace.GetCoordinates(from + direction));

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawRay(Ray r, float length, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		Gizmos.DrawLine(r.origin, r.origin + r.direction * length);

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawRay(WorldSpace worldSpace, Ray r, float length, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		Gizmos.DrawLine(worldSpace.GetCoordinates(r.origin), worldSpace.GetCoordinates(r.origin + r.direction * length));

		Gizmos.color = currentGizmosColor;
	}

	#endregion

	#region DrawArrow

	public static void DrawArrow(Vector3 from, Vector3 to, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		float lengthHead = Mathf.Min(Vector3.Distance(from, to) / 5, 5);
		Quaternion arrowAngle = Quaternion.LookRotation(to - from, Vector3.up);

		Gizmos.DrawLine(from, to);

		for (int i = 0; i < 150; i++)
		{
			float angle = (Mathf.PI * 2) / 150 * i;

			Gizmos.DrawLine(to, to + arrowAngle * new Vector3(Mathf.Cos(angle) * lengthHead / 2, Mathf.Sin(angle) * lengthHead / 2, -lengthHead));
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawArrow(WorldSpace worldSpace, Vector3 from, Vector3 to, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		float lengthHead = Mathf.Min(Vector3.Distance(from, to) / 5, 5);
		Quaternion arrowAngle = Quaternion.LookRotation(to - from, Vector3.up);

		Gizmos.DrawLine(worldSpace.GetCoordinates(from), worldSpace.GetCoordinates(to));

		for (int i = 0; i < 150; i++)
		{
			float angle = (Mathf.PI * 2) / 150 * i;

			Gizmos.DrawLine(worldSpace.GetCoordinates(to), worldSpace.GetCoordinates(to + arrowAngle * new Vector3(Mathf.Cos(angle) * lengthHead / 2, Mathf.Sin(angle) * lengthHead / 2, -lengthHead)));
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawArrow(Ray r, float length, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		float lengthHead = Mathf.Min(length / 5, 5);
		Quaternion arrowAngle = Quaternion.LookRotation(r.direction, Vector3.up);

		Gizmos.DrawLine(r.origin, r.origin + r.direction * length);

		for (int i = 0; i < 150; i++)
		{
			float angle = (Mathf.PI * 2) / 150 * i;

			Gizmos.DrawLine(r.origin + r.direction * length, r.origin + r.direction * length + arrowAngle * new Vector3(Mathf.Cos(angle) * lengthHead / 2, Mathf.Sin(angle) * lengthHead / 2, -lengthHead));
		}

		Gizmos.color = currentGizmosColor;
	}

	public static void DrawArrow(WorldSpace worldSpace, Ray r, float length, Color color = default)
	{
		Color currentGizmosColor = Gizmos.color;

		if (!Equals(color, Color.clear))
		{
			Gizmos.color = color;
		}

		float lengthHead = Mathf.Min(length / 5, 5);
		Quaternion arrowAngle = Quaternion.LookRotation(r.direction, Vector3.up);

		Gizmos.DrawLine(worldSpace.GetCoordinates(r.origin), worldSpace.GetCoordinates(r.origin + r.direction * length));

		for (int i = 0; i < 150; i++)
		{
			float angle = (Mathf.PI * 2) / 150 * i;

			Gizmos.DrawLine(worldSpace.GetCoordinates(r.origin + r.direction * length), worldSpace.GetCoordinates(r.origin + r.direction * length + arrowAngle * new Vector3(Mathf.Cos(angle) * lengthHead / 2, Mathf.Sin(angle) * lengthHead / 2, -lengthHead)));
		}

		Gizmos.color = currentGizmosColor;
	}

	#endregion

	#endregion
}
