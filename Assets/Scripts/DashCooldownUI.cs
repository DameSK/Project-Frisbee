using UnityEngine;
using UnityEngine.UI;

public class DashCooldownUI : MonoBehaviour
{
    public FrisbeeMovement frisbeeMovement;
    public Image cooldownImage;

    void Update()
    {
        if (frisbeeMovement == null || cooldownImage == null) return;

        if (frisbeeMovement.IsDashAvailable())
        {
            cooldownImage.fillAmount = 1f;
            cooldownImage.color = Color.green;
        }
        else
        {
            cooldownImage.fillAmount = frisbeeMovement.GetDashCooldownProgress();
            cooldownImage.color = Color.red;
        }
    }
}