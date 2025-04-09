using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public GameObject hiraganaPanel;
    public GameObject dakutenPanel;

    private bool isHiragana = true;

    public void SwitchKeyboard()
    {
        isHiragana = !isHiragana;
        hiraganaPanel.SetActive(isHiragana);
        dakutenPanel.SetActive(!isHiragana);
    }
}
