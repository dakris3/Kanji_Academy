using UnityEngine;

public class Keyboard : MonoBehaviour
{
    public GameObject sisi1Panel;
    public GameObject sisi2Panel;
    public GameObject sisi3Panel;

    public void ShowSisi1()
    {
        SetActivePanel(1);
    }

    public void ShowSisi2()
    {
        SetActivePanel(2);
    }

    public void ShowSisi3()
    {
        SetActivePanel(3);
    }

    private void SetActivePanel(int sisi)
    {
        sisi1Panel.SetActive(sisi == 1);
        sisi2Panel.SetActive(sisi == 2);
        sisi3Panel.SetActive(sisi == 3);
    }
}
