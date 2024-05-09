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

        public static readonly string s_KeybindsPath = Path.Combine(Paths.PluginPath, RelPaths.KEYBINDS_FILE);

        private static readonly List<KeyCode> s_PressedKeys = new List<KeyCode>();

        private static WindowControls s_window;

        private static Coroutine s_keybindCoroutine = null;

        public static void Initialize(WindowControls window)
        {
            s_window = window;
            if (!File.Exists(s_KeybindsPath))
            {
                PopulateKeybindActionsWithDefaultValues();
                AddActions();
                SaveConfigToFile();

                return;
            }

            using StreamReader sr = new(s_KeybindsPath);
            string jsonString = sr.ReadToEnd();
            s_KeybindActions = JsonConvert.DeserializeObject<Dictionary<KeyCode, KeybindAction>>(jsonString);
            sr.Close();

            if (s_KeybindActions is null || s_KeybindActions.Count == 0)
                PopulateKeybindActionsWithDefaultValues();

            AddActions();
        }

        private static void SaveConfigToFile()
        {
            string dirPath = Path.GetDirectoryName(s_KeybindsPath);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            File.Create(s_KeybindsPath).Close();
            string fileContent = JsonConvert.SerializeObject(s_KeybindActions, Formatting.Indented);
            var sw = new StreamWriter(s_KeybindsPath);
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

            if (s_PressedKeys.Contains(e.keyCode))
            {
                if (e.type == EventType.keyUp)
                {
                    s_PressedKeys.Remove(e.keyCode);
                }
            }
            else if (e.type == EventType.KeyDown)
            {
                s_PressedKeys.Add(e.keyCode);
                keybind.action?.Invoke();
            }
        }

        private static void PopulateKeybindActionsWithDefaultValues()
        {
            s_KeybindActions = new()
            {
                { KeyCode.KeypadPlus, new(KeyCode.KeypadPlus, "Toggle Menu", s_actionLookup["Toggle Menu"]) }
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
            yield return new WaitUntil(() => Event.current.type == EventType.keyUp && Event.current.keyCode != KeyCode.None);

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
            { "Toggle Menu", WindowControls.ToggleUI }
        };
    }
}
