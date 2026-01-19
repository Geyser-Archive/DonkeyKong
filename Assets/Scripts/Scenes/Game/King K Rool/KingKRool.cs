using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BossInput), typeof(BossAnimation), typeof(BossCollision))]
public class KingKRool : MonoBehaviour
{
    [SerializeField] private GameObject target;

    [SerializeField] private GameObject crown;
    [SerializeField] private GameObject cannonBall;

    [SerializeField] private Transform[] cannonBallSpawns;

    [SerializeField] private float initialWait = 4.5f;
    [SerializeField] private float wait = 2f;
    [SerializeField] private float runWait = 1f;
    [SerializeField] private float throwWait = 6f;
    [SerializeField] private float jumpWait = 10f;

    [SerializeField] private int[] modifiers = { 6, 3, 1 };

    [SerializeField] private int defaultThrowCooldown;
    [SerializeField] private int defaultJumpCooldown;

    [SerializeField] private int guaranteedThrow;
    [SerializeField] private int guaranteedJump;

    [SerializeField] private bool guaranteeThrow = false;
    [SerializeField] private bool guaranteeJump = true;

    [SerializeField] private bool guaranteeThrowAfterCooldown = true;
    [SerializeField] private bool guaranteeJumpAfterCooldown = false;

    private BossInput bossInput;
    private BossAnimation bossAnimation;
    private BossCollision bossCollision;

    private Animator animator;

    private float time = 0f;
    private float timer = 0f;

    private int throwCount = 0;
    private int jumpCount = 0;

    private int throwCooldown;
    private int jumpCooldown;

    private bool moving = false;

    private int lastHealth = 0;

    private void Awake()
    {
        this.bossInput = base.GetComponent<BossInput>();
        this.bossAnimation = base.GetComponent<BossAnimation>();
        this.bossCollision = base.GetComponent<BossCollision>();

        this.animator = base.GetComponent<Animator>();
    }

    private void Start()
    {
        this.bossCollision.Vulnerable = false;

        this.time = this.initialWait;

        this.throwCooldown = this.defaultThrowCooldown;
        this.jumpCooldown = this.defaultJumpCooldown;

        if (this.guaranteeThrow)
        {
            this.throwCount = this.guaranteedThrow;

            if (!this.guaranteeThrowAfterCooldown)
            {
                this.throwCooldown = 0;
            }
        }

        if (this.guaranteeJump)
        {
            this.jumpCount = this.guaranteedJump;

            if (!this.guaranteeJumpAfterCooldown)
            {
                this.jumpCooldown = 0;
            }
        }

        this.lastHealth = this.bossCollision.BossHealth.Health;
    }

    private void Update()
    {
        if (this.bossCollision.BossHealth.IsDead())
        {
            return;
        }

        if (this.lastHealth != this.bossCollision.BossHealth.Health)
        {
            this.animator.SetTrigger("Idle");

            this.bossCollision.Vulnerable = false;

            this.time = this.wait;

            this.timer = 0f;

            this.lastHealth = this.bossCollision.BossHealth.Health;
        }

        if (this.timer > this.time && this.bossCollision.Vulnerable)
        {
            this.animator.SetTrigger("Idle");

            this.bossCollision.Vulnerable = false;

            if (this.bossCollision.BossHealth.Health == this.modifiers[2])
            {
                this.time = this.jumpWait - 2f;
            }
            else
            {
                this.time = this.wait;
            }

            this.timer = 0f;
        }

        if (this.timer > this.time)
        {
            if (this.moving)
            {
                BossController2D bossController2D = this.bossInput.Boss.BossController2D;

                if (bossController2D.LeftOrRight())
                {
                    this.bossInput.Boss.DirectionalInput(Vector2.zero);

                    this.bossAnimation.Flip();

                    this.animator.SetBool("Run", false);

                    this.moving = false;

                    this.timer = 0f;
                }
            }
            else
            {
                int random = Random.Range(0, 3);

                if (this.throwCount == this.guaranteedThrow && this.throwCooldown <= 0)
                {
                    random = 1;
                }
                else if (this.jumpCount == this.guaranteedJump && this.jumpCooldown <= 0 && this.bossCollision.BossHealth.Health <= this.modifiers[0])
                {
                    random = 2;
                }

                bool success = false;

                switch (random)
                {
                    case 0:
                        this.Move();

                        this.time = this.runWait;

                        success = true;

                        break;
                    case 1:
                        if (this.throwCooldown > 0) break;

                        this.time = this.throwWait * (Mathf.Clamp(this.bossCollision.BossHealth.Health * 2f, 1f, this.bossCollision.BossHealth.InitialHealth) / this.bossCollision.BossHealth.InitialHealth);

                        if (this.bossCollision.BossHealth.Health == this.modifiers[2])
                        {
                            this.animator.SetTrigger("Jump");

                            this.bossInput.Jump();

                            base.Invoke(nameof(this.Jump), 1f);

                            base.Invoke(nameof(this.ThrowCrown), 2f);

                            this.time += 2f;
                        }
                        else
                        {
                            this.ThrowCrown();
                        }

                        this.throwCooldown = this.defaultThrowCooldown + 1;

                        this.throwCount = 0;

                        success = true;

                        break;

                    case 2:
                        if (this.jumpCooldown > 0) break;

                        if (this.bossCollision.BossHealth.Health > this.modifiers[0]) break;

                        this.animator.SetTrigger("Jump");

                        this.time = this.jumpWait;

                        this.jumpCooldown = this.defaultJumpCooldown + 1;

                        this.bossInput.Jump();

                        base.Invoke(nameof(this.Jump), 1f);

                        this.jumpCount = 0;

                        success = true;

                        break;
                }

                if (success)
                {
                    if ((random == 0 || random == 1) && this.jumpCooldown <= 0 && this.bossCollision.BossHealth.Health <= this.modifiers[0])
                    {
                        this.jumpCount++;
                    }
                    else if ((random == 0 || random == 2) && this.throwCooldown <= 0)
                    {
                        this.throwCount++;
                    }

                    this.throwCooldown--;
                    this.jumpCooldown--;

                    this.timer = 0f;
                }
            }
        }

        this.timer += Time.deltaTime;
    }

    private void Move()
    {
        this.animator.SetBool("Run", true);

        BossController2D bossController2D = this.bossInput.Boss.BossController2D;

        Vector2 directionalInput;

        int direction = bossController2D.FaceDirection();

        if (bossController2D.LeftOrRight())
        {
            direction = -direction;
        }

        directionalInput = new Vector2(direction, 0f);

        this.bossInput.Boss.DirectionalInput(directionalInput);

        this.moving = true;
    }

    private void ThrowCrown()
    {
        this.animator.SetTrigger("Throw");

        base.Invoke(nameof(this.Crown), 0.25f);
    }

    private void Jump()
    {
        if (this.cannonBallSpawns == null)
        {
            return;
        }

        if (this.bossCollision.BossHealth.Health <= this.modifiers[1])
        {
            this.DropCannonBallsSmart();
        }
        else
        {
            this.DropCannonBalls();
        }
    }

    private void DropCannonBalls()
    {
        base.StartCoroutine(this.CannonBalls());
    }

    private void DropCannonBallsSmart()
    {
        base.StartCoroutine(this.CannonBallsSmart());
    }

    private void Crown()
    {
        this.bossCollision.Vulnerable = true;

        Vector3 position = new Vector3(base.transform.position.x, base.transform.position.y - 0.25f, 0f);

        GameObject gameObject = Object.Instantiate(this.crown, position, Quaternion.identity);

        Crown crown = gameObject.GetComponent<Crown>();

        crown.Direction(-this.bossInput.Boss.BossController2D.FaceDirection());
    }

    private IEnumerator CannonBalls()
    {
        Transform[] spawns;

        int random = Random.Range(0, 2);

        if (random == 0)
        {
            spawns = this.cannonBallSpawns;
        }
        else
        {
            spawns = new Transform[this.cannonBallSpawns.Length];

            for (int i = 0; i < this.cannonBallSpawns.Length; i++)
            {
                spawns[i] = this.cannonBallSpawns[this.cannonBallSpawns.Length - 1 - i];
            }
        }

        foreach (Transform spawn in spawns)
        {
            Object.Instantiate(this.cannonBall, spawn.position, Quaternion.identity);

            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator CannonBallsSmart()
    {
        for (int i = 0; i < this.cannonBallSpawns.Length; i++)
        {
            Transform targetSpawn = null;

            foreach (Transform spawn in this.cannonBallSpawns)
            {
                Transform transform = this.target.transform;

                float targetX = transform.position.x;

                float spawnX = spawn.position.x;

                float distance = Mathf.Abs(targetX - spawnX);

                if (targetSpawn == null)
                {
                    targetSpawn = spawn;
                }
                else
                {
                    float targetSpawnX = targetSpawn.position.x;

                    float targetSpawnDistance = Mathf.Abs(targetX - targetSpawnX);

                    if (distance < targetSpawnDistance)
                    {
                        targetSpawn = spawn;
                    }
                }
            }

            Object.Instantiate(this.cannonBall, targetSpawn.position, Quaternion.identity);

            yield return new WaitForSeconds(1f);
        }
    }
}