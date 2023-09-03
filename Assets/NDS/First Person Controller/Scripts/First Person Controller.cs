/*Copyright © Non-Dynamic Studios*/
/*2023*/

using System.Collections;
using Unity.Burst;
using UnityEngine;
using NDS.InputManager;
using Cinemachine;
using UnityEngine.UI;

namespace NDS.FirstPersonController
{
    [BurstCompile]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(FPSInputManager))]
    [RequireComponent(typeof(AudioSource))]
    public class First_Person_Controller : MonoBehaviour
    {
        #region Variables

        // Player
        public bool playerCanMove;
        public CharacterController characterController;
        public float transitionSpeed;
        public float walkSpeed = 5f;
        public float walkSoundSpeed;
        //sprinting
        public bool playerCanSprint;
        public bool unlimitedSprinting;
        public bool isSprintHold;
        public float sprintSpeed = 8f;
        public float sprintDuration = 8f;
        public float sprintCooldown = 8f;
        public bool hasStaminaBar;
        public Slider staminaSlider;
        public float sprintSoundSpeed;

        private FPSInputManager inputManager;
        private float targetSpeed;
        private float transitionDelta;
        private Vector3 moveDirection;
        private bool isSprinting;
        private bool isSprintCooldown = false;
        private float sprintCooldownReset;
        private float sprintRemaining;


        // Gravity and Jumping
        public bool canJump;
        public float jumpHeight = 2f;
        public int groundLayerID;
        public float groundCheckRadius;
        public Transform groundCheckTransform;
        public float gravitationalForce = 10f;

        public bool isGrounded;
        public Vector3 jumpVelocity;
        private LayerMask groundLayerMask;

        // Crouching
        public bool canCrouch;
        public bool isCrouchHold;
        public float crouchedHeight = 1f;
        public float crouchedSpeed = 1f;
        public float crouchSoundPlayTime;

        private bool isCrouching;
        private float newHeight;
        private float initialHeight;
        private bool isTryingToUncrouch;
        private Vector3 initialCameraPosition;
        private Vector3 initialGroundCheckerPosition;

        // Camera
        public bool isCursorLocked;
        public Transform cameraFollow;
        public CinemachineVirtualCamera playerVirtualCamera;
        public float mouseSensitivity;
        public float maximumClamp;
        public float minimumClamp;
        public float sprintFOV;
        public float FOV;

        private float rotationY;

        //Zooming
        public bool enableZoom;
        public bool isZoomingHold;
        public float zoomFOV = 30f;
        public float zoomStepTime = 5f;

        // Internal Variables
        private bool isZoomed = false;

        //Head Bobbing effect
        public bool canHeadBob;
        public Transform cameraHeadBobHolder;
        public float headBobAmplitude = 0.01f;
        public float headBobFrequency = 18.5f;

        private float _toggleSpeed = 3.0f;
        private Vector3 _startPos;

        //Sound System
        public string grassTag;
        public AudioClip[] soundGrass;
        public string waterTag;
        public AudioClip[] soundWater;
        public string metalTag;
        public AudioClip[] soundMetal;
        public string concreteTag;
        public AudioClip[] soundConcrete;
        public string gravelTag;
        public AudioClip[] soundGravel;
        public AudioClip landClip;
        public AudioClip jumpClip;
        public float footstepSensitivity;


        private AudioSource audioSource;
        private float AudioEffectSpeed;
        private bool moving = false;
        private string floortag;

        // Handling Physics
        public bool canPush;
        public int pushLayersID;
        public float pushStrength = 1.1f;

        private LayerMask pushLayers;

        //Armature
        public bool hasArmature;
        public Transform armature;
        public float newArmatureHeight;

        private float originalArmatureHeight;

#if UNITY_EDITOR
        // Debug Only
        public bool debugMode;
#endif
#endregion
        #region MonoBehaviour Callbacks

        private void Start()
        {
            inputManager = GetComponent<FPSInputManager>();
            groundLayerMask = LayerMaskFromLayer(groundLayerID);
            if(canPush)
            {
                pushLayers = LayerMaskFromLayer(pushLayersID);
            }
            audioSource = GetComponent<AudioSource>();
            Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            playerVirtualCamera.m_Lens.FieldOfView = FOV;
            AudioEffectSpeed = walkSoundSpeed;
            _startPos = cameraFollow.localPosition;
            StartCoroutine(senseSteps());
            if (hasStaminaBar && unlimitedSprinting)
            {
                staminaSlider.gameObject.SetActive(false);
            }

            if (!canCrouch) return;
            initialHeight = characterController.height;
            initialCameraPosition = cameraHeadBobHolder.transform.localPosition;
            initialGroundCheckerPosition = groundCheckTransform.transform.localPosition;

            if (hasArmature)
            {
                originalArmatureHeight = armature.localScale.y;
            }


        }

        private void Update()
        {
            transitionDelta = Time.deltaTime * transitionSpeed;
            GravityAndJump();
            HandleMovements();
            Crouch();
            SoundSense();
            HandleZoom();
            HandleSprinting();

            if (!canHeadBob) return;
            CheckMotion();
            ResetPosition();
            cameraFollow.LookAt(FocusTarget());
        }

        private void LateUpdate()
        {
            HandleCameraLook();
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (canPush)
            {
                PushRigidBodies(hit);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (debugMode)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
            }
        }
#endif

        #endregion
        #region Private Methods
        LayerMask LayerMaskFromLayer(int layer)
        {
            return 1 << layer;
        }
        private void HandleSprinting()
        {

            if (playerCanSprint)
            {
                if (isSprintHold) isSprinting = inputManager.isSprintingHold;
                else isSprinting = inputManager.isSprintingTap;
                if (isSprinting)
                {
                    // Drain sprint remaining while sprinting
                    if (!unlimitedSprinting)
                    {
                        sprintRemaining -= 1 * Time.deltaTime;
                        if (sprintRemaining <= 0)
                        {
                            isSprinting = false;
                            isSprintCooldown = true;
                        }
                    }
                }
                else
                {
                    // Regain sprint while not sprinting
                    sprintRemaining = Mathf.Clamp(sprintRemaining += 1 * Time.deltaTime, 0, sprintDuration);
                }

                // Handles sprint cooldown 
                // When sprint remaining == 0 stops sprint ability until hitting cooldown
                if (isSprintCooldown)
                {
                    sprintCooldown -= 1 * Time.deltaTime;
                    if (sprintCooldown <= 0)
                    {
                        isSprintCooldown = false;
                    }
                }
                else
                {
                    sprintCooldown = sprintCooldownReset;
                }

                // Handles sprintBar 
                if (hasStaminaBar && !unlimitedSprinting)
                {
                    float sprintRemainingPercent = sprintRemaining / sprintDuration;
                    staminaSlider.value = sprintRemainingPercent;
                }
            }
        }
        #region Crouch System
        private void Crouch()
        {
            if (!canCrouch) return;
            if (isCrouchHold) isCrouching = inputManager.isCrouchingHold;
            else isCrouching = inputManager.isCrouchingTap;
            if (isCrouching)
            {
                isTryingToUncrouch = false;
                AdjustCrouchSettings(crouchedHeight);
                AudioEffectSpeed = crouchSoundPlayTime;
                
                if (!hasArmature) return;
                float newHeight = Mathf.Lerp(armature.localScale.y, newArmatureHeight, transitionDelta);
                armature.transform.localScale = new Vector3(armature.localScale.x, newHeight, armature.localScale.z);
            }
            else if (!isCrouching)
            {
                isTryingToUncrouch = true;
                AdjustCrouchSettings(initialHeight);
                if (!isSprinting)
                {
                    AudioEffectSpeed = walkSoundSpeed;
                }
                if(!hasArmature) return;
                float newHeight = Mathf.Lerp(armature.localScale.y, originalArmatureHeight, transitionDelta);
                armature.transform.localScale = new Vector3(armature.localScale.x, newHeight, armature.localScale.z);
            }
        }

        private void AdjustCrouchSettings(float targetHeight)
        {
            if(isTryingToUncrouch)
            {
                Vector3 castOrigin = transform.position + new Vector3(0f, newHeight / 2, 0f);
                if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
                {
                    float distanceToCeiling = hit.point.y - castOrigin.y;
                    targetHeight = Mathf.Max(newHeight + distanceToCeiling - 0.1f, crouchedHeight);
                }
            }
            newHeight = Mathf.Lerp(characterController.height, targetHeight, transitionDelta) + (inputManager.isCrouchingTap ? -0.0000001f : 0.0000001f);

            characterController.height =  newHeight;

                Vector3 halfHeightDifference = new Vector3(0, (initialHeight - newHeight) / 2, 0);
                Vector3 newCameraHeight = initialCameraPosition - halfHeightDifference;
                cameraHeadBobHolder.localPosition = newCameraHeight;

                Vector3 halfHeightDifferenceGroundChecker = new Vector3(0, (initialHeight - newHeight) / 2, 0);
                Vector3 newGroundCheckerHeight = initialGroundCheckerPosition + halfHeightDifferenceGroundChecker;
                groundCheckTransform.localPosition = newGroundCheckerHeight;
        }
        #endregion
        private void HandleCameraLook()
        {
            float mouseDirectionX = inputManager.mouseDirection.x * mouseSensitivity * Time.deltaTime;
            float mouseDirectionY = inputManager.mouseDirection.y * mouseSensitivity * Time.deltaTime;

            rotationY -= mouseDirectionY;
            rotationY = Mathf.Clamp(rotationY, minimumClamp, maximumClamp);

            transform.Rotate(mouseDirectionX * transform.up);
            cameraFollow.localRotation = Quaternion.Euler(rotationY, 0f, 0f);
        }
        private void HandleZoom()
        {
          if (enableZoom)
          {

                if (isZoomingHold) isZoomed = inputManager.isZoomingHold && !isSprinting;
                else isZoomed = inputManager.isZoomingTap && !isSprinting;

                // Lerps camera.fieldOfView to allow for a smooth transistion
                if (isZoomed)
            {
                playerVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(playerVirtualCamera.m_Lens.FieldOfView, zoomFOV, zoomStepTime * Time.deltaTime);
            }
            else if (!isZoomed && !isSprinting)
            {
                playerVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(playerVirtualCamera.m_Lens.FieldOfView, FOV, zoomStepTime * Time.deltaTime);
            }
          }
        }
        private void AdjustFOVSettings(float targetFOV) 
        {
            if (isZoomed) return;
            if (!moving)
            {
                targetFOV = FOV;
            }
            float currentFOV = playerVirtualCamera.m_Lens.FieldOfView;

            float newFOV = Mathf.Lerp(currentFOV, targetFOV, transitionDelta) + (isSprinting ? -0.0000001f : 0.0000001f);

            playerVirtualCamera.m_Lens.FieldOfView = newFOV;

        }
        private void HandleMovements()
        {
            if (!playerCanMove) return;
            bool approxHeight = Mathf.Approximately(characterController.height, initialHeight);
            if (isSprinting && !inputManager.isCrouchingTap && playerCanSprint && approxHeight)
            {
                AudioEffectSpeed = sprintSoundSpeed;
                targetSpeed = sprintSpeed;
                AdjustFOVSettings(sprintFOV);
            }
            else if(inputManager.isCrouchingTap)
            {
                targetSpeed = crouchedSpeed;
            }
            else if (!isSprinting && characterController.height > 1.2f)
            {
                AudioEffectSpeed = walkSoundSpeed;
                targetSpeed = walkSpeed;
                AdjustFOVSettings(FOV);
            }

            moveDirection = inputManager.moveDirection.x * targetSpeed * Time.deltaTime * transform.right
                + inputManager.moveDirection.y * targetSpeed * Time.deltaTime * transform.forward;

            characterController.Move(moveDirection);
        }
        private void GravityAndJump()
        {
            bool isPreviouslyGrounded = isGrounded; // Store previous grounded state
            isGrounded = Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundLayerMask);

            bool hasJumped = false;

            if (isGrounded && !isPreviouslyGrounded)
            {
                audioSource.PlayOneShot(landClip); // Play land audio only when just touching ground
            }

            if (isGrounded)
            {
                if (inputManager.haveJumped && !inputManager.isCrouchingTap && !hasJumped && canJump)
                {
                    jumpVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravitationalForce);
                    audioSource.PlayOneShot(jumpClip);
                    hasJumped = true;
                }
                else if (!isGrounded && jumpVelocity.y < 0f)
                {
                    jumpVelocity.y = -3f; // Reset jump velocity on landing
                    hasJumped = false; // Reset hasJumped flag when grounded
                }
            }
            else
            {
                jumpVelocity.y -= gravitationalForce * Time.deltaTime;
            }

            characterController.Move(Time.deltaTime * jumpVelocity.y * transform.up);
        }
        private void PushRigidBodies(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            if (body == null || body.isKinematic)
            {
                return;
            }

            var bodyLayerMask = 1 << body.gameObject.layer;

            if ((bodyLayerMask & pushLayers.value) == 0)
            {
                return;
            }

            if (hit.moveDirection.y < -0.3f)
            {
                return;
            }

            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.0f, hit.moveDirection.z);
            body.AddForce(pushDir * pushStrength, ForceMode.Impulse);
        }

        #region Head Bobbing
        private Vector3 FootStepMotion()
        {
            Vector3 pos = Vector3.zero;
            pos.y += Mathf.Sin(Time.time * headBobFrequency) * headBobAmplitude;
            pos.x += Mathf.Cos(Time.time * headBobFrequency / 2) * headBobAmplitude * 2;
            return pos;
        }
        private void CheckMotion()
        {
            float speed = characterController.velocity.magnitude;
            if (speed < _toggleSpeed) return;
            if (!isGrounded) return;

            PlayMotion(FootStepMotion());
        }
        private void PlayMotion(Vector3 motion)
        {
            cameraFollow.localPosition += motion;
        }

        private Vector3 FocusTarget()
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraFollow.localPosition.y, transform.position.z);
            pos += cameraFollow.forward * 15.0f;
            return pos;
        }
        private void ResetPosition()
        {
            if (cameraFollow.localPosition == _startPos) return;
            cameraFollow.localPosition = Vector3.Lerp(cameraFollow.localPosition, _startPos, 1 * Time.deltaTime);
        }
        #endregion

        #region Sound Management
        private void SoundSense()
        {
            Vector3 castOrigin = transform.position;
            if (Physics.Raycast(castOrigin, Vector3.down, out RaycastHit hit, 5f))
            {
                if (hit.collider.CompareTag(grassTag))
                {
                    floortag = grassTag;
                }
                else if (hit.collider.CompareTag(metalTag))
                {
                    floortag = metalTag;
                }
                else if (hit.collider.CompareTag(gravelTag))
                {
                    floortag = gravelTag;
                }
                else if (hit.collider.gameObject.CompareTag(waterTag))
                {
                    floortag = waterTag;
                }
                else if (hit.collider.CompareTag(concreteTag))
                {
                    floortag = concreteTag;
                }

            }
            //Sensing movement for players
            var velocity = characterController.velocity;
            var localVel = transform.InverseTransformDirection(velocity);

            if (localVel.z > footstepSensitivity)
            {
                moving = true;
            }
            else if (localVel.z < (footstepSensitivity * -1))
            {
                moving = true;
            }
            else if (localVel.x > footstepSensitivity)
            {
                moving = true;
            }
            else if (localVel.x < (footstepSensitivity * -1))
            {
                moving = true;
            }
            else
            {
                moving = false;
            }
        }

        private IEnumerator senseSteps()
        {
            while (true)
            {
                if (isGrounded && moving)
                {
                    if (floortag == grassTag)
                    {
                        audioSource.clip = soundGrass[Random.Range(0, soundGrass.Length)];
                    }
                    else if (floortag == gravelTag)
                    {
                        audioSource.clip = soundGravel[Random.Range(0, soundGravel.Length)];
                    }
                    else if (floortag == waterTag)
                    {
                        audioSource.clip = soundWater[Random.Range(0, soundWater.Length)];
                    }
                    else if (floortag == metalTag)
                    {
                        audioSource.clip = soundMetal[Random.Range(0, soundMetal.Length)];
                    }
                    else if (floortag == concreteTag)
                    {
                        audioSource.clip = soundConcrete[Random.Range(0, soundConcrete.Length)];
                    }
                    else
                    {
                        yield return 0;
                    }
                    if (audioSource.clip != null)
                        audioSource.PlayOneShot(audioSource.clip);
                    yield return new WaitForSeconds(AudioEffectSpeed);
                }
                else
                {
                    yield return 0;
                }

            }
        }
        #endregion

        #endregion
    }
}