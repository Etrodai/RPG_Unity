using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HelpOverlay : MonoBehaviour
{
    [SerializeField] private GameObject overlayContainer;
    [SerializeField] private List<GameObject> overlayImages = new List<GameObject>();
    [SerializeField] private PlayerInput playerInput;

    private int activeOverlayIndex;
    private const float stopTime = 0f;
    private const float startTime = 1f;

    /// <summary>
    /// Activates the help overlay via click on the question mark button
    /// </summary>
    public void OnClickHelpOverlay()
    {
        activeOverlayIndex = 0;
        overlayContainer.SetActive(true);
        overlayImages[activeOverlayIndex].SetActive(true);
        playerInput.actions["LeftClick"].performed += OnClickContinue;
        Time.timeScale = stopTime;
    }

    /// <summary>
    /// Switches to the next overlay on click
    /// </summary>
    /// <param name="context">Registers added input</param>
    public void OnClickContinue(InputAction.CallbackContext context)
    {
        if (activeOverlayIndex < overlayImages.Count -1)
        {
            overlayImages[activeOverlayIndex].SetActive(false);
            activeOverlayIndex++;
            overlayImages[activeOverlayIndex].SetActive(true);
        }
        else
        {
            overlayImages[activeOverlayIndex].SetActive(false);
            playerInput.actions["LeftClick"].performed -= OnClickContinue;
            overlayContainer.SetActive(false);
            Time.timeScale = startTime;
        }
    }
}
