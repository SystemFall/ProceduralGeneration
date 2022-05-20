using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void StateChange(PlayerMovement.state state)
    {
        if(state == PlayerMovement.state.idle)
        {
            _animator.SetBool("isWalk", false);
            _animator.SetBool("isRun", false);
        }
        if (state == PlayerMovement.state.walk)
        {
            _animator.SetBool("isWalk", true);
            _animator.SetBool("isRun", false);
        }
        if (state == PlayerMovement.state.run)
        {
            _animator.SetBool("isWalk", false);
            _animator.SetBool("isRun", true);
        }
    }
    public void Jump()
    {
        if(_animator != null)
        {
            _animator.SetTrigger("jump");
            _animator.SetBool("inJump", true);
        }
    }
    public void Roll()
    {
        _animator.SetTrigger("standToRoll");
    }
    public void OnGround()
    {
        _animator.SetBool("inJump", false);
    }
}
