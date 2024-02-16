using System.Collections;
using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Rigidbody characterBody;
    public float moveSpeed;
    public float jumpForce;
    private bool canJump;
    private bool isGrounded;
    public ImageTargetBehaviour imageTarget;
    private Animator characterAnimator;
    private bool isMoving;
    private Transform firstPos;
    private Vector3 lastPos;
    public float rotationSpeed;
    private GameObject pressurePlate;
    private int collectibleCount = 0;
    private Animator treeAnimator;
    private Animator lastCollectible;
    public ImageTargetBehaviour[] images;
    private bool trackingLost;

    //Movemement input
    private void InputMovements()
    {
        if (trackingLost) return;
        float horizontalInput = SimpleInput.GetAxis("Horizontal");
        float verticalInput = SimpleInput.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        transform.Translate(moveSpeed * Time.deltaTime * movementDirection, Space.World);
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        if (!canJump && isGrounded)
        {
            canJump = SimpleInput.GetButtonDown("Jump");
        }
    }

    private void SetupIsMoving()
    {
        firstPos = transform;
        lastPos = firstPos.position;
        isMoving = false;
    }

    private void CheckIfMoving()
    {
        isMoving = firstPos.position != lastPos;

        lastPos = firstPos.position;
    }

    private void Animation()
    {
        characterAnimator.SetBool("isMoving", isMoving);
        characterAnimator.SetBool("isGrounded", isGrounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "ImageTarget" || collision.gameObject.CompareTag("Environment"))
        {
            isGrounded = true;
        }
    }

    private void UnlockLastCollectible()
    {
        if (collectibleCount == 4)
        {
            lastCollectible.SetBool("IsGoingDown", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            GameObject collectible = other.gameObject;

            AudioSource collected = collectible.GetComponentInParent<AudioSource>();
            collected.Play(0);
            Destroy(collectible);

            collectibleCount += 1;
        }

        if (other.gameObject.name == "FailSafe")
        {
            foreach (var image in images)
            {
                if (image.TargetStatus.Status == Status.TRACKED)
                {
                    transform.position = image.transform.position + Vector3.up;
                    return;
                }
            }
            StartCoroutine(WaitForTracking());
        }
    }

    private IEnumerator WaitForTracking()
    {
        trackingLost = true;
        transform.position = Vector3.up;
        characterBody.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitUntil(() => images[0].TargetStatus.Status == Status.TRACKED);
        characterBody.constraints = RigidbodyConstraints.FreezeRotation;
        trackingLost = false;
    }

    void Start()
    {
        characterBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponent<Animator>();
        isGrounded = false;
        SetupIsMoving();
        Physics.gravity = new Vector3(0, -1f, 0);
    }

    private void Update()
    {
        InputMovements();
        Animation();
        CheckIfMoving();
        UnlockLastCollectible();
    }

    // Freezing character rotation
    private void PositionAndRotationFreeze(Rigidbody body)
    {
        body.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    private void FixedUpdate()
    {
        if (canJump && isGrounded)
        {
            characterBody.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            canJump = false;
            isGrounded = false;
        }

        if (imageTarget.TargetStatus.Status == Status.TRACKED)
        {
            characterBody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
