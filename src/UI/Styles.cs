using BepInEx;
using UnityEngine;
using System.IO;

namespace BagOfTricks
{
    public static class Styles
    {
        public static class Colors
        {
            public static readonly Color MainDark = new(0.13f, 0.13f, 0.13f);
            public static readonly Color LighterDark = new(0.18f, 0.18f, 0.18f);
            public static readonly Color DarkestDark = new(0.07f, 0.07f, 0.07f);
            public static readonly Color Gray = new(0.22f, 0.22f, 0.22f);
            public static readonly Color MainPurple = new(0.77f, 0.58f, 1f);
            public static readonly Color DarkPurple = new(0.2f, 0.18f, 0.23f);
            public static readonly Color Turquoise = new(0.08f, 0.86f, 0.78f);

            public static readonly Color ErrorRed = new(0.95f, 0.34f, 0.44f);
            public static readonly Color WarningYellow = new(0.95f, 0.88f, 0.58f);
            public static readonly Color SuccessGreen = new(0.52f, 0.83f, 0.76f);

            public static readonly Color ButtonPurpleHighlight = new(0.20f, 0.20f, 0.28f);
            public static readonly Color ButtonPurpleSelected = new(0.16f, 0.16f, 0.24f);

            public static readonly string GreenHex = ColorUtility.ToHtmlStringRGB(SuccessGreen);
        }
        
        public static class GUIStyles
        {
            public static GUIStyle WindowStyle;
            public static GUIStyle TopBarButtonStyle;
            public static GUIStyle HeaderLabelStyle;
            public static GUIStyle LabelStyle;
            public static GUIStyle ButtonStyle;
            public static GUIStyle ButtonStyleSelected;
            public static GUIStyle RoundedCategoryButtonStyle;
            public static GUIStyle RectCategoryButtonStyle;
            public static GUIStyle ToggleStyle;
            public static GUIStyle ToggleStyleEnabled;
            public static GUIStyle TextFieldStyle;
            public static GUIStyle LogStyle;
        }

        public static class Dimensions
        {
            public static readonly float HeaderHeight = 40f;
            public static readonly float VerticalSpaceBetweenItems = 15f;
            public static readonly float DefaultHeaderLabelWidth = 200f;
            public static readonly float DefaultCategoryElementHeight = 40f;
            public static readonly float DefaultRoundedButtonWidth = 115.2f;
            public static readonly float DefaultRectButtonWidth = 56.5f;
            public static readonly float DefaultTextFieldWidth = 79.5f;
            public static readonly int HeaderVerticalMargin = 20;
        }
        
        public static class Textures
        {
            public static Texture2D buttonTexture;
            public static Texture2D toggleTexture;
            public static Texture2D rectButtonTexture;
            public static Texture2D rectTextFieldTexture;
            public static Texture2D squareTexture;
            public static Texture2D scrollThumbTexture;
            public static Texture2D scrollBackgroundTexture;
            public static Texture2D magnifyingGlass;
            public static Texture2D achievementRowEven;
            public static Texture2D achievementRowOdd;
        }

        public static void Initialize()
        {
            string pluginPath = Paths.PluginPath;
            string relButtonPath = "BagOfTricks2\\UI\\Rounded Button.png";
            string fullPath = Path.Combine(pluginPath, relButtonPath);
            Textures.buttonTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relTogglePath = "BagOfTricks2\\UI\\Toggle.png";
            fullPath = Path.Combine(pluginPath, relTogglePath);
            Textures.toggleTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relRectPath = "BagOfTricks2\\UI\\Rect Button.png";
            fullPath = Path.Combine(pluginPath, relRectPath);
            Textures.rectButtonTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relTextRectPath = "BagOfTricks2\\UI\\Text Field Rect.png";
            fullPath = Path.Combine(pluginPath, relTextRectPath);
            Textures.rectTextFieldTexture = UI.GUIUtility.LoadTexture(fullPath);

            string relMagnifyingPath = "BagOfTricks2\\UI\\Magnifying Glass.png";
            fullPath = Path.Combine(pluginPath, relMagnifyingPath);
            Texture2D magnifyingIcon = UI.GUIUtility.LoadTexture(fullPath);
            Textures.magnifyingGlass = UI.GUIUtility.CreateColoredTexture(magnifyingIcon, Colors.MainPurple);
            
            Textures.achievementRowEven = UI.GUIUtility.CreateTexture(1, 1, Colors.LighterDark);
            Textures.achievementRowOdd = UI.GUIUtility.CreateTexture(1, 1, Colors.Gray);

            GUIStyles.WindowStyle = new GUIStyle();
            GUIStyles.WindowStyle.normal.background = UI.GUIUtility.CreateTexture(1, 1, Colors.MainDark);

            GUIStyles.TopBarButtonStyle = new GUIStyle();
            GUIStyles.TopBarButtonStyle.normal.background = UI.GUIUtility.CreateTexture(1, 1, Colors.LighterDark);
            GUIStyles.TopBarButtonStyle.normal.textColor = Color.white;
            GUIStyles.TopBarButtonStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyles.TopBarButtonStyle.fontStyle = FontStyle.Bold;
            GUIStyles.TopBarButtonStyle.fontSize = 22;

            GUIStyles.HeaderLabelStyle = new GUIStyle();
            GUIStyles.HeaderLabelStyle.normal.textColor = Color.white;
            GUIStyles.HeaderLabelStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyles.HeaderLabelStyle.fontStyle = FontStyle.Bold;
            GUIStyles.HeaderLabelStyle.fontSize = 18;

            GUIStyles.LabelStyle = new GUIStyle();
            GUIStyles.LabelStyle.normal.textColor = Color.white;
            GUIStyles.LabelStyle.alignment = TextAnchor.MiddleLeft;
            GUIStyles.LabelStyle.margin.left = (int)Dimensions.DefaultHeaderLabelWidth;
            GUIStyles.LabelStyle.fontStyle = FontStyle.Bold;

            Textures.squareTexture = UI.GUIUtility.CreateTexture(1, 1, Colors.LighterDark);
            GUIStyles.ButtonStyle = new GUIStyle();
            GUIStyles.ButtonStyle.normal.background = Textures.squareTexture;
            GUIStyles.ButtonStyle.normal.textColor = Color.white;
            GUIStyles.ButtonStyle.fontStyle = FontStyle.Bold;
            GUIStyles.ButtonStyle.alignment = TextAnchor.MiddleCenter;

            GUIStyles.ButtonStyleSelected = new GUIStyle();
            GUIStyles.ButtonStyleSelected.normal.background = UI.GUIUtility.CreateTexture(1, 1, Colors.Gray);
            GUIStyles.ButtonStyleSelected.normal.textColor = Color.white;
            GUIStyles.ButtonStyleSelected.fontStyle = FontStyle.Bold;
            GUIStyles.ButtonStyleSelected.alignment = TextAnchor.MiddleCenter;

            GUIStyles.RoundedCategoryButtonStyle = new GUIStyle();
            GUIStyles.RoundedCategoryButtonStyle.normal.background = UI.GUIUtility.CreateColoredTexture(Textures.buttonTexture, Colors.DarkPurple);
            GUIStyles.RoundedCategoryButtonStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyles.RoundedCategoryButtonStyle.normal.textColor = Colors.MainPurple;
            GUIStyles.RoundedCategoryButtonStyle.fontStyle = FontStyle.Bold;

            GUIStyles.RectCategoryButtonStyle = new GUIStyle();
            GUIStyles.RectCategoryButtonStyle.normal.background = UI.GUIUtility.CreateColoredTexture(Textures.rectButtonTexture, Colors.DarkPurple);
            GUIStyles.RectCategoryButtonStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyles.RectCategoryButtonStyle.normal.textColor = Colors.MainPurple;
            GUIStyles.RectCategoryButtonStyle.fontStyle = FontStyle.Bold;

            GUIStyles.ToggleStyle = new GUIStyle();
            GUIStyles.ToggleStyle.normal.background = UI.GUIUtility.CreateColoredTexture(Textures.toggleTexture, Colors.LighterDark);

            GUIStyles.ToggleStyleEnabled = new GUIStyle();
            GUIStyles.ToggleStyleEnabled.normal.background = UI.GUIUtility.CreateColoredTexture(Textures.toggleTexture, Colors.DarkPurple);
            GUIStyles.ToggleStyleEnabled.fontStyle = FontStyle.Bold;
            GUIStyles.ToggleStyleEnabled.fontSize = 26;
            GUIStyles.ToggleStyleEnabled.alignment = TextAnchor.MiddleCenter;
            GUIStyles.ToggleStyleEnabled.normal.textColor = Colors.MainPurple;

            GUIStyles.TextFieldStyle = new GUIStyle();
            GUIStyles.TextFieldStyle.normal.background = UI.GUIUtility.CreateColoredTexture(Textures.rectTextFieldTexture, Colors.LighterDark);
            GUIStyles.TextFieldStyle.alignment = TextAnchor.MiddleCenter;
            GUIStyles.TextFieldStyle.fontStyle = FontStyle.Bold;
            GUIStyles.TextFieldStyle.normal.textColor = Colors.MainPurple;
            GUIStyles.TextFieldStyle.padding.left = 5;
            GUIStyles.TextFieldStyle.padding.right = 5;
            GUIStyles.TextFieldStyle.margin.right = 25;

            GUIStyles.LogStyle = new GUIStyle();
            GUIStyles.LogStyle.normal.background = UI.GUIUtility.CreateTexture(1, 1, Colors.LighterDark);
            GUIStyles.LogStyle.wordWrap = true;
            GUIStyles.LogStyle.fontSize = 16;
            GUIStyles.LogStyle.alignment = TextAnchor.MiddleLeft;
            GUIStyles.LogStyle.margin.left = 10;
            GUIStyles.LogStyle.margin.right = 10;
            GUIStyles.LogStyle.padding.left = 10;
            GUIStyles.LogStyle.padding.right = 10;
            GUIStyles.LogStyle.padding.top = 15;
            GUIStyles.LogStyle.padding.bottom = 15;
            GUIStyles.LogStyle.richText = true;

            Textures.scrollThumbTexture = UI.GUIUtility.CreateColoredTexture(Textures.squareTexture, Colors.DarkPurple);
            Textures.scrollBackgroundTexture = UI.GUIUtility.CreateColoredTexture(Textures.squareTexture, Colors.DarkestDark);
        }
    }
}

