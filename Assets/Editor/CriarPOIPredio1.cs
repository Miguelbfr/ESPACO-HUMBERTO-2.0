using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Script de Editor para criar um Ponto de Interesse (POI)
/// automaticamente na frente do Predio 1 (ARMAZEM PRINCIPAL).
///
/// Como usar: Menu Unity > Tools > Espaco Humberto > Criar POI Predio 1
/// </summary>
public class CriarPOIPredio1 : Editor
{
    // Posição do ARMAZEM PRINCIPAL na cena: (73.1, 0, -46.18)
    // Colocamos o POI na frente (z positivo = frente do prédio)
    private static readonly Vector3 PosicaoPOI = new Vector3(73.1f, 2.5f, -36f);

    private const string NOME_POI = "POI_Predio1";
    private const string TEXTO_TITULO = "Prédio 1 – Armazém Principal";
    private const string TEXTO_DESCRICAO =
        "O Armazém Principal do Espaço Humberto é o coração do complexo cultural.\n" +
        "Aqui são realizadas exposições, ensaios e eventos da tradição do maracatu.";

    [MenuItem("Tools/Espaço Humberto/Criar POI Predio 1")]
    public static void CriarPOI()
    {
        // Verifica se já existe
        GameObject existente = GameObject.Find(NOME_POI);
        if (existente != null)
        {
            bool substituir = EditorUtility.DisplayDialog(
                "POI já existe",
                $"O objeto '{NOME_POI}' já existe na cena. Deseja substituí-lo?",
                "Sim, substituir",
                "Cancelar"
            );
            if (!substituir) return;
            DestroyImmediate(existente);
        }

        // ===== Cria o GameObject pai do POI =====
        GameObject poiRaiz = new GameObject(NOME_POI);
        poiRaiz.transform.position = PosicaoPOI;

        // --- Ícone 3D (cilindro marcador) ---
        GameObject marcador = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        marcador.name = "Marcador";
        marcador.transform.SetParent(poiRaiz.transform);
        marcador.transform.localPosition = new Vector3(0, -1.8f, 0);
        marcador.transform.localScale = new Vector3(0.08f, 1.8f, 0.08f);

        // Material laranja/amarelo para o marcador
        Renderer rend = marcador.GetComponent<Renderer>();
        Material matMarcador = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        matMarcador.color = new Color(1f, 0.55f, 0f); // laranja
        matMarcador.name = "Mat_POI_Marcador";
        rend.sharedMaterial = matMarcador;

        // Remove colisor do marcador (decorativo apenas)
        DestroyImmediate(marcador.GetComponent<CapsuleCollider>());

        // --- Ícone (esfera no topo) ---
        GameObject icone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        icone.name = "Icone";
        icone.transform.SetParent(poiRaiz.transform);
        icone.transform.localPosition = new Vector3(0, 0.3f, 0);
        icone.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        DestroyImmediate(icone.GetComponent<SphereCollider>());

        Renderer rendIcone = icone.GetComponent<Renderer>();
        Material matIcone = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        matIcone.color = new Color(1f, 0.55f, 0f);
        matIcone.name = "Mat_POI_Icone";
        rendIcone.sharedMaterial = matIcone;

        // --- Canvas World Space (painel de texto flutuante) ---
        GameObject canvasGO = new GameObject("Canvas_POI");
        canvasGO.transform.SetParent(poiRaiz.transform);
        canvasGO.transform.localPosition = new Vector3(0, 1.2f, 0);
        canvasGO.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        RectTransform canvasRT = canvasGO.GetComponent<RectTransform>();
        canvasRT.sizeDelta = new Vector2(400, 200);

        // Fundo semi-transparente
        GameObject fundo = new GameObject("Fundo");
        fundo.transform.SetParent(canvasGO.transform, false);
        Image imgFundo = fundo.AddComponent<Image>();
        imgFundo.color = new Color(0.05f, 0.05f, 0.05f, 0.88f);
        RectTransform fundoRT = fundo.GetComponent<RectTransform>();
        fundoRT.anchorMin = Vector2.zero;
        fundoRT.anchorMax = Vector2.one;
        fundoRT.sizeDelta = Vector2.zero;

        // Borda colorida (segundo Image como borda)
        GameObject borda = new GameObject("Borda");
        borda.transform.SetParent(canvasGO.transform, false);
        Image imgBorda = borda.AddComponent<Image>();
        imgBorda.color = new Color(1f, 0.55f, 0f, 1f);
        RectTransform bordaRT = borda.GetComponent<RectTransform>();
        bordaRT.anchorMin = new Vector2(0, 0);
        bordaRT.anchorMax = new Vector2(1, 0);
        bordaRT.sizeDelta = new Vector2(0, 6);
        bordaRT.anchoredPosition = new Vector2(0, 3);
        borda.transform.SetSiblingIndex(1);

        // Texto Título
        GameObject tituloGO = new GameObject("Titulo");
        tituloGO.transform.SetParent(canvasGO.transform, false);
        TextMeshProUGUI tituloTMP = tituloGO.AddComponent<TextMeshProUGUI>();
        tituloTMP.text = TEXTO_TITULO;
        tituloTMP.fontSize = 28;
        tituloTMP.fontStyle = FontStyles.Bold;
        tituloTMP.color = new Color(1f, 0.75f, 0.2f);
        tituloTMP.alignment = TextAlignmentOptions.Center;
        RectTransform tituloRT = tituloGO.GetComponent<RectTransform>();
        tituloRT.anchorMin = new Vector2(0.05f, 0.55f);
        tituloRT.anchorMax = new Vector2(0.95f, 0.95f);
        tituloRT.sizeDelta = Vector2.zero;

        // Texto Descrição
        GameObject descGO = new GameObject("Descricao");
        descGO.transform.SetParent(canvasGO.transform, false);
        TextMeshProUGUI descTMP = descGO.AddComponent<TextMeshProUGUI>();
        descTMP.text = TEXTO_DESCRICAO;
        descTMP.fontSize = 18;
        descTMP.color = Color.white;
        descTMP.alignment = TextAlignmentOptions.Center;
        RectTransform descRT = descGO.GetComponent<RectTransform>();
        descRT.anchorMin = new Vector2(0.05f, 0.08f);
        descRT.anchorMax = new Vector2(0.95f, 0.55f);
        descRT.sizeDelta = Vector2.zero;

        // Adiciona o script de billboard (sempre olha para a câmera)
        POIWorldLabel label = canvasGO.AddComponent<POIWorldLabel>();
        label.alturaFlutuacao = 0.12f;
        label.velocidadeFlutuacao = 1.0f;

        // Trigger de colisão (ao se aproximar exibe info no log)
        SphereCollider trigger = poiRaiz.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 3f;

        // Registra o Undo para poder desfazer pelo Unity
        Undo.RegisterCreatedObjectUndo(poiRaiz, "Criar POI Predio 1");

        // Seleciona o novo objeto no Editor
        Selection.activeGameObject = poiRaiz;

        // Foca a cena no novo objeto
        SceneView.lastActiveSceneView?.FrameSelected();

        Debug.Log($"[POI] Ponto de interesse '{NOME_POI}' criado com sucesso na posição {PosicaoPOI}!");
        EditorUtility.DisplayDialog("POI Criado!", $"'{TEXTO_TITULO}' foi adicionado à cena na frente do Prédio 1.", "OK");
    }
}
