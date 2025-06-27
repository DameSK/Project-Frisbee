using UnityEngine;

public class EnergyRing : MonoBehaviour
{
    public int energyAmount = 5;
    public GameObject symbolToHide; // Halka içindeki sembol veya ışık
    public EnergyRing nextRing;     // Sonraki halka
    public Material activeMaterial;
    public Material inactiveMaterial;
    public GameObject directionParticlesPrefab; // Yön partikül prefabı

    private bool hasGivenEnergy = false;
    private bool isActive = false;

    private Renderer rend;
    private GameObject directionParticlesInstance;

    void Start()
    {
        Debug.Log("START ÇALIŞTI: " + gameObject.name);

        rend = GetComponent<Renderer>();
        SetActive(gameObject.name.Contains("1")); // Sadece ilk halka aktif başlasın
    }

    public void SetActive(bool active)
    {
        isActive = active;
        Debug.Log("SetRingActive çağrıldı: " + active);

        if (rend == null)
            rend = GetComponent<Renderer>();

        if (rend != null && activeMaterial != null && inactiveMaterial != null)
        {
            rend.material = isActive ? activeMaterial : inactiveMaterial;
        }

        if (symbolToHide != null)
        {
            symbolToHide.SetActive(active);
        }

        // Partikül gösterge sistemi
        if (active && nextRing != null && directionParticlesPrefab != null)
        {
            if (directionParticlesInstance != null)
                Destroy(directionParticlesInstance);

            Vector3 direction = (nextRing.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            directionParticlesInstance = Instantiate(directionParticlesPrefab, transform.position, lookRotation);
            directionParticlesInstance.transform.SetParent(transform); // Bu halkaya bağlı kalsın
        }
        else
        {
            if (directionParticlesInstance != null)
            {
                Destroy(directionParticlesInstance);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($">>> OnTriggerEnter: {other.name}");

        if (!isActive || hasGivenEnergy)
        {
            Debug.Log("Halka pasif veya zaten enerji verdi.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            FrisbeeEnergy energy = other.GetComponent<FrisbeeEnergy>();
            if (energy != null)
            {
                Debug.Log("ENERJİ EKLENİYOR!");
                energy.AddEnergy(energyAmount);
                hasGivenEnergy = true;

                SetActive(false); // Bu halkayı pasif yap

                if (nextRing != null)
                {
                    Debug.Log("Sonraki halka aktif yapılıyor: " + nextRing.gameObject.name);
                    nextRing.SetActive(true);
                }
                else
                {
                    if (GameManager.Instance != null)
                    {
                        Debug.Log("Tüm halkalardan geçildi! GameManager'a bildiriliyor.");
                        GameManager.Instance.AllRingsCompleted();
                    }
                }
            }
            else
            {
                Debug.LogWarning("FrisbeeEnergy scripti bulunamadı!");
            }
        }
    }
}