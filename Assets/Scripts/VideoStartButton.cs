using UnityEngine;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VideoStartButton : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private XRSimpleInteractable interactable;

    private void Start()
    {
        interactable.selectEntered.AddListener((args) => { videoPlayer.Play(); });
    }
}
