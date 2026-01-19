using UnityEngine;

public class CrankyKong : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    public void PlaySound()
    {
        this.audioSource.Play();
    }
}