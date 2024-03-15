using UnityEngine;

public class HealthBar : MonoBehaviour {
    private float maxWidth, newWidth, percentage;
    private RectTransform rectTransform;
    private PlayerController playerController;
    public GameManager gameManager;

    void Start() {
        playerController = gameManager.player.GetComponent<PlayerController>();
        rectTransform = GetComponent<RectTransform>();
        maxWidth = rectTransform.localScale.x;
    }
    
    public void UpdateHealthBar() {
        percentage = (playerController.RemainingHealth / playerController.RemainingMaxHealth);
        newWidth = maxWidth * percentage;
        var scale = rectTransform.localScale;
        scale.x = newWidth;
        rectTransform.localScale = scale;
    }
}
