using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    private int _sceneIndex;
    [SerializeField] private float _loadDelay = 1f;
    private void Start()
    {
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")) {
            StartCoroutine(LoadNextLevel());
        }
    }

    private IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(_loadDelay);
        int nextIndex = _sceneIndex + 1;
        if(nextIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = 0;
        }
        ScenePersist.Instance.ResetScenePersist();
        SceneManager.LoadScene(nextIndex);
    }
}
