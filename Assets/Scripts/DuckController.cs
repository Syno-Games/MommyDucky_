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
       
            // ... (di�er kodlar�n�z)

       if (BodyParts.Count == 0 && hitWall == true)
       {
          // T�m v�cut par�alar� yoksa, oyuncuyu kaybetme i�lemini ba�lat�n.
          LoseGame();
       }
        
    }

    private void GrowDuck()
    {
        GameObject body = Instantiate(PuppyPrefab);
        BodyParts.Add(body);

        // 3 saniye sonra tag'i de�i�tirmek i�in Invoke kullan�n.
        Invoke("ChangeTag", 0.5f);
    }

    private void ChangeTag()
    {
        // Yeni objenin tag'ini de�i�tirin.
        if (BodyParts.Count > 0)
        {
            GameObject lastBodyPart = BodyParts[BodyParts.Count - 1];
            lastBodyPart.tag = "TailReady"; // Yeni tag'i burada ayarlay�n.
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
            GrowDuck(); // �rdek b�y�tme fonksiyonunu �a��r�n.
            Destroy(other.gameObject); // Yumurta nesnesini yok edin veya devre d��� b�rak�n, b�ylece bir daha kullan�lamaz.
        }
        if (other.CompareTag("Coop"))
        {
            ShrinkDuck(); // �rdek v�cut par�alar�n� 5 eksiltme fonksiyonunu �a��r�n.

            // Skorunuza 5 ekleyin. (Skorunuzu oyununuzun mevcut skor sistemine g�re ayarlaman�z gerekebilir.)
            // �rne�in, bir skor de�i�kenini art�rabilirsiniz:
            // score += 5;
        }
        if (other.CompareTag("TailReady")) // �rdek, PuppyPrefab veya di�er �rdek par�alar�na �arparsa
        {
            ShrinkDuck(); // �rdek v�cut par�alar�n� eksiltme fonksiyonunu �a��r�n.
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // Duvar nesnesinin etiketini "Wall" olarak kabul ediyoruz.
        {
            ShrinkDuck(); // �rdek v�cut par�alar�n� k���ltme fonksiyonunu �a��r�n.
            Debug.Log("Duvara �arp�ld�!");
            hitWall = true;
        }
    }
    
    void LoseGame()
    {
        // Oyuncuyu kaybetme i�lemini burada ger�ekle�tirin.
        // �rne�in, bir kaybetme ekran� g�sterebilir veya oyunu s�f�rlayabilirsiniz.
        // �rdek karakterini veya di�er gerekli i�lemleri s�f�rlayabilirsiniz.

        // �rnek: Oyunu yeniden ba�latma
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // �rnek: Kaybetme ekran�n� g�sterme
        // loseScreen.SetActive(true);
        Debug.Log("Kaybettin");
    }
}
