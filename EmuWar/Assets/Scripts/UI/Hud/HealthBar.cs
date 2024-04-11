using UnityEngine;
[AddComponentMenu(" Game Scripts / UI / HUD / HealthBar",0)]
public class HealthBar : MonoBehaviour {
    private float maxWidth, newWidth, percentage;
    private RectTransform rectTransform;
    private PlayerController playerController;

    void Start() {
        playerController = GameManager.Instance.player.GetComponent<PlayerController>();
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
