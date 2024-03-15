using BagOfTricks.Core;
using BagOfTricks.Debug;
using BagOfTricks.Storage;
using System;
using System.Reflection.Emit;
using UnityEngine;

namespace BagOfTricks.UI
{
    internal class WindowControls: MonoBehaviour
    {
        public bool showUI = false;
        public Rect GlobalWindowRect { get; private set; } = new Rect(0f, 0f, 960f, 540f);

        private static bool cheatsExpanded = false;
        private static bool statsExpanded = false;
        private static bool movementExpanded = false;
        private static bool achievementsExpanded = false;

        private void Awake()
        {
            try
            {
                Debug.Logger.ValidatePaths();
                UIStyles.Initialize();
            }
            catch (System.Exception e)
            {
                Debug.Logger.Write<Error>("Something went wrong when initializing UI styles.", exception: e);
            }
        }

        private void OnGUI()
        {
            if (!showUI)
                return;

            GlobalWindowRect = GUILayout.Window(0, GlobalWindowRect, DrawWindow, "Bag of Tricks", UIStyles.WindowStyle);
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.Space(40f);
            Templates.Header.Draw("Cheats", ref cheatsExpanded);
            DrawCheatSettings();

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Stats", ref statsExpanded);

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Movement", ref movementExpanded);

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Achievements", ref achievementsExpanded);

            GUI.DragWindow();
        }


        #region Cheat Settings
        public static void DrawCheatSettings()
        {
            if (!cheatsExpanded)
                return;
            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);

            GUILayout.BeginHorizontal();

            Templates.Toggle.Draw("Block Telemetry (Requires Restart)", value: ref Serialized.BlockTelemetry);

            GUILayout.FlexibleSpace();
            Templates.Button.DrawRounded("Kill All Enemies", onClick: Cheats.KillAllEnemies);
            
            GUILayout.EndHorizontal();

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();

            Templates.Toggle.Draw("Enable Godmode", value: ref Serialized.GodModeEnabled);

            GUILayout.FlexibleSpace();

            Templates.Button.DrawRounded("Clear Fog", onClick: Cheats.ClearFogOfWar);

            GUILayout.EndHorizontal();

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();

            Templates.Toggle.Draw("Enable Invisibility", value: ref Serialized.InvisibilityEnabled);

            GUILayout.EndHorizontal();

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();
            
            GUILayout.Label(
                text: "Add Currency",
                style: UIStyles.LabelStyle,
                options: new GUILayoutOption[] {
                    GUILayout.Width(300f),
                    GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
                }
            );

            NonSerialized.AddCurrencyAmount = GUILayout.TextField(
                NonSerialized.AddCurrencyAmount, 
                UIStyles.TextFieldStyle, 
                GUILayout.Width(UIStyles.DefaultTextFieldWidth), 
                GUILayout.Height(UIStyles.DefaultCategoryElementHeight)
            );

            if (Event.current.type == EventType.Repaint)
            {
                Rect textFieldRect = GUILayoutUtility.GetLastRect();
                Rect lineRect = new Rect(textFieldRect.x, textFieldRect.yMax + 2, textFieldRect.width, 2);
                
                GUI.color = UIStyles.MainPurple;
                GUI.DrawTexture(lineRect, Texture2D.whiteTexture);
                GUI.color = Color.white;
            }

            Templates.Button.DrawRect("Add", onClick: () => 
            {
                Cheats.AddCurrency(int.Parse(NonSerialized.AddCurrencyAmount));
            });

            GUILayout.EndHorizontal();
        }
        #endregion
    }
}
