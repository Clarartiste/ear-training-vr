using UnityEngine;

public class LetterAutoArc : MonoBehaviour
{
    [Header("Letter References - Drag your letters here")]
    public Transform letterA;
    public Transform letterB;
    public Transform letterC;
    public Transform letterD;
    public Transform letterE;
    public Transform letterF;
    public Transform letterG;

    void Start()
    {
        // Automatic perfect arc on start!
        CreatePerfectArc();
    }

    void CreatePerfectArc()
    {
        // Perfect arc positions based on your layout
        Vector3[] perfectPositions = {
            new Vector3(2.8f, 0f, 3.0f),   // A - smooth start
            new Vector3(1.4f, 0f, 1.9f),   // B - nice flow
            new Vector3(0.1f, 0f, 0.8f),   // C - center area
            new Vector3(-1.3f, 0f, -0.2f), // D - continuing arc
            new Vector3(-2.9f, 0f, -0.9f), // E - smooth curve
            new Vector3(-4.4f, 0f, -1.3f), // F - flowing end
            new Vector3(-5.8f, 0f, -1.5f)  // G - perfect finish
        };

        // Perfect rotations for natural look
        float[] perfectRotations = { -20f, -22f, -25f, -27f, -29f, -31f, -33f };

        Transform[] letters = { letterA, letterB, letterC, letterD, letterE, letterF, letterG };

        for (int i = 0; i < letters.Length; i++)
        {
            if (letters[i] != null)
            {
                letters[i].position = perfectPositions[i];
                letters[i].rotation = Quaternion.Euler(0f, perfectRotations[i], 0f);
            }
        }

        UnityEngine.Debug.Log("Perfect arc created automatically! 🎵");
    }
}