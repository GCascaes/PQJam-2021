using UnityEngine;
using UnityEngine.SceneManagement;
public class Loading : MonoBehaviour
{
    private static string sceneToLoad;

    private void Start()
    {
        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    /// <summary>
    /// Chama a cena de Loading enquanto carrega uma nova cena
    /// </summary>
    /// <param name="_sceneToLoad"></param>
    public static void LoadScene(string _sceneToLoad)
    {
        sceneToLoad = _sceneToLoad;
        SceneManager.LoadScene("Loading");
    }

}
