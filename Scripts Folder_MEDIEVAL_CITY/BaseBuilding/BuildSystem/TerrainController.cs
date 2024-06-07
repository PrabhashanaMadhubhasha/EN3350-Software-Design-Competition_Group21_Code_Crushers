using System;
using UnityEngine;

public class TerrainController : MonoBehaviour
{
    public static TerrainController Instance { get; set; }

    public Terrain terrain; // Reference to the terrain

    [Header("Input Construction related values")]
    public Vector2Int area = new Vector2Int(20, 20); // Size of the area around the midpoint
    public Vector3 midPoint;
    public float rotation = 60;
    public int divider = 800;

    private float[,] originalHeights; // Store original heights for resetting
    public TerrainData updatedTerrainData;

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

    // Initialize by storing original heights
    private void Start()
    {
        // Get the terrain's heightmap data
        TerrainData terrainData = terrain.terrainData;

        // Store original heights
        originalHeights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
    }

    // Set the height of the terrain at a specific area around a midpoint position

    public void SetTerrainHeight(Vector3 midPoint, Vector2 area, float rotation)
    {
        // Get the terrain's heightmap data
        TerrainData terrainData = terrain.terrainData;

        // Get the y coordinate of the terrain surface at the midpoint
        float baseHeight = terrain.SampleHeight(midPoint);

        // Convert the area to a list of corner points
        Vector2 halfArea = area / 2;
        Vector2[] corners = new Vector2[]
        {
        new Vector2(-halfArea.x, -halfArea.y),
        new Vector2(halfArea.x, -halfArea.y),
        new Vector2(halfArea.x, halfArea.y),
        new Vector2(-halfArea.x, halfArea.y)
        };

        // Rotate each corner around the midpoint
        for (int i = 0; i < corners.Length; i++)
        {
            corners[i] = RotateVector2(corners[i], rotation);
        }

        // Determine the bounds of the rotated area
        float minX = Mathf.Min(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float maxX = Mathf.Max(corners[0].x, corners[1].x, corners[2].x, corners[3].x);
        float minY = Mathf.Min(corners[0].y, corners[1].y, corners[2].y, corners[3].y);
        float maxY = Mathf.Max(corners[0].y, corners[1].y, corners[2].y, corners[3].y);

        // Convert the bounds to world space
        Vector3 worldStart = midPoint + new Vector3(minX, 0, minY);
        Vector3 worldEnd = midPoint + new Vector3(maxX, 0, maxY);

        // Convert the world positions to terrain local positions
        Vector3 terrainLocalStart = worldStart - terrain.transform.position;
        Vector3 terrainLocalEnd = worldEnd - terrain.transform.position;

        // Convert the terrain local positions to terrain sample positions
        int startX = Mathf.RoundToInt((terrainLocalStart.x / terrainData.size.x) * terrainData.heightmapResolution);
        int startZ = Mathf.RoundToInt((terrainLocalStart.z / terrainData.size.z) * terrainData.heightmapResolution);
        int endX = Mathf.RoundToInt((terrainLocalEnd.x / terrainData.size.x) * terrainData.heightmapResolution);
        int endZ = Mathf.RoundToInt((terrainLocalEnd.z / terrainData.size.z) * terrainData.heightmapResolution);

        // Get a temporary copy of the current heightmap data
        float[,] tempHeights = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);

        try
        {
            // Set the height for the specified area
            for (int z = Mathf.Max(0, startZ); z < Mathf.Min(terrainData.heightmapResolution, endZ); z++)
            {
                for (int x = Mathf.Max(0, startX); x < Mathf.Min(terrainData.heightmapResolution, endX); x++)
                {

                    tempHeights[z, x] = baseHeight / divider;

                }
            }

            // Apply the temporary heightmap data to the terrain
            terrainData.SetHeights(0, 0, tempHeights);
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to set the height and update: " + ex.Message);
        }

        updatedTerrainData = terrain.terrainData;
    }

    // Rotate a 2D vector by an angle in degrees
    private Vector2 RotateVector2(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);
        float x = vector.x * cos - vector.y * sin;
        float y = vector.x * sin + vector.y * cos;
        return new Vector2(x, y);
    }

    // Reset the terrain to its original heights
    public void ResetTerrain()
    {
        // Apply the original heightmap data to the terrain
        if (updatedTerrainData != null)
        {
            updatedTerrainData.SetHeights(0, 0, originalHeights);
        }
    }

    private void OnDestroy()
    {
        ResetTerrain();
    }
}