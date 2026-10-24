using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public sealed class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private bool applyOnUpdate = true;

    private RectTransform _panel;
    private Rect _lastSafeArea;
    private Vector2Int _lastScreenSize;
    private ScreenOrientation _lastOrientation;

    private void Awake()
    {
        _panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void OnEnable()
    {
        if (_panel == null) _panel = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        if (!applyOnUpdate) return;

        bool changed =
            Screen.safeArea != _lastSafeArea ||
            Screen.width != _lastScreenSize.x ||
            Screen.height != _lastScreenSize.y ||
            Screen.orientation != _lastOrientation;

        if (changed) ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (_panel == null) _panel = GetComponent<RectTransform>();

        Rect safe = Screen.safeArea;

        _lastSafeArea = safe;
        _lastScreenSize = new Vector2Int(Screen.width, Screen.height);
        _lastOrientation = Screen.orientation;

        // Пиксельные координаты safe area → нормализованные якоря (0..1)
        Vector2 anchorMin = safe.position;
        Vector2 anchorMax = safe.position + safe.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _panel.anchorMin = anchorMin;
        _panel.anchorMax = anchorMax;
        _panel.offsetMin = Vector2.zero;
        _panel.offsetMax = Vector2.zero;

        // Логирование для проверки в консоли
        Debug.Log($"[SafeAreaFitter] safeArea = {safe}  |  screen = {Screen.width}×{Screen.height}  |  orientation = {Screen.orientation}");
    }
}