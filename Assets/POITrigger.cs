using UnityEngine;

public class POITrigger : MonoBehaviour
{
    [TextArea]
    public string mensagem;

    public POIManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            manager.MostrarPOI(mensagem);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            manager.EsconderPOI();
        }
    }
}