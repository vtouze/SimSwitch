using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindingManager : MonoBehaviour
{
    #region Fields
    public InputActionReference _movementActionReference; 
    [Header("Buttons")]
    [SerializeField] private Button _forwardButton;
    [SerializeField] private Button _backwardButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [Header("Text")]
    [SerializeField] private TMP_Text _forwardButtonText;
    [SerializeField] private TMP_Text _backwardButtonText;
    [SerializeField] private TMP_Text _leftButtonText;
    [SerializeField] private TMP_Text _rightButtonText;
    private string _waitingForInput = "...";
    #endregion Fields

    #region Methods
    private void Start()
    {
        _forwardButton.onClick.AddListener(() => StartRebinding(1, _forwardButtonText));
        _backwardButton.onClick.AddListener(() => StartRebinding(2, _backwardButtonText));
        _leftButton.onClick.AddListener(() => StartRebinding(3, _leftButtonText));
        _rightButton.onClick.AddListener(() => StartRebinding(4, _rightButtonText));

        UpdateButtonTexts();
    }

    private void UpdateButtonTexts()
    {
        _forwardButtonText.text = GetBindingDisplayName(1);
        _backwardButtonText.text = GetBindingDisplayName(2);
        _leftButtonText.text = GetBindingDisplayName(3);
        _rightButtonText.text = GetBindingDisplayName(4);
    }
    private string GetBindingDisplayName(int bindingIndex)
    {
        InputBinding binding = _movementActionReference.action.bindings[bindingIndex];
        return InputControlPath.ToHumanReadableString(binding.effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private void StartRebinding(int bindingIndex, TMP_Text buttonText)
    {
        InputAction action = _movementActionReference.action;

        Debug.Log(action);

        if (action.enabled)
        {
            action.Disable();
        }

        buttonText.text = _waitingForInput;

        action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Mouse>")
            .OnComplete(operation =>
            {
                action.ApplyBindingOverride(bindingIndex, operation.selectedControl.path);
                string displayName = InputControlPath.ToHumanReadableString(operation.selectedControl.path, InputControlPath.HumanReadableStringOptions.OmitDevice);
                buttonText.text = displayName.ToUpper();

                action.Enable();

                operation.Dispose();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
            })
            .Start();
    }
    #endregion Methods
}