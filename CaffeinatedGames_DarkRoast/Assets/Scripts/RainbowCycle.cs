using UnityEngine;

public class RainbowCycle : MonoBehaviour
{
    public GameObject rainbowCube;

    //Members that manage changing the cube's color
    private Renderer objectRenderer;

    private int nextColor = 0;
    private Color color = new Color();

    private delegate void RotatingColors();

    private RotatingColors[] rotatingColors = new RotatingColors[6];
    [SerializeField] private float colorChangeSpeed;

    // Awake is called before Start to grab component references
    private void Awake()
    {
        objectRenderer = rainbowCube.GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize waypoint color to red
        color = new Color(1.0f, 0.0f, 0.0f);

        // Load up the rotatingColors array with functions to adjust rgb values cyclically
        rotatingColors[0] = GreenUp;
        rotatingColors[1] = RedDown;
        rotatingColors[2] = BlueUp;
        rotatingColors[3] = GreenDown;
        rotatingColors[4] = RedUp;
        rotatingColors[5] = BlueDown;
    }

    // Update is called once per frame
    private void Update()
    {
        // Cycle the color of the cube
        nextColor = nextColor % 6;
        rotatingColors[nextColor]();
        objectRenderer.material.color = color;
    }

    // RGB adjusters
    private void RedUp()
    {
        color.r += Time.deltaTime * colorChangeSpeed;
        if (color.r > 1.0f) { color.r = 1.0f; nextColor++; }
    }

    private void RedDown()
    {
        color.r -= Time.deltaTime * colorChangeSpeed;
        if (color.r < 0.0f) { color.r = 0.0f; nextColor++; }
    }

    private void GreenUp()
    {
        color.g += Time.deltaTime * colorChangeSpeed;
        if (color.g > 1.0f) { color.g = 1.0f; nextColor++; }
    }

    private void GreenDown()
    {
        color.g -= Time.deltaTime * colorChangeSpeed;
        if (color.g < 0.0f) { color.g = 0.0f; nextColor++; }
    }

    private void BlueUp()
    {
        color.b += Time.deltaTime * colorChangeSpeed;
        if (color.b > 1.0f) { color.b = 1.0f; nextColor++; }
    }

    private void BlueDown()
    {
        color.b -= Time.deltaTime * colorChangeSpeed;
        if (color.b < 0.0f) { color.b = 0.0f; nextColor++; }
    }
}