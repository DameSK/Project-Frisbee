using UnityEngine;
using TMPro;

public class FrisbeeEnergy : MonoBehaviour
{
    public int maxEnergy = 50;
    public int currentEnergy;
    public float energyDecreaseRate = 1f; // Saniyede 1 azalır
    private float timer;

    public bool isOutOfEnergy = false;

    public TextMeshProUGUI energyText;

    void Start()
    {
        currentEnergy = maxEnergy;
        UpdateUI();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            timer = 0f;
            DecreaseEnergy(1);
        }

        if (isOutOfEnergy)
        {
            Debug.Log("Enerji bitti, OUT OF ENERGY");
        }
    }

        public void AddEnergy(int amount)
    {
        currentEnergy += amount;

        // Enerji arttıysa tekrar hareket edebilir
        if (currentEnergy > 0)
        isOutOfEnergy = false;

        UpdateUI();
    }

    public void DecreaseEnergy(int amount)
    {
        currentEnergy = Mathf.Max(currentEnergy - amount, 0);
        UpdateUI();

        if (currentEnergy == 0 && !isOutOfEnergy)
        {
            isOutOfEnergy = true;
        }
    }

        private void UpdateUI()
    {
        if (energyText != null)
        energyText.text = "Energy: " + currentEnergy;
    }
}