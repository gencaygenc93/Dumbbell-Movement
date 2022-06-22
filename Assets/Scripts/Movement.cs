using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private CharacterController controller; // Character Controller komponent
    private Animator animator; // Animation komponent
    private Vector3 playerVelocity; //Hareket vekt�r�
    private bool groundedPlayer; //Karakterin yerde olup olmad��� bilgisini tutar
    private float jumpHeight = 1.0f; // Z�plama y�ksekli�i
    private float gravityValue = -9.81f; // Yer�ekimi de�eri
    [SerializeField] float playerSpeed = 2.0f; //H�z

    private void Start()
    {
        //Character Controller kompanenti de�i�kene atan�r (Caching i�lemi yap�l�r)
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {


        //Karakterin yerde olup olmad��� bilgisi de�i�kende saklan�r
        groundedPlayer = controller.isGrounded;
        animator.SetBool("isGrounded", groundedPlayer);
        Debug.Log(controller.isGrounded);
        if (groundedPlayer && playerVelocity.y < 0) //Karakter yerde ise ve y h�z� 0 dan k���kse
        {
            playerVelocity.y = 0f; // Y h�z� s�f�ra e�itlenir
        }

        //Input de�erleri de�i�kene atan�r
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed); //Move fonksiyonu ile karakter y�nlendirilir

        if (move != Vector3.zero) //Input de�erleri s�f�rdan farkl�ysa
        {
            //Karakterin ileri vekt�r� move vekt�r� ile g�ncellenir
            gameObject.transform.forward = move;

            animator.SetFloat("speed", 1);
        }
        else
            animator.SetFloat("speed", 0);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer) //Karakter yerdeyse ve z�plama tu�una bas�ld�ysa
        {
            //Karakter yer�ekimi de�eri hesaplan�r
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -5.0f * gravityValue);  // * -3.0f
            animator.SetTrigger("jumping");
        }

        //Karaktere yer�ekimi eklenir
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}

// Kontroller i�in farkl� bir y�ntem olarak: Transform.Translate kullan�labilirdi. Mevcut move de�i�kenini bu metot i�erisine aktarmak yeterli.
// Di�er bir y�ntem olarak Character AddForce ile veya rigidbody'nin velocity de�erine move atanarak de�i�tirilebilirdi.
// Biraz daha farkl� bir y�ntem ise New Input System ile girdi al�p bunu �stteki metotlar ile kullanabiliriz.