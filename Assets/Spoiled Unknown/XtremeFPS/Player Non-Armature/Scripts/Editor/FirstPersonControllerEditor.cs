/*Copyright � Spoiled Unknown*/
/*2024*/
/*Note: This is an important editor script*/

using UnityEditor;
using XtremeFPS.NonArm.FirstPersonController;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;


[CustomEditor(typeof(First_Person_Controller)), CanEditMultipleObjects]
public class FirstPersonControllerEditor : Editor
{
    First_Person_Controller fpsController;
    SerializedObject serFPS;

    private void OnEnable()
    {
        fpsController = (First_Person_Controller)target;
        serFPS = new SerializedObject(fpsController);
    }

    public override void OnInspectorGUI()
    {
        serFPS.Update();
        #region Intro
        EditorGUILayout.Space();
        GUI.color = Color.black;
        GUILayout.Label("Xtreme FPS Controller", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUI.color = Color.green;
        GUILayout.Label("Player Controller Script", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16 });
        EditorGUILayout.Space();
        #endregion
        #region Player Movement
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Player Movement Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        //Main Movement Settings
        GUI.color = Color.blue;
        GUILayout.Label("Basic Movement", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.characterController = (CharacterController)EditorGUILayout.ObjectField(new GUIContent("Character Crontroller", "Character controller attached to the player."), fpsController.characterController, typeof(CharacterController), true);
        fpsController.transitionSpeed = EditorGUILayout.Slider(new GUIContent("Transition Speed", "The speed at which any animation should play."), fpsController.transitionSpeed, 1f, 30f);
        fpsController.playerCanMove = EditorGUILayout.ToggleLeft(new GUIContent("Enable Player Movement", "Determines if the player is allowed to move."), fpsController.playerCanMove);
        if (fpsController.playerCanMove)
        {
            fpsController.walkSpeed = EditorGUILayout.Slider(new GUIContent("Walk Speed", "Determines how fast the player will move while walking."), fpsController.walkSpeed, .1f, fpsController.sprintSpeed);
            fpsController.walkSoundSpeed = EditorGUILayout.Slider(new GUIContent("Walk Sound Playback", "Determines the speed at which footstep sounds will play while walking."), fpsController.walkSoundSpeed, 0.1f, 0.5f);
            fpsController.playerCanSprint = EditorGUILayout.ToggleLeft(new GUIContent("Enable Sprinting", "Determines if the player is allowed to sprint."), fpsController.playerCanSprint);
            if (fpsController.playerCanSprint)
            {
                fpsController.isSprintHold = EditorGUILayout.ToggleLeft(new GUIContent("Is Sprint Hold", "Determines if the player has to hold sprint key or press/tap."), fpsController.isSprintHold);
                fpsController.sprintSpeed = EditorGUILayout.Slider(new GUIContent("Sprint Speed", "Determines how fast the player will move while sprinting."), fpsController.sprintSpeed, fpsController.walkSpeed, 20f);
                fpsController.sprintSoundSpeed = EditorGUILayout.Slider(new GUIContent("Sprint Sound Playback", "Determines the speed at which footstep sounds will play while sprinting."), fpsController.sprintSoundSpeed, 0.1f, 0.5f);

                EditorGUI.indentLevel++;
                fpsController.unlimitedSprinting = EditorGUILayout.ToggleLeft(new GUIContent("Unlimited Sprint", "Determines if 'Sprint Duration' is enabled. Turning this on will allow for unlimited sprint."), fpsController.unlimitedSprinting);
                if (!fpsController.unlimitedSprinting)
                {
                    fpsController.sprintDuration = EditorGUILayout.Slider(new GUIContent("Sprint Duration", "Determines how long the player can sprint while unlimited sprint is disabled."), fpsController.sprintDuration, 1f, 20f);
                    fpsController.sprintCooldown = EditorGUILayout.Slider(new GUIContent("Sprint Cooldown", "Determines how long the recovery time is when the player runs out of sprint."), fpsController.sprintCooldown, .1f, fpsController.sprintDuration);
                    fpsController.hasStaminaBar = EditorGUILayout.ToggleLeft(new GUIContent("Has Stamina Bar", "Determines whether a stamina bar is used or not."), fpsController.hasStaminaBar);
                    if (fpsController.hasStaminaBar)
                    {
                        fpsController.staminaSlider = (Slider)EditorGUILayout.ObjectField(new GUIContent("Stamina Bar", "Reference to the stamina bar itself."), fpsController.staminaSlider, typeof(Slider), true);
                    }
                }

                EditorGUI.indentLevel--;
            }
        }
        EditorGUILayout.Space();

        //Jumping and gravity settings
        GUI.color = Color.blue;
        GUILayout.Label("Jump And Gravity", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.canJump = EditorGUILayout.ToggleLeft(new GUIContent("Enable Player Jump", "Determines if the player is allowed to jump."), fpsController.canJump);
        if (fpsController.canJump)
        {
            fpsController.jumpHeight = EditorGUILayout.Slider(new GUIContent("Jump Height", "Determines how high can the player jump."), fpsController.jumpHeight, 0.1f, 10f);
        }
        fpsController.groundLayerID = EditorGUILayout.LayerField(new GUIContent("What is Ground?", "Determines what the ground is."), fpsController.groundLayerID);
        fpsController.groundCheckRadius = EditorGUILayout.Slider(new GUIContent("Ground Check Radius", "Sets the radius of the ground checking sphere."), fpsController.groundCheckRadius, 0.1f, 1f);
        fpsController.groundCheckTransform = (Transform)EditorGUILayout.ObjectField(new GUIContent("Ground Check Object", "Determines the position of ground checking sphere througha empty gameObject."), fpsController.groundCheckTransform, typeof(Transform), true);
        fpsController.gravitationalForce = EditorGUILayout.Slider(new GUIContent("Gravitational Force", "Sets the the gravitation force which will act on the player."), fpsController.gravitationalForce, 5f, 40f);
        EditorGUILayout.Space();

        //Crouching settings
        GUI.color = Color.blue;
        GUILayout.Label("Crouching", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.canCrouch = EditorGUILayout.ToggleLeft(new GUIContent("Enable Player Crouch", "Determines if the player is allowed to crouch."), fpsController.canCrouch);
        if (fpsController.canCrouch)
        {
            fpsController.isCrouchHold = EditorGUILayout.ToggleLeft(new GUIContent("Is Crouch Hold", "Determines if the player has to hold crouch key or press/tap."), fpsController.isCrouchHold);
            fpsController.crouchedHeight = EditorGUILayout.Slider(new GUIContent("Crouched Height", "Determines the height at which player should crouch."), fpsController.crouchedHeight, 0.5f, 5f);
            fpsController.crouchedSpeed = EditorGUILayout.Slider(new GUIContent("Crouched Speed", "Determines the speed at which player will move while crouched."), fpsController.crouchedSpeed, 1f, 5f);
            fpsController.crouchSoundPlayTime = EditorGUILayout.Slider(new GUIContent("Sound Playback Speed", "Determines the speed at which footstep sounds will play while crouched."), fpsController.crouchSoundPlayTime, 0.1f, 0.5f);
        }
        EditorGUILayout.Space();
        #endregion
        #region Camera Setup
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Player Camera Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        //Main Camera Settings
        GUI.color = Color.blue;
        GUILayout.Label("Camera Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.isCursorLocked = EditorGUILayout.ToggleLeft(new GUIContent("Is Cursor Locked", "Defines whether Cursor is locked."), fpsController.isCursorLocked);
        fpsController.cameraFollow = (Transform)EditorGUILayout.ObjectField(new GUIContent("Camera Root", "Camera root object which acts as look at point for cinemachine."), fpsController.cameraFollow, typeof(Transform), true);
        fpsController.playerVirtualCamera = (CinemachineVirtualCamera)EditorGUILayout.ObjectField(new GUIContent("Player Virtual Camera", "virtual Camera which player uses."), fpsController.playerVirtualCamera, typeof(CinemachineVirtualCamera), true);
        fpsController.FOV = EditorGUILayout.Slider(new GUIContent("Field Of View", "Determines the default Field Of View for the camera."), fpsController.FOV, 30f, 179f);
        fpsController.mouseSensitivity = EditorGUILayout.Slider(new GUIContent("Sensitivity", "Determines the senstivity at which camera will rotate."), fpsController.mouseSensitivity, 50f, 200f);
        fpsController.maximumClamp = EditorGUILayout.Slider(new GUIContent("Maximum Clamp Angle", "Determines the maximum angle at which the camera can reach while being rotated."), fpsController.maximumClamp, 0f, 90f);
        fpsController.minimumClamp = EditorGUILayout.Slider(new GUIContent("Minimum Clamp Angle", "Determines the minimum angle at which the camera can reach while being rotated."), fpsController.minimumClamp, 0f, -90f);
        fpsController.sprintFOV = EditorGUILayout.Slider(new GUIContent("Sprint FOV", "Determines the change in fov while sprinting."), fpsController.sprintFOV, fpsController.FOV, 179f);
        EditorGUILayout.Space();

        //Zoom Settings
        GUI.color = Color.blue;
        GUILayout.Label("Zoom Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.enableZoom = EditorGUILayout.ToggleLeft(new GUIContent("Enable Zoom", "Determines if the player is able to zoom in while playing."), fpsController.enableZoom);
        if (fpsController.enableZoom)
        {
            fpsController.isZoomingHold = EditorGUILayout.ToggleLeft(new GUIContent("Is Zoom Hold", "Determines if the player has to hold zoom key or press/tap."), fpsController.isZoomingHold);
            fpsController.zoomFOV = EditorGUILayout.Slider(new GUIContent("Zoom FOV", "Determines the field of view the camera zooms to."), fpsController.zoomFOV, .1f, fpsController.FOV);
            fpsController.zoomStepTime = EditorGUILayout.Slider(new GUIContent("Zoom Transition Speed", "Determines how fast the FOV transitions while zooming in."), fpsController.zoomStepTime, .1f, 10f);
        }

        //Head Bobbing Settings
        GUI.color = Color.blue;
        GUILayout.Label("Head Bobbing", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.canHeadBob = EditorGUILayout.ToggleLeft(new GUIContent("Can Head Bob", "Defines whether player's head can bob or not."), fpsController.canHeadBob);
        if (fpsController.canHeadBob)
        {
            fpsController.headBobAmplitude = EditorGUILayout.Slider(new GUIContent("Head Bob Amplitude", "Determines the amplitude at which nthe head will bob."), fpsController.headBobAmplitude, 0f, 1f);
            fpsController.headBobFrequency = EditorGUILayout.Slider(new GUIContent("Head Bob Frequency", "Defines how frequently the head will bob."), fpsController.headBobFrequency, 5f, 30f);
        }
        EditorGUILayout.Space();
        #endregion
        #region Audio Setup
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Audio Setup", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        GUI.color = Color.white;
        fpsController.canPlaySound = EditorGUILayout.ToggleLeft(new GUIContent("Can Play Sound", "Determines whether player should make footsteps sounds or not."), fpsController.canPlaySound);
        if (fpsController.canPlaySound)
        {
            fpsController.grassTag = EditorGUILayout.TagField(new GUIContent("Grass Tag", "Tag of the gameObject that will act as grass."), fpsController.grassTag);
            SerializedProperty soundGrassProperty = serializedObject.FindProperty("soundGrass");
            EditorGUILayout.PropertyField(soundGrassProperty, new GUIContent("Grass Sound Effect", "The sound that plays as footstep while walking on a grassy surface."), true);
            serializedObject.ApplyModifiedProperties();
            fpsController.concreteTag = EditorGUILayout.TagField(new GUIContent("Concrete Tag", "Tag of the gameObject that will act as concrete."), fpsController.concreteTag);
            SerializedProperty soundConcreteProperty = serializedObject.FindProperty("soundConcrete");
            EditorGUILayout.PropertyField(soundConcreteProperty, new GUIContent("Concrete Sound Effect", "The sound that plays as footstep while walking on a concrete."), true);
            serializedObject.ApplyModifiedProperties();
            fpsController.waterTag = EditorGUILayout.TagField(new GUIContent("Water Tag", "Tag of the gameObject that will act as water."), fpsController.waterTag);
            SerializedProperty soundWaterProperty = serializedObject.FindProperty("soundWater");
            EditorGUILayout.PropertyField(soundWaterProperty, new GUIContent("Water Sound Effect", "The sound that plays as footstep while walking on a water."), true);
            serializedObject.ApplyModifiedProperties();
            fpsController.metalTag = EditorGUILayout.TagField(new GUIContent("Metal Tag", "Tag of the gameObject that will act as metal."), fpsController.metalTag);
            SerializedProperty soundMetalProperty = serializedObject.FindProperty("soundMetal");
            EditorGUILayout.PropertyField(soundMetalProperty, new GUIContent("Metal Sound Effect", "The sound that plays as footstep while walking on a metallic surface."), true);
            serializedObject.ApplyModifiedProperties();
            fpsController.gravelTag = EditorGUILayout.TagField(new GUIContent("Gravel Tag", "Tag of the gameObject that will act as gravel."), fpsController.gravelTag);
            SerializedProperty soundGravelProperty = serializedObject.FindProperty("soundGravel");
            EditorGUILayout.PropertyField(soundGravelProperty, new GUIContent("Gravel Sound Effect", "The sound that plays as footstep while walking on a gravel."), true);
            fpsController.woodTag = EditorGUILayout.TagField(new GUIContent("Wood Tag", "Tag of the gameObject that will act as wood."), fpsController.woodTag);
            serializedObject.ApplyModifiedProperties();
            SerializedProperty soundWoodProperty = serializedObject.FindProperty("soundWood");
            EditorGUILayout.PropertyField(soundWoodProperty, new GUIContent("Wood Sound Effect", "The sound that plays as footstep while walking on wooden surface."), true);
            serializedObject.ApplyModifiedProperties();
            fpsController.jumpClip = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Jump Sound Effect", "The sound that plays when the player jumps."), fpsController.jumpClip, typeof(AudioClip), true);
            fpsController.landClip = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("land Sound Effect", "The sound that plays when the player Lands."), fpsController.landClip, typeof(AudioClip), true);
            fpsController.footstepSensitivity = EditorGUILayout.Slider(new GUIContent("Footstep Sensitivity", "Determines how fast the player should move before the footstep plays."), fpsController.footstepSensitivity, 0f, 5f);
        }
        EditorGUILayout.Space();
        #endregion
        #region Physics
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Physics Settings", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.white;
        fpsController.canPush = EditorGUILayout.ToggleLeft(new GUIContent("Can Push", "Defines whether player can push other objects or not."), fpsController.canPush);
        if (fpsController.canPush)
        {
            fpsController.pushLayersID = EditorGUILayout.LayerField(new GUIContent("What can be pushed?", "Determines what layers can the player push."), fpsController.pushLayersID);
            fpsController.pushStrength = EditorGUILayout.Slider(new GUIContent("Push Strength", "Determines the strength at which the player should push."), fpsController.pushStrength, 0f, 10f);
        }
        #endregion
        #region Armature Setting
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Armature Crouching (Beta)", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();

        GUI.color = Color.red;
        GUILayout.Label("Armature here means only simple meshes(like capsule).", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        GUI.color = Color.white;
        fpsController.hasArmature = EditorGUILayout.ToggleLeft(new GUIContent("Have Armature", "Defines whether player have a armature or not."), fpsController.hasArmature);
        if (fpsController.hasArmature)
        {
            fpsController.armature = (Transform)EditorGUILayout.ObjectField(new GUIContent("Armature", "Armature's transform position."), fpsController.armature, typeof(Transform), true);
            fpsController.newArmatureHeight = EditorGUILayout.FloatField(new GUIContent("Crouched Height", "The height which the armature will reach while crouching."), fpsController.newArmatureHeight);
        }
        #endregion
        #region Debug Only
        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Debug Only", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 13 }, GUILayout.ExpandWidth(true));
        EditorGUILayout.Space();
        GUI.color = Color.white;

        fpsController.debugMode = EditorGUILayout.ToggleLeft(new GUIContent("Debug Mode", "only for debugging, does not compiles in builds."), fpsController.debugMode);
        EditorGUILayout.Space();

        GUI.color = Color.black;
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        #endregion
        #region Update Changes
        //Sets any changes from the prefab
        if (GUI.changed)
        {
            EditorUtility.SetDirty(fpsController);
            Undo.RecordObject(fpsController, "FPC Change");
            serFPS.ApplyModifiedProperties();
        }
        #endregion
    }

}
