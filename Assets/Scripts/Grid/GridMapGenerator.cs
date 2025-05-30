using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static GridMapGenerator;
using static Unity.VisualScripting.Metadata;

public class GridMapGenerator : MonoBehaviour, IOnStart
{
    public GridLayoutGroup gridLayoutGroup;
    public RectTransform containerRect;

    UIManager uiManager;
    GridManager gridManager;

    Gamemode gamemode;
    int gridSize;

    private void Start()
    {
        uiManager = UIManager.Instance;
        gridManager = GridManager.Instance;

        GridManager.Instance.Init(gridLayoutGroup.transform);
    }
    public void OnStart()
    {
        Gamemode gamemode = gridManager.Gamemode;
        gridSize = gridManager.MapSize;

        foreach (Transform child in gridLayoutGroup.transform)
        {
            child.gameObject.SetActive(false);
        }

        gridManager.InitializeMatrix();
      

        switch (gamemode)
        {
            case Gamemode.PureRandom:
                GeneratePureRandomMap();
                break;

            case Gamemode.RandomWithGuaranteedPath:
                GenerateRandomWithGuaranteedPathMap();
                break;

            case Gamemode.RandomMaze:
                GenerateRandomMazeMap();
                break;
            case Gamemode.SandBox:
                GenerateSandBoxMap();
                break;
        }
    }
    public void SetUpLayourGroup()
    {
       
        float containerWidth = containerRect.rect.width;
        float containerHeight = containerRect.rect.height;

        float cellWidth = containerWidth / gridSize;
        float cellHeight = containerHeight / gridSize;

        Vector2 cellSize = new Vector2(cellWidth, cellHeight);

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = gridSize;
        gridLayoutGroup.spacing = Vector2.zero;
        gridLayoutGroup.cellSize = cellSize;
        gridLayoutGroup.padding = new RectOffset(0, 0, 0, 0);

    }

    public void GeneratePureRandomMap()
    {
        SetUpLayourGroup();

        Vector2Int end = new Vector2Int(gridSize - 1, Random.Range(0, gridSize));
        Vector2Int start = new Vector2Int(0, Random.Range(0, gridSize));

        Gamemode mode = gridManager.Gamemode;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Grid tile = GridManager.Instance.GetGridFromPool();

                tile.SetGridPoint(new Vector2Int(col, row));
                gridManager.AddGrid(tile);
                tile.name = $"Tile_{row}_{col}";
            }
        }

        gridManager.PureRandom(start, end);
        Resort();
    }

    public void GenerateRandomWithGuaranteedPathMap()
    {
        SetUpLayourGroup();

        int random1 = Random.Range(0, gridSize);
        int random2 = Random.Range(0, gridSize);
        while(random1 == random2)
        {
            random2 = Random.Range(0, gridSize);
        }

        Vector2Int end = new Vector2Int(gridSize - 1, random1);
        Vector2Int start = new Vector2Int(0, random2);

        Gamemode mode = gridManager.Gamemode;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Grid tile = GridManager.Instance.GetGridFromPool();
                tile.SetGridPoint(new Vector2Int(col, row));
                gridManager.AddGrid(tile);
                tile.name = $"Tile_{col}_{row}";
            }
        }

        gridManager.GenerateGuaranteedPath(start, end);
        Resort();
    }

    public void GenerateRandomMazeMap()
    {
        SetUpLayourGroup();

        Vector2Int end = new Vector2Int(gridSize - 1, Random.Range(1, gridSize));
        Vector2Int start = new Vector2Int(0, Random.Range(1, gridSize));

        Gamemode mode = gridManager.Gamemode;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                Grid tile = GridManager.Instance.GetGridFromPool();
                tile.SetGridPoint(new Vector2Int(col, row));
                gridManager.AddGrid(tile);
                tile.name = $"Tile_{col}_{row}";
            }
        }

        gridManager.GenerateMaze(start, end);

        Resort();
    }

    void GenerateSandBoxMap()
    {
        SetUpLayourGroup();
        int gridSize = gridManager.MapSize;

        for (int row = 0; row < gridSize; row++)
        {
            for (int col = 0; col < gridSize; col++)
            {
                GridSandBox tile = GridManager.Instance.GetGridSandBoxFromPool();
                tile.SetGridPoint(new Vector2Int(row, col));
                gridManager.AddGridSandBox(tile);
                tile.name = $"Tile_{col}_{row}";
                tile.OnStart();
            }
        }

        gridManager.GenerateSandBox();
        Resort();
    }

    void Resort()
    {
        Transform parent = gridLayoutGroup.transform;
        List<Transform> tiles = new List<Transform>();

        foreach (Transform child in parent)
        {
            tiles.Add(child);
        }

        tiles = tiles.OrderBy(t =>
        {
            string[] parts = t.name.Split('_');
            int x = int.Parse(parts[1]);
            int y = int.Parse(parts[2]);
            return y * 10000 + x; 
        }).ToList();

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].SetSiblingIndex(i);
        }
    }
}

public enum Gamemode
{
    PureRandom, RandomWithGuaranteedPath, RandomMaze, SandBox, None
}
