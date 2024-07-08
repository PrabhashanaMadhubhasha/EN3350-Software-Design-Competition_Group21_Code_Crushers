/*using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public static NavMeshUpdater Instance { get; set; }

    public float radius = 6000f;
    public float height = 1000f;

    //public NavMeshUpdater navMeshUpdater;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateNavMeshAtSelectedPoint(Vector3 areaCenter)
    {
        StartCoroutine(UpdateNavMesh(areaCenter));
    }

    public IEnumerator UpdateNavMesh(Vector3 center)
    {
        yield return new WaitForSeconds(3f);
        NavMeshModifierVolume volume = gameObject.AddComponent<NavMeshModifierVolume>();
        volume.center = center;
        volume.size = new Vector3(radius * 2, height, radius * 2);
        volume.area = 1; // Specify the area type you want to modify

        // Remove the modified area after a certain time or when no longer needed
        Destroy(volume, 5f);
    }
}*/

using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshUpdater : MonoBehaviour
{
    public static NavMeshUpdater Instance { get; set; }
    public NavMeshSurface navMeshSurface;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }
}
