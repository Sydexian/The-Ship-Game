using UnityEngine;
using UnityEngine.UIElements;

[DisallowMultipleComponent]
public sealed class SailHUD : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private SailController sail;
    private Label openingLabel;
    private Label rotationLabel;
    private PanelSettings runtimePanelSettings;

    private void OnEnable()
    {
        //crates a panel for HUD to render
        runtimePanelSettings = ScriptableObject.CreateInstance<PanelSettings>();
        runtimePanelSettings.name = "Runtime Sailing HUD Panel";
        runtimePanelSettings.renderMode = PanelRenderMode.ScreenSpaceOverlay;
        runtimePanelSettings.scaleMode = PanelScaleMode.ScaleWithScreenSize;
        runtimePanelSettings.referenceResolution = new Vector2Int(1920, 1080);
        document.panelSettings = runtimePanelSettings;

        VisualElement root = document.rootVisualElement;
        root.style.flexDirection = FlexDirection.Column;
        root.style.alignItems = Align.FlexStart;
        root.style.paddingLeft = 20;
        root.style.paddingTop = 16;
        openingLabel = CreateLabel(root, "opening-label");
        rotationLabel = CreateLabel(root, "rotation-label");
        UpdateLabels();
    }

    private void Update()
    {
        UpdateLabels();
    }

    private void UpdateLabels()
    {
        if (sail == null) return;
        if (openingLabel != null) openingLabel.text = $"SAIL OPENING  {Mathf.RoundToInt(sail.OpeningPercent)}%";
        if (rotationLabel != null) rotationLabel.text = $"SAIL ROTATION  {Mathf.RoundToInt(sail.RotationDegrees)}\u00B0";
    }

    private static Label CreateLabel(VisualElement root, string elementName)
    {
        Label label = new Label { name = elementName };
        label.style.color = Color.white;
        label.style.fontSize = 18;
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.backgroundColor = new Color(0.02f, 0.08f, 0.13f, 0.88f);
        label.style.paddingLeft = 14;
        label.style.paddingRight = 14;
        label.style.paddingTop = 9;
        label.style.paddingBottom = 9;
        label.style.marginBottom = 8;
        label.style.minWidth = 240;
        root.Add(label);
        return label;
    }
}
