using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MotorBikerScrollActivator : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonScrollPair
    {
        public Button button;
        public GameObject scrollView;
    }

    [SerializeField] private ButtonScrollPair[] buttonScrollPairs;

    private Button previousButton;
    private GameObject previousScrollView;

    [Header("Button Colors")]
    [SerializeField] private Color firstNormalColor;
    [SerializeField] private Color firstHighlightedColor;
    [SerializeField] private Color firstPressedColor;

    private void Start()
    {
        // Initialize all buttons
        foreach (ButtonScrollPair pair in buttonScrollPairs)
        {
            pair.button.colors = new ColorBlock
            {
                normalColor = firstNormalColor,
                highlightedColor = firstHighlightedColor,
                pressedColor = firstPressedColor,
                colorMultiplier = 1f,
                fadeDuration = 0.1f
            };
        }

        // SetMainScroll for the first button
        if (buttonScrollPairs.Length > 0)
        {
            SetMainScrollOnClick(buttonScrollPairs[0].button);
        }
    }

    public void SetMainScrollOnClick(Button mainButton)
    {
        if (mainButton == null)
        {
            Debug.LogWarning("Main button is not assigned.");
            return;
        }

        // Find the corresponding ScrollView
        GameObject mainScrollView = null;

        foreach (ButtonScrollPair pair in buttonScrollPairs)
        {
            if (pair.button == mainButton)
            {
                mainScrollView = pair.scrollView;
                break;
            }
        }

        if (mainScrollView == null)
        {
            Debug.LogWarning("No ScrollView found for this button.");
            return;
        }

        // Set the main button's disabled color
        mainButton.colors = new ColorBlock
        {
            normalColor = firstNormalColor,
            highlightedColor = firstHighlightedColor,
            pressedColor = firstPressedColor,
            disabledColor = firstPressedColor,
            colorMultiplier = 1f,
            fadeDuration = 0.1f
        };

        mainButton.interactable = false;

        // Activate the new ScrollView
        mainScrollView.SetActive(true);

        // Reset the previous button
        if (previousButton != null && previousButton != mainButton)
        {
            previousButton.colors = new ColorBlock
            {
                normalColor = firstNormalColor,
                highlightedColor = firstHighlightedColor,
                pressedColor = firstPressedColor,
                colorMultiplier = 1f,
                fadeDuration = 0.1f
            };

            previousButton.interactable = true;
        }

        // Deactivate the previous ScrollView
        if (previousScrollView != null && previousScrollView != mainScrollView)
        {
            previousScrollView.SetActive(false);
        }

        // Update previous references
        previousButton = mainButton;
        previousScrollView = mainScrollView;
    }
}
