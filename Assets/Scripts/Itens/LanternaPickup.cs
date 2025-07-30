using UnityEngine;
using System.Collections;

public class LanternaPickup : MonoBehaviour
{
    [Header("Lanterna")]
    public GameObject lanternaPrefab;
    public Transform pontoDaMaoDoPlayer;

    [Header("Áudio")]
    public AudioClip somPickup;
    private AudioSource audioSource;

    private bool jogadorPerto = false;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1f;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.maxDistance = 10f;
    }

    void Update()
    {
        if (jogadorPerto && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PegarLanterna());
        }
    }

    IEnumerator PegarLanterna()
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

        if (somPickup != null)
            AudioSource.PlayClipAtPoint(somPickup, transform.position);

        Destroy(gameObject);


        yield return new WaitForSeconds(somPickup != null ? somPickup.length : 0f);

       
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
