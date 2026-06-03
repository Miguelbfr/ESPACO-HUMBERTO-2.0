using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Editor Script — cria 3 Pontos de Interesse na cena:
///   1. Prédio 1 (Galpão Histórico do Trapiche)
///   2. Prédio 2 (Espaço de Convivência e Cultura)
///   3. Chaminés Industriais
///
/// Menu: Tools > Espaço Humberto > Criar Pontos de Interesse
/// </summary>
public static class CriarPontosDeInteresse
{
    // ── GUID do prefab ponto-de-interesse ─────────────────────────
    private const string PREFAB_GUID = ""; // Deixamos vazio: criamos via script

    // ── Dados de cada POI ─────────────────────────────────────────
    private struct DadosPOI
    {
        public string nome;
        public Vector3 posicao;
        public string titulo;
        public string descricao;
    }

    private static readonly DadosPOI[] POIS = new DadosPOI[]
    {
        new DadosPOI
        {
            nome      = "POI_Predio1_Trapiche",
            posicao   = new Vector3(34.03f, 1.4f, 19.0f),   // frente do PREDIO 1
            titulo    = "Galpão Histórico do Trapiche",
            descricao = "Você está diante de uma estrutura que preserva parte da memória " +
                        "econômica e arquitetônica de São Luís. Os galpões do Trapiche " +
                        "desempenharam importante função no armazenamento e movimentação de " +
                        "mercadorias, conectando o porto às atividades comerciais da cidade."
        },
        new DadosPOI
        {
            nome      = "POI_Predio2_Convivencia",
            posicao   = new Vector3(10.73f, 1.5f, 56.5f),   // frente do PREDIO 2
            titulo    = "Prédio 2 – Espaço de Convivência e Cultura",
            descricao = "Este edifício representa a adaptação de estruturas históricas para " +
                        "novos usos, integrando patrimônio, cultura e convivência. Atualmente, " +
                        "espaços como este contribuem para aproximar a população da história " +
                        "local, promovendo atividades culturais, educacionais e turísticas."
        },
        new DadosPOI
        {
            nome      = "POI_Chamines_Industriais",
            posicao   = new Vector3(18.5f, 2.0f, 12.5f),    // entre Chamine1 (21.98,0,17.03) e Chamine2 (14.97,0,16.89)
            titulo    = "Chaminés Industriais",
            descricao = "Estas chaminés representam a presença das atividades industriais " +
                        "que contribuíram para o desenvolvimento econômico de São Luís. " +
                        "Estruturas como estas faziam parte de instalações produtivas que " +
                        "impulsionavam o comércio e a circulação de mercadorias na região."
        }
    };

    // ── Configurações visuais ─────────────────────────────────────
    private static readonly Color COR_MARCADOR   = new Color(0.95f, 0.60f, 0.10f); // laranja
    private static readonly Color COR_FUNDO      = new Color(0.05f, 0.05f, 0.10f, 0.92f);
    private static readonly Color COR_TITULO_TXT = new Color(1.0f,  0.85f, 0.25f);
    private static readonly Color COR_BORDA      = new Color(0.95f, 0.60f, 0.10f);

    // ─────────────────────────────────────────────────────────────
    [MenuItem("Tools/Espaço Humberto/Criar Pontos de Interesse")]
    public static void CriarTodos()
    {
        int criados = 0;
        foreach (var poi in POIS)
        {
            CriarPOI(poi);
            criados++;
        }

        EditorUtility.DisplayDialog(
            "POIs Criados!",
            $"{criados} Pontos de Interesse foram adicionados à cena.\n\n" +
            "• POI_Predio1_Trapiche\n" +
            "• POI_Predio2_Convivencia\n" +
            "• POI_Chamines_Industriais\n\n" +
            "Salve a cena (Ctrl+S) para preservar as alterações.",
            "OK"
        );
    }

    // ─────────────────────────────────────────────────────────────
    private static void CriarPOI(DadosPOI dados)
    {
        // Remove se já existe
        GameObject existente = GameObject.Find(dados.nome);
        if (existente != null)
            GameObject.DestroyImmediate(existente);

        // ── Raiz ─────────────────────────────────────────────────
        GameObject raiz = new GameObject(dados.nome);
        raiz.transform.position = dados.posicao;

        // ── Trigger de colisão (zona de detecção) ─────────────────
        SphereCollider trigger = raiz.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 4f;

        // ── Script POI Interativo ──────────────────────────────────
        POIInterativo script = raiz.AddComponent<POIInterativo>();
        script.titulo    = dados.titulo;
        script.descricao = dados.descricao;

        // ── Marcador visual 3D ─────────────────────────────────────
        CriarMarcador3D(raiz);

        // ── Canvas World-Space com painel de texto ─────────────────
        GameObject canvasGO = CriarCanvasInfo(raiz, dados.titulo, dados.descricao);

        // Conecta o painel ao script
        script.painelInfo = canvasGO;

        // ── Registra Undo e seleciona ──────────────────────────────
        Undo.RegisterCreatedObjectUndo(raiz, $"Criar {dados.nome}");
        Selection.activeGameObject = raiz;

        Debug.Log($"[POI] '{dados.nome}' criado em {dados.posicao}");
    }

    // ─────────────────────────────────────────────────────────────
    private static void CriarMarcador3D(GameObject pai)
    {
        // Poste
        GameObject poste = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        poste.name = "Poste";
        poste.transform.SetParent(pai.transform);
        poste.transform.localPosition = new Vector3(0f, -1.5f, 0f);
        poste.transform.localScale    = new Vector3(0.07f, 1.5f, 0.07f);
        Object.DestroyImmediate(poste.GetComponent<CapsuleCollider>());
        AplicarMaterial(poste, COR_MARCADOR);

        // Ícone esfera
        GameObject esfera = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        esfera.name = "IconePOI";
        esfera.transform.SetParent(pai.transform);
        esfera.transform.localPosition = new Vector3(0f, 0.3f, 0f);
        esfera.transform.localScale    = new Vector3(0.45f, 0.45f, 0.45f);
        Object.DestroyImmediate(esfera.GetComponent<SphereCollider>());
        AplicarMaterial(esfera, COR_MARCADOR);
    }

    private static void AplicarMaterial(GameObject go, Color cor)
    {
        var rend = go.GetComponent<Renderer>();
        if (rend == null) return;
        var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        if (mat.shader == null || mat.shader.name == "Hidden/InternalErrorShader")
            mat = new Material(Shader.Find("Standard"));
        mat.color = cor;
        rend.sharedMaterial = mat;
    }

    // ─────────────────────────────────────────────────────────────
    private static GameObject CriarCanvasInfo(GameObject pai, string titulo, string descricao)
    {
        // Canvas (Screen Space - Overlay) — aparece no HUD quando o player entra no trigger
        GameObject canvasGO = new GameObject("PainelPOI");
        canvasGO.transform.SetParent(pai.transform);
        canvasGO.transform.localPosition = Vector3.zero;

        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;
        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<GraphicRaycaster>();

        // Painel de fundo
        GameObject painel = new GameObject("Painel");
        painel.transform.SetParent(canvasGO.transform, false);
        Image imgPainel = painel.AddComponent<Image>();
        imgPainel.color = COR_FUNDO;
        RectTransform rtPainel = painel.GetComponent<RectTransform>();
        rtPainel.anchorMin = new Vector2(0.5f, 0.0f);
        rtPainel.anchorMax = new Vector2(0.5f, 0.0f);
        rtPainel.pivot     = new Vector2(0.5f, 0.0f);
        rtPainel.sizeDelta = new Vector2(680f, 220f);
        rtPainel.anchoredPosition = new Vector2(0f, 40f);

        // Borda superior colorida
        GameObject borda = new GameObject("BordaSuperior");
        borda.transform.SetParent(painel.transform, false);
        Image imgBorda = borda.AddComponent<Image>();
        imgBorda.color = COR_BORDA;
        RectTransform rtBorda = borda.GetComponent<RectTransform>();
        rtBorda.anchorMin = new Vector2(0f, 1f);
        rtBorda.anchorMax = new Vector2(1f, 1f);
        rtBorda.pivot     = new Vector2(0.5f, 1f);
        rtBorda.sizeDelta = new Vector2(0f, 5f);
        rtBorda.anchoredPosition = Vector2.zero;

        // Título
        GameObject tituloGO = new GameObject("TextoTitulo");
        tituloGO.transform.SetParent(painel.transform, false);
        TextMeshProUGUI tmpTitulo = tituloGO.AddComponent<TextMeshProUGUI>();
        tmpTitulo.text      = titulo;
        tmpTitulo.fontSize  = 22f;
        tmpTitulo.fontStyle = FontStyles.Bold;
        tmpTitulo.color     = COR_TITULO_TXT;
        tmpTitulo.alignment = TextAlignmentOptions.Left;
        RectTransform rtTitulo = tituloGO.GetComponent<RectTransform>();
        rtTitulo.anchorMin        = new Vector2(0.03f, 0.60f);
        rtTitulo.anchorMax        = new Vector2(0.97f, 0.95f);
        rtTitulo.sizeDelta        = Vector2.zero;
        rtTitulo.anchoredPosition = Vector2.zero;

        // Descrição
        GameObject descGO = new GameObject("TextoDescricao");
        descGO.transform.SetParent(painel.transform, false);
        TextMeshProUGUI tmpDesc = descGO.AddComponent<TextMeshProUGUI>();
        tmpDesc.text      = descricao;
        tmpDesc.fontSize  = 15f;
        tmpDesc.color     = Color.white;
        tmpDesc.alignment = TextAlignmentOptions.Left;
        RectTransform rtDesc = descGO.GetComponent<RectTransform>();
        rtDesc.anchorMin        = new Vector2(0.03f, 0.05f);
        rtDesc.anchorMax        = new Vector2(0.97f, 0.58f);
        rtDesc.sizeDelta        = Vector2.zero;
        rtDesc.anchoredPosition = Vector2.zero;

        // Começa escondido
        canvasGO.SetActive(false);

        return canvasGO;
    }
}
