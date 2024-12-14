using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SpaceshipUIController : MonoBehaviour
{
    [SerializeField] private XRSimpleInteractable interactable;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject videoObject;
    [SerializeField] private GameObject uiObject;
    [SerializeField] private Button beachStartButton;
    [SerializeField] private Button cityStartButton;
    [SerializeField] private string beachSceneName;
    [SerializeField] private string citySceneName;

    private bool started;
    private bool isLoading; // Prevent multiple scene loads

    private void Start()
    {
        interactable.selectEntered.AddListener(OnInteractableSelected);
        beachStartButton.onClick.AddListener(OnBeachButtonClicked);
        cityStartButton.onClick.AddListener(OnCityButtonClicked);
        started = false;
        isLoading = false;
        uiObject.SetActive(false);
    }

    private void OnBeachButtonClicked()
    {
        if (isLoading) return;
        isLoading = true;

        // Disable both buttons
        beachStartButton.interactable = false;
        cityStartButton.interactable = false;

        LoadLevel(beachSceneName);
    }

    private void OnCityButtonClicked()
    {
        if (isLoading) return;
        isLoading = true;

        // Disable both buttons
        beachStartButton.interactable = false;
        cityStartButton.interactable = false;

        LoadLevel(citySceneName);
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void OnInteractableSelected(SelectEnterEventArgs args)
    {
        StartUI();
    }

    private void StartUI()
    {
        if (started) return;

        started = true;
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoComplete;

        interactable.selectEntered.RemoveListener(OnInteractableSelected);
    }

    private void OnVideoComplete(VideoPlayer vp)
    {
        Debug.Log("VIDEO LOOP POINT REACHED");
        videoPlayer.Stop();
        videoObject.SetActive(false);
        uiObject.SetActive(true);

        videoPlayer.loopPointReached -= OnVideoComplete;
    }
}