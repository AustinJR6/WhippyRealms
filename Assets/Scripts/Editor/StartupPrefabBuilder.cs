#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// Utility that builds the runtime startup prefab for WhippyRealms.
/// Creates a GameManager with required components and UI elements,
/// then saves it as Assets/Prefabs/WhippyStartup.prefab.
/// </summary>
public static class StartupPrefabBuilder
{
#if UNITY_EDITOR
    [MenuItem("WhippyRealms/Build Startup Prefab")]
    public static void BuildPrefab()
    {
        // Root object that will hold all managers
        GameObject root = new GameObject("GameManager");

        // Attach manager scripts
        var gm = root.AddComponent<GameManager>();
        var input = root.AddComponent<InputHandler>();
        var log = root.AddComponent<LogManager>();
        var dialogue = root.AddComponent<DialogueManager>();
        var combat = root.AddComponent<CombatManager>();
        var inventory = root.AddComponent<InventoryManager>();
        var timeMgr = root.AddComponent<TimeManager>();
        var ai = root.AddComponent<AICompanion>();
        var ui = root.AddComponent<UIManager>();

        // ---- UI setup ----------------------------------------------------
        GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(root.transform, false);
        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        // Panel fills the canvas
        GameObject panel = new GameObject("Panel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(canvasGO.transform, false);
        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");

        // Scrollable log area ------------------------------------------------
        GameObject scrollGO = new GameObject("LogScroll", typeof(RectTransform), typeof(ScrollRect), typeof(Image));
        scrollGO.transform.SetParent(panel.transform, false);
        RectTransform scrollRect = scrollGO.GetComponent<RectTransform>();
        scrollRect.anchorMin = new Vector2(0f, 0.25f);
        scrollRect.anchorMax = new Vector2(1f, 1f);
        scrollRect.offsetMin = new Vector2(10f, 10f);
        scrollRect.offsetMax = new Vector2(-10f, -10f);

        GameObject viewport = new GameObject("Viewport", typeof(RectTransform), typeof(Mask), typeof(Image));
        viewport.transform.SetParent(scrollGO.transform, false);
        RectTransform viewRect = viewport.GetComponent<RectTransform>();
        viewRect.anchorMin = Vector2.zero;
        viewRect.anchorMax = Vector2.one;
        viewRect.offsetMin = Vector2.zero;
        viewRect.offsetMax = Vector2.zero;
        viewport.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        viewport.GetComponent<Mask>().showMaskGraphic = false;

        GameObject logTextGO = new GameObject("LogText", typeof(RectTransform), typeof(Text));
        logTextGO.transform.SetParent(viewport.transform, false);
        RectTransform logTextRect = logTextGO.GetComponent<RectTransform>();
        logTextRect.anchorMin = new Vector2(0, 1);
        logTextRect.anchorMax = new Vector2(1, 1);
        logTextRect.pivot = new Vector2(0, 1);
        logTextRect.offsetMin = new Vector2(5, 0);
        logTextRect.offsetMax = new Vector2(-5, 0);
        Text logText = logTextGO.GetComponent<Text>();
        logText.font = font;
        logText.alignment = TextAnchor.UpperLeft;
        logText.horizontalOverflow = HorizontalWrapMode.Wrap;
        logText.verticalOverflow = VerticalWrapMode.Overflow;

        ScrollRect sr = scrollGO.GetComponent<ScrollRect>();
        sr.content = logTextRect;
        sr.viewport = viewRect;
        sr.horizontal = false;

        // Input field -------------------------------------------------------
        GameObject inputGO = new GameObject("CommandInput", typeof(RectTransform), typeof(Image), typeof(InputField));
        inputGO.transform.SetParent(panel.transform, false);
        RectTransform inputRect = inputGO.GetComponent<RectTransform>();
        inputRect.anchorMin = new Vector2(0, 0);
        inputRect.anchorMax = new Vector2(1, 0);
        inputRect.pivot = new Vector2(0.5f, 0);
        inputRect.sizeDelta = new Vector2(0, 30);
        inputRect.anchoredPosition = new Vector2(0, 15);

        GameObject textGO = new GameObject("Text", typeof(RectTransform), typeof(Text));
        textGO.transform.SetParent(inputGO.transform, false);
        RectTransform textRect = textGO.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(10, 6);
        textRect.offsetMax = new Vector2(-10, -7);
        Text textComp = textGO.GetComponent<Text>();
        textComp.font = font;
        textComp.alignment = TextAnchor.MiddleLeft;

        GameObject placeholderGO = new GameObject("Placeholder", typeof(RectTransform), typeof(Text));
        placeholderGO.transform.SetParent(inputGO.transform, false);
        RectTransform placeholderRect = placeholderGO.GetComponent<RectTransform>();
        placeholderRect.anchorMin = Vector2.zero;
        placeholderRect.anchorMax = Vector2.one;
        placeholderRect.offsetMin = new Vector2(10, 6);
        placeholderRect.offsetMax = new Vector2(-10, -7);
        Text placeholder = placeholderGO.GetComponent<Text>();
        placeholder.font = font;
        placeholder.text = "Enter command...";
        placeholder.fontStyle = FontStyle.Italic;
        placeholder.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        placeholder.alignment = TextAnchor.MiddleLeft;

        InputField inputField = inputGO.GetComponent<InputField>();
        inputField.textComponent = textComp;
        inputField.placeholder = placeholder;
        inputField.targetGraphic = inputGO.GetComponent<Image>();

        // ---- Reference assignments ---------------------------------------
        input.inputField = inputField;
        input.gameManager = gm;
        input.uiManager = ui;
        log.logText = logText;
        log.scrollRect = sr;
        gm.logManager = log;
        gm.dialogueManager = dialogue;
        gm.combatManager = combat;
        gm.inventoryManager = inventory;
        gm.aiCompanion = ai;
        gm.uiManager = ui;

        // Save as prefab ----------------------------------------------------
        const string dir = "Assets/Prefabs";
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        string path = Path.Combine(dir, "WhippyStartup.prefab");
        PrefabUtility.SaveAsPrefabAsset(root, path);
        GameObject.DestroyImmediate(root);

        Debug.Log("WhippyStartup prefab created at " + path);
    }
#endif
}
