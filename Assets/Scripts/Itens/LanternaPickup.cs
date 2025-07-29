using UnityEngine;

public class LanternaPickup : MonoBehaviour
{
    public GameObject lanternaPrefab;
    public Transform pontoDaMaoDoPlayer;

    private bool jogadorPerto = false;

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            GameObject lanternaInstanciada = Instantiate(
                lanternaPrefab,
                pontoDaMaoDoPlayer.position,
                pontoDaMaoDoPlayer.rotation,
                pontoDaMaoDoPlayer
            );

          
            Light luz = lanternaInstanciada.GetComponentInChildren<Light>();
            if (luz != null)
                luz.enabled = false;

           
            PlayerManager player = FindObjectOfType<PlayerManager>();
            player.RegistrarNovaLanterna(lanternaInstanciada);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jogadorPerto = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jogadorPerto = false;
    }
}
