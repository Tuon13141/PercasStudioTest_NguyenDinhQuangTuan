using Project_Data.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRule : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] Button closeButton;
    void Start()
    {
        closeButton.onClick.AddListener(CloseButton);
        StartCoroutine(DelayEnableCloseButton());
    }

    public void CloseButton()
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

    IEnumerator DelayEnableCloseButton()
    {
        yield return new WaitForSeconds(3f);

        closeButton.gameObject.SetActive(true);
    }
}
