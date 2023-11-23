using UnityEngine;

public class CanvasAutoDestroy : MonoBehaviour
{
    public float destroyDelay; // Time in seconds before the Canvas is destroyed

    void Start()
    {
        // Schedule the canvas to be destroyed after a delay
        Invoke("DestroyCanvas", destroyDelay);
    }

    void DestroyCanvas()
    {
        // Destroy the Canvas gameObject
        Destroy(gameObject);
    }
}