using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    private CharacterController controller; // Character Controller komponent
    private Animator animator; // Animation komponent
    private Vector3 playerVelocity; //Hareket vektörü
    private bool groundedPlayer; //Karakterin yerde olup olmadýðý bilgisini tutar
    private float jumpHeight = 1.0f; // Zýplama yüksekliði
    private float gravityValue = -9.81f; // Yerçekimi deðeri
    [SerializeField] float playerSpeed = 2.0f; //Hýz

    private void Start()
    {
        //Character Controller kompanenti deðiþkene atanýr (Caching iþlemi yapýlýr)
        controller = gameObject.GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {


        //Karakterin yerde olup olmadýðý bilgisi deðiþkende saklanýr
        groundedPlayer = controller.isGrounded;
        animator.SetBool("isGrounded", groundedPlayer);
        Debug.Log(controller.isGrounded);
        if (groundedPlayer && playerVelocity.y < 0) //Karakter yerde ise ve y hýzý 0 dan küçükse
        {
            playerVelocity.y = 0f; // Y hýzý sýfýra eþitlenir
        }

        //Input deðerleri deðiþkene atanýr
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed); //Move fonksiyonu ile karakter yönlendirilir

        if (move != Vector3.zero) //Input deðerleri sýfýrdan farklýysa
        {
            //Karakterin ileri vektörü move vektörü ile güncellenir
            gameObject.transform.forward = move;

            animator.SetFloat("speed", 1);
        }
        else
            animator.SetFloat("speed", 0);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer) //Karakter yerdeyse ve zýplama tuþuna basýldýysa
        {
            //Karakter yerçekimi deðeri hesaplanýr
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -5.0f * gravityValue);  // * -3.0f
            animator.SetTrigger("jumping");
        }

        //Karaktere yerçekimi eklenir
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}

// Kontroller için farklý bir yöntem olarak: Transform.Translate kullanýlabilirdi. Mevcut move deðiþkenini bu metot içerisine aktarmak yeterli.
// Diðer bir yöntem olarak Character AddForce ile veya rigidbody'nin velocity deðerine move atanarak deðiþtirilebilirdi.
// Biraz daha farklý bir yöntem ise New Input System ile girdi alýp bunu üstteki metotlar ile kullanabiliriz.