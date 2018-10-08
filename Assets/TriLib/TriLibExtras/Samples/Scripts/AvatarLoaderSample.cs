using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.SceneManagement;
#endif
using System.IO;
using System.Linq;

namespace TriLib.Extras
{
    /// <summary>
    /// Represents the avatar loader sample scene.
    /// </summary>
    [ExecuteInEditMode]
    public class AvatarLoaderSample : MonoBehaviour
    {
        /// <summary>
        /// Use this field to specity the Standard Assets FreeLookCam Prefab.
        /// </summary>
        public GameObject FreeLookCamPrefab;

        /// <summary>
        /// Use this field to specity the Standard Assets ThirdPersonController Prefab.
        /// </summary>
        public GameObject ThirdPersonControllerPrefab;

        /// <summary>
        /// Active Camera Game Object reference.
        /// </summary>
        public GameObject ActiveCameraGameObject;

        /// <summary>
        /// Use this field to specify your models directory within the current Application directory.
        /// </summary>
        public string ModelsDirectory = "Models";

        /// <summary>
        /// Available avatar files list.
        /// </summary>
        private string[] _files = null;

        /// <summary>
        /// UI Window area.
        /// </summary>
        private Rect _windowRect;

        /// <summary>
        /// UI scroll position.
        /// </summary>
        private Vector3 _scrollPosition;

        /// <summary>
        /// Avatar Loader script reference.
        /// </summary>
        private AvatarLoader _avatarLoader;

#if UNITY_EDITOR
        /// <summary>
        /// Setup sample menu item path.
        /// </summary>
        private const string SetupAvatarSampleMenuPath = "TriLib/Configure Avatar Loader Sample";

        /// <summary>
        /// Dialogs title.
        /// </summary>
        private const string DialogTitle = "TriLib - Configure Sample";

        /// <summary>
        /// Setups the Avatar Loader Sample Scene.
        /// </summary>
        [MenuItem(SetupAvatarSampleMenuPath)]
        private static void SetupAvatarLoaderSample()
        {
            var avatarLoaderSample = FindObjectOfType<AvatarLoaderSample>();
            if (avatarLoaderSample == null)
            {
                EditorUtility.DisplayDialog(DialogTitle, "Could not find \"AvatarLoaderSample\" Script instance.\nPlease re-extract the TriLib package contents and try-again.", "Ok");
                return;
            }
            var thirdPersonControllerResults = AssetDatabase.FindAssets("ThirdPersonController t:GameObject");
            foreach (var thirdPersonControllerResult in thirdPersonControllerResults)
            {
                var thirdPersonControllerPrefabPath = AssetDatabase.GUIDToAssetPath(thirdPersonControllerResult);
                if (thirdPersonControllerPrefabPath.IndexOf("/ThirdPersonController.prefab", System.StringComparison.Ordinal) > -1)
                {
                    avatarLoaderSample.ThirdPersonControllerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(thirdPersonControllerPrefabPath);
                    var freeLookCameraRigResults = AssetDatabase.FindAssets("FreeLookCameraRig t:GameObject");
                    foreach (var freeLookCameraRigResult in freeLookCameraRigResults)
                    {
                        var freeLookCameraRigPath = AssetDatabase.GUIDToAssetPath(freeLookCameraRigResult);
                        if (freeLookCameraRigPath.IndexOf("/FreeLookCameraRig.prefab", System.StringComparison.Ordinal) > -1)
                        {
                            avatarLoaderSample.FreeLookCamPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(freeLookCameraRigPath);
                            EditorUtility.DisplayDialog(DialogTitle, "Sample configured.", "Ok");
                            return;
                        }
                    }
                    break;
                }
            }
            EditorUtility.DisplayDialog(DialogTitle, "To configure the Avatar Loader Sample, please download and extract \"Standard Assets\" from Asset Store first.", "Ok");
        }

        /// <summary>
        /// Validates the Setup Sample Menu.
        /// </summary>
        /// <returns><c>true</c>, if user loaded AvatarLoader scene, <c>false</c> otherwise.</returns>
        [MenuItem(SetupAvatarSampleMenuPath, true)]
        public static bool SetupAvatarLoaderSampleValidate()
        {
            return SceneManager.GetActiveScene().name == "AvatarLoader";
        }
#endif

        /// <summary>
        /// Setups the Avatar Loader instance reference and fills the available files list.
        /// </summary>
        protected void Start()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (FreeLookCamPrefab == null || ThirdPersonControllerPrefab == null)
                {
                    if (EditorUtility.DisplayDialog(DialogTitle, "This sample is not configured.\nDo you want to configure it now?", "Yes", "No"))
                    {
                        SetupAvatarLoaderSample();
                    }
                }
            }
#endif
            _avatarLoader = FindObjectOfType<AvatarLoader>();
            if (_avatarLoader == null)
            {
#if TRILIB_OUTPUT_MESSAGES || ASSIMP_OUTPUT_MESSAGES
                Debug.LogError("Could not find any Avatar Loader script instance.");
#endif
                return;
            }
#if UNITY_EDITOR
            var modelsPath = Path.Combine(Path.GetFullPath("./Assets/TriLib/TriLibExtras/Samples"), ModelsDirectory);
#else
                        var modelsPath = Path.Combine(Path.GetFullPath("."), ModelsDirectory); 
#endif
            var supportedExtensions = AssetLoaderBase.GetSupportedFileExtensions();
            _files = Directory.GetFiles(modelsPath, "*.*").Where(x => supportedExtensions.Contains(Path.GetExtension(x).ToLower())).ToArray();
            _windowRect = new Rect(20, 20, 240, Screen.height - 40);
        }

        /// <summary>
        /// Shows available files and let user select them from the UI.
        /// </summary>
        protected void OnGUI()
        {
            if (_files == null || _avatarLoader == null || FreeLookCamPrefab == null || ThirdPersonControllerPrefab == null)
            {
                return;
            }
            _windowRect = GUI.Window(0, _windowRect, HandleWindowFunction, "Available Models");
        }

        /// <summary>
        /// Handles the available files UI Window.
        /// </summary>
        /// <param name="id">Window identifier.</param>
        private void HandleWindowFunction(int id)
        {
            GUILayout.BeginVertical();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            foreach (var file in _files)
            {
                if (GUILayout.Button(Path.GetFileName(file)))
                {
                    LoadFile(file);
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        private void LoadFromMemory(byte[] data, string fileExtension)
        {
            var thirdPersonController = Instantiate(ThirdPersonControllerPrefab);
            thirdPersonController.transform.DestroyChildren(true);
            if (_avatarLoader.LoadAvatarFromMemory(data, fileExtension, thirdPersonController))
            {
                if (ActiveCameraGameObject != null)
                {
                    Destroy(ActiveCameraGameObject.gameObject);
                }
                ActiveCameraGameObject = Instantiate(FreeLookCamPrefab);
            }
            else
            {
                if (ActiveCameraGameObject != null)
                {
                    Destroy(ActiveCameraGameObject.gameObject);
                }
                Destroy(thirdPersonController);
            }
        }

        private void LoadFile(string file)
        {
            var thirdPersonController = Instantiate(ThirdPersonControllerPrefab);
            thirdPersonController.transform.DestroyChildren(true);
            if (_avatarLoader.LoadAvatar(file, thirdPersonController))
            {
                if (ActiveCameraGameObject != null)
                {
                    Destroy(ActiveCameraGameObject.gameObject);
                }
                ActiveCameraGameObject = Instantiate(FreeLookCamPrefab);
            }
            else
            {
                if (ActiveCameraGameObject != null)
                {
                    Destroy(ActiveCameraGameObject.gameObject);
                }
                Destroy(thirdPersonController);
            }
        }
    }
}
