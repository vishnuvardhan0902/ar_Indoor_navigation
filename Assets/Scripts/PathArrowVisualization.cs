using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PathArrowVisualisation : MonoBehaviour
{
    [SerializeField]
    private NavigationController navigationController; // Make sure this class exists
    [SerializeField]
    private GameObject arrow;
    [SerializeField]
    private Slider navigationYOffset;
    [SerializeField]
    private float moveOnDistance;

    private NavMeshPath path;
    private float currentDistance;
    private Vector3[] pathOffset;
    private Vector3 nextNavigationPoint = Vector3.zero;

    private void Update()
    {
        if (navigationController == null || arrow == null || navigationYOffset == null)
        {
            Debug.LogError("Missing references.");
            return;
        }

        path = navigationController.CalculatedPath;

        if (path == null || path.corners.Length == 0)
        {
            Debug.LogWarning("Path is not calculated or has no corners.");
            return;
        }

        AddOffsetToPath();
        SelectNextNavigationPoint();
        AddArrowOffset();

        if (nextNavigationPoint != Vector3.zero)
        {
            arrow.transform.LookAt(nextNavigationPoint);
        }
    }

    private void AddOffsetToPath()
    {
        pathOffset = new Vector3[path.corners.Length];
        for (int i = 0; i < path.corners.Length; i++)
        {
            pathOffset[i] = new Vector3(path.corners[i].x, transform.position.y, path.corners[i].z);
        }
    }

    private void SelectNextNavigationPoint()
    {
        nextNavigationPoint = SelectNextNavigationPointWithinDistance();
    }

    private Vector3 SelectNextNavigationPointWithinDistance()
    {
        for (int i = 0; i < pathOffset.Length; i++)
        {
            currentDistance = Vector3.Distance(transform.position, pathOffset[i]);
            if (currentDistance > moveOnDistance)
            {
                return pathOffset[i];
            }
        }
        return navigationController.TargetPosition;
    }

    private void AddArrowOffset()
    {
        if (navigationYOffset.value != 0)
        {
            arrow.transform.position = new Vector3(arrow.transform.position.x, navigationYOffset.value, arrow.transform.position.z);
        }
    }
}
