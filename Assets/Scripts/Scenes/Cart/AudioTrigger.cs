using UnityEngine;

public class AudioTrigger : Callback
{
    private AudioSource audioSource;

    public override void Trigger()
    {
        if (this.audioSource != null)
        {
            this.audioSource.loop = false;
        }
    }

    private void Awake()
    {
        this.audioSource = base.GetComponent<AudioSource>();
    }
}