using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    // Bu fonksiyonu butona baðlayacaðýz
    public void SahneDegistir(string sahneAdi)
    {
        // Parantez içine yazdýðýn isimdeki sahneye yüklenir
        SceneManager.LoadScene(sahneAdi);
    }
}