using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] bool destroyBgmOnLoad = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey) {
            LoadNextScene();
        }
    }

    public void LoadNextScene() {
        Debug.Log(SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        if (destroyBgmOnLoad) DestroyBgm();
    }

    void DestroyBgm() {
        GameObject.Destroy(GameObject.Find("Background music"));
    }
}
