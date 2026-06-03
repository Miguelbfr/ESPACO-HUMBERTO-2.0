using UnityEngine;
using TMPro;

/// <summary>
/// Ponto de Interesse interativo com texto personalizado.
/// Ao entrar na zona de trigger, exibe o painel de informações na UI.
/// Ao sair, esconde.
/// </summary>
public class POIInterativo : MonoBehaviour
{
    [Header("Painel de UI")]
    [Tooltip("Arraste aqui o GameObject do painel (Canvas/Panel) que exibe as informações")]
    public GameObject painelInfo;

    [Header("Conteúdo do POI")]
    [Tooltip("Título do ponto de interesse")]
    public string titulo = "Título do POI";

    [TextArea(3, 10)]
    [Tooltip("Descrição completa do ponto de interesse")]
    public string descricao = "Descrição do ponto de interesse.";

    [Header("Referências de Texto (opcional)")]
    [Tooltip("TextMeshPro para o título (deixe vazio se o texto estiver fixo no prefab)")]
    public TextMeshProUGUI textoTitulo;

    [Tooltip("TextMeshPro para a descrição (deixe vazio se o texto estiver fixo no prefab)")]
    public TextMeshProUGUI textoDescricao;

    void Start()
    {
        if (painelInfo != null)
            painelInfo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (painelInfo == null) return;

        // Atualiza os textos se as referências estiverem configuradas
        if (textoTitulo != null)
            textoTitulo.text = titulo;

        if (textoDescricao != null)
            textoDescricao.text = descricao;

        painelInfo.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (painelInfo != null)
            painelInfo.SetActive(false);
    }
}
