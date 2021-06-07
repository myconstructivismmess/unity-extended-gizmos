using UnityEngine;

public class PositionRotationScaleRelativeGizmosTest : MonoBehaviour
{
    private WorldSpace worldSpace;

    void Start()
    {
        worldSpace = new WorldSpace();
        TransformToWorldSpaceParameters transformRules = new TransformToWorldSpaceParameters();
        transformRules.scaleRelative = true;
        worldSpace.Link(transform, transformRules);
    }

	void OnDrawGizmos()
	{
		if (worldSpace != null)
		{
			Gizmos.color = Color.magenta;

			// Wire Cube
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-5, 5, 5), new Vector3(10, 10, 10), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-25, 10, 5), new Vector3(10, 20, 10), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-10, 5, 25), new Vector3(20, 10, 10), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-25, 5, 40), new Vector3(10, 10, 20), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-5, 5, 50), new Vector3(10, 10, 20), Quaternion.Euler(10, 10, 10), Color.blue);
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-5, 5, 80), new Vector3(10, 10, 20), new Vector3(10, 10, 10), Color.blue);

			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-105, 5, 5), new Vector3(10, 10, 10));
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-125, 10, 5), new Vector3(10, 20, 10));
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-110, 5, 25), new Vector3(20, 10, 10));
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-125, 5, 40), new Vector3(10, 10, 20));
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-105, 5, 50), new Vector3(10, 10, 20), Quaternion.Euler(10, 10, 10));
			ExtendedGizmos.DrawWireCube(worldSpace, new Vector3(-105, 5, 80), new Vector3(10, 10, 20), new Vector3(10, 10, 10));

			// Oval
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-5, 0, 110), new Vector2(10, 20), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-10, 0, 135), new Vector2(20, 10), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-25, 0, 110), new Vector2(10, 20), Quaternion.Euler(10, 0, 10), Color.blue);
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-25, 0, 130), new Vector2(10, 20), new Vector3(10, 0, 10), Color.blue);

			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-105, 0, 110), new Vector2(10, 20));
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-110, 0, 135), new Vector2(20, 10));
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-125, 0, 110), new Vector2(10, 20), Quaternion.Euler(10, 0, 10));
			ExtendedGizmos.DrawOval(worldSpace, new Vector3(-125, 0, 130), new Vector2(10, 20), new Vector3(10, 0, 10));

			// Circle
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-10, 0, 190), 10, Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-20, 0, 180), 20, Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-20, 0, 180), 15, Quaternion.Euler(10, 0, 10), Color.blue);
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-20, 0, 180), 13, new Vector3(20, 0, 20), Color.blue);

			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-110, 0, 190), 10);
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-120, 0, 180), 20);
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-120, 0, 180), 15, Quaternion.Euler(10, 0, 10));
			ExtendedGizmos.DrawCircle(worldSpace, new Vector3(-120, 0, 180), 13, new Vector3(20, 0, 20));

			// WireEllipse
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-10, 5, 205), new Vector3(20, 10, 10), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-5, 10, 215), new Vector3(10, 20, 10), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-15, 5, 220), new Vector3(10, 10, 20), Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-10, 5, 245), new Vector3(20, 10, 10), Quaternion.Euler(20, 0, 20), Color.blue);
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-10, 5, 265), new Vector3(20, 10, 10), new Vector3(20, 0, 20), Color.blue);

			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-110, 5, 205), new Vector3(20, 10, 10));
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-105, 10, 215), new Vector3(10, 20, 10));
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-115, 5, 220), new Vector3(10, 10, 20));
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-110, 5, 245), new Vector3(20, 10, 10), Quaternion.Euler(20, 0, 20));
			ExtendedGizmos.DrawWireEllipse(worldSpace, new Vector3(-110, 5, 265), new Vector3(20, 10, 10), new Vector3(20, 0, 20));

			// WireSphere
			ExtendedGizmos.DrawWireSphere(worldSpace, new Vector3(-10, 10, 310), 10, Quaternion.identity, Color.blue);
			ExtendedGizmos.DrawWireSphere(worldSpace, new Vector3(-5, 5, 325), 5, Quaternion.Euler(45, 45, 45), Color.blue);
			ExtendedGizmos.DrawWireSphere(worldSpace, new Vector3(-5, 5, 335), 5, new Vector3(45, 45, 45), Color.blue);

			ExtendedGizmos.DrawWireSphere(worldSpace, new Vector3(-110, 10, 310), 10, Quaternion.identity);
			ExtendedGizmos.DrawWireSphere(worldSpace, new Vector3(-105, 5, 325), 5, Quaternion.Euler(45, 45, 45));
			ExtendedGizmos.DrawWireSphere(worldSpace, new Vector3(-105, 5, 335), 5, new Vector3(45, 45, 45));

			// DrawLine
			ExtendedGizmos.DrawLine(worldSpace, new Vector3(-10, 0, 390), new Vector3(-10, 10, 390), Color.blue);
			ExtendedGizmos.DrawLine(worldSpace, new Vector3(-10, 0, 380), new Vector3(-10, 20, 380), Color.blue);

			ExtendedGizmos.DrawLine(worldSpace, new Vector3(-110, 0, 390), new Vector3(-110, 10, 390));
			ExtendedGizmos.DrawLine(worldSpace, new Vector3(-110, 0, 380), new Vector3(-110, 20, 380));

			// DrawRay
			ExtendedGizmos.DrawRay(worldSpace, new Ray(new Vector3(-10, 0, 430), Vector3.up), 30, Color.blue);
			ExtendedGizmos.DrawRay(worldSpace, new Vector3(-10, 0, 420), new Vector3(0, 20, 0), Color.blue);
			ExtendedGizmos.DrawRay(worldSpace, new Vector3(-10, 0, 410), new Vector3(0, 10, 0), Color.blue);

			ExtendedGizmos.DrawRay(worldSpace, new Ray(new Vector3(-110, 0, 430), Vector3.up), 30);
			ExtendedGizmos.DrawRay(worldSpace, new Vector3(-110, 0, 420), new Vector3(0, 20, 0));
			ExtendedGizmos.DrawRay(worldSpace, new Vector3(-110, 0, 410), new Vector3(0, 10, 0));

			// DrawArrow
			ExtendedGizmos.DrawArrow(worldSpace, new Vector3(-10, 0, 490), new Vector3(-10, 10, 490), Color.blue);
			ExtendedGizmos.DrawArrow(worldSpace, new Vector3(-10, 0, 490), new Vector3(-10, 0, 460), Color.blue);
			ExtendedGizmos.DrawArrow(worldSpace, new Vector3(-10, 0, 490), new Vector3(-50, 0, 490), Color.blue);
			ExtendedGizmos.DrawArrow(worldSpace, new Ray(new Vector3(-10, 0, 490), Vector3.up + Vector3.left), 40, Color.blue);
			ExtendedGizmos.DrawArrow(worldSpace, new Ray(new Vector3(-10, 0, 490), Vector3.up + Vector3.back), 40, Color.blue);

			ExtendedGizmos.DrawArrow(worldSpace, new Vector3(-110, 0, 490), new Vector3(-110, 10, 490));
			ExtendedGizmos.DrawArrow(worldSpace, new Vector3(-110, 0, 490), new Vector3(-110, 0, 460));
			ExtendedGizmos.DrawArrow(worldSpace, new Vector3(-110, 0, 490), new Vector3(-150, 0, 490));
			ExtendedGizmos.DrawArrow(worldSpace, new Ray(new Vector3(-110, 0, 490), Vector3.up + Vector3.left), 40);
			ExtendedGizmos.DrawArrow(worldSpace, new Ray(new Vector3(-110, 0, 490), Vector3.up + Vector3.back), 40);
		}
	}
}
