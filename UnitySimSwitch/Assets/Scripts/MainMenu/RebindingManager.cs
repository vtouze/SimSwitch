using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindingManager : MonoBehaviour
{
    #region Fields

    [Tooltip("Reference to the input action for movement.")]
    public InputActionReference _movementActionReference; 

    [Header("Buttons")]
    [Tooltip("Button for rebinding the forward movement action.")]
    [SerializeField] private Button _forwardButton;

    [Tooltip("Button for rebinding the backward movement action.")]
    [SerializeField] private Button _backwardButton;

    [Tooltip("Button for rebinding the left movement action.")]
    [SerializeField] private Button _leftButton;

    [Tooltip("Button for rebinding the right movement action.")]
    [SerializeField] private Button _rightButton;

    [Header("Text")]
    [Tooltip("Text display for the forward button's current binding.")]
    [SerializeField] private TMP_Text _forwardButtonText;

    [Tooltip("Text display for the backward button's current binding.")]
    [SerializeField] private TMP_Text _backwardButtonText;

    [Tooltip("Text display for the left button's current binding.")]
    [SerializeField] private TMP_Text _leftButtonText;

    [Tooltip("Text display for the right button's current binding.")]
    [SerializeField] private TMP_Text _rightButtonText;

    private string _waitingForInput = "..."; // Placeholder text while waiting for the user input during rebinding

    #endregion Fields

    #region Methods

    /// <summary>
    /// Start method called when the script is initialized.
    /// It sets up button listeners for starting the rebinding process.
    /// </summary>
    private void Start()
    {
        // Add listeners to buttons for rebinding respective controls
        _forwardButton.onClick.AddListener(() => StartRebinding(1, _forwardButtonText));
        _backwardButton.onClick.AddListener(() => StartRebinding(2, _backwardButtonText));
        _leftButton.onClick.AddListener(() => StartRebinding(3, _leftButtonText));
        _rightButton.onClick.AddListener(() => StartRebinding(4, _rightButtonText));

        // Update the button text to reflect the current bindings
        UpdateButtonTexts();
    }

    /// <summary>
    /// Updates the text on the buttons to show the current key bindings.
    /// </summary>
    private void UpdateButtonTexts()
    {
        // Update button texts for all movement actions
        _forwardButtonText.text = GetBindingDisplayName(1);
        _backwardButtonText.text = GetBindingDisplayName(2);
        _leftButtonText.text = GetBindingDisplayName(3);
        _rightButtonText.text = GetBindingDisplayName(4);
    }

    /// <summary>
    /// Retrieves the human-readable display name for the given input action binding.
    /// </summary>
    /// <param name="bindingIndex">The index of the binding in the action's bindings list.</param>
    /// <returns>A human-readable string representing the control bound to the specified action.</returns>
    private string GetBindingDisplayName(int bindingIndex)
    {
        // Get the input binding at the specified index
        InputBinding binding = _movementActionReference.action.bindings[bindingIndex];
        
        // Convert the effective path of the binding to a human-readable string and return it
        return InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    /// <summary>
    /// Starts the rebinding process for a specific action.
    /// </summary>
    /// <param name="bindingIndex">The index of the binding to be rebound.</param>
    /// <param name="buttonText">The TMP_Text component to update with the new binding's display name.</param>
    private void StartRebinding(int bindingIndex, TMP_Text buttonText)
    {
        InputAction action = _movementActionReference.action;

        Debug.Log(action);

        // If the action is already enabled, disable it to allow rebinding
        if (action.enabled)
        {
            action.Disable();
        }

        // Set the button text to indicate that the system is waiting for user input
        buttonText.text = _waitingForInput;

        // Start the interactive rebinding process, excluding the mouse
        action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>") // Exclude mouse to prevent accidental mouse rebind
            .OnComplete(operation =>
            {
                // Apply the new binding once the rebinding is completed
                action.ApplyBindingOverride(bindingIndex, operation.selectedControl.path);

                // Convert the new control's path to a human-readable string and update the button text
                string displayName = InputControlPath.ToHumanReadableString(operation.selectedControl.path, InputControlPath.HumanReadableStringOptions.OmitDevice);
                buttonText.text = displayName.ToUpper();

                // Re-enable the action after rebinding
                action.Enable();

                // Dispose of the rebinding operation to free resources
                operation.Dispose();
            })
            .OnCancel(operation =>
            {
                // If the rebinding is canceled, dispose of the operation
                operation.Dispose();
            })
            .Start(); // Start the rebinding process
    }

    #endregion Methods
}