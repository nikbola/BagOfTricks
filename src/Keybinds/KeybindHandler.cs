using BagOfTricks.Debug;
using BagOfTricks.Extensions;
using BagOfTricks.Meta;
using BagOfTricks.UI;
using BepInEx;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace BagOfTricks.Keybinds
{
    internal static class KeybindHandler
    {
        public static Dictionary<KeyCode, KeybindAction> s_KeybindActions;

        public static readonly string s_keybindsPath = Path.Combine(Paths.PluginPath, RelPaths.KEYBINDS_FILE);

        private static string s_editingKeybindName = string.Empty;

        private static readonly List<KeyCode> s_pressedKeys = new List<KeyCode>();

        private static WindowControls s_window;

        private static Coroutine s_keybindCoroutine = null;

        public static void Initialize(WindowControls window)
        {
            s_window = window;
            if (!File.Exists(s_keybindsPath))
            {
                PopulateKeybindActionsWithDefaultValues();
                AddActions();
                SaveConfigToFile();

                return;
            }

            using StreamReader sr = new(s_keybindsPath);
            string jsonString = sr.ReadToEnd();
            s_KeybindActions = JsonConvert.DeserializeObject<Dictionary<KeyCode, KeybindAction>>(jsonString);
            sr.Close();

            if (s_KeybindActions is null || s_KeybindActions.Count == 0)
                PopulateKeybindActionsWithDefaultValues();

            AddActions();
        }

        private static void SaveConfigToFile()
        {
            string dirPath = Path.GetDirectoryName(s_keybindsPath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.Create(s_keybindsPath).Close();
            string fileContent = JsonConvert.SerializeObject(s_KeybindActions, Formatting.Indented);
            var sw = new StreamWriter(s_keybindsPath);
            sw.Write(fileContent);
            sw.Close();
        }

        public static void RegisterKeybindChange(KeyValuePair<KeyCode, KeybindAction> keybind)
        {
            if (s_keybindCoroutine != null)
                s_window.StopCoroutine(s_keybindCoroutine);

            s_keybindCoroutine = s_window.StartCoroutine(AwaitInput(keybind));
        }

        public static void ProcessKeybinds()
        {
            if (s_keybindCoroutine != null)
                return;

            Event e = Event.current;
            if (!e.isKey)
                return;

            if (!s_KeybindActions.TryGetValue(e.keyCode, out var keybind))
                return;

            if (s_pressedKeys.Contains(e.keyCode))
            {
                if (e.type == EventType.keyUp)
                {
                    s_pressedKeys.Remove(e.keyCode);
                }
            }
            else if (e.type == EventType.KeyDown)
            {
                s_pressedKeys.Add(e.keyCode);
                keybind.action?.Invoke();
            }
        }

        private static void PopulateKeybindActionsWithDefaultValues()
        {
            string toggleMenu = KeybindNames.s_toggleMenu;
            string killAllEnemies = KeybindNames.s_killAllEnemies;
            string toggleGodmode = KeybindNames.s_toggleGodmode;
            string clearFog = KeybindNames.s_clearFog;
            string toggleInvisibility = KeybindNames.s_toggleInvisibility;
            string addCurrency = KeybindNames.s_addCurrency;

            s_KeybindActions = new()
            {
                { KeyCode.Keypad0, new(KeyCode.Keypad0, toggleMenu, s_actionLookup[toggleMenu]) },
                { KeyCode.Keypad1, new(KeyCode.Keypad1, killAllEnemies, s_actionLookup[killAllEnemies]) },
                { KeyCode.Keypad2, new(KeyCode.Keypad2, toggleGodmode, s_actionLookup[toggleGodmode]) },
                { KeyCode.Keypad3, new(KeyCode.Keypad3, clearFog, s_actionLookup[clearFog]) },
                { KeyCode.Keypad4, new(KeyCode.Keypad4, toggleInvisibility, s_actionLookup[toggleInvisibility]) },
                { KeyCode.Keypad4, new(KeyCode.Keypad4, addCurrency, s_actionLookup[addCurrency]) },
            };
        }

        private static void AddActions()
        {
            foreach (var action in s_actionLookup)
            {
                var pair = s_KeybindActions.FirstOrDefault(x => x.Value.actionName == action.Key);
                KeybindAction keybindAction = s_KeybindActions[pair.Key];
                keybindAction.action = action.Value;
                s_KeybindActions[pair.Key] = keybindAction;
            }
        }

        private static IEnumerator AwaitInput(KeyValuePair<KeyCode, KeybindAction> keybind)
        {
            s_editingKeybindName = keybind.Value.actionName;
            yield return new WaitUntil(() => Event.current.type == EventType.keyUp && Event.current.keyCode != KeyCode.None);
            s_editingKeybindName = string.Empty;

            try
            {
                KeyCode newBind = Event.current.keyCode;
                if (s_KeybindActions.TryGetValue(newBind, out var collision))
                {
                    string collisionName = collision.actionName;
                    Debug.Logger.Write<Warning>($"The given key \"{newBind}\" is already bound to action \"{collisionName}\"");
                    s_keybindCoroutine = null;
                    yield break;
                }

                if (s_KeybindActions.UpdateKey(keybind.Key, newBind))
                    Debug.Logger.Write<Success>($"Successfully updated keybind for action {keybind.Value.actionName} to {newBind}");

                var keybindAction = s_KeybindActions[newBind];
                keybindAction.keybind = newBind;
                s_KeybindActions[newBind] = keybindAction;

                SaveConfigToFile();

                s_keybindCoroutine = null;
            }
            catch (Exception e)
            {
                Debug.Logger.Write<Error>("Something went wrong when updating keybinds.", exception: e);
                s_keybindCoroutine = null;
                throw e;
            }

        }

        public struct KeybindAction
        {
            public KeyCode keybind;
            public string actionName;

            [NonSerialized] public Action action;

            public KeybindAction(KeyCode keybind, string actionName, Action action)
            {
                this.keybind = keybind;
                this.actionName = actionName;
                this.action = action;
            }
        }

        public static Dictionary<string, Action> s_actionLookup = new()
        {
            { KeybindNames.s_toggleMenu, WindowControls.ToggleUI },
            { KeybindNames.s_killAllEnemies, Core.Cheats.KillAllEnemies },
            { KeybindNames.s_toggleGodmode, Core.Cheats.ToggleGodMode },
            { KeybindNames.s_clearFog, Core.Cheats.ClearFogOfWar },
            { KeybindNames.s_toggleInvisibility, Core.Cheats.ToggleInvisibility },
            { KeybindNames.s_addCurrency, () => { Core.Cheats.AddCurrency(500); } }
        };

        public static string EditingKeybindName => s_editingKeybindName;
    }
}
