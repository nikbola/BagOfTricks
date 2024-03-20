using BagOfTricks.Debug;
using BagOfTricks.Keybinds;
using BagOfTricks.Meta;
using BagOfTricks.UI;
using BepInEx;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BagOfTricks
{
    [BepInPlugin(ProjectInfo.PLUGIN_GUID, ProjectInfo.PLUGIN_NAME, ProjectInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        WindowControls windowControls;

        public static Queue<string> UnhandledLogs = new Queue<string>();

        Harmony harmonyPatcher = new Harmony(ProjectInfo.PLUGIN_GUID);

        private void Awake()
        {
            try
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                harmonyPatcher.PatchAll();
                KeybindHandler.Initialize();

                Debug.Logger.Write<Success>($"Successfully loaded mod!");
            }
            catch (System.Exception e)
            {
                Debug.Logger.Write<Error>(message: "Something went wrong when loading the mod!", exception: e);
            }
        }

        private void Update()
        {
            for (int i = 0; i < UnhandledLogs.Count; i++)
            {
                string message = UnhandledLogs.Dequeue();
                Logger.LogInfo($"Internal log: {message}");
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Logger.Write<Info>($"Scene loaded: {scene.name}");

            if (scene.name != "MainMenu")
                return;

            var controllerObject = new GameObject("[Bag of Tricks] GUIController", typeof(WindowControls));
            windowControls = controllerObject.GetComponent<WindowControls>();
            DontDestroyOnLoad(controllerObject);
        }
    }
}
