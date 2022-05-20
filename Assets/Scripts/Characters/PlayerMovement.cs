using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform camera;

    public SmoothAreaLoading smoothAreaLoading;

    private AnimationHandler _animationHandler;
    private Transform _player;
    private Rigidbody _rigid;

    [SerializeField]
    private float _jumpPower;
    private float _speedModifier;
    private float _walkSpeedModifier;
    private float _runSpeedModifier;

    private Quaternion _currentPlayerRotation;

    private worldState _worldState;
    private state _currentState;
    private state _lastState;

    private Vector2Int _currentCenterChunk;
    private int _chunkSizeXZ;

    private enum worldState
    {
        onGround,
        inJump
    }
    public enum state
    {
        idle,
        walk,
        run
    }
    private void Start()
    {
        _player = transform;
        _rigid = GetComponent<Rigidbody>();
        _animationHandler = GetComponent<AnimationHandler>();

        _worldState = worldState.inJump;
        _currentState = state.idle;
        _lastState = _currentState;

        _currentPlayerRotation = _player.rotation;

        _jumpPower = 550;
        _walkSpeedModifier = 4f;
        _runSpeedModifier = 8f;

        _animationHandler.Jump();
    }
    private void FixedUpdate()
    {
        if (_currentState != _lastState)
        {
            _animationHandler.StateChange(_currentState);
            _lastState = _currentState;
        }
        if (_currentState != state.idle)
        {
            CheckCenterChunk();
        }

        PlayerMove();
        PlayerJump();
        PlayerRoll();
    }
    private void CheckCenterChunk()
    {
        if (_currentCenterChunk != CalculateCenterChunk())
        {
            _currentCenterChunk = CalculateCenterChunk();
            smoothAreaLoading.SetCenterOfLoadedArea(_currentCenterChunk);
        }
    }
    private void PlayerMove()
    {
        _currentState = state.idle;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (vertical != 0 || horizontal != 0)
        {
            _speedModifier = _walkSpeedModifier;
            _currentState = state.walk;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                _speedModifier = _runSpeedModifier;
                _currentState = state.run;
            }

            PlayerRotate();
        }

        Vector3 velocity = (camera.forward * vertical + camera.right * horizontal) * _speedModifier * Time.fixedDeltaTime * 40;
        velocity.y = _rigid.velocity.y;
        _rigid.velocity = velocity;
    }
    private void PlayerRotate()
    {
        if (_rigid.velocity != Vector3.zero)
        {
            Vector3 rotation = Quaternion.LookRotation(_rigid.velocity, Vector3.up).eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            _currentPlayerRotation = Quaternion.Lerp(_player.rotation, Quaternion.Euler(rotation), Time.deltaTime * 10);
        }

        _player.rotation = _currentPlayerRotation;
    }
    private void PlayerJump()
    {
        if (Input.GetAxis("Jump") > 0 && _worldState != worldState.inJump)
        {
            _worldState = worldState.inJump;
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);

            _animationHandler.Jump();
        }
    }
    private void PlayerRoll()
    {
        if (Input.GetMouseButtonDown(2) && _worldState != worldState.inJump && _currentState != state.idle)
        {
            _animationHandler.Roll();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            _worldState = worldState.onGround;
            _animationHandler.OnGround();
        }
    }
    private Vector2Int CalculateCenterChunk()
    {
        return new Vector2Int(Mathf.FloorToInt(transform.position.x / _chunkSizeXZ), Mathf.FloorToInt(transform.position.z / _chunkSizeXZ));
    }
    public void SetChunkSize(int chunkSize)
    {
        _chunkSizeXZ = chunkSize;

        _currentCenterChunk = CalculateCenterChunk();

        smoothAreaLoading.SetCenterOfLoadedArea(_currentCenterChunk);
    }
}