using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Grid Attributes")]
    public int width;
    public int height;
    public float gridCellSize;

    [Header("Misc")]
    public Camera mainCamera;
    public Vector2 gridOffsets;

    [Header("Entity Spawning")]
    public int maxCountToSpawn = 15;
    public float spawnDelay = 0.05f;
    //public Material[] entityMaterials;

    private float lastSpawned;
    private MyGrid grid;
    private Vector3 gridLeftBottom;
    private ObjectPooler objectPooler;

    [SerializeField]
    private int objectCount;

    void Start()
    {
        DrawGrid();

        objectPooler = ObjectPooler.instance;

        objectCount = 0;

        // Register for object despawning event
        ObjectPooler.onObjectDespawned += DecreaseObjectCount;
    }

    private void OnDestroy()
    {
        ObjectPooler.onObjectDespawned -= DecreaseObjectCount;
    }

    private void Update()
    {
        DrawGrid();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //    //Debug.Log(grid.GetValue(worldPoint));
        //    SpawnObject();
        //}

        while (objectCount < maxCountToSpawn && Time.time - lastSpawned > spawnDelay)
        {
            SpawnObject();

            lastSpawned = Time.time;
        }
    }

    //public int GetMaterialCount()
    //{
    //    return entityMaterials.Length;
    //}

    // For visual debugging purposes
    private void DrawGrid()
    {
        gridLeftBottom = new Vector3(mainCamera.transform.position.x - gridOffsets.x, mainCamera.transform.position.y - gridOffsets.y, 0);

        grid = new MyGrid(width, height, gridCellSize, gridLeftBottom);
    }

    private void SpawnObject()
    {
        // Get random x and y to select a cell
        int gridX = Random.Range(0, width);
        int gridY = Random.Range(0, height);
        //Debug.Log(new Vector2((int)gridCellInfo.x, (int)gridCellInfo.y));

        if((gridX > 5 && gridX < 9) && (gridY > 2 && gridY < 5))
        {
            return;
        }

        // Check if any object can be spawned in that grid cell
        // TODO

        // Get the world coordinate of that selected cell
        Vector3 gridWorldPosition = grid.GetGridCellWorldPosition(gridX, gridY);
        //Debug.Log(gridWorldPosition);

        // From that world position, get a randomized position
        float xPos = Random.Range(gridWorldPosition.x, gridWorldPosition.x + gridCellSize);
        float yPos = Random.Range(gridWorldPosition.y, gridWorldPosition.y + gridCellSize);

        // Spawn the object in that particular position
        // Random shapes
        float probabilityRoll = Random.Range(0f, 1f);
        string entity;

        // Random colors
        //int randomIndex = Random.Range(0, entityMaterials.Length);

        if (probabilityRoll > 0.6f)
            entity = "Rectangle";
        else
            entity = "Circle";

        GameObject newBlock = objectPooler.SpawnFromPool(entity, new Vector3(xPos, yPos, 0), Quaternion.identity);
        //newBlock.GetComponent<Entity>().SetEntityMaterial(entityMaterials[randomIndex]);
        newBlock.GetComponent<Entity>().OnSpawn();

        objectCount++;

        newBlock.GetComponent<Entity>().SetGridCellInfo(new Vector2(gridX, gridY)); // For debug
    }

    private void DecreaseObjectCount()
    {
        objectCount--;
    }
}
