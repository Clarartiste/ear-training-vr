using UnityEngine;
using NumbersAndLetters.Shared.ScriptableObjects.Audio;
using NumbersAndLetters.Shared.Managers.Audio;

public class TutorialNoteLetter : MonoBehaviour
{
    public SoundEffectSO noteSoundSO;
    private SfxHandler _sfxHandler;
    private AudioSource _ambientAudio; // Ambient audio reference

    void Awake()
    {
        _sfxHandler = FindObjectOfType<SfxHandler>();
        if (_sfxHandler == null)
        {
            UnityEngine.Debug.LogError("NoteLetter: SfxHandler not found in scene!");
        }

        // Find the ambient audio manager
        GameObject ambientManager = GameObject.Find("AmbientAudioManager");
        if (ambientManager != null)
        {
            _ambientAudio = ambientManager.GetComponent<AudioSource>();
        }
    }

    public void PlayNote()
    {
        if (noteSoundSO != null && _sfxHandler != null)
        {
            // Lower ambient volume during note playback
            if (_ambientAudio != null)
            {
                _ambientAudio.volume = 0.1f; // Very low volume
            }

            // Play the piano sound
            _sfxHandler.PlaySFX(noteSoundSO);
            UnityEngine.Debug.Log("Playing sound for " + gameObject.name);

            // Calculate sound duration and spawn visual note
            float soundDuration = 1.0f;
            if (noteSoundSO.clip != null)
            {
                soundDuration = noteSoundSO.clip.length;
            }

            // Spawn the 3D visual note above the letter
            if (NoteSpawner.Instance != null && NoteSpawner.Instance.notePrefabs.Length > 0)
            {
                Vector3 notePosition = transform.position + new Vector3(0, 2f, 0);
                int randomIndex = UnityEngine.Random.Range(0, NoteSpawner.Instance.notePrefabs.Length);
                GameObject chosenPrefab = NoteSpawner.Instance.notePrefabs[randomIndex];

                // Simple rotation - orientation will be handled by NoteOrientation script
                Quaternion noteRotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);

                // Debug: Display which note is created and where
                UnityEngine.Debug.Log("Creating note: " + chosenPrefab.name + " at position: " + notePosition);

                // Create the note
                GameObject note = Instantiate(chosenPrefab, notePosition, noteRotation);
                note.name = "NOTE_" + chosenPrefab.name;

                // Add orientation script and execute it
                NoteOrientation orientation = note.AddComponent<NoteOrientation>();
                orientation.autoOrientOnStart = false; // Execute manually
                orientation.OrientNote(); // Orient the note correctly

                // Keep normal size for now
                // note.transform.localScale = Vector3.one * 0.01f; // COMMENTED to see notes

                Destroy(note, soundDuration);
            }

            // Restore ambient volume after note finishes
            if (_ambientAudio != null)
            {
                Invoke("RestoreAmbientVolume", soundDuration + 0.5f);
            }
        }
    }

    // Restore ambient audio volume
    private void RestoreAmbientVolume()
    {
        if (_ambientAudio != null)
        {
            _ambientAudio.volume = 0.3f; // Normal volume
        }
    }
}