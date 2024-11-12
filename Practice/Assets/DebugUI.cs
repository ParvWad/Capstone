using UnityEngine;
using TMPro;  // Import if using TextMeshPro

public class DebugUI : MonoBehaviour
{
    public TextMeshProUGUI debugText;  // Use TextMeshProUGUI for TextMeshPro, or use UnityEngine.UI.Text for regular UI Text

    private static DebugUI instance;

    void Awake()
    {
        instance = this;
    }

    // Static method to update the debug text from anywhere
    public static void Log(string message)
    {
        if (instance != null && instance.debugText != null)
        {
            instance.debugText.text = message;
        }
    }
}
