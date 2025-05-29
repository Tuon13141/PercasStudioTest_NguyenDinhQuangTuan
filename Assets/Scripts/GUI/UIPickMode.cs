using Project_Data.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickMode : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] Button pureRandomModeButton;
    [SerializeField] Button randomWithGuaranteedPathModeButton;
    [SerializeField] Button randomMazeModeButton;
    [SerializeField] Button pickTypeModeButton;

    void Start()
    {
        pureRandomModeButton.onClick.AddListener(PureRandomModeButton);
        randomWithGuaranteedPathModeButton.onClick.AddListener(RandomWithGuaranteedPathModeButton);
        randomMazeModeButton.onClick.AddListener(RandomMazeModeButton);
        pickTypeModeButton.onClick.AddListener(PickTypeModeButton);
    }

    public void PureRandomModeButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.PickGamemode(Gamemode.PureRandom);
    }

    public void RandomWithGuaranteedPathModeButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.PickGamemode(Gamemode.RandomWithGuaranteedPath);
    }

    public void RandomMazeModeButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.PickGamemode(Gamemode.RandomMaze);
    }
    public void PickTypeModeButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.PickGamemode(Gamemode.SandBox);
    }

    public override void Show()
    {
        StartCoroutine(CommonUIAnimation.OpenPopUp(holder.GetComponent<RectTransform>(), Vector2.zero, Vector2.one, 0.15f, base.Show));
    }

    public override void Hide()
    {
        StartCoroutine(CommonUIAnimation.ClosePopUp(holder.GetComponent<RectTransform>(), Vector2.one, Vector2.zero, 0.15f, base.Hide));
    }
}
