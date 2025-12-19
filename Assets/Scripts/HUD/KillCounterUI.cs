using UnityEngine;
using UnityEngine.UI;

public class KillCounterUI : MonoBehaviour
{
    [SerializeField] private Sprite[] digits;
    [SerializeField] private Image hundredsImage;
    [SerializeField] private Image tensImage;
    [SerializeField] private Image onesImage;

    public void SetKills(int kills)
    {
        kills = Mathf.Clamp(kills, 0, 999);
        
        int hundreds = kills / 100; 
        
        int tens = (kills % 100) / 10; 
        
        int ones = kills % 10;
        
        if (hundredsImage != null) hundredsImage.sprite = digits[hundreds];
        if (tensImage != null) tensImage.sprite = digits[tens];
        if (onesImage != null) onesImage.sprite = digits[ones];
        
        if (hundredsImage != null) 
        {
            hundredsImage.enabled = kills >= 100;
        }
        if (tensImage != null) 
        {
            tensImage.enabled = kills >= 10;
        }
        
    }
}
