using UnityEngine;
using System;

/// <summary>
/// �����¼�������
/// </summary>
public class AnimationEventTrigger : MonoBehaviour
{
    public Action<string> aniEvent;

    public void AniMsgStr(string msg)
    {
        aniEvent?.Invoke(msg);
    }
}
