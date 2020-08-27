using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMovement : MonoBehaviour
{

    #region Variables
    public Camera camera;
    Animator animator;
    CharacterController controller;
    public GameControllerScript gameController;

    public bool allowWalk = true;
    // Moving speed of the character
    public float speed;
    // Running speed of the character
    public float runSpeed;
    // Temp for storing the speed
    float speedTemp;
    // Player's gravity
    public float gravity = -9.8f;
    // Multiplier for gravity
    public float fallMultiplier = 2.5f;
    // Jump speed of the character
    public float jumpHeight;
    // Boolean if the player is grounded
    public bool isGrounded;
    // New point to move
    public Vector3 move = Vector3.zero;
    // Velocity for falling and jumping
    Vector3 velocity;
    // Boolean if the player is walking
    bool isWalking;
    // Boolean if the player is running
    bool isRunning;
    // Float to check the distance between the player and the ground
    public float groundDistance = 0.4f;
    // Layer for ground
    public LayerMask groundMask;
    // Player smoothness for rotation
    [Range(0.0f, 10.0f)]
    public float smooth;
    public bool allowInput;
    public bool jumping;


    [Header("Battle Mode")]
    public bool inBattle;
    public bool battleCheck = false;
    public float battleWalkSpeed = 2f;
    public int attackCounter = 0;



    private Vector3 rightFootPosition, leftFootPosition, rightFootIKPosition, leftFootIKPosition;
    private Quaternion leftFootIKRotation, rightFootIKRotation;
    private float lastPelvisPositionY, lastRightFootPositionY, lastLeftFootPositionY;

    [Header("Feet Grounder")]
    public bool enableFeetIK = true;
    [Range(0,2)] [SerializeField] private float heightFromGroundRayCast = 1.14f;
    [Range(0, 2)] [SerializeField] private float raycastDownDistance = 1.5f;
    [SerializeField] private LayerMask environmentLayer;
    [SerializeField] private float pelvisOffset = 0f;
    [Range(0,1)] [SerializeField] private float pelvisUpAndDownSpeed = 0.28f;
    [Range(0,1)] [SerializeField] private float feetToIKPositionSpeed = 0.5f;

    public string leftFootAnimVariableName = "LeftFootCurve";
    public string rightFootAnimVariableName = "RightFootCurve";

    public bool useProIKFeature = false;
    public bool showSolverDebug = true;

    [Header("Weapon")]
    public GameObject swordOnBack;
    public GameObject swordOnHand;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        allowInput = gameController.getAllowInput();
        controller = this.GetComponent<CharacterController>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        updateWithGameController();
        if(allowInput){
            KeyBoardMovement();
            Jump();
        } else {
            animationHandler();
        }
        checkGrounded();
        Falling();
        switchBetweenNormalAndBattle();
        BattlePhase();
    }

    #region Player Movement Controller

    // Player movement 
    void KeyBoardMovement(){
        float moveY = Input.GetAxis("Horizontal");
        float moveX = Input.GetAxis("Vertical");

        animator.SetFloat("moveX", moveX);
        animator.SetFloat("moveY", moveY);

        // If there is any input, we rotate the player facing the camera direction
        if(moveY != 0 || moveX !=0){
            Vector3 rotation = new Vector3(0f, camera.transform.eulerAngles.y, 0f);
            //transform.rotation = Quaternion.Euler(rotation);
            transform.localRotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler(rotation), Time.deltaTime * smooth);
            isWalking = true;
        } else {
            isWalking = false;
        }

        // Check if the player is running
        if(Input.GetKey(KeyCode.LeftShift) && isWalking && !inBattle){
            speedTemp = runSpeed;
            isRunning = true;
            isWalking = false;
            //animator.SetBool("running", isRunning);
            //animator.SetBool("walking", isWalking);
        } else if(inBattle){
            speedTemp = battleWalkSpeed;
            //animator.SetBool("walking", isWalking);
        } else {
            speedTemp = speed;
            isRunning = false;
            //animator.SetBool("walking", isWalking);
            //animator.SetBool("running", isRunning);
        }

        animator.SetBool("running", isRunning);
        animator.SetBool("walking", isWalking);
        
        if(allowWalk){
            // Moving the player
            move = transform.right * moveY + transform.forward * moveX;
            controller.Move(move * speedTemp * Time.deltaTime);
        }

    }
    void switchBetweenNormalAndBattle(){
        inBattle = gameController.getInBattle();   
        animator.SetBool("battle", inBattle);
        if(inBattle){
            if(battleCheck == true){
                allowWalk = true;
                transform.LookAt(gameController.getEnemy().transform);
            } else {
                allowWalk = false;
                isWalking = false;
                isRunning = false;
            }
            animator.SetBool("walking", isWalking);
            animator.SetBool("running", isRunning);
            if(battleCheck == false){
                if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Draw Sword")){
                    if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                        swordOnBack.SetActive(false);
                        swordOnHand.SetActive(true);
                    }
                }else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Great Sword Idle")){
                    battleCheck = true;
                    animator.SetBool("battleCheck", battleCheck);
                    allowWalk = true;
                    isWalking = true;
                    animator.SetBool("walking", isWalking);
                }
            }
        }else {
            battleCheck = false;
            animator.SetBool("battleCheck", battleCheck);
            //allowWalk = true;
            swordOnBack.SetActive(true);
            swordOnHand.SetActive(false);
        }
    }

    // Method to check is the player is grounded or not
    // If so, it will change isGrounded to true and cancel the jumping animation
    void checkGrounded(){
        isGrounded = Physics.CheckSphere(controller.transform.position, groundDistance, groundMask);
        animator.SetBool("isJumping", !isGrounded);
        if(isGrounded && velocity.y < 0){
            velocity.y = -2f;
        }
    }

    // Method for the player to jump
    // The player can jump only when grounded
    void Jump(){ 
        if(Input.GetButtonDown("Jump") && isGrounded){
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumping = true;
            animator.SetBool("isJumping", true);
        }
    }

    // Method for calculating player's falling
    void Falling(){
        velocity.y += gravity * fallMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void BattlePhase(){
        if(inBattle && battleCheck){
            if(Input.GetMouseButtonDown(0)){
                attackCounter ++;
                animator.SetInteger("attackCounter", attackCounter);
            }

            if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Great Sword Slash 1")){
                if(animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1){
                    if(attackCounter == 1){
                        attackCounter = 0;
                    }
                    animator.SetInteger("attackCounter", attackCounter);
                }
            } else if(animator.GetCurrentAnimatorStateInfo(0).IsTag("Great Sword Slash 2")){
                attackCounter = 0;
                animator.SetInteger("attackCounter", attackCounter);
            }
        }
    }

    public void animationHandler(){
        animator.SetBool("running", isRunning);
        animator.SetBool("walking", isWalking);    
    }

    public void updateWithGameController(){
        this.inBattle = gameController.getInBattle();
        this.allowInput = gameController.getAllowInput();
    }

    public void setInBattle(bool b){
        this.inBattle = b;
        animator.SetBool("battle", this.inBattle);
    }

    public bool getInBattle(){
        return this.inBattle;
    }

    public void setBattleCheck(bool b){
        this.battleCheck = b;
        animator.SetBool("battleCheck", this.battleCheck);
    }

    public bool getBattleCheck(){
        return this.battleCheck;
    }

    public void setIsWalking(bool b){
        this.isWalking = b;
    }

    public bool getIsWalking(){
        return this.isWalking;
    }

    public void setIsRunning(bool b){
        this.isRunning = b;
    }

    public bool getIsRunning(){
        return this.isRunning;
    }

    #endregion

    #region FeetGrounding

    private void FixedUpdate(){
        if(enableFeetIK == false){
            return;
        }
        if(animator == null){
            return;
        }

        AdjustFeetTarget(ref rightFootPosition, HumanBodyBones.RightFoot);
        AdjustFeetTarget(ref leftFootPosition, HumanBodyBones.LeftFoot);

        // find and raycast to the ground to find positions
        FeetPositionSolver(rightFootPosition, ref rightFootIKPosition, ref rightFootIKRotation); // handle the solver for right foot
        FeetPositionSolver(leftFootPosition, ref leftFootIKPosition, ref leftFootIKRotation); // handle the solver for left foot

    }

    private void onAnimatorIK(int layerIndex){
        if(enableFeetIK == false){
            return;
        }
        if(animator == null){
            return;
        }

        MovePelvisHeight();

        // Right foot ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);

        if(useProIKFeature){
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat(rightFootAnimVariableName));
        }

        MoveFeetToIKPoint(AvatarIKGoal.RightFoot, rightFootIKPosition, rightFootIKRotation, ref lastRightFootPositionY);

        // Left foot ik position and rotation
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);

        if(useProIKFeature){
            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat(leftFootAnimVariableName));
        }

        MoveFeetToIKPoint(AvatarIKGoal.LeftFoot, leftFootIKPosition, leftFootIKRotation, ref lastLeftFootPositionY);
    }





    #endregion

    #region FeetGroundingMethods

    void MoveFeetToIKPoint(AvatarIKGoal foot, Vector3 positionIKHolder, Quaternion rotationIKHolder, ref float lastFootPositionY){
        Vector3 targetIKPosition = animator.GetIKPosition(foot);

        if(positionIKHolder != Vector3.zero){
            targetIKPosition = transform.InverseTransformPoint(targetIKPosition);
            positionIKHolder = transform.InverseTransformPoint(positionIKHolder);

            float yVariable = Mathf.Lerp(lastFootPositionY, positionIKHolder.y, feetToIKPositionSpeed);
            targetIKPosition.y += yVariable;

            lastFootPositionY = yVariable;

            targetIKPosition = transform.TransformPoint(targetIKPosition);

            animator.SetIKRotation(foot, rotationIKHolder);
        }

        animator.SetIKPosition(foot, targetIKPosition);
    }

    private void MovePelvisHeight(){
        if(rightFootIKPosition == Vector3.zero || leftFootIKPosition == Vector3.zero || lastPelvisPositionY == 0){
            lastPelvisPositionY = animator.bodyPosition.y;
            return;
        }

        float leftOffsetPosition = leftFootIKPosition.y - transform.position.y;
        float rightOffsetPosition = rightFootIKPosition.y - transform.position.y;

        float totalOffset = (leftOffsetPosition < rightOffsetPosition) ? leftOffsetPosition : rightOffsetPosition;

        Vector3 newPelvisPosition = animator.bodyPosition + Vector3.up * totalOffset;

        newPelvisPosition.y = Mathf.Lerp(lastPelvisPositionY, newPelvisPosition.y, pelvisUpAndDownSpeed);

        animator.bodyPosition = newPelvisPosition;

        lastPelvisPositionY = animator.bodyPosition.y;
    }

    private void FeetPositionSolver(Vector3 fromSkyPosition, ref Vector3 feetIKPosition, ref Quaternion feetIKRotations){
        //Raycast handling section
        RaycastHit feetOutHit;

        if(showSolverDebug){
            Debug.DrawLine(fromSkyPosition, fromSkyPosition + Vector3.down * (raycastDownDistance + heightFromGroundRayCast), Color.yellow);
        }

        if(Physics.Raycast(fromSkyPosition, Vector3.down, out feetOutHit, raycastDownDistance + heightFromGroundRayCast, environmentLayer)){
            feetIKPosition = fromSkyPosition;
            feetIKPosition.y = feetOutHit.point.y + pelvisOffset;
            feetIKRotations = Quaternion.FromToRotation(Vector3.up, feetOutHit.normal) * transform.rotation;
            return;
        }

        feetIKPosition = Vector3.zero;
    }

    private void AdjustFeetTarget(ref Vector3 feetPositions, HumanBodyBones foot){
        // This just dont work
        // Dont know why, figure it out later
        //feetPositions = animator.GetBoneTransform(foot).position;
        //feetPositions.y = transform.position.y + heightFromGroundRayCast;
    }

    #endregion




}
