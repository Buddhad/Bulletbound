using UnityEngine;
using UnityEngine.SceneManagement;

public class FixedCam : MonoBehaviour
{
    [Header("Player Follow")]
    [SerializeField] private Transform targetFollow;

    [Header("Camera Limits")]
    [SerializeField] private float maxX = 3f;
    [SerializeField] private float minX = -10f;

    [SerializeField] private float maxY = 2f;
    [SerializeField] private float minY = 0f;

    // Camera shake offset.
    // CamShake changes this temporarily.
    private Vector3 shakeOffset = Vector3.zero;


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        FindPlayer();
    }


    // =========================================================
    // SCENE LOADED
    // =========================================================

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        FindPlayer();

        // Reset any previous shake when entering a new level.
        shakeOffset = Vector3.zero;
    }


    // =========================================================
    // FIND PLAYER
    // =========================================================

    private void FindPlayer()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            targetFollow = player.transform;

            Debug.Log(
                "FixedCam: Player found."
            );
        }
        else
        {
            Debug.LogWarning(
                "FixedCam: Player with Tag 'Player' was not found."
            );
        }
    }


    // =========================================================
    // LATE UPDATE
    // =========================================================

    private void LateUpdate()
    {
        if (targetFollow == null)
        {
            FindPlayer();
            return;
        }


        // -----------------------------------------------------
        // CALCULATE NORMAL CAMERA POSITION
        // -----------------------------------------------------

        float x =
            Mathf.Clamp(
                targetFollow.position.x,
                minX,
                maxX
            );

        float y =
            Mathf.Clamp(
                targetFollow.position.y,
                minY,
                maxY
            );


        // -----------------------------------------------------
        // APPLY NORMAL POSITION + SHAKE OFFSET
        // -----------------------------------------------------

        transform.position =
            new Vector3(
                x,
                y,
                transform.position.z
            )
            + shakeOffset;
    }


    // =========================================================
    // SET SHAKE OFFSET
    // =========================================================

    public void SetShakeOffset(Vector3 offset)
    {
        shakeOffset = offset;
    }


    // =========================================================
    // RESET SHAKE
    // =========================================================

    public void ResetShakeOffset()
    {
        shakeOffset = Vector3.zero;
    }
}