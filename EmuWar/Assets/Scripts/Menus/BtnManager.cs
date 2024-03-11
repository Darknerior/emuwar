using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Button manager using onclick functions
/// </summary>
public class BtnManager : MonoBehaviour {
    [SerializeField] GameObject optionAnim;
    private bool optionsOpen = false;
    
    public void PlayGame() {
        SceneManager.LoadScene("Main");
    }

    public void Options(){
        var anim = optionAnim.GetComponent<Animator>();
        if(!optionsOpen) {
            anim.SetBool("optOpen", true);
            optionsOpen = true;
        }
        else {
            anim.SetBool("optOpen", false);
            optionsOpen = false;
        }
    }
    

    public void QuitGame() {
        Application.Quit();
    }
}
