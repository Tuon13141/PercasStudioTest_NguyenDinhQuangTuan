using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSandBox : Grid, IOnStart
{
    [SerializeField] Button gridButton;

    bool canCLick = true;

    public void OnStart()
    {
        gridButton.onClick.AddListener(GridButton);
        GridButton();
        canCLick = true;
    }

    public void GridButton()
    {
        if (!canCLick) return;

        switch (this.GridType)
        {
            case GridType.None:
                GridType = GridType.Road;
                backgroundImg.sprite = roadSprite;
                break;
            case GridType.Road:
                GridType = GridType.Wall;
                backgroundImg.sprite = wallSprite;
                break;
            case GridType.Wall:
                GridType = GridType.Road;
                backgroundImg.sprite = roadSprite;

                break;
            case GridType.EndPoint:
                backgroundImg.sprite = destinationSprite;
                break;
            case GridType.CorrectRoad:
                GridType = GridType.Wall;
                backgroundImg.sprite = wallSprite;
                break;
            default:

                break;
        }
    }

    public void SetCanClick(bool canClick) { this.canCLick = canClick; }
}
