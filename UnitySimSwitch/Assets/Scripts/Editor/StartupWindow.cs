using UnityEditor;
using UnityEngine;

public class StartupWindow : EditorWindow
{
    #region Fields

    // Scroll position for the window content
    private Vector2 scrollPosition;

    #endregion

    #region Initialization

    /// <summary>
    /// Initializes the window when the editor loads.
    /// </summary>
    [InitializeOnLoadMethod]
    private static void Init()
    {
        EditorApplication.delayCall += ShowWindow; // Delay window display to ensure it's ready after editor startup
    }

    /// <summary>
    /// Displays the window when called.
    /// </summary>
    private static void ShowWindow()
    {
        // Create and show the startup window
        var window = GetWindow<StartupWindow>("Startup Window");
        window.minSize = new Vector2(500, 300);
        window.maxSize = new Vector2(500, 300);

        // Center the window on the screen or Unity editor window
        CenterWindowOnScreen(window);

        window.Show();
    }


    #endregion

    #region GUI Handling

    /// <summary>
    /// Draws the content of the startup window.
    /// </summary>
    private void OnGUI()
    {
        // Begin scroll view to allow content to be scrollable
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Define font styles for the labels
        GUIStyle bodyFontStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 15
        };

        GUIStyle headerFontStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 18,
            fontStyle = FontStyle.Bold
        };

        // Add header and body text
        GUILayout.Label("Welcome to the SimSwitch Unity Project!", headerFontStyle);
        GUILayout.Label("Access key project documents & resources here:", bodyFontStyle);

        GUILayout.Space(10);

        // Load icons for the resources
        Texture2D githubIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Editor/Github_Icon.png");
        Texture2D figmaIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Editor/Figma_Icon.png");
        Texture2D slidesIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Editor/GoogleSlides_Icon.png");
        Texture2D sheetsIcon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Resources/Editor/GoogleSheets_Icon.png");

        // Display key points (project links)
        DisplayKeyPoints("Game Design Document", slidesIcon, "https://docs.google.com/presentation/d/1ltvFjbtSLsmk_aHYUMIdQF2QMEU0XPMopAOMGaYNMcE/edit?usp=sharing", bodyFontStyle);
        DisplayKeyPoints("Assets Moodboards", figmaIcon, "https://www.figma.com/design/OF7hPUn9ixie4JpAE9iM51/Assets-Moodboards?node-id=2-27&t=1RUUvYep0sftzP11-1", bodyFontStyle);
        DisplayKeyPoints("Games Moodboards", figmaIcon, "https://www.figma.com/design/IYrEKz4Q5rXmeqMGveQRjc/Games-Moodboards?node-id=21-45&t=XdMD7pzzXyB47aZF-1", bodyFontStyle);
        DisplayKeyPoints("UI Design", figmaIcon, "https://www.figma.com/design/Mb3RvDPiwcYtZeUvSrciX3/UI-Design?node-id=16-2&t=ZNVrm4kETAb2x04x-1", bodyFontStyle);
        DisplayKeyPoints("Inventory", sheetsIcon, "https://docs.google.com/spreadsheets/d/1HCYITuk9SxpUtSEZ5xunQOp8vD2HxPQpEPztbqNiPzU/edit?usp=sharing", bodyFontStyle);
        DisplayKeyPoints("Project Repository", githubIcon, "https://github.com/vtouze/SimSwitch", bodyFontStyle);

        // Button to close the window
        if (GUILayout.Button("Close Window"))
        {
            Close();
        }

        GUILayout.EndScrollView();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Displays a clickable link with an icon for a key project document.
    /// </summary>
    /// <param name="text">The text to display for the document.</param>
    /// <param name="icon">The icon to display next to the document text.</param>
    /// <param name="link">The URL to open when the icon is clicked.</param>
    /// <param name="fontStyle">The style for the text.</param>
    private void DisplayKeyPoints(string text, Texture2D icon, string link, GUIStyle fontStyle)
    {
        GUILayout.BeginHorizontal(); // Start horizontal layout for the text and icon

        // Display the text with the provided font style
        GUILayout.Label(new GUIContent(text), fontStyle);

        // Add flexible space to push the button to the right
        GUILayout.FlexibleSpace();

        // Display the icon as a button
        if (GUILayout.Button(new GUIContent(icon), GUILayout.Width(50), GUILayout.Height(30)))
        {
            Application.OpenURL(link);
        }

        GUILayout.EndHorizontal(); 
    }

    /// <summary>
    /// Centers the window on the screen or relative to the Unity Editor window.
    /// </summary>
    /// <param name="window">The EditorWindow to center.</param>
    private static void CenterWindowOnScreen(EditorWindow window)
    {
        // Get the screen width and height
        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        // Get the position of the Unity Editor window on the screen
        Rect editorWindowRect = new Rect(0, 0, screenWidth, screenHeight);
    
        // Calculate the center position
        float windowWidth = window.position.width;
        float windowHeight = window.position.height;

        // Set the new position to center the window
        float xPosition = (screenWidth - windowWidth) / 2;
        float yPosition = (screenHeight - windowHeight) / 2;

        // Set the window position
        window.position = new Rect(xPosition, yPosition, windowWidth, windowHeight);
    }
    #endregion
}