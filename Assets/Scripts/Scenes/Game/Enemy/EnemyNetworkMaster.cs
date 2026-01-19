using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(EnemyMaster))]
public class EnemyNetworkMaster : NetworkBehaviour
{
    [SerializeField] private float threshold = 0.01f;

    private SpriteRenderer spriteRenderer;

    private float x = 0f;

    public void Die()
    {
        this.HandleDeathServerRpc();
    }

    private void Awake()
    {
        this.spriteRenderer = base.GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (base.IsOwner)
        {
            EnemyInput enemyInput = base.GetComponent<EnemyInput>();

            enemyInput.ActivateAlternative();
        }
        else
        {
            base.GetComponent<EnemyMaster>().enabled = false;
            base.GetComponent<EnemyAnimation>().enabled = false;
            base.GetComponent<EnemyInput>().enabled = false;
            base.GetComponent<Enemy>().enabled = false;
        }

        EnemyCollision enemyCollision = base.GetComponent<EnemyCollision>();

        enemyCollision.ActivateAlternativeDeath();
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner)
        {
            float currentX = base.transform.position.x;

            float difference = currentX - this.x;

            if (difference > this.threshold || difference < -this.threshold)
            {
                this.spriteRenderer.flipX = difference < 0f;
            }

            this.x = currentX;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void HandleDeathServerRpc()
    {
        this.HandleDeathClientRpc();

        EnemyCollision enemyCollision = base.GetComponent<EnemyCollision>();

        enemyCollision.AlternativeDeath();
    }

    [ClientRpc]
    private void HandleDeathClientRpc()
    {
        EnemyMaster enemyMaster = base.GetComponent<EnemyMaster>();

        enemyMaster.DisableCollider();
    }
}