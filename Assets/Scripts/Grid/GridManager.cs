using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    public Grid gridPrefab;
    public GridSandBox gridSandBoxPrefab;

    private ObjectPool<Grid> gridPool;
    private ObjectPool<GridSandBox> gridSandBoxPool;

    public int MapSize { get; set; } = 0;
    public Gamemode Gamemode { get; set; } = Gamemode.None;

    Dictionary<Vector2Int, Grid> gridDict = new Dictionary<Vector2Int, Grid>();
    Dictionary<Vector2Int, GridSandBox> gridSandBoxDict = new Dictionary<Vector2Int, GridSandBox>();

    Vector2Int startPoint = new Vector2Int(-1, -1);
    Vector2Int endPoint = new Vector2Int(-1, -1);

    [SerializeField] NPCManager npcManager;

    bool hadSolve = false;
    public void InitializeMatrix()
    {
        ReturnAllToPool();
        Debug.Log("Reset");
        gridDict.Clear();
        gridSandBoxDict.Clear();
        npcManager.StopAnim();
        hadSolve = false;
    }
    public void AddGrid(Grid grid)
    {
        gridDict[grid.GridPoint] = grid;
    }
    public void AddGridSandBox(GridSandBox gridSandBox) 
    {
        gridSandBoxDict[gridSandBox.GridPoint] = gridSandBox; 
    }
    public Grid GetGrid(Vector2Int pos)
    {
        return gridDict.TryGetValue(pos, out var grid) ? grid : null;
    }

    public void SortGridDictByPosition()
    {
        gridDict = gridDict
            .OrderBy(kv => kv.Key.y)
            .ThenBy(kv => kv.Key.x)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }

    public void SortGridSandBoxDictByPosition()
    {
        gridSandBoxDict = gridSandBoxDict
            .OrderBy(kv => kv.Key.y)
            .ThenBy(kv => kv.Key.x)
            .ToDictionary(kv => kv.Key, kv => kv.Value);
    }



    public GridSandBox GetGridSandBox(Vector2Int pos)
    {
        return gridSandBoxDict.TryGetValue(pos, out var grid) ? grid : null;
    }


    public void FillAll(GridType type)
    {
        foreach (var grid in gridDict.Values)
        {
            grid.ChangeGridType(type);
        }
    }

    public void SetStartAndEndPoint(Vector2Int start, Vector2Int end)
    {
        GetGrid(start).ChangeGridType(GridType.None);
        GetGrid(end).ChangeGridType(GridType.None);

        this.startPoint = start;
        this.endPoint = end;

        GetGrid(start).ChangeGridType(GridType.StartPoint);
        GetGrid(end).ChangeGridType(GridType.EndPoint);
    }

    public void PureRandom(Vector2Int start, Vector2Int end, float rate = 0.3f)
    {

        FillAll(GridType.Road);

        foreach (var grid in gridDict.Values)
        {
            if (Random.value < rate)
            {
                grid.ChangeGridType(GridType.Wall);
            }
        }

        SetStartAndEndPoint(start, end);

    }

    public void GenerateGuaranteedPath(Vector2Int start, Vector2Int end, float rate = 0.5f)
    {

        // 1. Đặt toàn bộ thành Road
        FillAll(GridType.Road);

        // 2. Tìm 1 đường bất kỳ từ start -> end (luôn thành công vì toàn Road)
        List<Vector2Int> path = FindRandomPathToEnd(start, end);
        if (path == null || path.Count == 0)
        {
            Debug.LogError("Không tìm được đường đi đến đích!");
            return;
        }

        HashSet<Vector2Int> pathSet = new HashSet<Vector2Int>(path);

        // 3. Đánh dấu lại các ô không thuộc path: random thành Wall/Road
        foreach (var pair in gridDict)
        {
            Vector2Int pos = pair.Key;
            if (!pathSet.Contains(pos))
            {
                GridType type = Random.value < rate ? GridType.Wall : GridType.Road;
                pair.Value.ChangeGridType(type);
            }
        }

        SetStartAndEndPoint(start, end);

    }

    List<Vector2Int> FindRandomPathToEnd(Vector2Int start, Vector2Int end)
    {
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int[] directions = new Vector2Int[]
        {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            // Trộn hướng để path không cố định
            directions = directions.OrderBy(x => Random.value).ToArray();

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (!gridDict.ContainsKey(next) || visited.Contains(next)) continue;
                if (GetGrid(next).GridType == GridType.Wall) continue;

                queue.Enqueue(next);
                visited.Add(next);
                parent[next] = current;

                if (next == end)
                {
                    // Lập tức truy ngược lại path
                    List<Vector2Int> path = new List<Vector2Int>();
                    Vector2Int p = end;
                    while (p != start)
                    {
                        path.Add(p);
                        p = parent[p];
                    }
                    path.Add(start);
                    path.Reverse();
                    return path;
                }
            }
        }

        return null;
    }


    public void GenerateMaze(Vector2Int start, Vector2Int end)
    {

        int size = MapSize;

        FillAll(GridType.Wall);

        bool[,] visited = new bool[size, size];
        bool foundEnd = false;

        void Dig(Vector2Int current)
        {
            if (foundEnd) return;
            visited[current.x, current.y] = true;
            GetGrid(current).ChangeGridType(GridType.Road);

            if (current == end)
            {
                foundEnd = true;
                return;
            }

            var directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
            directions = directions.OrderBy(_ => Random.value).ToList();

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir * 2;
                Vector2Int wall = current + dir;

                if (next.x >= 0 && next.x < size && next.y >= 0 && next.y < size && !visited[next.x, next.y])
                {
                    GetGrid(wall).ChangeGridType(GridType.Road);
                    Dig(next);
                    if (foundEnd) return;
                }
            }
        }

        Dig(start);
        SetStartAndEndPoint(start, end);

    }

    public void FindPath()
    {
        //sử dụng BFS
        int size = MapSize;

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> parent = new Dictionary<Vector2Int, Vector2Int>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        queue.Enqueue(startPoint);
        visited.Add(startPoint);

        Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };


        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();

            if (current == endPoint)
            {
                List<Vector2Int> path = new List<Vector2Int>();
                List<RectTransform> transforms = new List<RectTransform>();
                Vector2Int p = endPoint;

                while (p != startPoint)
                {
                    path.Add(p);
                    p = parent[p];
                }

                path.Add(startPoint);
                path.Reverse();

                for (int i = 0; i < path.Count; i++)
                {
                    if (Gamemode != Gamemode.SandBox && Gamemode != Gamemode.None)
                    {
                        transforms.Add(gridDict[path[i]].GetRectTransform());
                        if (i == path.Count - 1) continue;
                        gridDict[path[i]].ChangeGridType(GridType.CorrectRoad);
                    }
                    else if (Gamemode == Gamemode.SandBox)
                    {
                        transforms.Add(gridSandBoxDict[path[i]].GetRectTransform());
                        if (i == path.Count - 1) continue;
                        gridSandBoxDict[path[i]].ChangeGridType(GridType.CorrectRoad);  
                    }
                }
                npcManager.SpawnNPC(transforms[0], GameUI.Instance.Get<UIGame>().GetNPCParent());
                npcManager.PlayNPCAnim(transforms);

                UIManager.Instance.ChangeState(UIStates.NpcAnim);
                hadSolve = true;
                return;
            }

            foreach (var dir in directions)
            {
                Vector2Int next = current + dir;

                if (Gamemode != Gamemode.SandBox && Gamemode != Gamemode.None)
                {
                    if (!gridDict.ContainsKey(next) || visited.Contains(next)) continue;
                    var type = gridDict[next].GridType;
                    if (type == GridType.Road || type == GridType.EndPoint)
                    {
                        queue.Enqueue(next);
                        visited.Add(next);
                        parent[next] = current;
                    }
                }    
                else if (Gamemode == Gamemode.SandBox)
                {
                    Debug.Log("Sand Box");
                    if (!gridSandBoxDict.ContainsKey(next) || visited.Contains(next)) continue;
                    var type = gridSandBoxDict[next].GridType;
                    if (type == GridType.Road || type == GridType.EndPoint)
                    {
                        queue.Enqueue(next);
                        visited.Add(next);
                        parent[next] = current;
                    }

                    foreach(GridSandBox gridSandBox in gridSandBoxDict.Values) gridSandBox.SetCanClick(false);
                }

               //Debug.Log("loop");
            }
        }

        if (hadSolve) return;
        UIManager.Instance.ChangeState(UIStates.NoWay);
    }

    public void GenerateSandBox()
    {
        GetGridSandBox(Vector2Int.zero).ChangeGridType(GridType.Wall);
        GetGridSandBox(Vector2Int.zero).ChangeGridType(GridType.StartPoint);
        startPoint = Vector2Int.zero;
        GetGridSandBox(new Vector2Int(MapSize-1, MapSize-1)).ChangeGridType(GridType.Wall);
        GetGridSandBox(new Vector2Int(MapSize-1, MapSize-1)).ChangeGridType(GridType.EndPoint);
        endPoint = new Vector2Int(MapSize - 1, MapSize - 1);
    }

    #region Object Pool
    public void Init(Transform gridLayoutGroup)
    {
        gridPool = new ObjectPool<Grid>(gridPrefab, gridLayoutGroup);
        gridSandBoxPool = new ObjectPool<GridSandBox>(gridSandBoxPrefab, gridLayoutGroup);
    }

    public Grid GetGridFromPool()
    {
        return gridPool.Get();
    }

    public GridSandBox GetGridSandBoxFromPool()
    {
        return gridSandBoxPool.Get();
    }

    public void ReturnAllToPool()
    {
        foreach (var grid in gridDict.Values)
            gridPool.Return(grid);

        foreach (var gridPick in gridSandBoxDict.Values)
            gridSandBoxPool.Return(gridPick);

        gridDict.Clear();
        gridSandBoxDict.Clear();
    }
    #endregion
}
