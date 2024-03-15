using BepInEx;
using UnityEngine;
using System.IO;

namespace BagOfTricks
{
    public static class UIStyles
    {
        #region Colors
        public static readonly Color MainDark = new(0.13f, 0.13f, 0.13f);
        public static readonly Color LighterDark = new(0.18f, 0.18f, 0.18f);
        public static readonly Color DarkestDark = new(0.07f, 0.07f, 0.07f);
        public static readonly Color Gray = new(0.22f, 0.22f, 0.22f);
        public static readonly Color MainPurple = new(0.77f, 0.58f, 1f);
        public static readonly Color DarkPurple = new(0.2f, 0.18f, 0.23f);
        public static readonly Color Turquoise = new(0.08f, 0.86f, 0.78f);
        public static readonly Color ButtonPurpleHighlight = new(0.20f, 0.20f, 0.28f);
        public static readonly Color ButtonPurpleSelected = new(0.16f, 0.16f, 0.24f);
        #endregion

        #region GUIStyles
        private static GUIStyle _windowStyle;
        private static GUIStyle _headerLabelStyle;
        private static GUIStyle _labelStyle;
        private static GUIStyle _buttonStyle;
        private static GUIStyle _buttonStyleSelected;
        private static GUIStyle _roundedCategoryButtonStyle;
        private static GUIStyle _rectCategoryButtonStyle;
        private static GUIStyle _toggleStyle;
        private static GUIStyle _toggleStyleEnabled;
        private static GUIStyle _textFieldStyle;
        #endregion

        #region Dimensions
        public static readonly float HeaderHeight = 40f;
        public static readonly float VerticalSpaceBetweenItems = 15f;
        public static readonly float DefaultHeaderLabelWidth = 200f;
        public static readonly float DefaultCategoryElementHeight = 40f;
        public static readonly float DefaultRoundedButtonWidth = 115.2f;
        public static readonly float DefaultRectButtonWidth = 56.5f;
        public static readonly float DefaultTextFieldWidth = 79.5f;
        public static readonly int HeaderVerticalMargin = 20;
        #endregion

        public static Texture2D buttonTexture;
        public static Texture2D toggleTexture;
        public static Texture2D rectButtonTexture;
        public static Texture2D rectTextFieldTexture;

        public static void Initialize()
        {
            string pluginPath = Paths.PluginPath;
            string relButtonPath = "BagOfTricks2\\UI\\Rounded Button.png";
            string fullPath = Path.Combine(pluginPath, relButtonPath);
            buttonTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relTogglePath = "BagOfTricks2\\UI\\Toggle.png";
            fullPath = Path.Combine(pluginPath, relTogglePath);
            toggleTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relRectPath = "BagOfTricks2\\UI\\Rect Button.png";
            fullPath = Path.Combine(pluginPath, relRectPath);
            rectButtonTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relTextRectPath = "BagOfTricks2\\UI\\Text Field Rect.png";
            fullPath = Path.Combine(pluginPath, relTextRectPath);
            rectTextFieldTexture = UI.GUIUtility.LoadTexture(fullPath);

            _windowStyle = new GUIStyle();
            _windowStyle.normal.background = UI.GUIUtility.CreateTexture(1, 1, MainDark);

            _headerLabelStyle = new GUIStyle();
            _headerLabelStyle.normal.textColor = Color.white;
            _headerLabelStyle.alignment = TextAnchor.MiddleCenter;
            _headerLabelStyle.fontStyle = FontStyle.Bold;
            _headerLabelStyle.fontSize = 18;

            _labelStyle = new GUIStyle();
            _labelStyle.normal.textColor = Color.white;
            _labelStyle.alignment = TextAnchor.MiddleLeft;
            _labelStyle.margin.left = (int)DefaultHeaderLabelWidth;
            _labelStyle.fontStyle = FontStyle.Bold;

            _buttonStyle = new GUIStyle();
            _buttonStyle.normal.background = UI.GUIUtility.CreateTexture(1, 1, LighterDark);
            _buttonStyle.normal.textColor = Color.white;
            _buttonStyle.fontStyle = FontStyle.Bold;
            _buttonStyle.alignment = TextAnchor.MiddleCenter;

            _buttonStyleSelected = new GUIStyle();
            _buttonStyleSelected.normal.background = UI.GUIUtility.CreateTexture(1, 1, Gray);
            _buttonStyleSelected.normal.textColor = Color.white;
            _buttonStyleSelected.fontStyle = FontStyle.Bold;
            _buttonStyleSelected.alignment = TextAnchor.MiddleCenter;

            _roundedCategoryButtonStyle = new GUIStyle();
            _roundedCategoryButtonStyle.normal.background = UI.GUIUtility.CreateColoredTexture(buttonTexture, DarkPurple);
            _roundedCategoryButtonStyle.alignment = TextAnchor.MiddleCenter;
            _roundedCategoryButtonStyle.normal.textColor = MainPurple;
            _roundedCategoryButtonStyle.fontStyle = FontStyle.Bold;

            _rectCategoryButtonStyle = new GUIStyle();
            _rectCategoryButtonStyle.normal.background = UI.GUIUtility.CreateColoredTexture(rectButtonTexture, DarkPurple);
            _rectCategoryButtonStyle.alignment = TextAnchor.MiddleCenter;
            _rectCategoryButtonStyle.normal.textColor = MainPurple;
            _rectCategoryButtonStyle.fontStyle = FontStyle.Bold;

            _toggleStyle = new GUIStyle();
            _toggleStyle.normal.background = UI.GUIUtility.CreateColoredTexture(toggleTexture, LighterDark);

            _toggleStyleEnabled = new GUIStyle();
            _toggleStyleEnabled.normal.background = UI.GUIUtility.CreateColoredTexture(toggleTexture, DarkPurple);
            _toggleStyleEnabled.fontStyle = FontStyle.Bold;
            _toggleStyleEnabled.fontSize = 26;
            _toggleStyleEnabled.alignment = TextAnchor.MiddleCenter;
            _toggleStyleEnabled.normal.textColor = MainPurple;

            _textFieldStyle = new GUIStyle();
            _textFieldStyle.normal.background = UI.GUIUtility.CreateColoredTexture(rectTextFieldTexture, LighterDark);
            _textFieldStyle.alignment = TextAnchor.MiddleCenter;
            _textFieldStyle.fontStyle = FontStyle.Bold;
            _textFieldStyle.normal.textColor = MainPurple;
            _textFieldStyle.padding.left = 5;
            _textFieldStyle.padding.right = 5;
            _textFieldStyle.margin.right = 25;
        }

        public static GUIStyle WindowStyle
        {
            get { return _windowStyle; }
        }

        public static GUIStyle HeaderLabelStyle
        {
            get { return _headerLabelStyle; }
        }

        public static GUIStyle LabelStyle
        {
            get { return _labelStyle; }
        }

        public static GUIStyle ButtonStyle
        {
            get { return _buttonStyle; }
        }

        public static GUIStyle ButtonStyleSelected
        {
            get { return _buttonStyleSelected; }
        }

        public static GUIStyle RoundedCategoryButtonStyle
        {
            get { return _roundedCategoryButtonStyle; }
        }

        public static GUIStyle RectCategoryButtonStyle
        {
            get { return _rectCategoryButtonStyle; }
        }

        public static GUIStyle ToggleStyle
        {
            get { return _toggleStyle; }
        }

        public static GUIStyle ToggleStyleEnabled
        {
            get { return _toggleStyleEnabled; }
        }

        public static GUIStyle TextFieldStyle
        {
            get { return _textFieldStyle; }
        }


        
    }
}

