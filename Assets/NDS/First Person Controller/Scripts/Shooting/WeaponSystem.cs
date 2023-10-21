using UnityEngine;
using Random = UnityEngine.Random;
using TMPro;
using Unity.Burst;
using NDS.InputManager;
using NDS.BulletSystem.Parabolic;
using NDS.FirstPersonController;

namespace NDS.UniversalWeaponSystem
{
    [BurstCompile]
    [RequireComponent(typeof(AudioSource))]
    public class WeaponSystem : MonoBehaviour
    {
        #region Variables
        //Reference
        public FPSInputManager inputManager;
        public First_Person_Controller fpsController;
        public Transform shootPoint;
        public GameObject muzzleFlash;
        public GameObject bulletPrefab;
        public TextMeshProUGUI bulletCount;

        //Bullet Physics
        public float bulletSpeed;
        public float bulletLifeTime;
        public float bulletGravitationalForce;

        //Gun stats
        public bool isGunAuto;
        public bool isAimHold;
        public float timeBetweenEachShots;
        public float timeBetweenShooting;
        public float Spread;
        public int magazineSize;
        public int totalBullets;
        public int bulletsPerTap;
        public float muzzelEffectLifeTime;
        public float reloadTime;
        public float reloadRotationSpeed = 30f;
        public bool haveProceduralReload;
        public bool reloading;
        public bool aiming;

        private int bulletsLeft;
        private int bulletsShot;
        private bool readyToShoot;
        private bool shooting;
        private Quaternion originalReloadRotation;

        //Aiming
        public bool canAim;
        public Transform weaponHolder;
        public Vector3 aimingLocalPosition;
        public float aimSmoothing = 10;

        private Vector3 normalLocalPosition;

        //Peeking Right
        public bool canPeek;
        public Vector3 peekLeftPosition;
        public Vector3 peekRightPosition;
        public float peekSmoothing;
        public Transform cameraHolderForPeeking;

        private Vector3 normalLocalPeekPosition;
        //Camera Recoil 
        public bool haveCameraRecoil;
        public Transform cameraRecoilHolder;
        public float recoilRotationSpeed;
        public float recoilReturnSpeed;
        public Vector3 hipFireRecoil = new Vector3(2f, 2f, 2f);
        public Vector3 adsFireRecoil = new Vector3(0.5f, 0.5f, 0.5f);

        private Vector3 currentRotation;
        private Vector3 Rot;

        //Weapon Recoil 
        public bool haveWeaponRecoil;
        public Transform gunRotationHolder;
        public Transform gunPositionHolder;
        public float gunRecoilPositionSpeed;
        public float gunPositionReturnSpeed;
        public Vector3 recoilKickBackHip = new Vector3(0.015f, 0f, 0.2f);
        public Vector3 recoilKickBackAds = new Vector3(0.005f, 0f, 0.02f);
        public float gunRecoilRotationSpeed;
        public float gunRotationReturnSpeed;
        public Vector3 recoilRotationHip = new Vector3(10f, 5f, 7f);
        public Vector3 recoilRotationAds = new Vector3(10f, 4f, 6f);

        private Vector3 rotationRecoil;
        private Vector3 positionRecoil;
        private Vector3 rot;

        //Weapon Positional sway
        public bool havePositionalSway;
        public float swayIntensity;
        public float swayAmount;
        public float swaySmoothness;

        private Vector3 originPosition;
        private float mouseX;
        private float mouseY;

        //Weapon Rotational Sway
        public bool haveRotationalSway;
        public Transform rotationSwayTransform;
        public float rotaionSwayIntensity;
        public float rotationSwaySmoothness;

        private Quaternion rotationSwayOriginalRotation;

        //Jump Sway
        public bool haveJumpSway;
        public float jumpIntensity;
        public float weaponMaxClamp;
        public float weaponMinClamp;
        public float jumpSmooth;
        public float landingIntensity;
        public float landingSmooth;
        public float recoverySpeed;

        private float impactForce = 0;

        //Weapon Rotational Tilt
        public bool haveRotationalTilt;
        public float tiltIntensity;
        public float tiltAmount;
        public float tiltSmoothness;
        public bool rotateX;
        public bool rotateY;
        public bool rotateZ;

        private Quaternion originRotation;

        //Weapon Move Bobbing
        public bool haveBobbing;
        public float magnitude = 0.009f;
        public float idleSpeed;
        public float walkSpeedMultiplier;
        public float walkSpeedMax;
        public float aimReduction;

        private float sinY = 0f;
        private float sinX = 0f;
        private Vector3 lastPosition;

        //Audio Setup
        public AudioClip bulletSoundClip;
        public AudioClip bulletReloadClip;
        public float soundVolume;

        private AudioSource bulletSound;
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            bulletsLeft = magazineSize;
            readyToShoot = true;
            bulletSound = GetComponent<AudioSource>();
            originRotation = transform.localRotation;
            originPosition = transform.localPosition;
            rotationSwayOriginalRotation = rotationSwayTransform.localRotation;
            lastPosition = transform.position;
            originalReloadRotation = gunPositionHolder.localRotation;
            normalLocalPosition = weaponHolder.transform.localPosition;
            normalLocalPeekPosition = cameraHolderForPeeking.transform.localPosition;
        }
        private void Update()
        {
            MyInput();
            DetermineAim();
            DeterminePeek();
            HandleWeaponSway();
            HandleTilt();
            HandleCameraRotation();
            HandleGunRecoil();
            WeaponRotationSway();
            WeaponMoveBobbing();
            JumpSwayEffect();
            SetUIElements();
            HandleReloadAnimation();
        }

        #endregion

        #region Private Methods
        private void MyInput()
        {
             if (isGunAuto) shooting = inputManager.isFiringHold;
             else shooting = inputManager.isFiringTap;

            //handle inputs
            mouseX = inputManager.mouseDirection.x;
            mouseY = inputManager.mouseDirection.y;
            if (isAimHold) aiming = inputManager.isAimingHold;
            else aiming = inputManager.isAimingTap;
            if (inputManager.isReloading && bulletsLeft < magazineSize && !reloading) Reload();

            //Shoot
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletsShot = bulletsPerTap;
                Shoot();
            }
        }
        private void DetermineAim()
        {
            if (!canAim) return;
            Vector3 target = normalLocalPosition;
            if (aiming) target = aimingLocalPosition;

            Vector3 desiredPosition = Vector3.Lerp(weaponHolder.transform.localPosition, target, Time.deltaTime * aimSmoothing);

            weaponHolder.transform.localPosition = desiredPosition;
        }

        private void DeterminePeek()
        {
            if (!canPeek) return;
            Vector3 target = normalLocalPeekPosition;
            Debug.Log(inputManager.peekDirection.x);
            if(inputManager.peekDirection.x == 1f)
            {
                target = peekRightPosition;
            }
            else if (inputManager.peekDirection.x == -1f)
            {
                target = peekLeftPosition;
            }
            Vector3 desiredPosition = Vector3.Lerp(cameraHolderForPeeking.transform.localPosition, target, Time.deltaTime * peekSmoothing);

            cameraHolderForPeeking.transform.localPosition = desiredPosition;
        }
        private void Shoot()
        {
            readyToShoot = false;

            // Parabolic shoot code here
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            ParabolicBullet bulletScript = bullet.GetComponent<ParabolicBullet>();
            if (bulletScript)
            {
                bulletScript.Initialize(shootPoint, bulletSpeed, bulletGravitationalForce, Spread);
            }
            Destroy(bullet, bulletLifeTime);


            //Graphics
            GameObject muzzelEffect = Instantiate(muzzleFlash, shootPoint.position, Quaternion.identity);
            Destroy(muzzelEffect, muzzelEffectLifeTime);
            bulletSound.PlayOneShot(bulletSoundClip, (soundVolume * 0.01f));
            if(aiming)
            {
                currentRotation += new Vector3(-adsFireRecoil.x, Random.Range(-adsFireRecoil.y, adsFireRecoil.y), Random.Range(-adsFireRecoil.z, adsFireRecoil.z));

                rotationRecoil += new Vector3(-recoilRotationAds.x, Random.Range(-recoilRotationAds.y, recoilRotationAds.y), Random.Range(-recoilRotationAds.z, recoilRotationAds.z));
                positionRecoil += new Vector3(Random.Range(-recoilKickBackAds.x, recoilKickBackAds.y), Random.Range(-recoilKickBackAds.y, recoilKickBackAds.y), recoilKickBackAds.z);
            }
            else
            {
                currentRotation += new Vector3(-hipFireRecoil.x, Random.Range(-hipFireRecoil.y, hipFireRecoil.y), Random.Range(-hipFireRecoil.z, hipFireRecoil.z));

                rotationRecoil += new Vector3(-recoilRotationHip.x, Random.Range(-recoilRotationHip.y, recoilRotationHip.y), Random.Range(-recoilRotationHip.z, recoilRotationHip.z));
                positionRecoil += new Vector3(Random.Range(-recoilKickBackHip.x, recoilKickBackHip.y), Random.Range(-recoilKickBackHip.y, recoilKickBackHip.y), recoilKickBackHip.z);
            }
            bulletsLeft--;
            bulletsShot--;

            Invoke(nameof(ResetShot), timeBetweenShooting);

            if (bulletsShot > 0 && bulletsLeft > 0)
                Invoke(nameof(Shoot), timeBetweenEachShots);
        }
        private void ResetShot()
        {
            readyToShoot = true;
        }
        private void HandleReloadAnimation()
        {
            if (!haveProceduralReload || aiming) return;
            if (reloading)
            {
                // Rotate the object continuously
                gunPositionHolder.Rotate(Vector3.forward, reloadRotationSpeed * Time.deltaTime);
            }
            else
            {
                // Return to the original rotation
                gunPositionHolder.localRotation = Quaternion.Lerp(gunPositionHolder.localRotation, originalReloadRotation, reloadRotationSpeed * Time.deltaTime);
            }
            
        }
        private void Reload()
        {
            if (totalBullets >= magazineSize)
            {
                reloading = true;
                bulletSound.PlayOneShot(bulletReloadClip, (soundVolume * 0.01f));
                Invoke(nameof(ReloadFinished), reloadTime);
                
            }
        }
        private void ReloadFinished()
        {
            if (totalBullets >= magazineSize)
            {
                bulletsLeft = magazineSize;
                totalBullets -= magazineSize;
                reloading = false;
            }

        }
        private void HandleWeaponSway()
        {
            if (!havePositionalSway) return;

            float tiltX = Mathf.Clamp(mouseX * swayIntensity * -1f, -swayAmount, swayAmount);
            float tiltY = Mathf.Clamp(mouseY * swayIntensity * -1f, -swayAmount, swayAmount);

            Vector3 finalPosition = new Vector3(0, tiltY, tiltX);
            transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + originPosition, Time.deltaTime * swaySmoothness);
        }
        private void HandleTilt()
        {
            if (!haveRotationalTilt) return;
            float tiltX = Mathf.Clamp(mouseX * tiltIntensity, -tiltAmount, tiltAmount);
            float tiltY = Mathf.Clamp(mouseY * tiltIntensity, -tiltAmount, tiltAmount);

            Quaternion finalRotation = Quaternion.Euler(new Vector3(
                rotateX ? tiltX : 0f,
                rotateY ? tiltY : 0f,
                rotateZ ? tiltY : 0f
                ));
            transform.localRotation = Quaternion.Slerp(transform.localRotation, finalRotation * originRotation, swaySmoothness * Time.deltaTime);
        }
        private void HandleCameraRotation()
        {
            if (!haveCameraRecoil) return;

            currentRotation = Vector3.Lerp(currentRotation, Vector3.zero, recoilReturnSpeed * Time.deltaTime);
            Rot = Vector3.Slerp(Rot, currentRotation, recoilRotationSpeed * Time.deltaTime);
            cameraRecoilHolder.transform.localRotation = Quaternion.Euler(Rot);
        }
        private void HandleGunRecoil()
        {
            if(!haveWeaponRecoil) return;
            rotationRecoil = Vector3.Lerp(rotationRecoil, Vector3.zero, gunRotationReturnSpeed * Time.deltaTime);
            positionRecoil = Vector3.Lerp(positionRecoil, Vector3.zero, gunPositionReturnSpeed * Time.deltaTime);

            gunPositionHolder.localPosition = Vector3.Slerp(gunPositionHolder.localPosition, positionRecoil, gunRecoilPositionSpeed * Time.deltaTime);
            rot = Vector3.Slerp(rot, rotationRecoil, gunRecoilRotationSpeed* Time.deltaTime);
            gunRotationHolder.localRotation = Quaternion.Euler(rot);
        }
        private void WeaponRotationSway()
        {
            if(!haveRotationalSway) return;

            Quaternion newAdjustedRotationX = Quaternion.AngleAxis(rotaionSwayIntensity * mouseX * -1f, Vector3.up);
            Quaternion targetRotation = rotationSwayOriginalRotation * newAdjustedRotationX;

            rotationSwayTransform.localRotation = Quaternion.Lerp(rotationSwayTransform.localRotation, targetRotation, rotationSwaySmoothness * Time.deltaTime);

        }
        private void WeaponMoveBobbing()
        {
            if(!haveBobbing) return;

            if (fpsController.isGrounded)
            {
                float delta = Time.deltaTime * idleSpeed;
                float velocity = (lastPosition - transform.position).magnitude * walkSpeedMultiplier;
                delta += Mathf.Clamp(velocity, 0, walkSpeedMax);

                // Reduce by two so that the gun animation is more U shaped
                sinX += delta / 2;
                sinY += delta;

                sinX %= Mathf.PI * 2;
                sinY %= Mathf.PI * 2;

                float magnitude = aiming ? this.magnitude / aimReduction : this.magnitude;
                transform.localPosition = Vector3.zero + magnitude * Mathf.Sin(sinY) * Vector3.up;
                transform.localPosition += magnitude * Mathf.Sin(sinX) * Vector3.right;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
            }
            lastPosition = transform.position;
        }
        private void JumpSwayEffect()
        {
            if(!haveJumpSway || aiming) return;

            if (!fpsController.isGrounded)
            {
                float yVelocity = fpsController.jumpVelocity.y;
                yVelocity = Mathf.Clamp(yVelocity, -weaponMinClamp, weaponMaxClamp);
                impactForce = -yVelocity * landingIntensity;
                if (aiming)
                {
                    yVelocity = Mathf.Max(yVelocity, 0);
                }
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(0f, 0f, yVelocity * jumpIntensity), Time.deltaTime * jumpSmooth);
            }
            else if (fpsController.isGrounded & impactForce >= 0)
            {
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(0, 0, impactForce), Time.deltaTime * landingSmooth);
                impactForce -= recoverySpeed * Time.deltaTime;
            }
            else
            {
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.identity, Time.deltaTime * landingSmooth);
            }
        }
        private void SetUIElements()
        {
            //SetText
            bulletCount.SetText(bulletsLeft + " / " + totalBullets);
        }
        #endregion
    }
}
