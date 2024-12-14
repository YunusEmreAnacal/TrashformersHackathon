using UnityEngine;
using Neocortex.Data;

namespace Neocortex.Samples
{

    public class AudioChatSample : MonoBehaviour
    {
        public Animator characterAnimator;

        [SerializeField] private AudioSource audioSource;
        
        [Header("Neocortex Components")]
        [SerializeField] private NeocortexAudioReceiver audioReceiver;
        [SerializeField] private NeocortexSmartAgent agent;
        [SerializeField] private NeocortexThinkingIndicator thinking;
        [SerializeField] private NeocortexChatPanel chatPanel;
        [SerializeField] private NeocortexAudioChatInput audioChatInput;
        
        private void Start()
        {
            agent.OnTranscriptionReceived.AddListener(OnTranscriptionReceived);
            agent.OnChatResponseReceived.AddListener(OnChatResponseReceived);
            agent.OnAudioResponseReceived.AddListener(OnAudioResponseReceived);
            audioReceiver.OnAudioRecorded.AddListener(OnAudioRecorded);
            
        }

        private void StartMicrophone()
        {
            audioReceiver.StartMicrophone();
        }
        
        private void OnAudioRecorded(AudioClip clip)
        {
            agent.AudioToAudio(clip);
            thinking.Display(true);
            audioChatInput.SetChatState(false);
        }

        private void OnTranscriptionReceived(string transcription)
        {
            chatPanel.AddMessage(transcription, true);
        }

        private void OnChatResponseReceived(ChatResponse response)
        {
            chatPanel.AddMessage(response.message, false);

            string action = response.action;
            if (!string.IsNullOrEmpty(action))
            {
                Debug.Log($"[ACTION] {action}");

                // Aksiyonlara göre iþlem yapmak için switch kullanýyoruz
                switch (action)
                {
                    case "TAKE_TRASH":
                        TriggerTakeTrashAnimation();
                        break;

                    // Yeni aksiyonlar için ek case'ler ekleyebilirsiniz
                    default:
                        Debug.Log($"Unknown action: {action}");
                        break;
                }
            }
        }

        // TAKE_TRASH animasyonunu tetikleyen fonksiyon
        private void TriggerTakeTrashAnimation()
        {
            if (characterAnimator != null)
            {
                characterAnimator.SetTrigger("TakeTrash"); // Animator'daki "TakeTrash" trigger'ýný tetikler
                Debug.Log("TakeTrash animation triggered.");
            }
            else
            {
                Debug.LogError("Character Animator is not assigned.");
            }
        }

        private void OnAudioResponseReceived(AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();

            Invoke(nameof(StartMicrophone), audioClip.length);
            
            thinking.Display(false);
            audioChatInput.SetChatState(true);
        }
    }
}
