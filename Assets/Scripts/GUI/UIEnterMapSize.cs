using Project_Data.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEnterMapSize : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    [SerializeField] InputField inputField;
    [SerializeField] Button startButton;

    private void Start()
    {
        startButton.interactable = false;
        inputField.contentType = InputField.ContentType.IntegerNumber;
        inputField.onValueChanged.AddListener(ValidateInput);
        startButton.onClick.AddListener(StartButton);
    }

    private void ValidateInput(string input)
    {
        if (int.TryParse(input, out int number))
        {
            if (number > 10 && number <= 100)
            {
                startButton.interactable = true;
            }
            else
            {
                startButton.interactable = false;
            }
        }
        else
        {
            startButton.interactable = false;
        }
    }

    public void StartButton()
    {
        SoundManager.Instance.PlayClickSound();
        string input = inputField.text;

        int size = int.Parse(input);

        inputField.text = "";

        UIManager.Instance.EnterMapSize(size);
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
