using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UINoWay : UIElement
{
    public override bool ManualHide => true;

    public override bool DestroyOnHide => false;

    public override bool UseBehindPanel => true;

    Coroutine anim;

    public override void Show()
    {
        if(anim != null) StopCoroutine(anim);
        anim = StartCoroutine(CommonUIAnimation.OpenThenClosePopUp(holder.GetComponent<RectTransform>(), Vector2.zero, Vector2.one * 1.3f, 0.5f, base.Hide));
    }

}
