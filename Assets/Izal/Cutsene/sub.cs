using TMPro;
using UnityEngine;

public class SubtitleController : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;

    public void SetSubtitle(string text)
    {
        subtitleText.text = text;
    }

    public void ClearSubtitle()
    {
        subtitleText.text = "";
    }
}