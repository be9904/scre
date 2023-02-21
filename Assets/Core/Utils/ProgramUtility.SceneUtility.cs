using UnityEngine.SceneManagement;

public static partial class ProgramUtility
{
    public static void ReturnToMain()
    {
        SmoothLoading.SetNextSceneIndex(0);
        SceneManager.LoadScene(1);
    }
}
