using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamagePopup : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float lifeTime = 1f;
    [SerializeField] private float fadeOutSpeed = 3f;
    
    [SerializeField] private float characterSpacing = 0.4f; 

    private GameObject digitPrefab; 
    private List<SpriteRenderer> digitRenderers = new List<SpriteRenderer>();

    private float timer;
    private Color textColor;

    public void Setup(int damageAmount, Sprite[] digitSprites, GameObject digitPrefabRef)
    {
        this.digitPrefab = digitPrefabRef;
        timer = lifeTime;
        textColor = Color.white;
        transform.localScale = Vector3.one; 

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        digitRenderers.Clear();

        string damageString = damageAmount.ToString();

        // --- CALCUL POUR CENTRER LE NOMBRE ---
        // Exemple : Si on a 2 chiffres et un espace de 0.5, la largeur totale est 1.0.
        // On commence à -0.5 pour que le "centre" du nombre soit bien sur l'ennemi.
        float totalWidth = (damageString.Length - 1) * characterSpacing;
        float startX = -totalWidth / 2f;

        for (int i = 0; i < damageString.Length; i++)
        {
            int digitIndex = damageString[i] - '0';

            GameObject digitGO = Instantiate(digitPrefab, transform);
            SpriteRenderer sr = digitGO.GetComponent<SpriteRenderer>();

            sr.sprite = digitSprites[digitIndex];
            sr.color = textColor; 

            // --- Placement manuel ---
            // On place le chiffre à : Position de départ + (Numéro du chiffre * Ecart)
            float xPosition = startX + (i * characterSpacing);
            digitGO.transform.localPosition = new Vector3(xPosition, 0, 0);

            digitRenderers.Add(sr);
        }
    }

    void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            textColor.a -= fadeOutSpeed * Time.deltaTime;
            foreach (SpriteRenderer sr in digitRenderers)
            {
                sr.color = textColor;
            }

            if (textColor.a <= 0)
            {
                 DamagePopupManager.Instance.ReturnToPool(this);
            }
        }
    }
}