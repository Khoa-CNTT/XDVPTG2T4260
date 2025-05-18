using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class GridPlacer : EditorWindow
{
    private enum GridShape { Square, Circle, Triangle, Hexagon }

    [SerializeField] private GameObject objectToPlace;
    [SerializeField] private GridShape selectedShape = GridShape.Square;
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float gridSpacing = 1.0f;
    [SerializeField] private float radius = 5f;
    [SerializeField] private bool flipTriangle = false;

    [MenuItem("Tuleeeeee/Tools/Grid Placer")]
    public static void ShowWindow()
    {
        GetWindow<GridPlacer>("Grid Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Grid Placement Settings", EditorStyles.boldLabel);

        // Object to place field
        objectToPlace = (GameObject)EditorGUILayout.ObjectField("Object to Place", objectToPlace, typeof(GameObject), false);

        // Shape selection
        selectedShape = (GridShape)EditorGUILayout.EnumPopup("Grid Shape", selectedShape);

        // Common settings
        gridWidth = EditorGUILayout.IntSlider("Grid Width", gridWidth, 1, 100);
        gridWidth = Mathf.Max(1, gridWidth);
        if (selectedShape == GridShape.Square || selectedShape == GridShape.Triangle || selectedShape == GridShape.Hexagon)
        {
            gridHeight = EditorGUILayout.IntSlider("Grid Height", gridHeight, 1, 100);
            gridHeight = Mathf.Max(1, gridHeight);
        }

        // Spacing between objects
        if (!(selectedShape == GridShape.Circle))
        {
            gridSpacing = EditorGUILayout.Slider("Grid Spacing", gridSpacing, 0, 100);
            gridSpacing = Mathf.Max(0, gridSpacing);
        }
        // Radius for circular grid
        if (selectedShape == GridShape.Circle)
        {
            radius = EditorGUILayout.FloatField("Radius", radius);
        }

        // Option to flip triangle
        if (selectedShape == GridShape.Triangle)
        {
            flipTriangle = EditorGUILayout.Toggle("Flip Triangle", flipTriangle);
        }

        // Button to place objects in grid
        if (GUILayout.Button("Place Objects in Grid"))
        {
            switch (selectedShape)
            {
                case GridShape.Square:
                    PlaceSquareGrid();
                    break;
                case GridShape.Circle:
                    PlaceCircularGrid();
                    break;
                case GridShape.Triangle:
                    PlaceTriangularGrid();
                    break;
                case GridShape.Hexagon:
                    PlaceHexagonalGrid();
                    break;
            }
        }
    }

    private void PlaceSquareGrid()
    {
        if (objectToPlace == null) return;

        // Remove previous objects
        //    RemovePlacedObjects();
        GameObject parentObject = new GameObject($"{objectToPlace.name}");
        // Place objects in square grid (X-axis and Y-axis)
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x * gridSpacing, y * gridSpacing, 0);
                GameObject newObj = InstantiateObject(position);
                newObj.transform.parent = parentObject.transform;
            }
        }
    }

    private void PlaceCircularGrid()
    {
        if (objectToPlace == null) return;

        // Remove previous objects
        //    RemovePlacedObjects();
        GameObject parentObject = new GameObject($"{objectToPlace.name}");
        // Place objects in circular pattern
        int numObjects = gridWidth * gridHeight;  // Total number of objects to place
        for (int i = 0; i < numObjects; i++)
        {
            float angle = i * Mathf.PI * 2 / numObjects;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 position = new Vector3(x, y, 0);
            GameObject newObj = InstantiateObject(position);
            newObj.transform.parent = parentObject.transform;
        }
    }

    private void PlaceTriangularGrid()
    {
        if (objectToPlace == null) return;

        // Remove previous objects
        //    RemovePlacedObjects();
        GameObject parentObject = new GameObject($"{objectToPlace.name}");
        // Place objects in a real triangular grid pattern
        float verticalSpacing = Mathf.Sqrt(3) / 2 * gridSpacing;
        int sign = flipTriangle ? 1 : -1;
        // Iterate over the rows (gridHeight) to build the triangular pattern
        for (int y = 0; y < gridHeight; y++)
        {
            // Each row is offset horizontally based on the row number (for staggering effect)
            float xOffset = -(y * gridSpacing / 2f);

            for (int x = 0; x <= y; x++)
            {
                // Calculate the position for each object
                float xPos = (flipTriangle ? -(x * gridSpacing + xOffset) : (x * gridSpacing + xOffset));
                float yPos = (flipTriangle ? -(y * verticalSpacing) : (y * verticalSpacing));

                Vector3 position = new Vector3(xPos, yPos, 0);
                GameObject newObj = InstantiateObject(position);
                newObj.transform.parent = parentObject.transform;
            }
        }
    }

    private void PlaceHexagonalGrid()
    {
        if (objectToPlace == null) return;

        // Remove previous objects
        //   RemovePlacedObjects();
        GameObject parentObject = new GameObject($"{objectToPlace.name}");

        float hexWidth = gridSpacing * Mathf.Sqrt(3);  // Hexagonal width

        // Place objects in a hexagonal grid pattern
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                float xOffset = (y % 2 == 0) ? 0 : hexWidth / 2;
                Vector3 position = new Vector3(x * hexWidth + xOffset, y * (gridSpacing * 3f / 2f), 0);
                GameObject newObj = InstantiateObject(position);
                newObj.transform.parent = parentObject.transform;
            }
        }
    }

    /*  private void InstantiateObject(Vector3 position)
      {
          GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(objectToPlace);
          newObj.transform.position = position;
          newObj.tag = "GridPlacedObject";
      }*/
    private GameObject InstantiateObject(Vector3 position)
    {
        GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(objectToPlace);
        newObj.transform.position = position;
        return newObj;
    }
    /*   private void RemovePlacedObjects()
       {
           foreach (GameObject obj in GameObject.FindGameObjectsWithTag("GridPlacedObject"))
           {
               DestroyImmediate(obj);
           }
       }*/
}
#endif
