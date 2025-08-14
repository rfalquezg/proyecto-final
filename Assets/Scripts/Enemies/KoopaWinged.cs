
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaWinged : Koopa
{
    bool isFly;
    public GameObject wingPrefab;

    protected override void Start()
    {
        base.Start();
        isFly = true;
        animator.SetBool("Fly", true);
    }

    void LoseWings()
    {
        isFly = false;
        if (rb2D.linearVelocity.x < 0)
        {
            autoMovement.speed *= -1;
        }
        rb2D.linearVelocity = Vector2.zero;
        rb2D.isKinematic = false;
        autoMovement.useWaysPoints = false;
        autoMovement.isFall = true;
        animator.SetBool("Fly", false);
        LoseWingsAnimation();
    }

    public override void Stomped(Transform player)
    {
        if (isFly)
        {
            AudioManager.Instance.PlayStomp();
            LoseWings();
            NoDamageTemp();
        }
        else
        {
            base.Stomped(player);
        }
    }
    public override void HitFireBall()
    {
        if (isFly)
        {
            LoseWings();
        }
        base.HitFireBall();
    }
    public override void HitRollingShell()
    {
        if (isFly)
        {
            LoseWings();
        }
        base.HitRollingShell();
    }
    public override void HitStarman()
    {
        if (isFly)
        {
            LoseWings();
        }
        base.HitStarman();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.L))
        {
            Stomped(Mario.Instance.transform);
        }
    }
    void LoseWingsAnimation()
    {
        GameObject wing;
        wing = Instantiate(wingPrefab, transform.position, Quaternion.identity);
        wing.GetComponent<Rigidbody2D>().AddForce(new Vector2(3f, 9f), ForceMode2D.Impulse);

        wing = Instantiate(wingPrefab, transform.position, Quaternion.identity);
        wing.transform.localScale = new Vector3(-1f, 1f, 1f);
        wing.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3f, 9f), ForceMode2D.Impulse);
    }
}
