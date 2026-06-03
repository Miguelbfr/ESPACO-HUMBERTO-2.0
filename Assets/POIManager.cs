using UnityEngine;
using TMPro;

/// <summary>
/// Gerencia a exibição do painel de UI com informações do Ponto de Interesse.
/// Coloque este script num GameObject com um Canvas (Screen Space - Overlay ou Camera).
/// Conecte o painelTexto e o textoPOI pelo Inspector.
/// </summary>
public class POIManager : MonoBehaviour
{
    [Header("Referências")]
    public GameObject painelTexto;
    public TextMeshProUGUI textoPOI;

    [Header("Comportamento")]
    [Tooltip("Se > 0, esconde o painel automaticamente após X segundos")]
    public float tempoAutoEsconder = 0f;

    private Coroutine rotinaNautomatica;

    void Start()
    {
        if (painelTexto != null)
            painelTexto.SetActive(false);
    }

    public void MostrarPOI(string mensagem)
    {
        if (painelTexto == null) return;

        painelTexto.SetActive(true);

        if (textoPOI != null)
            textoPOI.text = mensagem;

        if (tempoAutoEsconder > 0f)
        {
            if (rotinaNautomatica != null)
                StopCoroutine(rotinaNautomatica);
            rotinaNautomatica = StartCoroutine(EsconderApos(tempoAutoEsconder));
        }
    }

    public void EsconderPOI()
    {
        if (rotinaNautomatica != null)
        {
            StopCoroutine(rotinaNautomatica);
            rotinaNautomatica = null;
        }

        if (painelTexto != null)
            painelTexto.SetActive(false);
    }

    private System.Collections.IEnumerator EsconderApos(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        EsconderPOI();
    }
}