using Cinemachine;
using UnityEngine;

public class FreeCameraHandler : MonoBehaviour
{
    private CinemachineFreeLook freeLook;
    private float yAxisValue = 0.7f;
    private float xAxisValue;
    public float mouseSensitivityX = 90f;
    public float mouseSensitivityY = 0.7f;
    public float stickSensitivityX = 120f;
    public float stickSensitivityY = 0.5f;
    public GameObject lockonCamera;

    public float maxCameraDistanceTop = 3f;
    public float maxCameraDistanceMiddle = 7f;
    public float maxCameraDistanceBottom = 2f;

    private void Awake()
    {
        freeLook = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        if (CinemachineCore.Instance.IsLive(freeLook))
        {
            if (PlayerInputHandler.instance == null) {
                return;
            }
            Vector2 stick = PlayerInputHandler.instance.cameraStickDirection;
            //Debug.Log(stick);

            if (stick.magnitude > 0.1f)
            {
                xAxisValue = stickSensitivityX * stick.x * Time.deltaTime;
                yAxisValue -= stickSensitivityY * stick.y * Time.deltaTime;
            }
            else
            {
                xAxisValue = mouseSensitivityX * Input.GetAxis("Mouse X") * Time.deltaTime;
                yAxisValue -= mouseSensitivityY * Input.GetAxis("Mouse Y") * Time.deltaTime;
            }

            yAxisValue = Mathf.Clamp01(yAxisValue);

            freeLook.m_XAxis.Value = xAxisValue;
            freeLook.m_YAxis.Value = yAxisValue;
        } else {
            // While we're in locked on mode, this camera isn't in use. But we still want to sync its position and rotation with the lock on camera,
            // so when we switch back to free mode, there isn't a completely different camera angle.
            // The way I'm doing this is janky but it works. Basically figure out what the difference in rotation is and simulate an input in the correct direction.

            float angleDiff = Mathf.DeltaAngle(lockonCamera.transform.eulerAngles.y, freeLook.State.RawOrientation.eulerAngles.y);
            freeLook.m_XAxis.Value = -0.5f * angleDiff;
        }
    }
}