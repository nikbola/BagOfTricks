using BagOfTricks.Storage;
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

                GUILayout.Label(label, UIStyles.HeaderLabelStyle, GUILayout.Width(200f), GUILayout.Height(UIStyles.HeaderHeight));

                string buttonText;
                GUIStyle style;
                if (isExpanded)
                {
                    buttonText = "Collapse";
                    style = UIStyles.ButtonStyleSelected;
                }
                else
                {
                    buttonText = "Expand";
                    style = UIStyles.ButtonStyle;
                }
                style.margin.right = UIStyles.HeaderVerticalMargin;

                Rect buttonRect = default;
                if (GUILayout.Button(buttonText, style, GUILayout.Height(UIStyles.HeaderHeight)))
                {
                    isExpanded = !isExpanded;
                }

                if (Event.current.type == EventType.Repaint)
                {
                    buttonRect = GUILayoutUtility.GetLastRect();
                }

                GUILayout.EndHorizontal();

                GUI.color = UIStyles.MainPurple;
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
                    style: UIStyles.LabelStyle,
                    options: new GUILayoutOption[] {
                        GUILayout.Width(300f),
                        GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                    }
                );

                GUIStyle toggleStyle = value ? UIStyles.ToggleStyleEnabled : UIStyles.ToggleStyle;

                value = GUILayout.Toggle(
                    value: value,
                    text: value ? "✓" : string.Empty,
                    style: toggleStyle,
                    options: new GUILayoutOption[] {
                        GUILayout.Width(40f),
                        GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                    }
                );
            }
        }

        internal static class Button
        {
            internal static void DrawRounded(string text, Action onClick)
            {
                var buttonStyleWithMargin = new GUIStyle(
                    UIStyles.RoundedCategoryButtonStyle
                );

                buttonStyleWithMargin.margin.right = UIStyles.HeaderVerticalMargin;

                float width = UIStyles.DefaultRoundedButtonWidth;

                var buttonRect = GUILayoutUtility.GetRect(
                    new GUIContent(text), 
                    buttonStyleWithMargin, 
                    GUILayout.Width(width), 
                    GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                );
                Rect outlineRect = new Rect(buttonRect.x - 2, buttonRect.y - 2, buttonRect.width + 4, buttonRect.height + 4);

                GUI.color = UIStyles.MainPurple;
                GUI.DrawTexture(outlineRect, UIStyles.buttonTexture);
                GUI.color = Color.white;

                if (GUI.Button(buttonRect, text, buttonStyleWithMargin))
                {
                    onClick?.Invoke();
                }
            }

            internal static void DrawRect(string text, Action onClick)
            {
                var buttonStyleWithMargin = new GUIStyle(
                    UIStyles.RectCategoryButtonStyle
                );

                buttonStyleWithMargin.margin.right = UIStyles.HeaderVerticalMargin;

                float width = UIStyles.DefaultRectButtonWidth;
                var buttonRect = GUILayoutUtility.GetRect(
                    new GUIContent(text),
                    buttonStyleWithMargin,
                    GUILayout.Width(width),
                    GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                );
                Rect outlineRect = new Rect(buttonRect.x - 2, buttonRect.y - 2, buttonRect.width + 4, buttonRect.height + 4);

                GUI.color = UIStyles.MainPurple;
                GUI.DrawTexture(outlineRect, UIStyles.rectButtonTexture);
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
                    style: UIStyles.LabelStyle,
                    options: new GUILayoutOption[] {
                        GUILayout.Width(300f),
                        GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                    }
                );

                value = GUILayout.TextField(
                    value,
                    UIStyles.TextFieldStyle,
                    GUILayout.Width(UIStyles.DefaultTextFieldWidth),
                    GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                );

                if (Event.current.type == EventType.Repaint)
                {
                    Rect textFieldRect = GUILayoutUtility.GetLastRect();
                    Rect lineRect = new(textFieldRect.x, textFieldRect.yMax + 2, textFieldRect.width, 2);

                    GUI.color = UIStyles.MainPurple;
                    GUI.DrawTexture(lineRect, Texture2D.whiteTexture);
                    GUI.color = Color.white;
                }
            }
        }
    }
}
