using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{

    //Vertical Movement Variables
    [SerializeField] float moveSpeed, walkSpeed, runSpeed;
    //Gravity-Jump Variables
    [SerializeField] bool isGrounded, isJumping;
    [SerializeField] float groundCheckDistance, gravity, jumpHeight;
    [SerializeField] LayerMask groundMask;
    //Weapon Controls
    [SerializeField] bool weaponsOn;
    [SerializeField] float cooldownTime, nextFireTime, lastClickedTime, maxComboDelay;
    [SerializeField] static int noOfClicks; 

    Vector3 _moveDirection;
    Vector3 _velocity;

    //References
    public GameObject SwordBehind;
    public GameObject SwordEquipped;
    CharacterController _playerController;
    Animator _playerAnimator;

    void Start()
    {
        _playerController = GetComponent<CharacterController>();
        _playerAnimator= GetComponentInChildren<Animator>();
        weaponsOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovementControls();
        WeaponEquip();
        if (weaponsOn)
        {
            Attack();
        }

    }

    void MovementControls()
    {
        #region Doesnotwork-1
        //_side = Input.GetAxis("Horizontal");
        //_rotation = Input.GetAxis("Mouse X");

        //_movementcoefficient = speed * Time.deltaTime;
        //_rotationcoefficient = sens * Time.deltaTime;

        //move = _forward * transform.forward * _movementcoefficient + _side * transform.right * _movementcoefficient;
        //_playerAgent.Move(move);

        //transform.Rotate(0, _rotation * _rotationcoefficient, 0); 
        #endregion
        #region DoesnotWork-2
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && _velocity.y <0)
        {
            _velocity.y = -2f;
        }

        float forward = Input.GetAxis("Vertical");
        _moveDirection = new Vector3(0, 0, forward);
        _moveDirection = transform.TransformDirection(_moveDirection);

        if (isGrounded)
        {
            if (_moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                Walk();
            }
            else if (_moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            else if (_moveDirection == Vector3.zero)
            {
                Idle();
            }
            _moveDirection *= moveSpeed;
        }

        _playerController.Move(_moveDirection * Time.deltaTime);

        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        _velocity.y += gravity * Time.deltaTime;
        _playerController.Move(_velocity * Time.deltaTime);
        #endregion
        #region Doesnotwork-3
        //float horizontal = Input.GetAxisRaw("Horizontal");
        //float vertical = Input.GetAxisRaw("Vertical");
        //Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        //if(direction.magnitude >= 0.1f)
        //{
        //    float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
        //    _playerController.Move(direction * moveSpeed * Time.deltaTime);
        //} 
        #endregion
    }

    #region DoesnotWork-2
    private void Idle()
    {
        if (weaponsOn)
        {
            _playerAnimator.SetFloat("SwordSpeed", 0f, 0.1f, Time.deltaTime);
        }
        else if (!weaponsOn)
        {
            _playerAnimator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        }
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        if (weaponsOn)
        {
            _playerAnimator.SetFloat("SwordSpeed", 0.5f, 0.1f, Time.deltaTime);
        }
        else if (!weaponsOn)
        {
            _playerAnimator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
        }
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        if (weaponsOn)
        {
            _playerAnimator.SetFloat("SwordSpeed", 1f, 0.1f, Time.deltaTime);
        }
        else if (!weaponsOn)
        {
            _playerAnimator.SetFloat("Speed", 1f, 0.1f, Time.deltaTime);
        }
    }

    private void Jump()
    {

        _velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private void WeaponEquip()
    {
        if (weaponsOn && Input.GetKeyDown(KeyCode.Tab))
        {
            _playerAnimator.SetBool("Sword", false);
            weaponsOn = !weaponsOn;
            SwordBehind.SetActive(true);
            SwordEquipped.SetActive(false);

        }
        else if (!weaponsOn && Input.GetKeyDown(KeyCode.Tab))
        {
            _playerAnimator.SetBool("Sword", true);
            weaponsOn = !weaponsOn;
            SwordBehind.SetActive(false);
            SwordEquipped.SetActive(true);
        }
    }

    private void Attack()
    {
        if (_playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash1"))
        {
            _playerAnimator.SetBool("Slash1", false);
        }
        if (_playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash2"))
        {
            _playerAnimator.SetBool("Slash2", false);
        }
        if (_playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash3"))
        {
            _playerAnimator.SetBool("Slash3", false);
            noOfClicks = 0;
        }

        if (Time.time - lastClickedTime > maxComboDelay)
        {
            noOfClicks = 0;
        }

        if (Time.time > nextFireTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ComboAttack();
            }
        }
    }

    private void ComboAttack()
    {
        lastClickedTime = Time.time;
        noOfClicks++;
        if (noOfClicks == 1)
        {
            _playerAnimator.SetBool("Slash1", true);
        }
        noOfClicks = Mathf.Clamp(noOfClicks, 0, 3);

        if (noOfClicks >= 2 &&
            _playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && 
            _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash1"))
        {
            _playerAnimator.SetBool("Slash1", false);
            _playerAnimator.SetBool("Slash2", true);
        }

        if (noOfClicks >= 3 &&
            _playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && 
            _playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Slash2"))
        {
            _playerAnimator.SetBool("Slash2", false);
            _playerAnimator.SetBool("Slash3", true);
        }
    }
    #endregion
}
