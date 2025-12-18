using UnityEngine;
using System.Collections.Generic;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager Instance { get; private set; }

    [Header("Configuration")]
    [Tooltip("Glisse ici tes 10 sprites découpés (ordre 0 à 9 obligatoire !)")]
    [SerializeField] private Sprite[] digitSprites;
    
    [Tooltip("Glisse le Prefab 'DamagePopup_Container'")]
    [SerializeField] private DamagePopup popupContainerPrefab;
    
    [Tooltip("Glisse le Prefab 'DigitSprite'")]
    [SerializeField] private GameObject singleDigitPrefab;

    private Queue<DamagePopup> pool = new Queue<DamagePopup>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        if (digitSprites.Length != 10)
            Debug.LogError("ERREUR : Il faut exactement 10 sprites (0-9) dans le tableau !");
    }
    public void CreatePopup(Vector3 position, int damageAmount)
    {
        DamagePopup popupToUse;

        if (pool.Count > 0)
        {
            popupToUse = pool.Dequeue();
            popupToUse.gameObject.SetActive(true);
        }
        else
        {
            DamagePopup newPopup = Instantiate(popupContainerPrefab);
            popupToUse = newPopup;
        }

        popupToUse.transform.position = position + new Vector3(0, 1f, 0);
        
        popupToUse.Setup(damageAmount, digitSprites, singleDigitPrefab);
    }

    public void ReturnToPool(DamagePopup popup)
    {
        popup.gameObject.SetActive(false);
        pool.Enqueue(popup);
    }
}