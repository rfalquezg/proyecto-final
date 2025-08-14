using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour
{
    Animator animator;
    Colisiones colisiones;
    Mover mover;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        colisiones = GetComponent<Colisiones>();
        mover = GetComponent<Mover>();
    }

    void Update()
    {
        // Llamamos a los métodos individuales para mantener todo sincronizado
        Grounded(colisiones.Grounded());
        Velocity(mover.rb2D.linearVelocity.x); // Usamos linearVelocity según tu lógica
        Jumping(mover.isJumping);
        Skid(mover.isSkidding);
    }

    public void Grounded(bool isGrounded)
    {
        animator.SetBool("Grounded", isGrounded);
    }

    public void Velocity(float velocityX)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(velocityX));
    }

    public void Jumping(bool isJumping)
    {
        animator.SetBool("Jumping", isJumping);
    }

    public void Skid(bool isSkidding)
    {
        animator.SetBool("Skid", isSkidding);
    }

    public void Dead()
    {
        animator.SetTrigger("Dead");
    }
    public void NewState(int state)
    {
        animator.SetInteger("State", state);
    }

    public void PowerUp()
    {
        animator.SetTrigger("PowerUp");
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");
    }
    public void Shoot()
    {
        animator.SetTrigger("Shoot");
    }
    public void InvincibleMode(bool activate)
    {
        animator.SetBool("Invincible", activate);
    }
    public void Hurt(bool activate)
    {
        animator.SetBool("Hurt", activate);
    }
    public void Crouch(bool activate)
    {
        animator.SetBool("Crouched", activate);
    }
    public void Climb(bool activate)
    {
        animator.SetBool("Climb", activate);
    }
    public void Pause()
    {
        animator.speed = 0;
    }
    public void Continue()
    {
        animator.speed = 1;
    }
    public void Reset()
    {
        animator.SetBool("Grounded", false);
        animator.SetFloat("VelocityX", 0);
        animator.SetBool("Jumping", false);
        animator.SetBool("Skid", false);
        animator.SetBool("Invincible", false);
        animator.SetBool("Hurt", false);
        animator.SetBool("Crouched", false);
        animator.SetBool("Climb", false);

        animator.ResetTrigger("Dead");
        animator.ResetTrigger("PowerUp");
        animator.ResetTrigger("Hit");
        animator.ResetTrigger("Shoot");

        animator.SetInteger("State", 0);
        animator.Play("States");
    }

}
