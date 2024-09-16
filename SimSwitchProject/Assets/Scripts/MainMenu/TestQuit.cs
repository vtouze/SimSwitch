using UnityEngine;

public class TestQuit : MonoBehaviour
{
    public void Quit()
    {
        Application.OpenURL("https://teez21.itch.io/testwebgl2022");
        Application.Quit();
    }
}
