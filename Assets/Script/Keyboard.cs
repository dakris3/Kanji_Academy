using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public GameObject sisi1Panel;
    public GameObject sisi2Panel;

    private bool isSisi1 = true;

    public void SwitchKeyboard()
    {
        isSisi1 = !isSisi1;
        sisi1Panel.SetActive(isSisi1);
        sisi2Panel.SetActive(!isSisi1);
    }
}
