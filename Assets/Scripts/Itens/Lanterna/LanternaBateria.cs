using UnityEngine;
using TMPro;

public class LanternaBateria : MonoBehaviour
{
    public float bateria = 100f;
    public float tempoPorPorcentagem = 30f;
    public Light luzDaLanterna;
    public TextMeshProUGUI textoBateria;

    public bool estaNaMao = false;

    private float timer = 0f;
    private bool textoVisivelAnterior = false;

    void Start()
    {
        if (textoBateria != null && textoBateria.transform.parent != null)
            textoBateria.transform.parent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (estaNaMao && !textoVisivelAnterior)
        {
            if (textoBateria != null && textoBateria.transform.parent != null)
                textoBateria.transform.parent.gameObject.SetActive(true);

            textoVisivelAnterior = true;
        }
        else if (!estaNaMao && textoVisivelAnterior)
        {
            if (textoBateria != null && textoBateria.transform.parent != null)
                textoBateria.transform.parent.gameObject.SetActive(false);

            textoVisivelAnterior = false;
        }

        if (estaNaMao && luzDaLanterna != null && luzDaLanterna.enabled)
        {
            timer += Time.deltaTime;

            if (timer >= tempoPorPorcentagem && bateria > 0f)
            {
                bateria -= 1f;
                bateria = Mathf.Clamp(bateria, 0f, 100f);
                timer = 0f;

                if (bateria <= 0f)
                {
                    luzDaLanterna.enabled = false;
                }
            }
        }

        if (estaNaMao && textoBateria != null && textoBateria.transform.parent.gameObject.activeSelf)
        {
            textoBateria.text = Mathf.RoundToInt(bateria).ToString();
        }
    }
}
