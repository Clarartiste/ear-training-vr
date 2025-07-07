using UnityEngine;

public class TutorialNoteOrientation : MonoBehaviour
{
    [Header("Note Orientation Settings")]
    public bool autoOrientOnStart = true;

    void Start()
    {
        if (autoOrientOnStart)
        {
            OrientNote();
        }
    }

    public void OrientNote()
    {
        string noteName = gameObject.name.ToLower();
        Vector3 currentRotation = transform.eulerAngles;

        // Keep Y rotation (player orientation) and adjust X and Z
        Vector3 newRotation = new Vector3(90, currentRotation.y, 0); // Default orientation

        if (noteName.Contains("eighth") && !noteName.Contains("beamed"))
        {
            // Eighth note - flip it upright
            newRotation = new Vector3(0, currentRotation.y, 10);
        }
        else if (noteName.Contains("beamed"))
        {
            // Beamed eighth notes (double eighth notes)
            newRotation = new Vector3(270, currentRotation.y + 180, 0);
        }
        else if (noteName.Contains("half"))
        {
            // Half note
            newRotation = new Vector3(270, currentRotation.y + 180, 0);
        }
        else if (noteName.Contains("quarter"))
        {
            // Quarter note
            newRotation = new Vector3(270, currentRotation.y, 0);
        }
        else if (noteName.Contains("whole"))
        {
            // Whole note
            newRotation = new Vector3(90, currentRotation.y, 0);
        }

        transform.eulerAngles = newRotation;

        // Debug log
        UnityEngine.Debug.Log("Note " + noteName + " oriented to: " + newRotation);
    }
}