using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Vuforia;

public class ARStateController : MonoBehaviour
{
    private Text debugText;
    private bool isLocalized = false;
    private bool isLocalizationCountdownStarted = false;
    private readonly float LOCALIZATION_TIME_OUT = 10f;

    public UnityEvent PositionFoundEvent = new UnityEvent();
    public UnityEvent PositionLostEvent = new UnityEvent();

    private void Awake()
    {
        // Get the Text component from the GameObject this script is attached to
        debugText = GetComponent<Text>();

        if (debugText == null)
        {
            Debug.LogError("Text UI component is missing.");
            return;
        }
    }

    void Start()
    {
        // Register observer for status changes
        VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged += OnStatusChanged;
        debugText.text = "Waiting for Vuforia to initialize...";
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus targetStatus)
    {
        Status status = targetStatus.Status;
        StatusInfo statusInfo = targetStatus.StatusInfo;

        // Update the on-screen debug text
        debugText.text = $"Status: {status}\nStatus Info: {statusInfo}";

        if (status == Status.TRACKED)
        {
            debugText.text += "\nDevice localization is stable.";
            isLocalized = true;
        }
        else if (status == Status.LIMITED)
        {
            debugText.text += "\nTracking is limited. Please scan surroundings.";
            isLocalized = false;
        }
        else if (status == Status.NO_POSE)
        {
            debugText.text += "\nNo device pose available. Move the device to scan the environment.";
            isLocalized = false;

            if (!isLocalizationCountdownStarted)
            {
                isLocalizationCountdownStarted = true;
                StartCoroutine(LocalizationAttemptsCountdown());
            }
        }
        else if (status == Status.EXTENDED_TRACKED)
        {
            debugText.text += "\nRunning on extended tracking.";
            isLocalized = true;
        }
        else
        {
            debugText.text += "\nUnknown tracking state.";
        }
    }

    IEnumerator LocalizationAttemptsCountdown()
    {
        float counter = LOCALIZATION_TIME_OUT;
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
            debugText.text = $"Localization Timeout in {counter} seconds...";
        }
        ShowLocalizationFailedMessage();
        isLocalizationCountdownStarted = false;
    }

    void ShowLocalizationFailedMessage()
    {
        debugText.text = "Device could not be localized. Please restart AR experience.";
        Debug.LogWarning("Localization failed.");
    }

    void OnDestroy()
    {
        VuforiaBehaviour.Instance.DevicePoseBehaviour.OnTargetStatusChanged -= OnStatusChanged;
        StopAllCoroutines();
    }
}
