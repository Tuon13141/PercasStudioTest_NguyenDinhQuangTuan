using Project_Data.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] Button onSoundButton;
    [SerializeField] Button offSoundButton;

    [SerializeField] Button onMusicButton;
    [SerializeField] Button offMusicButton;

    [SerializeField] Image soundOnImage;
    [SerializeField] Image soundOffImage;

    [SerializeField] Image musicOnImage;
    [SerializeField] Image musicOffImage;

    [SerializeField] Button backButton;
    // Start is called before the first frame update
    void Start()
    {
        onMusicButton.onClick.AddListener(OnMusicButton);
        offMusicButton.onClick.AddListener(OffMusicButton);

        onSoundButton.onClick.AddListener(OnSoundButton);
        offSoundButton.onClick.AddListener(OffSoundButton);

        backButton.onClick.AddListener(BackButton);

        if(SoundManager.Instance.IsSoundOn)
        {
            OnSoundButton();
        }
        else
        {
            OffSoundButton();
        }

        if (SoundManager.Instance.IsMusicOn)
        {
            OnMusicButton();
        }
        else
        {
            OffMusicButton();
        }
    }

    public void OnSoundButton()
    {
        soundOnImage.gameObject.SetActive(true);
        soundOffImage.gameObject.SetActive(false);

        SoundManager.Instance.SetSounds(true);
    }

    public void OffSoundButton()
    {
        soundOnImage.gameObject.SetActive(false);
        soundOffImage.gameObject.SetActive(true);

        SoundManager.Instance.SetSounds(false);
    }

    public void OnMusicButton()
    {
        musicOnImage.gameObject.SetActive(true);
        musicOffImage.gameObject.SetActive(false);

        SoundManager.Instance.SetMusic(true);
    } 

    public void OffMusicButton()
    {
        musicOnImage.gameObject.SetActive(false);
        musicOffImage.gameObject.SetActive(true);

        SoundManager.Instance.SetMusic(false);
    }

    public void BackButton()
    {
        SoundManager.Instance.PlayClickSound();
        UIManager.Instance.ChangeState(UIStates.Home);
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
