using BagOfTricks.Debug;
using BagOfTricks.Meta;
using BagOfTricks.UI;
using BepInEx;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BagOfTricks.Keybinds
{
    internal class KeybindHandler
    {
        public static Keybinds s_Keybinds;

        public static Dictionary<KeyCode, Action> s_KeybindActions;

        public static readonly string s_KeybindsPath = Path.Combine(Paths.PluginPath, RelPaths.KEYBINDS_FILE);

        private static List<KeyCode> s_PressedKeys = new List<KeyCode>();

        public static void Initialize()
        {
            if (!File.Exists(s_KeybindsPath))
            {
                s_Keybinds = new Keybinds();
                
                string dirPath = Path.GetDirectoryName(s_KeybindsPath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                File.Create(s_KeybindsPath).Close();
                var sw = new StreamWriter(s_KeybindsPath);
                var fileContent = JsonUtility.ToJson(s_Keybinds, prettyPrint: true); 
                sw.Write(fileContent);
                sw.Close();

                PopulateKeybindActions();

                return;
            }

            using StreamReader sr = new(s_KeybindsPath);
            string jsonString = sr.ReadToEnd();
            s_Keybinds = JsonUtility.FromJson<Keybinds>(jsonString);
            sr.Close();

            PopulateKeybindActions();
        }

        public static void ProcessKeybinds()
        {
            Event e = Event.current;
            if (!e.isKey)
                return;

            if (!s_KeybindActions.TryGetValue(e.keyCode, out var action))
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
                action?.Invoke();
            }
        } 

        private static void PopulateKeybindActions()
        {
            Debug.Logger.Write<Info>(s_Keybinds.toggleMenu);
            s_KeybindActions = new()
            {
                { s_Keybinds.toggleMenu, WindowControls.ToggleUI }
            };
        }
    }
}
