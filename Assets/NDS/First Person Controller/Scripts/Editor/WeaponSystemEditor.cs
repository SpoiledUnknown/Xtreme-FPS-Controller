using NDS.FirstPersonController;
using NDS.InputManager;
using NDS.UniversalWeaponSystem;
using TMPro;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponSystem)), CanEditMultipleObjects]
public class WeaponSystemEditor : Editor
{
    WeaponSystem ws;
    SerializedObject serWS;
        

    private void OnEnable()
    {
        ws = (WeaponSystem)target;
        serWS = new SerializedObject(ws);
    }

    public override void OnInspectorGUI()
    {
        serWS.Update();
        #region Intro
            EditorGUILayout.Space();
            GUI.color = Color.black;
            GUILayout.Label("Xtreme FPS Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUI.color = Color.green;
            GUILayout.Label("Universal Weapon Script", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
            EditorGUILayout.Space();
        #endregion
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("References", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        
        //Main Movement Settings
        GUI.color = Color.white;
        ws.inputManager = (FPSInputManager)EditorGUILayout.ObjectField(new GUIContent("Input Manager", "Reference to Input Manager script."), ws.inputManager, typeof(FPSInputManager), true);
        ws.fpsController = (First_Person_Controller)EditorGUILayout.ObjectField(new GUIContent("Player Controller", "Reference to player controller script."), ws.fpsController, typeof(First_Person_Controller), true);
        ws.shootPoint = (Transform)EditorGUILayout.ObjectField(new GUIContent("Shoot Point", "Reference to the transform of the point fro where bullets will spawn (Ideally it should be a child of gun model itself)."), ws.shootPoint, typeof(Transform), true);
        ws.muzzleFlash = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Muzzle Flash", "Reference to the gameobject that will be spawned as muzzle flash (spawns at the co-ordinates of shootPoint)."), ws.muzzleFlash, typeof(GameObject), true);
        ws.bulletPrefab = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Bullet", "Reference to the bullet gameobject itself (it can be any same but should contain ParabolicBullet script)."), ws.bulletPrefab, typeof(GameObject), true);
        ws.bulletCount = (TextMeshProUGUI)EditorGUILayout.ObjectField(new GUIContent("Bullet Count", "Reference to the text that shows number of bullets on UI."), ws.bulletCount, typeof(TextMeshProUGUI), true);
        EditorGUILayout.Space();
        
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Bullet Physics", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        
        GUI.color = Color.white;
        ws.bulletSpeed = EditorGUILayout.Slider(new GUIContent("Bullet Velocity", "The velocity at which the bullet will move."), ws.bulletSpeed, 200f, 1500f);
        ws.bulletLifeTime = EditorGUILayout.Slider(new GUIContent("Bullet Life", "The time after which the bullet will despawn itself."), ws.bulletLifeTime, 5f, 100f);
        ws.bulletGravitationalForce = EditorGUILayout.Slider(new GUIContent("Bullet Gravity", "Defines the value of gravity that will act on the bullet."), ws.bulletGravitationalForce, 0f, 15f);
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Weapon Stats", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        GUI.color = Color.white;
        ws.isGunAuto = EditorGUILayout.ToggleLeft(new GUIContent("Is Gun Auto", "Determines if the gun is automatic or tap-tap."), ws.isGunAuto);
        ws.timeBetweenEachShots = EditorGUILayout.Slider(new GUIContent("Time Between Each Shots", "Determines the time after which each shots will be fired."), ws.timeBetweenEachShots, 0f, 1f);
        ws.timeBetweenShooting = EditorGUILayout.Slider(new GUIContent("Time Between Shooting", "Determines the time it will take for weapon to load new bullets."), ws.timeBetweenShooting, 0f, 1f);
        ws.Spread = EditorGUILayout.Slider(new GUIContent("Bullet Spread", "Determines the bullet spread."), ws.Spread, 0f, 1f);
        ws.magazineSize = EditorGUILayout.IntSlider(new GUIContent("Magazine Size", "Determines the number of bullet the weapon will hold."), ws.magazineSize, 0, 200);
        ws.totalBullets = EditorGUILayout.IntSlider(new GUIContent("Total Bullets", "Determines the number of bullet the player have."), ws.totalBullets, 0, 999);
        ws.bulletsPerTap = EditorGUILayout.IntSlider(new GUIContent("Bullet Per Shoot", "Determines the number of bullet the player will shoot in single tap/shoot cycle."), ws.bulletsPerTap, 0, 30);
        ws.muzzelEffectLifeTime = EditorGUILayout.Slider(new GUIContent("Muzzle LifeTime", "Determines the time muzzle flash will saty before going out of existence for good."), ws.muzzelEffectLifeTime, 0f, 10f);
        ws.reloadTime = EditorGUILayout.Slider(new GUIContent("Reloading Time", "Determines the time weapon takes to reload."), ws.reloadTime, 0f, 10f);
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Reload Animation (Beta)", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.haveProceduralReload = EditorGUILayout.ToggleLeft(new GUIContent("Have Procedural Animation", "Determines if the gun should play built-in animation or not (turn off if you have another animation handler or you have another animation)."), ws.haveProceduralReload);
        if (ws.haveProceduralReload)
        {
            ws.reloadRotationSpeed = EditorGUILayout.Slider(new GUIContent("Animation Speed", "Determines the speed at which the animation will play."), ws.reloadRotationSpeed, 0f, 1000f);
        }
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("ADS Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Aiming", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.canAim = EditorGUILayout.ToggleLeft(new GUIContent("ADS", "Determines if the weapon should aim or not."), ws.canAim);
        GUI.enabled = ws.canAim;
        ws.isAimHold = EditorGUILayout.ToggleLeft(new GUIContent("Is Aim Hold", "Determines if the player has to hold aim key or press/tap."), ws.isAimHold);
        ws.weaponHolder = (Transform)EditorGUILayout.ObjectField(new GUIContent("Weapon Holder", "Reference to the Weapon Holder gameobject (can be anything but must be child of camera and parent of all object needed for weapon system to work)."), ws.weaponHolder, typeof(Transform), true);
        ws.aimingLocalPosition = EditorGUILayout.Vector3Field(new GUIContent("Aim Position", "Determines the position which the gun will saty at while aiming."), ws.aimingLocalPosition);
        ws.aimSmoothing = EditorGUILayout.Slider(new GUIContent("Aim Speed", "Determines the speed at which the gun will reach aim state."), ws.aimSmoothing, 5f, 100f);
        GUI.enabled = true;
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Peek", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.canPeek = EditorGUILayout.ToggleLeft(new GUIContent("Peek", "Determines if the player can peek or not."), ws.canPeek);
        GUI.enabled = ws.canPeek;
        ws.cameraHolderForPeeking = (Transform)EditorGUILayout.ObjectField(new GUIContent("Camera Holder", "Reference to the Camera Holder gameobject (gameobject parent of camera (cameraRecoil as well if game have recoil system))."), ws.cameraHolderForPeeking, typeof(Transform), true);
        ws.peekLeftPosition = EditorGUILayout.Vector3Field(new GUIContent("Peek Left Position", "Determines the position which the gun will saty at while peeking left."), ws.peekLeftPosition);
        ws.peekRightPosition = EditorGUILayout.Vector3Field(new GUIContent("Peek Right Position", "Determines the position which the gun will saty at while peeking right."), ws.peekRightPosition);
        ws.peekSmoothing = EditorGUILayout.Slider(new GUIContent("Peek Speed", "Determines the speed at which the gun will reach peek state."), ws.peekSmoothing, 5f, 100f);
        GUI.enabled = true;

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Recoil Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Weapon Recoil", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.haveWeaponRecoil = EditorGUILayout.ToggleLeft(new GUIContent("Weapon has Recoil", "Determines if the weapon should have recoil or not."), ws.haveWeaponRecoil);
        GUI.enabled = ws.haveWeaponRecoil;
        ws.gunPositionHolder = (Transform)EditorGUILayout.ObjectField(new GUIContent("Weapon Position Holder", "Reference to the transform of GameObject that holds gun for positional recoil (A Parent object of gunRotationHolder)."), ws.gunPositionHolder, typeof(Transform), true);
        ws.gunRecoilPositionSpeed = EditorGUILayout.Slider(new GUIContent("Positional Speed", "Determines the speed at which the gun will move."), ws.gunRecoilPositionSpeed, 0f, 50f);
        ws.gunPositionReturnSpeed = EditorGUILayout.Slider(new GUIContent("Positional Return Speed", "Determines the speed at which the gun will return to its usual position."), ws.gunPositionReturnSpeed, 0f, 50f);
        ws.recoilKickBackAds = EditorGUILayout.Vector3Field(new GUIContent("Kick Back (ADS)", "Determines the positional kick of gun while aiming."), ws.recoilKickBackAds);
        ws.recoilKickBackHip = EditorGUILayout.Vector3Field(new GUIContent("Kick Back (Hip)", "Determines the positional kick of gun while hipfire."), ws.recoilKickBackHip);
        ws.gunRotationHolder = (Transform)EditorGUILayout.ObjectField(new GUIContent("Weapon Rotation Holder", "Reference to the transform of GameObject that holds gun for rotaional recoil."), ws.gunRotationHolder, typeof(Transform), true);
        ws.gunRecoilRotationSpeed = EditorGUILayout.Slider(new GUIContent("Rotational Speed", "Determines the speed at which the gun will rotate."), ws.gunRecoilRotationSpeed, 0f, 50f);
        ws.gunRotationReturnSpeed= EditorGUILayout.Slider(new GUIContent("Rotational Return Speed", "Determines the speed at which the gun will return to its usual rotation."), ws.gunRotationReturnSpeed, 0f, 50f);
        ws.recoilRotationAds = EditorGUILayout.Vector3Field(new GUIContent("Rotation (ADS)", "Determines the rotational position change of gun while aiming."), ws.recoilRotationAds);
        ws.recoilRotationHip = EditorGUILayout.Vector3Field(new GUIContent("Rotation (Hip)", "Determines the rotational position change of gun while hipfire."), ws.recoilRotationHip);
        GUI.enabled = true;
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Camera Recoil", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.haveCameraRecoil = EditorGUILayout.ToggleLeft(new GUIContent("Have Camera Recoil", "Determines if the camera should have recoil or not."), ws.haveCameraRecoil);
        GUI.enabled = ws.haveCameraRecoil;
        ws.cameraRecoilHolder = (Transform)EditorGUILayout.ObjectField(new GUIContent("Camera Recoil Holder", "Reference to the transform of GameObject that holds camera."), ws.cameraRecoilHolder, typeof(Transform), true);
        ws.recoilRotationSpeed = EditorGUILayout.Slider(new GUIContent("Recoil Speed", "Determines the speed at which the camera will shake."), ws.recoilRotationSpeed, 0f, 50f);
        ws.recoilReturnSpeed = EditorGUILayout.Slider(new GUIContent("Recoil Return Speed", "Determines the speed at which the camera will return to its usual position."), ws.recoilReturnSpeed, 0f, 50f);
        ws.adsFireRecoil = EditorGUILayout.Vector3Field(new GUIContent("Recoil (ADS)", "Determines the recoil camera will feel while aiming."), ws.adsFireRecoil);
        ws.hipFireRecoil = EditorGUILayout.Vector3Field(new GUIContent("Recoil (Hip)", "Determines the recoil camera will feel while hipfire."), ws.hipFireRecoil);
        GUI.enabled = true;
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Weapon Sway & Tilt Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Positional Sway", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.havePositionalSway = EditorGUILayout.ToggleLeft(new GUIContent("Have Positional Sway", "Determines if the gun have positional sway or not."), ws.havePositionalSway);
        GUI.enabled = ws.havePositionalSway;
        ws.swayIntensity = EditorGUILayout.Slider(new GUIContent("Intensity", "Determines the intensity at which the gun will sway."), ws.swayIntensity, 0f, 1f);
        ws.swayAmount = EditorGUILayout.Slider(new GUIContent("Amount", "Determines the amount of sway."), ws.swayAmount, 0f, 10f);
        ws.swaySmoothness = EditorGUILayout.Slider(new GUIContent("Speed", "Determines the speed at which the gun will sway."), ws.swaySmoothness, 0f, 50f);
        GUI.enabled = true;
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Rotational Sway", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.haveRotationalSway = EditorGUILayout.ToggleLeft(new GUIContent("Have Rotational Sway", "Determines if the gun have rotational sway or not."), ws.haveRotationalSway);
        GUI.enabled = ws.haveRotationalSway;
        if(ws.haveRotationalSway)
        {
            ws.rotationSwayTransform = (Transform)EditorGUILayout.ObjectField(new GUIContent("Sway Holder", "Reference to the transform of GameObject that holds the gun (usually child of 'Weapon Rotation Holder' and parent of gun itself)."), ws.rotationSwayTransform, typeof(Transform), true);
        }
        ws.rotaionSwayIntensity = EditorGUILayout.Slider(new GUIContent("Intensity", "Determines the intensity at which the gun will rotationally sway."), ws.rotaionSwayIntensity, 0f, 10f);
        ws.rotationSwaySmoothness = EditorGUILayout.Slider(new GUIContent("Speed", "Determines the speed at which the gun will rotationally sway."), ws.rotationSwaySmoothness, 0f, 50f);
        GUI.enabled = true;
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Jump Sway", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.haveJumpSway = EditorGUILayout.ToggleLeft(new GUIContent("Have Jump Sway", "Determines if the gun have jump sway or not."), ws.haveJumpSway);
        GUI.enabled = ws.haveJumpSway;
        ws.weaponMaxClamp = EditorGUILayout.Slider(new GUIContent("Maximum Clamp Angle", "Determines the maximum clamp angle."), ws.weaponMaxClamp, 0f, 25f);
        ws.weaponMinClamp = EditorGUILayout.Slider(new GUIContent("Minimum Clamp Angle", "Determines the minimum clamp angle."), ws.weaponMinClamp, 0f, 25f);
        ws.jumpIntensity = EditorGUILayout.Slider(new GUIContent("Jump Intensity", "Determines the intensity of rotation when jumping."), ws.jumpIntensity, 0f, 25f);
        ws.jumpSmooth = EditorGUILayout.Slider(new GUIContent("Jump Smooth", "Determines the speed at which gun will change rotation when jumping."), ws.jumpSmooth, 0f, 25f);
        ws.landingIntensity = EditorGUILayout.Slider(new GUIContent("Landing Intensity", "Determines the intensity of rotation when landing."), ws.landingIntensity, 0f, 25f);
        ws.landingSmooth = EditorGUILayout.Slider(new GUIContent("Landing Smooth", "Determines the speed at which gun will change rotation when landing."), ws.landingSmooth, 0f, 25f);
        ws.recoverySpeed = EditorGUILayout.Slider(new GUIContent("Recovery Speed", "Determines the speed at which gun will recover and reach its usual position/rotation."), ws.recoverySpeed, 0f, 100f);
        GUI.enabled = true;
        EditorGUILayout.Space();
        GUI.color = Color.blue;
        GUILayout.Label("Weapon Tilt", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        ws.haveRotationalTilt = EditorGUILayout.ToggleLeft(new GUIContent("Have Tilt", "Determines if the gun can tilt or not."), ws.haveRotationalTilt);
        GUI.enabled = ws.haveRotationalTilt;
        ws.tiltIntensity = EditorGUILayout.Slider(new GUIContent("Intensity", "Determines the intensity of the tilting."), ws.tiltIntensity, 0f, 25f);
        ws.tiltAmount = EditorGUILayout.Slider(new GUIContent("Amount", "Determines the amount of the tilting."), ws.tiltAmount, 0f, 25f);
        ws.tiltSmoothness = EditorGUILayout.Slider(new GUIContent("Smoothness", "Determines how fast the tilting will happened."), ws.tiltSmoothness, 0f, 25f);
        ws.rotateX = EditorGUILayout.ToggleLeft(new GUIContent("Tilt X", "Determines the axis at which the weapon will rotate (X - Axis)."), ws.rotateX);
        ws.rotateY = EditorGUILayout.ToggleLeft(new GUIContent("Tilt Y", "Determines the axis at which the weapon will rotate (Y - Axis)."), ws.rotateY);
        ws.rotateZ = EditorGUILayout.ToggleLeft(new GUIContent("Tilt Z", "Determines the axis at which the weapon will rotate (Z - Axis)."), ws.rotateZ);
        GUI.enabled = true;
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Weapon Bobbing", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.white;
        ws.haveBobbing = EditorGUILayout.ToggleLeft(new GUIContent("have Bobbing", "Determines if the gun have bobbing effect or not."), ws.haveBobbing);
        GUI.enabled = ws.haveBobbing;
        ws.magnitude = EditorGUILayout.Slider(new GUIContent("Magnitude", "Determines the magnitude of the bobbing effect."), ws.magnitude, 0f, 0.02f);
        ws.idleSpeed = EditorGUILayout.Slider(new GUIContent("Idle Speed", "Determines speed of bobbing when not moving."), ws.idleSpeed, 0f, 5f);
        ws.walkSpeedMultiplier = EditorGUILayout.Slider(new GUIContent("Walk Speed Multiplier", "Determines how fast the weapon should bob depending upon the speed of player."), ws.walkSpeedMultiplier, 0f, 10f);
        ws.walkSpeedMax = EditorGUILayout.Slider(new GUIContent("Walk Maximum Speed", "Determines the maximum speed the weapon can bob."), ws.walkSpeedMax, 0f, 15f);
        ws.aimReduction = EditorGUILayout.Slider(new GUIContent("ADS Reduction", "Determines the reduction of speed of bobbing effect when aiming/ taking ADS."), ws.aimReduction, 0f, 10f);
        GUI.enabled = true;
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Sound Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.white;
        ws.bulletSoundClip = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Fire fx", "The sound that plays when player shoots."), ws.bulletSoundClip, typeof(AudioClip), true);
        ws.bulletReloadClip = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Reload fx", "The sound that plays when the player reloads the gun."), ws.bulletReloadClip, typeof(AudioClip), true);
        ws.soundVolume = EditorGUILayout.Slider(new GUIContent("Volume", "Determines the volume of the sound clips in percentage."), ws.soundVolume, 0f, 100f);
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        #region Update Changes
        //Sets any changes from the prefab
        if (GUI.changed)
            {
                EditorUtility.SetDirty(ws);
                Undo.RecordObject(ws, "FPC Change");
                serWS.ApplyModifiedProperties();
            }
        #endregion
    } 
}
