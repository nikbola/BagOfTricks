using BagOfTricks.Core;
using BagOfTricks.Debug;
using BagOfTricks.Extensions;
using BagOfTricks.Storage;
using Game;
using SDK;
using System;
using UnityEngine;

namespace BagOfTricks.UI
{
    internal class WindowControls: MonoBehaviour
    {
        public static Action<bool> OnWindowStateChanged;

        private bool _showUI = false;
        public bool ShowUI { 
            get { return _showUI; } 
            set 
            { 
                _showUI = value;
                OnWindowStateChanged?.Invoke(value);
            } 
        }
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
                OnWindowStateChanged += HandleWindowStateChange;
            }
            catch (System.Exception e)
            {
                Debug.Logger.Write<Error>("Something went wrong when initializing UI styles.", exception: e);
            }
        }

        private void OnGUI()
        {
            if (!_showUI)
                return;

            GlobalWindowRect = GUILayout.Window(0, GlobalWindowRect, DrawWindow, "Bag of Tricks", UIStyles.WindowStyle);
        }

        private static void HandleWindowStateChange(bool showUI)
        {
            Debug.Logger.Write<Info>("Show UI: " + showUI);

            if (!showUI)
                return;

            Core.Stats.ClearPartyMembers();
            NonSerialized.partyMembers = Core.Stats.GetPartyMembers();
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.ExpandWidth(false);
            GUISkin skin = GUI.skin;
            skin.verticalScrollbarThumb = new GUIStyle();
            Texture2D texture = UI.GUIUtility.CreateColoredTexture(UIStyles.squareTexture, UIStyles.DarkPurple);
            skin.verticalScrollbarThumb.normal.background = texture;
            NonSerialized.ScrollPosition = GUILayout.BeginScrollView(NonSerialized.ScrollPosition);

            GUILayout.Space(40f);
            Templates.Header.Draw("Cheats", ref cheatsExpanded);
            DrawCheatSettings();

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Stats", ref statsExpanded);
            DrawStatsSettings();

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Movement", ref movementExpanded);

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Achievements", ref achievementsExpanded);

            GUILayout.EndScrollView();
            GUI.DragWindow();
        }


        #region Cheat Settings
        private static void DrawCheatSettings()
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

            Templates.TextField.Draw("Add Currency", ref NonSerialized.AddCurrencyAmount);

            Templates.Button.DrawRect("Add", onClick: () => 
            {
                Cheats.AddCurrency(int.Parse(NonSerialized.AddCurrencyAmount));
            });

            GUILayout.EndHorizontal();
        }
        #endregion

        #region Stats Settings
        private static void DrawStatsSettings()
        {
            if (!statsExpanded)
                return;

            GUILayout.Space(UIStyles.VerticalSpaceBetweenItems);

            string[] cNames = NonSerialized.partyMembers.GetNames();
            for (int i = 0; i < cNames.Length; i++)
            {
                Game.PartyMemberAI partyMember = NonSerialized.partyMembers[i];
                
                GUILayout.FlexibleSpace();
                GUIStyle nameLabelStyle = new(UIStyles.LabelStyle);
                nameLabelStyle.alignment = TextAnchor.MiddleCenter;
                nameLabelStyle.margin.left = (int)UIStyles.DefaultHeaderLabelWidth;
                nameLabelStyle.normal.background = UIStyles.squareTexture;

                GUILayout.Label(cNames[i], nameLabelStyle, GUILayout.Width(500f), GUILayout.Height(UIStyles.DefaultCategoryElementHeight));
                GUILayout.Label("Attributes:", UIStyles.LabelStyle, GUILayout.Width(250f), GUILayout.Height(UIStyles.DefaultCategoryElementHeight));

                foreach (Game.CharacterStats.AttributeScoreType type in Enum.GetValues(typeof(Game.CharacterStats.AttributeScoreType)))
                {
                    GUILayout.BeginHorizontal();
                    string LabelString = "";
                    switch (type)
                    {
                        case Game.CharacterStats.AttributeScoreType.Might:
                            LabelString = "Might ";
                            break;
                        case Game.CharacterStats.AttributeScoreType.Resolve:
                            LabelString = "Resolve ";
                            break;
                        case Game.CharacterStats.AttributeScoreType.Finesse:
                            LabelString = "Finesse ";
                            break;
                        case Game.CharacterStats.AttributeScoreType.Quickness:
                            LabelString = "Quickness ";
                            break;
                        case Game.CharacterStats.AttributeScoreType.Wits:
                            LabelString = "Wits ";
                            break;
                        case Game.CharacterStats.AttributeScoreType.Vitality:
                            LabelString = "Vitality ";
                            break;
                        case Game.CharacterStats.AttributeScoreType.Count:
                            LabelString = "Count ";
                            break;
                        default:
                            break;
                    }
                    
                    int statValue = Stats.GetBaseAttributeScore(type, partyMember);
                    GUILayout.Label(LabelString, UIStyles.LabelStyle, GUILayout.Width(100f), GUILayout.Height(UIStyles.DefaultCategoryElementHeight));

                    GUIStyle buttonStyle = new GUIStyle(UIStyles.ToggleStyle);
                    buttonStyle.alignment = TextAnchor.MiddleCenter;
                    buttonStyle.normal.textColor = Color.white;
                    if (GUILayout.Button("-", buttonStyle, GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        Stats.SetBaseAttributeScore(type, partyMember, statValue - 1);
                    }

                    var style = GUI.skin.GetStyle("Label");
                    style.alignment = TextAnchor.UpperCenter;

                    GUI.color = UIStyles.MainPurple;
                    GUILayout.Label(statValue.ToString() + " ", style, GUILayout.Width(45), GUILayout.Height(UIStyles.DefaultCategoryElementHeight));
                    GUI.color = Color.white;

                    //style.alignment = TextAnchor.UpperLeft;
                    if (GUILayout.Button("+", buttonStyle, GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        Stats.SetBaseAttributeScore(type, partyMember, statValue + 1);
                    }
                    GUILayout.EndHorizontal();
                }
            }
        }
        #endregion
    }
}
