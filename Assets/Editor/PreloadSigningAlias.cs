using UnityEditor;

[InitializeOnLoad]
public class PreloadSigningAlias
{
    static PreloadSigningAlias()
    {
        PlayerSettings.Android.keystorePass = "tgnpp1920";
        PlayerSettings.Android.keyaliasName = "student";
        PlayerSettings.Android.keyaliasPass = "tgnpp1920";
    }
}
