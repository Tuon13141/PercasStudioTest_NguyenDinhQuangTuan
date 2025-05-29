using Project_Data.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHome : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => false;

    [SerializeField] Button playButton;
    [SerializeField] RectTransform playRectTransform;
    [SerializeField] Button settingButton;
    [SerializeField] Button infoButton;

    Coroutine playButtonAnim;

    void Start()
    {
        playButton.onClick.AddListener(PlayButton);
        settingButton.onClick.AddListener(SettingButton);
        infoButton.onClick.AddListener(InfoButton);
    }

    public void PlayButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.EnterGame();
    }

    public void SettingButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.ChangeState(UIStates.Setting);
    }

    public void InfoButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.ChangeState(UIStates.Info);
    }

    public override void Hide()
    {
        base.Hide();

        StopAnim();
    }

    public void StopAnim()
    {
        if (playButtonAnim != null) StopCoroutine(playButtonAnim);
    }

    public override void Show()
    {
        base.Show();

        StopAnim();
        playButtonAnim = StartCoroutine(CommonUIAnimation.BreathingAnimation(playRectTransform, Vector2.one, Vector2.one * 1.15f));
    }
}
