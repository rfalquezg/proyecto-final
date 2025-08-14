using UnityEngine;

public class Gomba : Enemy
{
    public override void Stomped(Transform player)
    {
        AudioManager.Instance.PlayStomp();
        animator.SetTrigger("Hit");
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Destroy(gameObject, 1f);
        autoMovement.PauseMovement();
        Dead();
    }
}
