using UnityEngine;
using UnityEngine.Localization.Components;

[RequireComponent(typeof(LocalizeSpriteEvent))]
public class InputAwareLocalization : MonoBehaviour
{
    [SerializeField] private LocalizeSpriteEvent localizeSpriteEvent;
    [SerializeField] private InputDeviceService inputDeviceService;
    [SerializeField] private string keyboardEntryKey = "Keyboard_Entry_Key";
    [SerializeField] private string gamepadEntryKey = "Gamepad_Entry_Key";

    private string _lastAppliedEntryKey;

    void Awake()
    {
        if (localizeSpriteEvent == null) localizeSpriteEvent = GetComponent<LocalizeSpriteEvent>();
        ResolveInputDeviceService();

        if (localizeSpriteEvent == null)
        {
            Debug.LogWarning($"{nameof(InputAwareLocalization)} on {name} requires a {nameof(LocalizeSpriteEvent)} component.", this);
        }
    }

    void OnEnable()
    {
        ResolveInputDeviceService();

        if (inputDeviceService == null)
        {
            Debug.LogError($"{nameof(InputAwareLocalization)} on {name} requires an active {nameof(InputDeviceService)} in scene.", this);
            enabled = false;
            return;
        }

        inputDeviceService.DeviceTypeChanged += OnDeviceTypeChanged;
        inputDeviceService.RefreshFromCurrentState();
        ApplyReference(inputDeviceService.IsUsingGamepad);
    }

    void OnDisable()
    {
        if (inputDeviceService != null)
            inputDeviceService.DeviceTypeChanged -= OnDeviceTypeChanged;
    }

    private void OnDeviceTypeChanged(InputDeviceType deviceType)
    {
        ApplyReference(deviceType == InputDeviceType.Gamepad);
    }

    private void ApplyReference(bool useGamepad)
    {
        if (localizeSpriteEvent == null)
            return;

        string targetKey = useGamepad ? gamepadEntryKey : keyboardEntryKey;
        if (_lastAppliedEntryKey == targetKey)
            return;

        _lastAppliedEntryKey = targetKey;
        localizeSpriteEvent.AssetReference.SetReference(localizeSpriteEvent.AssetReference.TableReference, targetKey);
    }

    private void ResolveInputDeviceService()
    {
        if (inputDeviceService == null) inputDeviceService = InputDeviceService.Instance;
    }
}