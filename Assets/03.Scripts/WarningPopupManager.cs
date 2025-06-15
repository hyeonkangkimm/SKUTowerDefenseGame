using System.Collections;
using TMPro;
using UnityEngine;

public class WarningPopupManager : MonoBehaviour
{
    public static WarningPopupManager Instance { get; private set; }

    public GameObject warningPanel;
    public TextMeshProUGUI warningText;
    public float displayDuration = 2f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        warningPanel.SetActive(false);
    }

    public void ShowWarning(string message)
    {
        warningText.text = message;
        warningPanel.SetActive(true);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        warningPanel.SetActive(false);
    }
}
