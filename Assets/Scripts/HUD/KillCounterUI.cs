using UnityEngine;
using UnityEngine.UI;

public class KillCounterUI : MonoBehaviour
{
    [SerializeField] private Sprite[] digits;
    [SerializeField] private Image tensImage;
    [SerializeField] private Image onesImage;

    public void SetKills(int kills)
    {
        kills = Mathf.Clamp(kills, 0, 99);
        
        int  tens = kills / 10;
        int ones = kills % 10;
        
        tensImage.sprite = digits[tens];
        onesImage.sprite = digits[ones];
    }
}
