using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class DuckController : MonoBehaviour
{
    [SerializeField] private DynamicJoystick dynamicJoystic;

    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public int Gap = 30;
    public float BodySpeed = 5;

    private bool hitWall = false;

    public GameObject PuppyPrefab;

    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        
    }
  
    // Update is called once per frame
    void Update()
    {
       
        // move forward
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        //  float steerDirection = Input.GetAxis("Horizontal");
        float steerDirection = dynamicJoystic.Horizontal;
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        PositionsHistory.Insert(0, transform.position);
        
        int index = 0;
        foreach (var body in BodyParts) 
        {
            Vector3 point = PositionsHistory[Mathf.Min(index *Gap, PositionsHistory.Count-2)];
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;
            body.transform.LookAt(point);
            index++;
        }
       
            // ... (diðer kodlarýnýz)

       if (BodyParts.Count == 0 && hitWall == true)
       {
          // Tüm vücut parçalarý yoksa, oyuncuyu kaybetme iþlemini baþlatýn.
          LoseGame();
       }
        
    }

    private void GrowDuck()
    {
        GameObject body = Instantiate(PuppyPrefab);
        BodyParts.Add(body);

        // 3 saniye sonra tag'i deðiþtirmek için Invoke kullanýn.
        Invoke("ChangeTag", 0.5f);
    }

    private void ChangeTag()
    {
        // Yeni objenin tag'ini deðiþtirin.
        if (BodyParts.Count > 0)
        {
            GameObject lastBodyPart = BodyParts[BodyParts.Count - 1];
            lastBodyPart.tag = "TailReady"; // Yeni tag'i burada ayarlayýn.
        }
    }

    private void ShrinkDuck()
    {
        if (BodyParts.Count > 0)
        {
            GameObject body = BodyParts[BodyParts.Count - 1];
            BodyParts.RemoveAt(BodyParts.Count - 1);
            Destroy(body);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Egg")) // Yumurta nesnesinin etiketini "Egg" olarak kabul ediyoruz.
        {
            GrowDuck(); // Ördek büyütme fonksiyonunu çaðýrýn.
            Destroy(other.gameObject); // Yumurta nesnesini yok edin veya devre dýþý býrakýn, böylece bir daha kullanýlamaz.
        }
        if (other.CompareTag("Coop"))
        {
            ShrinkDuck(); // Ördek vücut parçalarýný 5 eksiltme fonksiyonunu çaðýrýn.

            // Skorunuza 5 ekleyin. (Skorunuzu oyununuzun mevcut skor sistemine göre ayarlamanýz gerekebilir.)
            // Örneðin, bir skor deðiþkenini artýrabilirsiniz:
            // score += 5;
        }
        if (other.CompareTag("TailReady")) // Ördek, PuppyPrefab veya diðer ördek parçalarýna çarparsa
        {
            ShrinkDuck(); // Ördek vücut parçalarýný eksiltme fonksiyonunu çaðýrýn.
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Duvar nesnesinin etiketini "Wall" olarak kabul ediyoruz.
        {
            ShrinkDuck(); // Ördek vücut parçalarýný küçültme fonksiyonunu çaðýrýn.
            Debug.Log("Duvara çarpýldý!");
            hitWall = true;
        }
    }
    
    void LoseGame()
    {
        // Oyuncuyu kaybetme iþlemini burada gerçekleþtirin.
        // Örneðin, bir kaybetme ekraný gösterebilir veya oyunu sýfýrlayabilirsiniz.
        // Ördek karakterini veya diðer gerekli iþlemleri sýfýrlayabilirsiniz.

        // Örnek: Oyunu yeniden baþlatma
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // Örnek: Kaybetme ekranýný gösterme
        // loseScreen.SetActive(true);
        Debug.Log("Kaybettin");
    }
}
