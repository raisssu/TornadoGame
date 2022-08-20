using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class PlayerMovement : MonoBehaviour {
    
    public float gravity;
    [Header("Movememt")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    
    public float groundDrag;
    
    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    
    [Header("Crouch")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYSclae;
    
    [Header("Keybind")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    
    [Header("Slope")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitSlope;
    
    public Transform orientation;
    
    float horizontalInput;
    float verticalInput;
    
    Vector3 moveDir;
    Rigidbody rigidbody;
    
    public MovementState state;
    
    public enum MovementState{
        walking,
        sprinting,
        crouching, 
        air
    }
    
    private void Start() {
        Physics.gravity = new Vector3(0, gravity, 0);
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
        
        readyToJump = true;
        
        startYSclae = transform.localScale.y;
    }
    
    private void Update() {
        
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        UserInput();
        speedControl();
        StateHandler();
        
        if(grounded){
            rigidbody.drag = groundDrag;
        }
        else{
            rigidbody.drag = 0; 
        }
        
    }
    private void FixedUpdate() {
        MovePlayer();
    }
    
    private void UserInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if(Input.GetKey(jumpKey) && readyToJump && grounded){
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        if (Input.GetKeyDown(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rigidbody.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey)){
            transform.localScale = new Vector3(transform.localScale.x, startYSclae, transform.localScale.z);
        }
        
    }
    
    private void StateHandler() {
        
        if (Input.GetKey(crouchKey)){
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        
        else if (grounded && Input.GetKey(sprintKey)){
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }
        else if(grounded){
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        else{
            state = MovementState.air;
        }
        
    }
    
    private void MovePlayer(){
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(OnSlope() && !exitSlope){
            rigidbody.AddForce(GetSlopeDir() * moveSpeed * 20f, ForceMode.Force);
            
            if(rigidbody.velocity.y > 0){
                rigidbody.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (grounded) {
            rigidbody.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if(!grounded){
            rigidbody.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        rigidbody.useGravity = !OnSlope();
    }
    
    private void speedControl(){
        
        if (OnSlope() && !exitSlope){
            if(rigidbody.velocity.magnitude > moveSpeed){
                rigidbody.velocity = rigidbody.velocity.normalized * moveSpeed;
            }
        }
        else {
            Vector3 flatVel = new Vector3(rigidbody.velocity.x, 0f, rigidbody.velocity.z);

            if (flatVel.magnitude > moveSpeed) {
                Vector3 limitVel = flatVel.normalized * moveSpeed;
                rigidbody.velocity = new Vector3(limitVel.x, rigidbody.velocity.y, limitVel.z);
            }
        }
        
    }
    
    private void Jump(){
        exitSlope = true;
        if(state == MovementState.air){
            return;
        }
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, jumpForce, rigidbody.velocity.z);
        rigidbody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        state = MovementState.air;
    }
    
    private void ResetJump(){
        readyToJump = true;
        exitSlope = false;
    }

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeDir() { 
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

}
