using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    [SerializeField] protected Image backgroundImg;

    [SerializeField] protected Sprite roadSprite;
    [SerializeField] protected Sprite wallSprite;
    [SerializeField] protected Sprite correctRoadSprite;
    [SerializeField] protected Sprite destinationSprite;
    [SerializeField] protected Sprite npcSprite;
    [SerializeField] protected Sprite noneSprite;

    public bool HaveType {  get; protected set; }
    public GridType GridType { get; protected set; } = GridType.None;

    public Vector2Int GridPoint { get; protected set; } = Vector2Int.zero;

    public void SetGridPoint(Vector2Int gridPoint) { this.GridPoint = gridPoint; }

    public void ChangeGridType(GridType gridType)
    {
        if (this.GridType == gridType) return;
        
        this.GridType = gridType;
        EnterNewType(); 
    }

    protected void EnterNewType()
    {
        switch (this.GridType)
        {
            case GridType.None:
                backgroundImg.sprite = noneSprite;
                break;
            case GridType.Road:
                backgroundImg.sprite = roadSprite;
                break;
            case GridType.Wall:
                backgroundImg.sprite = wallSprite;
                break;
            case GridType.StartPoint:
                backgroundImg.sprite = npcSprite;
                break;
            case GridType.EndPoint:
                backgroundImg.sprite = destinationSprite;
                break;
            case GridType.CorrectRoad:
                backgroundImg.sprite = correctRoadSprite;
                break;
            default:
                break;
        }
    }
}

public enum GridType
{
    None, Road, CorrectRoad, Wall, StartPoint, EndPoint
}
