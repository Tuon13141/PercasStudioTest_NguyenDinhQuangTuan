using System.Collections;
using UnityEngine;
using Data;
using UnityEngine.AI;

public class UIManager : Singleton<UIManager>
{  
    [SerializeField] private UIStates _state = UIStates.None;
    
    GameUI gameUI = null;
   

    private void Start()
    {
        gameUI = GameUI.Instance;
        ChangeState(UIStates.Home); 

        ChangeState(UIStates.Info);
    }

    public void ChangeState(UIStates newState)
    {
        if (newState == _state) return;
        
        ExitCurrentState();
        _state = newState;
        EnterNewState();
    }

    private void EnterNewState()
    {
        switch (_state)
        {
            case UIStates.Home:
                gameUI.Get<UIHome>().Show();
                break;
            case UIStates.Play:
                gameUI.Get<UIGame>().StartGame();
                break;
            case UIStates.PickGamemode:
                gameUI.Get<UIPickMode>().Show();
                break;
            case UIStates.EnterMapSize:
                gameUI.Get<UIGame>().Show();
                gameUI.Get<UIEnterMapSize>().Show();
                break;
            case UIStates.Setting:
                gameUI.Get<UISetting>().Show();
                break;
            case UIStates.Info:
                gameUI.Get<UIInfo>().Show();
                break;
            default:
                break;
        }
    }

    public void ExitCurrentState()
    {
        switch (_state)
        {
            case UIStates.Home:
                gameUI.Get<UIHome>().StopAnim();
                break;
            case UIStates.Play:

                break;
            case UIStates.PickGamemode:
                gameUI.Get<UIPickMode>().Hide();
                gameUI.Get<UIHome>().Hide();
                break;
            case UIStates.EnterMapSize:
                gameUI.Get<UIEnterMapSize>().Hide();
                break;
            case UIStates.Setting:
                gameUI.Get<UISetting>().Hide();
                break;
            case UIStates.Info:
                gameUI.Get<UIInfo>().Hide();
                break;
            default:
                break;
        }
    }

    public void EnterGame()
    {
        ChangeState(UIStates.PickGamemode);
    }

    public void PickGamemode(Gamemode gamemode)
    {
        GridManager.Instance.Gamemode = gamemode;
        ChangeState(UIStates.EnterMapSize);
    }

    public void EnterMapSize(int mapSize)
    {
        GridManager.Instance.MapSize = mapSize;
        ChangeState(UIStates.Play);
    }
}

public enum UIStates
{
    Play, Home, PickGamemode, EnterMapSize, Setting, Info, None
}
