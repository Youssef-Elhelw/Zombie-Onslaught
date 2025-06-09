using UnityEngine;

public class GridSlot : MonoBehaviour
{
    public float gridSize = 1.0f;

    public Vector3 GetNearestGridPosition(Vector3 rawPosition)
    {
        float x = Mathf.Round(rawPosition.x / gridSize) * gridSize;
        float z = Mathf.Round(rawPosition.z / gridSize) * gridSize; // Snap on Z instead of Y
        return new Vector3(x, 0, z); // Keep Y at 0 to stay on the ground
    }
}
