using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBehavior : SingletonPersistent<SceneBehavior>   
{
    public void ReloadScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
