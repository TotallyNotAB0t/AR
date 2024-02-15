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


    //Gère les mouvements à partir d'inputs du clavier
    private void InputMovements()
    {
        if (trackingLost) return;
        //Récuparation des mouvements à partir du clavier
        float horizontalInput = SimpleInput.GetAxis("Horizontal");
        float verticalInput = SimpleInput.GetAxis("Vertical");

        //On store chaque mouvement dans une variable locale
        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        //Le personnage avance ensuite dans cette direction
        transform.Translate(moveSpeed * Time.deltaTime * movementDirection, Space.World);
        if (movementDirection != Vector3.zero)
        {
            //Utilisation de Quaternion pour rendre la rotation moins soudaine
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        //Saut du personnage
        if (!canJump && isGrounded)
        {
            //canJump = Input.GetKeyDown(KeyCode.Space);
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

    //Gère les animations du personnage
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

    //Lors de la collision avec le collider du collectible, destruction de l'élément touché
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Collectible"))
        {
            GameObject collectible = other.gameObject;

            //Récupération du son via objet parent
            AudioSource collected = collectible.GetComponentInParent<AudioSource>();
            collected.Play(0);
            Destroy(collectible);

            //Incrémentation du compteur
            collectibleCount += 1;
        }

        //On reload la scene si on tombe
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

    // Start is called before the first frame update
    void Start()
    {
        //Récupération du rigidbody de l'élément attaché a ce script
        characterBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponent<Animator>();
        isGrounded = false;

        SetupIsMoving();

        //Réduction de la gravité vu que l'on travaille sur des objets très petits
        Physics.gravity = new Vector3(0, -1f, 0);

    }

    // Update is called once per frame
    private void Update()
    {
        InputMovements();
        Animation();
        CheckIfMoving();
        UnlockLastCollectible();
    }

    //Permet de freeze le personnage
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

        //Lorsque l'image est détectée, on débloque notre personnage sur l'axe des Y
        if (imageTarget.TargetStatus.Status == Status.TRACKED)
        {
            characterBody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        //Si on perds l'image, on stoppe notre personnage sur tous les axes
        /*if (imageTarget.TargetStatus.Status == Status.NO_POSE)
        {
            Debug.Log("no");
            PositionAndRotationFreeze(characterBody);
        }*/
    }
}
