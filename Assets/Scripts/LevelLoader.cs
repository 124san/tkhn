using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
     [SerializeField] bool destroyBgmOnNextLevel = false;

    public float transitionTime = 1f;

    public void ReloadScene() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel() {
        if (destroyBgmOnNextLevel) DestroyBgm();
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex) {
        // Play animation
        transition.SetTrigger("Start");
        // Wait for animation
        yield return new WaitForSeconds(transitionTime);
        // Load scene
        SceneManager.LoadScene(levelIndex);
    }
    void DestroyBgm() {
        GameObject.Destroy(GameObject.Find("Background music"));
    }
}
