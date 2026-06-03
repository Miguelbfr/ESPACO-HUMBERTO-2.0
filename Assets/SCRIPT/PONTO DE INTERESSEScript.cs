using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject pontodeinteresse;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        pontodeinteresse.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        pontodeinteresse.SetActive(false);
    }
}