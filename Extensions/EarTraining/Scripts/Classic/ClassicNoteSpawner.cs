using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    public static NoteSpawner Instance { get; private set; }

    [Header("Note Visual Feedback")]
    public GameObject[] notePrefabs;
    public Vector3 spawnOffset = new Vector3(0, 0.5f, 0); // Offset from spawn position

    void Awake()
    {
        // Singleton pattern for global access
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnNote(float duration)
    {
        // Legacy method - now handled directly in NoteLetter.cs
        UnityEngine.Debug.Log("SpawnNote called but logic now handled in NoteLetter");
    }
}