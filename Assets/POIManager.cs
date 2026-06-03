using UnityEngine;
using TMPro;

public class POIManager : MonoBehaviour
{
    public GameObject painelTexto;
    public TextMeshProUGUI textoPOI;

    void Start()
    {
        painelTexto.SetActive(false);
    }

    public void MostrarPOI(string mensagem)
    {
        painelTexto.SetActive(true);
        textoPOI.text = mensagem;
    }

    public void EsconderPOI()
    {
        painelTexto.SetActive(false);
    }
}