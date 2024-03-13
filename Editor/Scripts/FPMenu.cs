using FuzzPhyte.Utility;
using FuzzPhyte.Utility.Editor;
using System.Collections;
using UnityEditor;
using UnityEngine;
using FuzzPhyte.Control;
namespace FuzzPhyte.Control.Editor
{
    public class FPMenu : MonoBehaviour, IFPProductEditorUtility
    {
        private const string MenuB = FP_UtilityData.MENU_COMPANY + "/" + FPControlUtility.PRODUCT_NAME;
        private const string SetupMenuB = MenuB + "/Setup";
        private const string SetupSpawnController = SetupMenuB + "/Spawn Controller";

        //fake menus
        private const string SampleOutputHeader = MenuB + "/IMPORTED SAMPLES";


        [MenuItem(SetupSpawnController, priority = FP_UtilityData.ORDER_SUBMENU_LVL7+8)]
        protected static void ControllerSetup()
        {
            var returnPathVersion = FP_Utility_Editor.CreatePackageSampleFolder(FPControlUtility.PRODUCT_NAME_UNITY, FPControlUtility.BASEVERSION);
            var localPath = returnPathVersion.Item1;
            FP_Utility_Editor.CreateLayer("Player");
            //var fullPathDir = returnPathVersion.Item2;
            var asset = SO_ControlParameters.CreateInstance<SO_ControlParameters>();
            asset.LookSpeed = 1f;
            asset.SkinWidth = 0.01f;
            asset.StepOffset = 0.1f;
            asset.GroundLayerMask = LayerMask.GetMask("Default");
            asset.CanJump = true;
            asset.InverseLook = true;
            asset.CanRun = true;
            asset.JumpHeight = 2f;
            asset.GravityScale = 1f;
            asset.InAirJumpScale = 0.1f;
            asset.CapsuleRadius = 0.35f;
            asset.CapsuleHeight = 1.75f;
            asset.CharacterCenter = new Vector3(0, 0, 0);
            string assetPath = AssetDatabase.GenerateUniqueAssetPath("Assets\\"+localPath + "\\ControlParameter.asset");
            var returnPath = FP_Utility_Editor.CreateAssetAt(asset, assetPath);
            AssetDatabase.Refresh();
            Debug.Log($"{returnPath}");
            //spawn gameobject
            var go = new GameObject("FPControl");
            go.AddComponent<CharacterController>();
            go.layer = LayerMask.NameToLayer("Player");
            var feet = new GameObject("FPFeet");
            feet.transform.SetParent(go.transform);
            feet.transform.localPosition = new Vector3(0, -asset.CapsuleHeight/2, 0);
            go.GetComponent<CharacterController>().excludeLayers = LayerMask.GetMask("Player");
            var childCamera = new GameObject("Camera");
            childCamera.transform.SetParent(go.transform);
            childCamera.transform.localPosition = new Vector3(0, (asset.CapsuleHeight*0.5f)/1.1f, 0);
            childCamera.AddComponent<Camera>();
            go.AddComponent<UnityMonoController>();
            var getMonoController = go.GetComponent<UnityMonoController>();
            if (getMonoController != null)
            {
                getMonoController.PlayerControlData = asset;
                if(go.GetComponent<CharacterController>() != null)
                {
                    getMonoController.CharacterControllerComponent = go.GetComponent<CharacterController>();
                }
                getMonoController.ThePlayer = getMonoController.gameObject.transform;
                getMonoController.ThePlayerCamera = childCamera.GetComponent<Camera>();
                getMonoController.UseOldInputSystem = true;
                getMonoController.ThePlayerFeet = feet.transform;
            }
            //add mono script
            //assign camera as child
            //assign character controller

        }

        #region Fake Menu Headers
        // FAKE HEADER
        [MenuItem(SampleOutputHeader, false, FP_UtilityData.ORDER_SUBMENU_LVL4)]
        private static void FalseDataMenuHeader() { }

        // Ensure the header is non-functional
        [MenuItem(SampleOutputHeader, true)]
        private static bool FalseDataMenuHeaderValidation() => false;
        #endregion
        
        #region ProductEditor Utility Interface Requirements

        public string ReturnProductName()
        {
            return FPControlUtility.PRODUCT_NAME;
        }

        public string ReturnSamplePath()
        {
            return FPControlUtility.SAMPLESPATH;
        }
        #endregion
    }
}
