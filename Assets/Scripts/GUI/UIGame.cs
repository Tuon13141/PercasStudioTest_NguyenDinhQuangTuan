using Project_Data.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] GridMapGenerator gridMapGenerator;

    [SerializeField] Button homeButton;
    [SerializeField] Button reloadButton;
    [SerializeField] Button findPathButton;

    [SerializeField] Transform npcParent;

    private void Start()
    {
        homeButton.onClick.AddListener(HomeButton);
        reloadButton.onClick.AddListener(ReloadButton);
        findPathButton.onClick.AddListener(FindPathButton);
    }
    public void StartGame()
    {
        gridMapGenerator.OnStart();
    }

    public void HomeButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.ChangeState(UIStates.Home);
    }

    public void ReloadButton()
    {
        SoundManager.Instance.PlayClickSound();
        StartGame();
    }

    public void FindPathButton()
    {
        SoundManager.Instance.PlayClickSound();
        GridManager.Instance.FindPath();
    }

    public Transform GetNPCParent() { return npcParent; }

}
