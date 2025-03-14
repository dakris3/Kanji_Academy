using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class SambungKata : MonoBehaviour
{
    public LineRenderer lineRenderer; // Line renderer to draw the lines
    private List<Vector3> points = new List<Vector3>(); // List of points for the line

    public List<Button> romajiButtons = new List<Button>(); // Fixed list of Romaji buttons (5 buttons)
    public List<Button> kanjiButtons = new List<Button>();  // Fixed list of Kanji buttons (5 buttons)

    public List<string> romajiSet1 = new List<string>(); // Set 1 Romaji content (text or image names)
    public List<string> kanjiSet1 = new List<string>();  // Set 1 Kanji content
    public List<string> romajiSet2 = new List<string>(); // Set 2 Romaji content
    public List<string> kanjiSet2 = new List<string>();  // Set 2 Kanji content

    // New: Pairings for Romaji and Kanji for each set, can be set in the Inspector
    public List<string> correctPairSet1 = new List<string>(); // Correct pairs for Set 1
    public List<string> correctPairSet2 = new List<string>(); // Correct pairs for Set 2

    private Button selectedRomaji; // Currently selected Romaji button
    private Button selectedKanji;  // Currently selected Kanji button

    private List<Button> connectedRomajiButtons = new List<Button>();
    private List<Button> connectedKanjiButtons = new List<Button>();

    public Color correctButtonColor;  // Color for correct pairings
    public Color defaultButtonColor;  // Optional: Reset button color after reset

    private int currentSet = 1; // Tracks the current set (1 or 2)

    void Start()
    {
        lineRenderer.enabled = false;

        // Initialize listeners for the buttons
        SetupListeners();

        // Set content for Set 1 initially
        SetContent(romajiSet1, kanjiSet1);
    }

    void SetupListeners()
    {
        // Add listeners to Romaji buttons
        foreach (Button romaji in romajiButtons)
        {
            romaji.onClick.AddListener(() => OnRomajiButtonClicked(romaji));
        }

        // Add listeners to Kanji buttons
        foreach (Button kanji in kanjiButtons)
        {
            kanji.onClick.AddListener(() => OnKanjiButtonClicked(kanji));
        }
    }

    // Set the content of the buttons for the current set
    void SetContent(List<string> romajiContent, List<string> kanjiContent)
    {
        // Set Romaji button text or image
        for (int i = 0; i < romajiButtons.Count; i++)
        {
            romajiButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = romajiContent[i];
        }

        // Set Kanji button text or image
        for (int i = 0; i < kanjiButtons.Count; i++)
        {
            kanjiButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = kanjiContent[i];
        }
    }

    // Called when a Romaji button is clicked
    void OnRomajiButtonClicked(Button clickedButton)
    {
        if (selectedRomaji == null)
        {
            selectedRomaji = clickedButton;  // Select the Romaji button
        }
    }

    // Called when a Kanji button is clicked
    void OnKanjiButtonClicked(Button clickedButton)
    {
        if (selectedRomaji != null && selectedKanji == null)
        {
            selectedKanji = clickedButton; // Select the Kanji button
            points.Clear();

            // Add the selected Romaji and Kanji button positions to the line
            points.Add(selectedRomaji.transform.position);
            points.Add(selectedKanji.transform.position);

            // Check if they match (by checking button names or content)
            if (IsCorrectPair(selectedRomaji.GetComponentInChildren<TextMeshProUGUI>().text, 
                              selectedKanji.GetComponentInChildren<TextMeshProUGUI>().text))
            {
                Debug.Log("Correct");

                selectedRomaji.GetComponent<Image>().color = correctButtonColor;
                clickedButton.GetComponent<Image>().color = correctButtonColor;

                connectedRomajiButtons.Add(selectedRomaji);
                connectedKanjiButtons.Add(selectedKanji);

                // Check if all buttons in the current set are connected
                if (connectedRomajiButtons.Count == 5 && currentSet == 1)
                {
                    // Move to Set 2 after Set 1 is complete
                    currentSet = 2;
                    SwitchToSet2();
                }
            }

            // Reset selections for the next pair
            selectedRomaji = null;
            selectedKanji = null;
        }
    }

    // Method to check if the selected Romaji and Kanji form the correct pair
    bool IsCorrectPair(string romaji, string kanji)
    {
        if (currentSet == 1)
        {
            // Check in correctPairSet1
            return correctPairSet1.Contains(romaji + "-" + kanji);
        }
        else if (currentSet == 2)
        {
            // Check in correctPairSet2
            return correctPairSet2.Contains(romaji + "-" + kanji);
        }

        return false;
    }

    // Switch to Set 2 and change button content
    void SwitchToSet2()
    {
        // Clear previous connections
        connectedRomajiButtons.Clear();
        connectedKanjiButtons.Clear();

        // Reset the colors of the buttons
        foreach (Button button in romajiButtons)
        {
            button.GetComponent<Image>().color = defaultButtonColor;
        }
        foreach (Button button in kanjiButtons)
        {
            button.GetComponent<Image>().color = defaultButtonColor;
        }

        // Set the new content for Set 2
        SetContent(romajiSet2, kanjiSet2);

        Debug.Log("Switched to Set 2");
    }

    // Optional: Draw a line between the selected Romaji and Kanji
    void DrawLine()
    {
        lineRenderer.enabled = true; // Enable the line renderer
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
        lineRenderer.startWidth = 0.1f; // Set line thickness (start)
        lineRenderer.endWidth = 0.1f;   // Set line thickness (end)
    }
}
