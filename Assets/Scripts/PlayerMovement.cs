using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveForce;

    [SerializeField]
    float maxSpeed = 3;

    [SerializeField]
    AnimationCurve accelerationCurve;

    [SerializeField]
    int maxBlinks;

    [SerializeField]
    float blinkForce, blinkCooldown;

    private Rigidbody2D rigidBody;
    private Vector2 lookDirection, error;
    int blinksRemaining;
    bool shouldBlink = false;
    Camera mainCam;
    Renderer[] renderers;
    bool isWrappingX = false;
    bool isWrappingY = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(ResetBlinks(0));
        mainCam = Camera.main;
        renderers = GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle blink input
        HandleBlinkInput();
        HandleSteerInput();
        ScreenWrap();
    }

    void HandleBlinkInput()
    {
        if (Input.GetKeyDown("space"))
        {
            if (blinksRemaining > 0)
            {
                shouldBlink = true;
            }
        }
    }

    void HandleSteerInput()
    {
        lookDirection = (
            (Vector2)mainCam.ScreenToWorldPoint(Input.mousePosition)
            - (Vector2)transform.position
        ).normalized;
        if (Input.GetMouseButton(1))
        {
            error = lookDirection - rigidBody.velocity / maxSpeed;
        }
        else
        {
            error = Vector2.zero;
        }
    }

    bool CheckRenderers()
    {
        foreach (Renderer renderer in renderers)
        {
            // If at least one render is visible, return true
            if (renderer.isVisible)
            {
                return true;
            }
        }
        // Otherwise, the object is invisible
        return false;
    }


    void ScreenWrap()
    {
        bool isVisible = CheckRenderers();

        if (isVisible)
        {
            isWrappingX = false;
            isWrappingY = false;
            return;
        }

        if (isWrappingX && isWrappingY)
        {
            return;
        }
        Vector2 viewportPosition = mainCam.WorldToViewportPoint(transform.position);
        Vector2 newPosition = transform.position;
        if (!isWrappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            newPosition.x = -newPosition.x;
            isWrappingX = true;
        }
        if (!isWrappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            newPosition.y = -newPosition.y;
            isWrappingY = true;
        }
        transform.position = newPosition;
    }

    void FixedUpdate()
    {
        if (error.magnitude > 0)
        {
            Vector2 moveDir = new Vector2(accelerationCurve.Evaluate(error.x), accelerationCurve.Evaluate(error.y));
            rigidBody.AddForce(moveDir * moveForce, ForceMode2D.Force);
        }
        if (shouldBlink)
        {
            rigidBody.AddForce(lookDirection * blinkForce, ForceMode2D.Impulse);
            blinksRemaining -= 1;
            shouldBlink = false;
            StartCoroutine(ResetBlinks(blinkCooldown));
        }
    }

    IEnumerator ResetBlinks(float duration)
    {
        yield return new WaitForSeconds(duration);
        blinksRemaining = maxBlinks;
    }
}
