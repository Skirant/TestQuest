using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    public List<GameObject> prefabsToSpawn;
    public int gridSizeX = 5;
    public int gridSizeY = 5;
    public float gridSpacing = 1.0f;
    public int numberOfPrefabsToSpawn = 5;
    public GameObject fencePrefab; // Добавьте это поле для префаба забора

    private List<GameObject> spawnedFences = new List<GameObject>(); // Хранит ссылки на созданные заборы

    void Start()
    {
        /*if (!Application.isPlaying)
        {
            GenerateGrid();
        }*/
    }

    void OnDrawGizmos()
    {
        GenerateGridGizmos();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            int randomX = Random.Range(0, gridSizeX);
            int randomY = Random.Range(0, gridSizeY);

            int prefabIndex = Random.Range(0, prefabsToSpawn.Count);

            Vector3 spawnPosition = new Vector3(randomX * gridSpacing, prefabsToSpawn[prefabIndex].transform.position.y, randomY * gridSpacing);

            GameObject prefabToSpawn = prefabsToSpawn[prefabIndex];

            GameObject spawnedPrefab = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
            // Дополнительные настройки spawnedPrefab или его компонентов.
        }

        // Создайте забор вокруг границ сетки
        CreateFence();
    }

    void GenerateGridGizmos()
    {
        Gizmos.color = Color.blue;

        for (float x = 0; x < gridSizeX * gridSpacing; x += gridSpacing)
        {
            for (float y = 0; y < gridSizeY * gridSpacing; y += gridSpacing)
            {
                Gizmos.DrawWireCube(new Vector3(x, 0, y), new Vector3(gridSpacing, 0.1f, gridSpacing));
            }
        }
    }

    void CreateFence()
    {
        float fenceThickness = 0.1f; // Толщина забора
        float gapBetweenFences = -5f; // Расстояние между заборами

        float fenceYPosition = -10.0f; // Высота, на которой будут спавниться заборы

        // Создайте верхний и нижний забор
        for (int x = 0; x < gridSizeX; x++)
        {
            Vector3 fencePositionTop = new Vector3(x * gridSpacing - gapBetweenFences, fenceYPosition, -fenceThickness / 2 + gapBetweenFences);
            Vector3 fencePositionBottom = new Vector3(x * gridSpacing + gapBetweenFences, fenceYPosition, gridSizeY * gridSpacing + fenceThickness / 2 + gapBetweenFences);

            GameObject topFence = Instantiate(fencePrefab, fencePositionTop, Quaternion.Euler(0, 0, 0));
            GameObject bottomFence = Instantiate(fencePrefab, fencePositionBottom, Quaternion.Euler(0, -180, 0));

            // Добавляем ссылки на заборы в список
            spawnedFences.Add(topFence);
            spawnedFences.Add(bottomFence);
        }

        // Создайте левый и правый забор с учетом расстояния между заборами
        for (int y = 0; y < gridSizeY; y++)
        {
            // Modified the X position for the left fence
            Vector3 fencePositionLeft = new Vector3(fenceThickness / 2 + gapBetweenFences, fenceYPosition, y * gridSpacing + gapBetweenFences); // Changed the Z position

            Vector3 fencePositionRight = new Vector3(gridSizeX * gridSpacing + fenceThickness / 2 + gapBetweenFences, fenceYPosition, y * gridSpacing - gapBetweenFences); // Changed the Z position

            GameObject leftFence = Instantiate(fencePrefab, fencePositionLeft, Quaternion.Euler(0, 90, 0));
            GameObject rightFence = Instantiate(fencePrefab, fencePositionRight, Quaternion.Euler(0, -90, 0));

            // Добавляем ссылки на заборы в список
            spawnedFences.Add(leftFence);
            spawnedFences.Add(rightFence);
        }
    }

    public void SpawnFence()
    {
        ClearFences(); // Очистите существующие заборы перед спавном нового

        // Создайте забор вокруг границ сетки
        CreateFence();
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GridManager))]
    public class GridManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridManager gridManager = (GridManager)target;

            EditorGUILayout.Space();

            if (GUILayout.Button("Spawn Prefabs"))
            {
                gridManager.ClearGrid();
                gridManager.GenerateGrid();
            }

            if (GUILayout.Button("Clear Grid"))
            {
                gridManager.ClearGrid();
            }

            // Добавьте кнопку для спавна забора
            if (GUILayout.Button("Spawn Fence"))
            {
                gridManager.SpawnFence();
            }

            if (GUILayout.Button("Clear Fences"))
            {
                gridManager.ClearFences();
            }
        }
    }
#endif

    // Добавьте этот метод для очистки сетки
    public void ClearGrid()
    {
        GameObject[] spawnedPrefabs = GameObject.FindGameObjectsWithTag("LowPolyFruits");
        foreach (GameObject prefab in spawnedPrefabs)
        {
            DestroyImmediate(prefab);
        }
    }

    // Добавьте этот метод для удаления заборов
    public void ClearFences()
    {
        foreach (GameObject fence in spawnedFences)
        {
            DestroyImmediate(fence);
        }

        // Очистите список ссылок на заборы
        spawnedFences.Clear();
    }

    public Vector3 GetGridCenter(int x, int y)
    {
        float centerX = x * gridSpacing;
        float centerY = 0;
        float centerZ = y * gridSpacing;

        return new Vector3(centerX, centerY, centerZ);
    }

    public bool IsInsideGrid(Vector3 position)
    {
        float minX = 0;
        float minZ = 0;
        float maxX = gridSizeX * gridSpacing;
        float maxZ = gridSizeY * gridSpacing;

        return position.x >= minX && position.x <= maxX && position.z >= minZ && position.z <= maxZ;
    }
}