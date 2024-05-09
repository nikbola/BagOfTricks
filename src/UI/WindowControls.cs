using BagOfTricks.Core;
using BagOfTricks.Debug;
using BagOfTricks.Extensions;
using BagOfTricks.Keybinds;
using BagOfTricks.Storage;
using BepInEx;
using Game;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace BagOfTricks.UI
{
    internal class WindowControls: MonoBehaviour
    {
        public static Action<bool> OnWindowStateChanged;

        private static bool _showUI = false;
        public static bool ShowUI { 
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

        private static int characterCount = 0;
        private static bool[] attrMenuExpanded = new bool[0];

        private static string searchString = string.Empty;

        private void Awake()
        {
            try
            {
                Debug.Logger.ValidatePaths();
                Styles.Initialize();
                OnWindowStateChanged += HandleWindowStateChange;
            }
            catch (System.Exception e)
            {
                Debug.Logger.Write<Error>("Something went wrong when initializing UI styles.", exception: e);
            }
        }

        public static void ToggleUI()
        {
            ShowUI = !ShowUI;
        }

        private void OnGUI()
        {
            KeybindHandler.ProcessKeybinds();

            if (!_showUI)
                return;

            GlobalWindowRect = GUILayout.Window(0, GlobalWindowRect, DrawWindow, "Bag of Tricks", Styles.GUIStyles.WindowStyle);
        }

        private static void HandleWindowStateChange(bool showUI)
        {
            Debug.Logger.Write<Info>("Show UI: " + showUI);

            UICamera uiCamera = FindObjectOfType<UICamera>();            
            if (uiCamera != null)
                uiCamera.useMouse = !showUI;

            GameInput gameInput = FindObjectOfType<GameInput>();
            if (gameInput != null)
                gameInput.enabled = !showUI;

            if (showUI)
            {
                Core.Stats.ClearPartyMembers();
                NonSerialized.s_PartyMembers = Core.Stats.GetPartyMembers();

                PartyMemberAI[] partyMembers = NonSerialized.s_PartyMembers;
                NonSerialized.s_Movers = Movement.GetMovers(partyMembers);

                characterCount = NonSerialized.s_PartyMembers?.Length ?? 0;
                attrMenuExpanded = new bool[characterCount];

                Achievements.Fetch();

                return;
            }

            Movement.ApplySettings();
        }

        private void DrawWindow(int windowID)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Main", Styles.GUIStyles.TopBarButtonStyle, GUILayout.Height(60)))
            {
                NonSerialized.s_SelectedTopBarCategory = NonSerialized.TopBarCategory.Main;
            }

            Rect mainButtonRect = default;
            Rect logsButtonRect = default;
            Rect keybindsButtonRect = default;

            if (Event.current.type == EventType.Repaint)
            {
                mainButtonRect = GUILayoutUtility.GetLastRect();
            }

            var separatorRect = new Rect(mainButtonRect.xMax, mainButtonRect.y, 2, mainButtonRect.height);

            if (GUILayout.Button("Logs", Styles.GUIStyles.TopBarButtonStyle, GUILayout.Height(60)))
            {
                NonSerialized.s_SelectedTopBarCategory = NonSerialized.TopBarCategory.Logs;
            }

            if (Event.current.type == EventType.repaint)
            {
                logsButtonRect = GUILayoutUtility.GetLastRect();
            }

            var separatorRect2 = new Rect(logsButtonRect.xMax, logsButtonRect.y, 2, logsButtonRect.height);

            if (GUILayout.Button("Keybinds", Styles.GUIStyles.TopBarButtonStyle, GUILayout.Height(60)))
            {
                NonSerialized.s_SelectedTopBarCategory = NonSerialized.TopBarCategory.Keybinds;
            }

            if (Event.current.type == EventType.repaint)
            {
                keybindsButtonRect = GUILayoutUtility.GetLastRect();
            }

            bool isMainSelected = NonSerialized.s_SelectedTopBarCategory == NonSerialized.TopBarCategory.Main;
            Rect selectedRect = isMainSelected ? mainButtonRect : logsButtonRect;

            if (NonSerialized.s_SelectedTopBarCategory == NonSerialized.TopBarCategory.Main)
                selectedRect = mainButtonRect;
            else if (NonSerialized.s_SelectedTopBarCategory == NonSerialized.TopBarCategory.Logs)
                selectedRect = logsButtonRect;
            else if (NonSerialized.s_SelectedTopBarCategory == NonSerialized.TopBarCategory.Keybinds)
                selectedRect = keybindsButtonRect;

            GUI.color = Styles.Colors.DarkestDark;
            GUI.DrawTexture(separatorRect, Texture2D.whiteTexture);
            GUI.DrawTexture(separatorRect2, Texture2D.whiteTexture);
            GUI.color = Color.white;

            var selectionRect = new Rect(selectedRect.x, selectedRect.yMax - 1, selectedRect.width, 2);
            GUI.color = Styles.Colors.MainPurple;
            GUI.DrawTexture(selectionRect, Texture2D.whiteTexture);
            GUI.color = Color.white;

            GUILayout.EndHorizontal();

            GUILayout.ExpandWidth(false);
            GUISkin skin = GUI.skin;
            skin.verticalScrollbarThumb = new GUIStyle();
            skin.verticalScrollbarThumb.normal.background = Styles.Textures.scrollThumbTexture;
            skin.verticalScrollbar.normal.background = Styles.Textures.scrollBackgroundTexture;
            NonSerialized.s_ScrollPosition = GUILayout.BeginScrollView(NonSerialized.s_ScrollPosition);

            if (NonSerialized.s_SelectedTopBarCategory == NonSerialized.TopBarCategory.Main)
                DrawMainUI();
            else if (NonSerialized.s_SelectedTopBarCategory == NonSerialized.TopBarCategory.Logs)
                DrawLogUI();
            else 
                DrawKeybindsUI();

            GUILayout.EndScrollView();
            GUI.DragWindow();
        }

        private static void DrawMainUI()
        {
            GUILayout.Space(40f);
            Templates.Header.Draw("Cheats", ref cheatsExpanded);
            DrawCheatSettings();

            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Stats", ref statsExpanded);
            DrawStatsSettings();

            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Movement", ref movementExpanded);
            DrawMovementSettings();

            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            Templates.Header.Draw("Achievements", ref achievementsExpanded);
            DrawAchievementSettings();
        }

        private void DrawLogUI()
        {
            List<LogEntry> logEntries = Debug.Logger.logHistory;
            GUI.skin.label.wordWrap = true;
            for (int i = 0; i < logEntries.Count; i++)
            {
                GUILayout.Space(20);
                var style = new GUIStyle(Styles.GUIStyles.LogStyle);
                style.normal.textColor = logEntries[i].logColor;
                GUILayout.Label(logEntries[i].logMessage, style, GUILayout.Width(960 - 20));
            }
        }

        private void DrawKeybindsUI()
        {
            var keybindActions = KeybindHandler.s_KeybindActions;
            foreach (var keybind in keybindActions)
            {
                GUILayout.Space(20);
                GUILayout.BeginHorizontal();

                var style = new GUIStyle(Styles.GUIStyles.LogStyle);
                bool isEditing = keybind.Value.actionName == KeybindHandler.EditingKeybindName;

                Color textColor;
                string keybindValue;
                string buttonText;

                if (isEditing)
                {
                    textColor = Styles.Colors.WarningYellow;
                    keybindValue = "Waiting for input...";
                    buttonText = "Waiting...";
                }
                else
                {
                    textColor = Color.white;
                    keybindValue = keybind.Key.ToString();
                    buttonText = "Change";
                }

                style.normal.textColor = textColor;
                GUILayout.Label(keybind.Value.actionName, style, GUILayout.Width((960 - 20) / 2));
                GUILayout.Label(keybindValue, style);

                Templates.Button.DrawRounded(buttonText, onClick: () => {
                    KeybindHandler.RegisterKeybindChange(keybind);
                });
                GUILayout.EndHorizontal();
            }
        }

        #region Cheat Settings
        private static void DrawCheatSettings()
        {
            if (!cheatsExpanded)
                return;

            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();

            Templates.Toggle.Draw("Block Telemetry (Requires Restart)", value: ref Serialized.BlockTelemetry, ref NonSerialized.DeltaBlockTelemetry, () => 
            {
                // TODO:
                // Implement this functionality. Try to make it so that the changes are applied during runtime
            });

            GUILayout.FlexibleSpace();
            Templates.Button.DrawRounded("Kill All Enemies", onClick: Cheats.KillAllEnemies);
            
            GUILayout.EndHorizontal();
            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();

            Templates.Toggle.Draw("Enable Godmode", ref Serialized.GodModeEnabled, ref NonSerialized.DeltaGodmodeEnabled, () => 
            {
                Cheats.ToggleGodMode(Serialized.GodModeEnabled);
            });

            GUILayout.FlexibleSpace();

            Templates.Button.DrawRounded("Clear Fog", onClick: Cheats.ClearFogOfWar);

            GUILayout.EndHorizontal();
            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();

            Templates.Toggle.Draw("Enable Invisibility", ref Serialized.InvisibilityEnabled, ref NonSerialized.DeltaInvisibilityEnabled, () => {
                Cheats.ToggleInvisibility(Serialized.InvisibilityEnabled);
            });

            GUILayout.EndHorizontal();
            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);
            GUILayout.BeginHorizontal();

            Templates.TextField.Draw("Add Currency", ref NonSerialized.s_AddCurrencyAmount);

            Templates.Button.DrawRect("Add", onClick: () => 
            {
                Cheats.AddCurrency(int.Parse(NonSerialized.s_AddCurrencyAmount));
            });

            GUILayout.EndHorizontal();
        }
        #endregion

        #region Stats Settings
        private static void DrawStatsSettings()
        {
            if (!statsExpanded)
                return;

            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);

            string[] cNames = NonSerialized.s_PartyMembers.GetNames();
            for (int i = 0; i < cNames.Length; i++)
            {
                PartyMemberAI partyMember = NonSerialized.s_PartyMembers[i];

                GUILayout.Space(15);

                GUILayout.FlexibleSpace();
                GUIStyle nameLabelStyle = new(Styles.GUIStyles.LabelStyle)
                {
                    alignment = TextAnchor.MiddleCenter
                };
                nameLabelStyle.margin.left = (int)Styles.Dimensions.DefaultHeaderLabelWidth;
                nameLabelStyle.normal.background = Styles.Textures.squareTexture;

                if (GUILayout.Button(cNames[i], nameLabelStyle, GUILayout.Width(500f), GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight)))
                {
                    attrMenuExpanded[i] = !attrMenuExpanded[i];
                }

                if (!attrMenuExpanded[i])
                {
                    if (i == cNames.Length - 1)
                        GUILayout.Space(15);
                    continue;
                }

                Rect pMemberBtn = default;
                if (Event.current.type == EventType.Repaint)
                {
                    pMemberBtn = GUILayoutUtility.GetLastRect();
                }

                Rect pMemberBtnSelection = new(pMemberBtn.x, pMemberBtn.yMax, pMemberBtn.width, 2);
                GUI.color = Styles.Colors.MainPurple;
                GUI.DrawTexture(pMemberBtnSelection, Texture2D.whiteTexture);
                GUI.color = Color.white;

                GUILayout.Space(10);

                GUILayout.Label("Attributes:", Styles.GUIStyles.LabelStyle, GUILayout.Width(250f), GUILayout.Height(Styles.Dimensions.DefaultCategoryElementHeight));

                foreach (Game.CharacterStats.AttributeScoreType type in Enum.GetValues(typeof(Game.CharacterStats.AttributeScoreType)))
                {
                    GUILayout.Space(10);

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
                    GUILayout.Label(LabelString, Styles.GUIStyles.LabelStyle, GUILayout.Width(100f), GUILayout.Height(25));

                    GUIStyle buttonStyle = new GUIStyle(Styles.GUIStyles.ToggleStyle);
                    buttonStyle.alignment = TextAnchor.MiddleCenter;
                    buttonStyle.normal.textColor = Color.white;
                    if (GUILayout.Button("-", buttonStyle, GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        Stats.SetBaseAttributeScore(type, partyMember, statValue - 1);
                    }

                    var style = GUI.skin.GetStyle("Label");
                    style.alignment = TextAnchor.UpperCenter;

                    GUI.color = Styles.Colors.MainPurple;
                    GUILayout.Label(statValue.ToString() + " ", style, GUILayout.Width(45), GUILayout.Height(25));
                    GUI.color = Color.white;

                    if (GUILayout.Button("+", buttonStyle, GUILayout.Width(25), GUILayout.Height(25)))
                    {
                        Stats.SetBaseAttributeScore(type, partyMember, statValue + 1);
                    }
                    GUILayout.EndHorizontal();
                }

                GUILayout.Space(10);
            }
        }
        #endregion

        public static void DrawMovementSettings()
        {
            if (!movementExpanded)
                return;

            GUIStyle textFieldStyle = new(Styles.GUIStyles.ToggleStyle);
            textFieldStyle.normal.textColor = Styles.Colors.MainPurple;
            textFieldStyle.alignment = TextAnchor.MiddleCenter;
            textFieldStyle.fontStyle = FontStyle.Bold;
            textFieldStyle.normal.background = Styles.Textures.achievementRowEven;
            textFieldStyle.margin.right = 10;

            GUILayout.Space(Styles.Dimensions.VerticalSpaceBetweenItems);

            DrawMovementSlider("Run Speed:", textFieldStyle, ref Serialized.RunSpeed, ref NonSerialized.DefaultRunSpeed);

            GUILayout.Space(8f);

            DrawMovementSlider("Walk Speed:", textFieldStyle, ref Serialized.WalkSpeed, ref NonSerialized.DefaultWalkSpeed);

            GUILayout.Space(8f);

            DrawMovementSlider("Stealth Speed:", textFieldStyle, ref Serialized.StealthSpeed, ref NonSerialized.DefaultStealthSpeed);
        }

        private static void DrawMovementSlider(string label, GUIStyle style, ref float value, ref float defaultValue)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(100f));
            string InputFieldText = value.ToString("0.0#", CultureInfo.InvariantCulture);
            InputFieldText = GUILayout.TextField(InputFieldText, 10, style, GUILayout.Height(28f), GUILayout.Width(75f));

            // Must cache because ref values cannot be used in lambda methods
            float cachedValue = value;
            float cachedDefaultValue = defaultValue;
            Templates.Button.DrawRounded("Reset", onClick: () =>
            {
                cachedValue = cachedDefaultValue;
                InputFieldText = cachedDefaultValue.ToString("0.0#", CultureInfo.InvariantCulture);
            }, scaleFactor: 0.7f);

            value = cachedValue;
            cachedDefaultValue = defaultValue;

            if (!float.TryParse(InputFieldText, out value))
            {
                value = defaultValue;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            value = GUILayout.HorizontalSlider(value, 0.0f, 25.0f);

            GUILayout.Space(8f);
            GUILayout.BeginHorizontal();

            var defaultAlignment = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
            GUILayout.Label(NonSerialized.MinMovementSpeed.ToString());
            GUI.skin.label.alignment = TextAnchor.UpperRight;
            GUILayout.Label(NonSerialized.MaxMovementSpeed.ToString());
            GUI.skin.label.alignment = defaultAlignment;
            GUILayout.EndHorizontal();
        }

        private static void DrawAchievementSettings()
        {
            if (!achievementsExpanded) return;

            if (AchievementTracker.Instance == null)
            {
                GUILayout.Label("AchievementTracker not instantiated. Load a save to access achievements.", Styles.GUIStyles.LabelStyle);
                return;
            }
            GUILayout.Space(5f);
            GUILayout.BeginVertical();

            GUIStyle searchbarStyle = Styles.GUIStyles.TextFieldStyle;
            searchbarStyle.alignment = TextAnchor.MiddleLeft;
            Color backgroundColor = Styles.Colors.LighterDark;
            Texture2D texture = new(1, 1);
            texture.SetPixel(0, 0, backgroundColor);
            texture.Apply();
            searchbarStyle.normal.background = texture;
            var width = GUILayout.Width(400f);
            var height = GUILayout.Height(40);

            GUILayout.Space(6f);

            searchString = GUILayout.TextField(searchString, searchbarStyle, width, height);

            Rect searchbarRect = default;
            if (Event.current.type == EventType.Repaint)
            {
                searchbarRect = GUILayoutUtility.GetLastRect();
            }

            float offsetY = searchbarRect.height / 4f;
            var searchIconRect = new Rect(searchbarRect.xMax - 30, searchbarRect.y + offsetY, 20, 20);
            GUI.DrawTexture(searchIconRect, Styles.Textures.magnifyingGlass);

            for (int i = 0; i < NonSerialized.s_AchievementInfo.Count; i++)
            {
                Tuple<string, string, string> achievement = NonSerialized.s_AchievementInfo[i];
                string name = achievement.First;
                string descr = achievement.Second;
                string APIName = achievement.Third;

                if (searchString.IsNullOrWhiteSpace())
                {
                    DrawAchievementRow(name, descr, APIName, i);
                    continue;
                }
                else
                {
                    bool wasInName = false;
                    bool wasInDescr = false;
                    if (name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                        wasInName = true;
                    if (descr.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) 
                        wasInDescr = true;

                    if (!wasInName && !wasInDescr)
                        continue;

                    if (wasInName)
                        name = Achievements.HighlightSearch(name, searchString);
                    if (wasInDescr) 
                        descr = Achievements.HighlightSearch(descr, searchString);

                    DrawAchievementRow(name, descr, APIName, i);
                }
            }

            GUILayout.EndVertical();
        }

        private static void DrawAchievementRow(string name, string descr, string APIName, int index)
        {
            GUILayout.Space(6f);

            GUIStyle style = new();
            Texture2D backgroundTexture = index % 2 == 0 ? Styles.Textures.achievementRowEven : Styles.Textures.achievementRowOdd;
            style.normal.background = backgroundTexture;

            GUILayout.BeginHorizontal(style);

            GUIStyle labelStyle = new(Styles.GUIStyles.LabelStyle);
            labelStyle.margin.left = Styles.Dimensions.HeaderVerticalMargin;
            GUILayout.Label(name + " - " + descr, labelStyle, GUILayout.Height(40));

            var buttonHeight = 40 * 0.7f;
            var buttonYOffset = (40 - buttonHeight) / 2;

            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical();
            GUILayout.Space(buttonYOffset);
            Templates.Button.DrawRounded("Unlock", onClick: () =>
            {
                AchievementTracker.Instance.SetAchievement(APIName);
            }, scaleFactor: 0.7f);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }
}
