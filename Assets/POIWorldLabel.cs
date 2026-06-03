using UnityEngine;

/// <summary>
/// Faz o painel de texto do POI sempre olhar para a camera principal (billboard).
/// Coloque este script no GameObject pai do Canvas World Space do POI.
/// </summary>
public class POIWorldLabel : MonoBehaviour
{
    [Header("Visual do POI")]
    [Tooltip("Amplitude de flutuação vertical (metros)")]
    public float alturaFlutuacao = 0.15f;

    [Tooltip("Velocidade da flutuação")]
    public float velocidadeFlutuacao = 1.2f;

    private Vector3 posicaoBase;
    private Camera camPrincipal;

    void Start()
    {
        posicaoBase = transform.position;
        camPrincipal = Camera.main;
    }

    void Update()
    {
        // Efeito de flutuação suave
        float offsetY = Mathf.Sin(Time.time * velocidadeFlutuacao) * alturaFlutuacao;
        transform.position = posicaoBase + new Vector3(0, offsetY, 0);

        // Billboard: sempre olha para a câmera
        if (camPrincipal != null)
        {
            transform.LookAt(camPrincipal.transform);
            transform.Rotate(0, 180f, 0); // inverte para o texto ficar legível
        }
    }
}
