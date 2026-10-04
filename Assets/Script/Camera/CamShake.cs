using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    [Header("Camera Shake Animator")]
    [SerializeField] private Animator anim;

    [Header("Animator Parameters")]
    [SerializeField] private string shakeTrigger = "shake";
    [SerializeField] private string shakeStateParameter = "shakeState";

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 0.12f;

    [SerializeField] private float shakeAmount = 0.08f;

    private FixedCam fixedCam;

    private Coroutine shakeCoroutine;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }

        fixedCam = GetComponent<FixedCam>();


        if (anim == null)
        {
            Debug.LogWarning(
                "CamShake: Animator not found.",
                this
            );
        }


        if (fixedCam == null)
        {
            Debug.LogError(
                "CamShake: FixedCam component not found.",
                this
            );
        }
    }


    // =========================================================
    // SHAKE CAMERA
    // =========================================================

    public void ShakeCamera()
    {
        if (fixedCam == null)
            return;


        // Stop an existing shake.
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);

            shakeCoroutine = null;
        }


        // -----------------------------------------------------
        // RANDOM ANIMATION STATE
        // -----------------------------------------------------
        //
        // 0 = shake1
        // 1 = shake2
        // 2 = shake3
        //

        int randomShake =
            Random.Range(0, 3);


        // Tell Animator which state to use.
        if (anim != null)
        {
            anim.SetInteger(
                shakeStateParameter,
                randomShake
            );

            // Start the shake transition.
            anim.SetTrigger(
                shakeTrigger
            );
        }


        Debug.Log(
            "Camera Shake: shake" +
            (randomShake + 1)
        );


        // Start actual camera movement.
        shakeCoroutine =
            StartCoroutine(
                ShakeRoutine()
            );
    }


    // =========================================================
    // SHAKE ROUTINE
    // =========================================================

    private IEnumerator ShakeRoutine()
    {
        float elapsed = 0f;


        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;


            // -------------------------------------------------
            // RANDOM OFFSET
            // -------------------------------------------------

            float x =
                Random.Range(
                    -shakeAmount,
                    shakeAmount
                );

            float y =
                Random.Range(
                    -shakeAmount,
                    shakeAmount
                );


            Vector3 offset =
                new Vector3(
                    x,
                    y,
                    0f
                );


            // Apply temporary offset.
            fixedCam.SetShakeOffset(offset);


            yield return null;
        }


        // -----------------------------------------------------
        // RESET CAMERA
        // -----------------------------------------------------

        fixedCam.ResetShakeOffset();

        shakeCoroutine = null;


        Debug.Log(
            "Camera Shake finished."
        );
    }
}