using UnityEngine;

public class DoorSoundTrigger : MonoBehaviour
{
    public string playerTag = "Player";

    private MeshRenderer meshRenderer;
    private BoxCollider boxCollider;

    private bool alreadyTriggered = false;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();

        if (meshRenderer != null)
            meshRenderer.enabled = false;

        if (boxCollider != null)
            boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered) return;

        if (other.CompareTag(playerTag))
        {
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (playerAudio != null)
            {
                playerAudio.Play();
            }

            if (meshRenderer != null)
            {
                meshRenderer.enabled = true;
            }

            if (boxCollider != null)
            {
                boxCollider.isTrigger = false;
            }

            alreadyTriggered = true;
        }
    }
}
