using UnityEngine;

public class ShopInteraction : MonoBehaviour
{
    // =========================================================
    // UI REFERENCES
    // =========================================================

    [Header("Shop UI")]

    // "Press E to Shop" UI.
    [SerializeField] private GameObject shopPrompt;

    // Actual shop panel.
    [SerializeField] private GameObject shopPanel;


    // =========================================================
    // INTERACTION SETTINGS
    // =========================================================

    [Header("Interaction Settings")]

    // Key used to open/close the Shop.
    [SerializeField] private KeyCode interactKey = KeyCode.E;


    // Extra size added around the Shop collider.
    // This makes the interaction area easier to enter.
    [SerializeField] private float interactionPadding = 0.2f;


    // =========================================================
    // DEBUG SETTINGS
    // =========================================================

    [Header("Debug Settings")]

    // Enable/disable debug messages.
    [SerializeField] private bool enableDebug = true;

    // If enabled, prints a status message periodically.
    [SerializeField] private bool continuousDebug = true;

    // How often the continuous debug message appears.
    [SerializeField] private float debugInterval = 1f;


    // =========================================================
    // INTERNAL VARIABLES
    // =========================================================

    // True when the Player is inside the Shop interaction area.
    private bool playerNearby = false;

    // True when the Shop Panel is open.
    private bool shopOpen = false;

    // Number of Player colliders currently detected.
    private int playerColliderCount = 0;

    // Shop collider reference.
    private BoxCollider2D shopCollider;

    // Timer used for periodic debug messages.
    private float debugTimer = 0f;

    // Used to detect changes in Player detection.
    private bool previousPlayerNearby = false;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        // Get the BoxCollider2D attached to the Shop.
        shopCollider = GetComponent<BoxCollider2D>();

        if (shopCollider == null)
        {
            Debug.LogError(
                "SHOP ERROR: BoxCollider2D is missing from Shop!",
                this
            );
        }
        else
        {
            Debug.Log(
                "SHOP: BoxCollider2D found.",
                this
            );
        }
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        // Hide Shop Prompt at the beginning.
        if (shopPrompt != null)
        {
            shopPrompt.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "SHOP ERROR: Shop Prompt is NOT assigned!",
                this
            );
        }


        // Hide Shop Panel at the beginning.
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }
        else
        {
            Debug.LogError(
                "SHOP ERROR: Shop Panel is NOT assigned!",
                this
            );
        }


        Debug.Log(
            "SHOP STARTED: " +
            gameObject.name,
            this
        );
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // -----------------------------------------------------
        // CONTINUOUS PLAYER CHECK
        // -----------------------------------------------------
        //
        // This runs every frame and checks whether the Player
        // is currently inside the Shop interaction area.
        //
        CheckPlayer();


        // -----------------------------------------------------
        // E KEY
        // -----------------------------------------------------

        if (playerNearby && Input.GetKeyDown(interactKey))
        {
            Debug.Log(
                "SHOP: E PRESSED - Toggling Shop.",
                this
            );

            ToggleShop();
        }


        // -----------------------------------------------------
        // PERIODIC DEBUG
        // -----------------------------------------------------

        if (continuousDebug)
        {
            debugTimer += Time.unscaledDeltaTime;

            if (debugTimer >= debugInterval)
            {
                debugTimer = 0f;

                Debug.Log(
                    "SHOP STATUS | " +
                    "Player Nearby: " + playerNearby +
                    " | Player Colliders: " + playerColliderCount +
                    " | Shop Open: " + shopOpen +
                    " | Shop Prompt Active: " +
                    IsObjectActive(shopPrompt) +
                    " | Shop Panel Active: " +
                    IsObjectActive(shopPanel),
                    this
                );
            }
        }
    }


    // =========================================================
    // CONTINUOUS PLAYER CHECK
    // =========================================================

    private void CheckPlayer()
    {
        // Make sure the Shop collider exists.
        if (shopCollider == null)
            return;


        // -----------------------------------------------------
        // CREATE CHECK AREA
        // -----------------------------------------------------
        //
        // We use the Shop collider bounds as the interaction
        // area.
        //
        // This does NOT depend on OnTriggerEnter2D.
        //

        Bounds bounds = shopCollider.bounds;

        Vector2 checkSize = new Vector2(
            bounds.size.x + interactionPadding * 2f,
            bounds.size.y + interactionPadding * 2f
        );


        // -----------------------------------------------------
        // FIND COLLIDERS INSIDE SHOP
        // -----------------------------------------------------

        Collider2D[] detectedColliders =
            Physics2D.OverlapBoxAll(
                bounds.center,
                checkSize,
                0f
            );


        // Reset detected Player collider count.
        int detectedPlayerColliders = 0;

        bool foundPlayer = false;


        // -----------------------------------------------------
        // CHECK EVERY COLLIDER
        // -----------------------------------------------------

        foreach (Collider2D col in detectedColliders)
        {
            if (col == null)
                continue;


            // Check if this collider belongs to the Player.
            bool isPlayer =
                col.CompareTag("Player") ||
                col.GetComponentInParent<PlayerMovement>() != null;


            if (!isPlayer)
                continue;


            foundPlayer = true;
            detectedPlayerColliders++;


            // Detailed debug information.
            if (enableDebug)
            {
                Debug.Log(
                    "SHOP DETECTED PLAYER COLLIDER: " +
                    col.gameObject.name +
                    " | Tag: " +
                    col.tag +
                    " | Layer: " +
                    LayerMask.LayerToName(col.gameObject.layer),
                    this
                );
            }
        }


        // Store the number of Player colliders found.
        playerColliderCount = detectedPlayerColliders;


        // -----------------------------------------------------
        // PLAYER ENTERED
        // -----------------------------------------------------

        if (foundPlayer && !playerNearby)
        {
            playerNearby = true;

            Debug.Log(
                "====================================\n" +
                "SHOP: PLAYER FOUND!\n" +
                "Player Collider Count: " +
                playerColliderCount +
                "\n" +
                "====================================",
                this
            );


            // Show Shop Prompt.
            if (!shopOpen && shopPrompt != null)
            {
                shopPrompt.SetActive(true);

                Debug.Log(
                    "SHOP: ShopPrompt ENABLED.",
                    this
                );
            }
        }


        // -----------------------------------------------------
        // PLAYER LEFT
        // -----------------------------------------------------

        else if (!foundPlayer && playerNearby)
        {
            playerNearby = false;

            Debug.Log(
                "====================================\n" +
                "SHOP: PLAYER LEFT!\n" +
                "====================================",
                this
            );


            // Hide Shop Prompt.
            if (shopPrompt != null)
            {
                shopPrompt.SetActive(false);
            }


            // Close Shop if the Player walks away.
            if (shopOpen)
            {
                CloseShop();
            }
        }


        // -----------------------------------------------------
        // DETECTION STATE CHANGED
        // -----------------------------------------------------

        if (previousPlayerNearby != playerNearby)
        {
            Debug.Log(
                "SHOP DETECTION CHANGED: " +
                previousPlayerNearby +
                " → " +
                playerNearby,
                this
            );

            previousPlayerNearby = playerNearby;
        }
    }


    // =========================================================
    // TOGGLE SHOP
    // =========================================================

    private void ToggleShop()
    {
        if (shopOpen)
        {
            CloseShop();
        }
        else
        {
            OpenShop();
        }
    }


    // =========================================================
    // OPEN SHOP
    // =========================================================

    private void OpenShop()
    {
        // Player must be near the Shop.
        if (!playerNearby)
        {
            Debug.Log(
                "SHOP: Cannot open. Player is not nearby.",
                this
            );

            return;
        }


        shopOpen = true;


        Debug.Log(
            "SHOP: PANEL OPENED.",
            this
        );


        // Hide "Press E to Shop".
        if (shopPrompt != null)
        {
            shopPrompt.SetActive(false);
        }


        // Show Shop Panel.
        if (shopPanel != null)
        {
            shopPanel.SetActive(true);
        }
    }


    // =========================================================
    // CLOSE SHOP
    // =========================================================

    private void CloseShop()
    {
        shopOpen = false;


        Debug.Log(
            "SHOP: PANEL CLOSED.",
            this
        );


        // Hide Shop Panel.
        if (shopPanel != null)
        {
            shopPanel.SetActive(false);
        }


        // If Player is still near the Shop,
        // show the prompt again.
        if (playerNearby && shopPrompt != null)
        {
            shopPrompt.SetActive(true);
        }
    }


    // =========================================================
    // TRIGGER DEBUG
    // =========================================================
    //
    // These are kept even though we use continuous detection.
    // They help us determine whether Unity's trigger system
    // itself is working.
    //

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(
            "SHOP TRIGGER ENTER: " +
            other.gameObject.name +
            " | Tag: " +
            other.tag,
            this
        );
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log(
            "SHOP TRIGGER EXIT: " +
            other.gameObject.name +
            " | Tag: " +
            other.tag,
            this
        );
    }


    // =========================================================
    // CHECK GAMEOBJECT ACTIVE STATE
    // =========================================================

    private bool IsObjectActive(GameObject obj)
    {
        if (obj == null)
            return false;

        return obj.activeSelf;
    }


    // =========================================================
    // SCENE DEBUG GIZMO
    // =========================================================

    private void OnDrawGizmosSelected()
    {
        BoxCollider2D col =
            GetComponent<BoxCollider2D>();

        if (col == null)
            return;


        // Draw normal Shop collider.
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            col.bounds.center,
            col.bounds.size
        );


        // Draw expanded interaction area.
        Gizmos.color = Color.yellow;

        Vector3 expandedSize = new Vector3(
            col.bounds.size.x +
            interactionPadding * 2f,

            col.bounds.size.y +
            interactionPadding * 2f,

            0.1f
        );

        Gizmos.DrawWireCube(
            col.bounds.center,
            expandedSize
        );
    }
}