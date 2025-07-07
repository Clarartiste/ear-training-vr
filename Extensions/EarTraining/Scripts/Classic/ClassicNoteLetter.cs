using UnityEngine;
using NumbersAndLetters.Shared.ScriptableObjects.Audio;
using NumbersAndLetters.Shared.Managers.Audio;

public class NoteLetterClassic : MonoBehaviour
{
    public SoundEffectSO noteSoundSO;
    private SfxHandler _sfxHandler;
    private AudioSource _ambientAudio;

    void Awake()
    {
        _sfxHandler = FindObjectOfType<SfxHandler>();
        if (_sfxHandler == null)
        {
            UnityEngine.Debug.LogError("NoteLetterClassic: SfxHandler not found in scene!");
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
            // Lower ambient volume during note
            if (_ambientAudio != null)
            {
                _ambientAudio.volume = 0.1f;
            }

            // Play the piano sound (NO SOUND in Classic - just for feedback)
            _sfxHandler.PlaySFX(noteSoundSO);
            UnityEngine.Debug.Log("Classic Mode: Selected " + gameObject.name);

            // Calculate duration for visual feedback
            float soundDuration = 1.0f;
            if (noteSoundSO.clip != null)
            {
                soundDuration = noteSoundSO.clip.length;
            }

            // Spawn visual note ABOVE THE TENT
            if (NoteSpawner.Instance != null && NoteSpawner.Instance.notePrefabs.Length > 0)
            {
                // Position above tent
                Vector3 notePosition = new Vector3(0.27f, 1.71f, -0.53f);

                int randomIndex = UnityEngine.Random.Range(0, NoteSpawner.Instance.notePrefabs.Length);
                GameObject chosenPrefab = NoteSpawner.Instance.notePrefabs[randomIndex];

                Vector3 prefabRotation = chosenPrefab.transform.eulerAngles;
                Quaternion noteRotation = Quaternion.Euler(prefabRotation.x, transform.eulerAngles.y, prefabRotation.z);

                GameObject note = Instantiate(chosenPrefab, notePosition, noteRotation);
                note.transform.localScale = Vector3.one * 0.01f;
                Destroy(note, soundDuration);
            }

            // TODO: Send answer to ClassicModeManager for validation

            // Restore ambient volume
            if (_ambientAudio != null)
            {
                Invoke("RestoreAmbientVolume", soundDuration + 0.5f);
            }
        }
    }

    private void RestoreAmbientVolume()
    {
        if (_ambientAudio != null)
        {
            _ambientAudio.volume = 0.3f;
        }
    }
}