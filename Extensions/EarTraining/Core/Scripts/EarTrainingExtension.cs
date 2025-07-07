using UnityEngine;
using NumbersAndLetters.Shared.ScriptableObjects.Audio;
using NumbersAndLetters.Shared.Managers.Audio;
// Ajoute cette ligne si tu utilises TextMeshPro pour l'UI Text
// using TMPro; 

namespace Extensions.EarTraining
{
    /// <summary>
    /// Extension Ear Training - Mode Tutorial Simple
    /// Gameplay: Lettres A-G en ligne, clic pour entendre la note correspondante
    /// Pas de scoring, juste exploration libre pour entraîner l'oreille
    /// </summary>
    public class EarTrainingExtension : MonoBehaviour
    {
        [Header("Audio Configuration")]
        [SerializeField] private SoundEffectSO[] pianoNotes = new SoundEffectSO[7]; // A-G ScriptableObjects

        [Header("Characters A-G")]
        [SerializeField] private GameObject[] characters = new GameObject[7]; // A-G personnages en ligne

        // [Header("Note Maker (Background)")]
        // [SerializeField] private GameObject noteMaker; // Décoratif, pas utilisé pour l'instant

        // [Header("UI Tutorial")]
        // [SerializeField] private UnityEngine.UI.Text instructionText; // Commenté car non utilisé pour l'instant
        // Si tu utilises TextMeshPro, tu devrais utiliser:
        // [SerializeField] private TMPro.TextMeshProUGUI instructionText; 


        // Références systèmes
        private SfxHandler sfxHandler;

        void Start()
        {
            UnityEngine.Debug.Log("[EarTraining] Mode Tutorial démarré !");

            // Récupérer le SFX Handler
            sfxHandler = FindObjectOfType<SfxHandler>();

            if (ValidateConfiguration())
            {
                UnityEngine.Debug.Log("[EarTraining] Configuration Tutorial validée ✅");
                StartTutorialMode();
            }
            else
            {
                UnityEngine.Debug.LogError("[EarTraining] Configuration Tutorial invalide ❌");
            }
        }

        bool ValidateConfiguration()
        {
            // Vérifier les ScriptableObjects de notes
            for (int i = 0; i < pianoNotes.Length; i++)
            {
                if (pianoNotes[i] == null)
                {
                    UnityEngine.Debug.LogError($"[EarTraining] Piano Note SO {i} (Lettre {(char)('A' + i)}) manquante !");
                    return false;
                }
            }

            // Vérifier les personnages
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] == null)
                {
                    UnityEngine.Debug.LogError($"[EarTraining] Personnage {i} (Lettre {(char)('A' + i)}) manquant !");
                    return false;
                }
            }

            // Vérifier le SFX Handler
            if (sfxHandler == null)
            {
                UnityEngine.Debug.LogError("[EarTraining] SfxHandler non trouvé ! Assurez-vous qu'il y a un GameObject avec le script SfxHandler dans la scène.");
                return false;
            }

            return true;
        }

        void StartTutorialMode()
        {
            // Mettre à jour l'UI (le texte sera seulement dans la console maintenant)
            UpdateUI("Mode Tutorial - Cliquez sur les lettres A-G pour entendre les notes de piano !");

            // Configurer les interactions sur chaque personnage
            SetupCharacterInteractions();

            // Animation du Note Maker en arrière-plan (décoratif) - COMMENTÉ
            // AnimateNoteMaker();

            UnityEngine.Debug.Log("[EarTraining] Tutorial ready - Exploration libre activée");
        }

        void SetupCharacterInteractions()
        {
            for (int i = 0; i < characters.Length; i++)
            {
                if (characters[i] != null)
                {
                    // Ajouter ou vérifier les composants d'interaction VR
                    var interactable = characters[i].GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
                    if (interactable == null)
                    {
                        interactable = characters[i].AddComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();
                    }

                    // Configurer l'événement de clic
                    int noteIndex = i; // Capture pour closure
                    interactable.selectEntered.RemoveAllListeners();
                    interactable.selectEntered.AddListener((_) => OnCharacterClicked(noteIndex));

                    UnityEngine.Debug.Log($"[EarTraining] Interaction configurée pour {(char)('A' + i)}");
                }
            }
        }

        /// <summary>
        /// Appelé quand un personnage est cliqué
        /// </summary>
        /// <param name="characterIndex">Index du personnage (0=A, 1=B, etc.)</param>
        public void OnCharacterClicked(int characterIndex)
        {
            if (characterIndex < 0 || characterIndex >= pianoNotes.Length) return;

            char noteLetter = (char)('A' + characterIndex);
            UnityEngine.Debug.Log($"[EarTraining] 🎵 Clic sur {noteLetter} - Joue la note correspondante");

            // Jouer la note de piano via le SFX Handler
            if (sfxHandler != null && pianoNotes[characterIndex] != null)
            {
                sfxHandler.PlaySFX(pianoNotes[characterIndex]);
            }

            // Animation du personnage cliqué
            AnimateCharacter(characters[characterIndex]);

            // Mettre à jour l'interface (le texte sera seulement dans la console maintenant)
            UpdateUI($"Note {noteLetter} jouée ! Continuez à explorer les sons...");
        }

        /// <summary>
        /// Animation simple du personnage quand il est cliqué
        /// </summary>
        void AnimateCharacter(GameObject character)
        {
            if (character != null)
            {
                StartCoroutine(BounceAnimation(character));
            }
        }

        /// <summary>
        /// Animation de bounce pour feedback visuel
        /// </summary>
        System.Collections.IEnumerator BounceAnimation(GameObject obj)
        {
            Vector3 originalPos = obj.transform.position;
            Vector3 targetPos = originalPos + Vector3.up * 0.3f;

            // Monter
            float duration = 0.2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;

                obj.transform.position = Vector3.Lerp(originalPos, targetPos, progress);
                yield return null;
            }

            // Redescendre
            elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float progress = elapsed / duration;

                obj.transform.position = Vector3.Lerp(targetPos, originalPos, progress);
                yield return null;
            }

            obj.transform.position = originalPos;
        }

        // /// <summary>
        // /// Animation décorative du Note Maker en arrière-plan - COMMENTÉ
        // /// </summary>
        // void AnimateNoteMaker()
        // {
        //     // if (noteMaker != null) // La variable noteMaker est commentée, donc ce bloc n'est plus pertinent
        //     // {
        //     //     StartCoroutine(IdleNoteMakerAnimation());
        //     // }
        // }

        // /// <summary>
        // /// Animation idle continue du Note Maker - COMMENTÉ
        // /// </summary>
        // System.Collections.IEnumerator IdleNoteMakerAnimation()
        // {
        //     // Vector3 originalPos = noteMaker.transform.position; // Commenté

        //     while (true) // Animation continue
        //     {
        //         // Léger mouvement de haut en bas
        //         // float time = Time.time; // Commenté
        //         // float yOffset = Mathf.Sin(time * 0.5f) * 0.1f; // Commenté

        //         // noteMaker.transform.position = originalPos + Vector3.up * yOffset; // Commenté

        //         // Légère rotation
        //         // noteMaker.transform.Rotate(0, 10 * Time.deltaTime, 0); // Commenté

        //         yield return null;
        //     }
        // }

        /// <summary>
        /// Mettre à jour l'interface utilisateur
        /// </summary>
        void UpdateUI(string instruction)
        {
            // if (instructionText != null) // La variable instructionText est commentée
            //     instructionText.text = instruction; // Cette ligne ne s'exécutera plus

            UnityEngine.Debug.Log($"[EarTraining] UI: {instruction}"); // Toujours afficher dans la console
        }

        /// <summary>
        /// Méthode publique pour redémarrer le tutorial
        /// </summary>
        public void RestartTutorial()
        {
            UnityEngine.Debug.Log("[EarTraining] Redémarrage du Tutorial");
            UpdateUI("Mode Tutorial redémarré - Explorez les notes !");
        }

        /// <summary>
        /// Méthode pour passer aux autres modes (Classic, Time Trial, Survival)
        /// </summary>
        public void SwitchToClassicMode()
        {
            UnityEngine.Debug.Log("[EarTraining] Passage au mode Classic (à implémenter)");
            UpdateUI("Mode Classic - Fonctionnalité à venir !");
        }

        void OnDisable()
        {
            UnityEngine.Debug.Log("[EarTraining] Tutorial mode arrêté");
        }
    }
}