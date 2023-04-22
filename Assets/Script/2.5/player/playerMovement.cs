using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    [Header("移动")]
    public float moveSpeed;//移动速度
    public float groundDrag;//地面阻力
    public float faceX,faceY;//这是为了让角色不每次都回弹到正面

    public bool isWalk;//判断是否在行走
    public bool isHorse;//判断玩家是否在马上

    public Animator anim;

    public Horse horse;//获取马脚本

//控制能不能移动和检测按键
    private bool isMoving;
    public bool inputDisable;
    


    [Header("按键")]
//     public KeyCode jumpKey = KeyCode.Space;//跳跃按键

//     [Header("地面检测")]
//     public float playerHeight;//玩家高度
//     public LayerMask whatIsGround;//哪里是地面
    //bool grounded;//是否在地面
    public Transform orientation;//方向定位
 
    //水平和垂直按键检测
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;//移动方向

    Rigidbody rb;//获得Rigidbody组件

    private void Start()
   {
       rb = GetComponent<Rigidbody>();
       anim = GetComponent<Animator>();
   }

    private void Update()
   {
        // ground check
        //Physics.Raycast射线检测（起点，方向，最大距离，检测层级）
//         grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);
        if(inputDisable == false)
        {
           MyInput();//输入检测
        }
        
        MovePlayer();//角色移动
        SpeedControl();
        playerAnim();

    }

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition += OnMoveToPosition;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
        EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
        EventHandler.MoveToPosition -= OnMoveToPosition;
    }

    private void FixedUpdate()
 {
     
 }

    private void OnMoveToPosition(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }

    private void OnAfterSceneLoadedEvent()
    {
        inputDisable = false;
    }

    private void OnBeforeSceneUnloadEvent()
    {
        inputDisable = true;
    }

    private void MyInput()
 {
     horizontalInput = Input.GetAxisRaw("Horizontal");
     verticalInput = Input.GetAxisRaw("Vertical");

 }

 private void MovePlayer()
 {
        // calculate movement direction
         moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);//ForceMode.Force向此刚体添加连续力
    }

        private void SpeedControl()
  {
     Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);//获得刚体的平面速度

        // limit velocity if needed
     if(flatVel.magnitude > moveSpeed)//如果大于了设置的移动速度则
   {
     Vector3 limitedVel = flatVel.normalized * moveSpeed;//将他的方向*速度，其实就等于你设置的速度
     rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);//然后再将x和z设置到这个被限制的速度
   }
 }
    private void playerAnim()
    {

        if (moveDirection != Vector3.zero)//&& horse.isHorse == false
        {
            isWalk = true;
            anim.SetBool("isWalk", true);
            faceX = horizontalInput;
            faceY = verticalInput;
            //print(faceX);
        }
        else
        {
            isWalk = false;
            anim.SetBool("isWalk", false);
        }

        // if(moveDirection != Vector3.zero && horse.isHorse == true)
        // {
        //     isHorse = true;//玩家也判断是否在马上，然后做相应的动画
        //     horse.isWalk = true;
        //     horse.anim.SetBool("isWalk",true);
        //     // faceX = horizontal;
        //     // faceY = vertical;
        // }
        // else if(moveDirection == Vector3.zero)
        // {
        //     horse.isWalk = false;
        //     horse.anim.SetBool("isWalk",false);
        // }
        // if(horse.isHorse == false)
        // {
        //     isHorse = false;
        // }

        anim.SetFloat("horizontal", faceX);
        anim.SetFloat("vertical", faceY);
       // anim.SetBool("isHorse", isHorse);
    }

}
