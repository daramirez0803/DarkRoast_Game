using UnityEngine;

public class BlackWhiteCycle : MonoBehaviour
{
    // Fade the color from red to green
    // back and forth over the defined duration

    private float duration = 10.0f;
    private Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        float lerp = Mathf.PingPong(Time.time, duration) / duration;
        rend.material.color = Color.Lerp(Color.black, Color.white, lerp);
    }
}