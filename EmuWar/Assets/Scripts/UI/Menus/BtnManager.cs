using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Button manager using onclick functions
/// </summary>
public class BtnManager : MonoBehaviour {
    [SerializeField] GameObject options;
    private bool optionsOpen = false;
    
    /// <summary>
    /// Loads main game scene
    /// </summary>
    public void PlayGame() {
        SoundManager.Instance.PlayButtonClickSound();
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// Shows additional options in main menu
    /// </summary>
    public void Options(){
        SoundManager.Instance.PlayButtonClickSound();
        var anim = options.GetComponent<Animator>();
        if(!optionsOpen) {
            anim.SetBool("optOpen", true);
            optionsOpen = true;
        }
        else {
            anim.SetBool("optOpen", false);
            optionsOpen = false;
        }
    }
    
    /// <summary>
    /// Loads Menu Scene
    /// </summary>
    public void LoadMenu() {
        SoundManager.Instance.PlayButtonClickSound();
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("MainMenu");
    }
    
    /// <summary>
    /// Shows additional options pause menu
    /// </summary>
    public void OptionsPause(){
        SoundManager.Instance.PlayButtonClickSound();
        options.SetActive(!options.activeSelf);
    }
    
    /// <summary>
    /// Exits application
    /// </summary>
    public void QuitGame() {
        SoundManager.Instance.PlayButtonClickSound();
        Application.Quit();
    }
}
