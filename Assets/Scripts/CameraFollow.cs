using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target; // Glisse ton objet "Player" ici
    [SerializeField] private float smoothSpeed = 0.125f; // Plus c'est bas, plus la caméra est "lourde/fluide"
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10); // Pour garder la caméra reculée (Z = -10)

    void LateUpdate() // LateUpdate est appelé après le mouvement du joueur (évite les tremblements)
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        
        // Lerp permet un mouvement fluide au lieu d'une téléportation brute
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        
        transform.position = smoothedPosition;
    }
}