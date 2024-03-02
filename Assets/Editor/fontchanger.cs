using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class fontchanger : EditorWindow
{
    Font selectedFont;

    [MenuItem("GameUtility/Font Changer")]
    private static void ShowWindow()
    {
        fontchanger w = GetWindow<fontchanger>(false, "UI Font Changer", true);
        w.minSize = new Vector2(200, 110);
        w.maxSize = new Vector2(200, 110);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Font: ", selectedFont?.name);

        SelectFontButton();
        OnSelectorClosed();

        ResetFontButton();
        ChangeAllFontsButton();
    }

    private void SelectFontButton()
    {
        EditorGUILayout.Space();
        if (GUILayout.Button("Select Font"))
        {
            EditorGUIUtility.ShowObjectPicker<Font>(selectedFont, true, "", GUIUtility.GetControlID(FocusType.Passive) + 100);
        }
    }

    private void OnSelectorClosed()
    {
        if (Event.current.commandName == "ObjectSelectorClosed")
        {
            if (EditorGUIUtility.GetObjectPickerObject() != null)
            {
                selectedFont = (Font)EditorGUIUtility.GetObjectPickerObject();
            }
        }
    }

    private void ResetFontButton()
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Reset Font"))
        {
            selectedFont = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        }
    }

    private void ChangeAllFontsButton()
    {
        EditorGUILayout.Space();

        if (GUILayout.Button("Change All Fonts In Scene"))
        {
            ChangeAllFonts();
            SceneView.lastActiveSceneView.Repaint();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        }
    }

    public void ChangeAllFonts()
    {
        var textCount = 0;
        var fontChangedCount = 0;

        var allTextObjects = Resources.FindObjectsOfTypeAll(typeof(Text));

        foreach (Text t in allTextObjects)
        {
            textCount++;

            if (t.font != selectedFont)
            {
                Debug.Log(t.name);
                fontChangedCount++;
                t.font = selectedFont;
            }
        }
        Debug.Log(string.Format("ã�� �ؽ�Ʈ UI {0}�� �� {1}�� ����", textCount, fontChangedCount));
    }
}