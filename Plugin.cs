using BagOfTricks.Debug;
using BagOfTricks.Meta;
using BagOfTricks.UI;
using BepInEx;
using System;
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

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Logger.Write<Info>($"Plugin {ProjectInfo.PLUGIN_NAME} is loaded!");
        }

        private void Update()
        {
            for (int i = 0; i < UnhandledLogs.Count; i++)
            {
                string message = UnhandledLogs.Dequeue();
                Logger.LogInfo($"Internal log: {message}");
            }

            if (!Input.GetKeyDown(KeyCode.KeypadPlus))
                return;

            windowControls.ShowUI = !windowControls.ShowUI;
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
