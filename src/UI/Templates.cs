using System;
using UnityEngine;

namespace BagOfTricks.UI
{
    internal static class Templates
    {
        internal static class Header
        {
            internal static void Draw(string label, ref bool isExpanded)
            {
                GUILayout.BeginHorizontal();

                GUILayout.Label(label, Styles.GUIStyles.HeaderLabelStyle, GUILayout.Width(200f), GUILayout.Height(Styles.Dimensions.HeaderHeight));

                string buttonText;
                GUIStyle style;
                if (isExpanded)
                {
                    buttonText = "Collapse";
                    style = Styles.GUIStyles.ButtonStyleSelected;
                }
                else
                {
                    buttonText = "Expand";
                    style = Styles.GUIStyles.ButtonStyle;
                }
                style.margin.right = Styles.Dimensions.HeaderVerticalMargin;

                Rect buttonRect = default;
                if (GUILayout.Button(buttonText, style, GUILayout.Height(Styles.Dimensions.HeaderHeight)))
                {
                    isExpanded = !isExpanded;
                }

                if (Event.current.type == EventType.Repaint)
                {
                    buttonRect = GUILayoutUtility.GetLastRect();
                }

                GUILayout.EndHorizontal();

                GUI.color = Styles.Colors.MainPurple;
                Rect separatorRect = new Rect(buttonRect.x, buttonRect.yMax, buttonRect.width, 2);
                GUI.DrawTexture(separatorRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }
        }

        internal static class Toggle
        {
            internal static void Draw(string label, ref bool value)
            {
                GUILayout.Label(
                    text: label,
                    style: Styles.GUIStyles.LabelStyle,
                    options: new GUILayoutOption[] {
                        GUILayout.Width(300f),
                        GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight)
                    }
                );

                GUIStyle toggleStyle = value ? Styles.GUIStyles.ToggleStyleEnabled : Styles.GUIStyles.ToggleStyle;

                value = GUILayout.Toggle(
                    value: value,
                    text: value ? "✓" : string.Empty,
                    style: toggleStyle,
                    options: new GUILayoutOption[] {
                        GUILayout.Width(40f),
                        GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight)
                    }
                );
            }
        }

        internal static class Button
        {
            internal static void DrawRounded(string text, Action onClick, float scaleFactor = 1f)
            {
                var buttonStyleWithMargin = new GUIStyle(
                    Styles.GUIStyles.RoundedCategoryButtonStyle
                );

                buttonStyleWithMargin.margin.right = Styles.Dimensions.HeaderVerticalMargin;

                var buttonRect = GUILayoutUtility.GetRect(
                    new GUIContent(text), 
                    buttonStyleWithMargin, 
                    GUILayout.Width(Styles.Dimensions.DefaultRoundedButtonWidth * scaleFactor), 
                    GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight * scaleFactor)
                );
                var outlineRect = new Rect(buttonRect.x - 2, buttonRect.y - 2, buttonRect.width + 4, buttonRect.height + 4);

                GUI.color = Styles.Colors.MainPurple;
                GUI.DrawTexture(outlineRect, Styles.Textures.buttonTexture);
                GUI.color = Color.white;

                if (GUI.Button(buttonRect, text, buttonStyleWithMargin))
                {
                    onClick?.Invoke();
                }
            }

            internal static void DrawRect(string text, Action onClick)
            {
                var buttonStyleWithMargin = new GUIStyle(
                    Styles.GUIStyles.RectCategoryButtonStyle
                );

                buttonStyleWithMargin.margin.right = Styles.Dimensions.HeaderVerticalMargin;

                float width = Styles.Dimensions.DefaultRectButtonWidth;
                var buttonRect = GUILayoutUtility.GetRect(
                    new GUIContent(text),
                    buttonStyleWithMargin,
                    GUILayout.Width(width),
                    GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight)
                );
                Rect outlineRect = new Rect(buttonRect.x - 2, buttonRect.y - 2, buttonRect.width + 4, buttonRect.height + 4);

                GUI.color = Styles.Colors.MainPurple;
                GUI.DrawTexture(outlineRect, Styles.Textures.rectButtonTexture);
                GUI.color = Color.white;

                if (GUI.Button(buttonRect, text, buttonStyleWithMargin))
                {
                    onClick?.Invoke();
                }
            }
        }

        internal static class TextField
        {
            internal static void Draw(string label, ref string value)
            {
                GUILayout.Label(
                    text: label,
                    style: Styles.GUIStyles.LabelStyle,
                    options: new GUILayoutOption[] {
                        GUILayout.Width(300f),
                        GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight)
                    }
                );

                value = GUILayout.TextField(
                    value,
                    Styles.GUIStyles.TextFieldStyle,
                    GUILayout.Width(Styles.Dimensions.DefaultTextFieldWidth),
                    GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight)
                );

                if (Event.current.type == EventType.Repaint)
                {
                    Rect textFieldRect = GUILayoutUtility.GetLastRect();
                    Rect lineRect = new(textFieldRect.x, textFieldRect.yMax + 2, textFieldRect.width, 2);

                    GUI.color = Styles.Colors.MainPurple;
                    GUI.DrawTexture(lineRect, Texture2D.whiteTexture);
                    GUI.color = Color.white;
                }
            }
        }
    }
}
