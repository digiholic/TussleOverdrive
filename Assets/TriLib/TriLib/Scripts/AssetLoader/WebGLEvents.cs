using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace TriLib
{
    /// <summary>
    /// Represents a series of WebGL interoperability methods.
    /// </summary>
    public class WebGLEvents : MonoBehaviour
    {
        /// <summary>
        /// Represents an HTML file upload event data.
        /// </summary>
        [Serializable]
        public struct UploadFileResult
        {
            /// <summary>
            /// Uploaded file name.
            /// </summary>
            public string Name;

            /// <summary>
            /// Uploaded file size, in bytes.
            /// </summary>
            public int Size;

            /// <summary>
            /// Uploaded file data pointer.
            /// </summary>
            public int Pointer;
        }

        [DllImport("__Internal")]
        private static extern void WebGL_SetupUploadEvent(string gameObjectName, string formName, string inputName);

        [DllImport("__Internal")]
        private static extern void WebGL_SetupPasteEvent(string gameObjectName);

        /// <summary>
        /// UploadDataLoadedEvent callback.
        /// </summary>
        private Action<string, byte[]> _onUploadDataLoaded;

        /// <summary>
        /// OnTextPasteEvent callback.
        /// </summary>
        private Action<string> _onTextPaste;

        /// <summary>
        /// Setups the file upload event.
        /// When the user selects a file on the input named as "inputName" inside the "formName" form, the "onUploadDataLoaded" callback will be called.
        /// </summary>
        /// <param name="formName">Element form name.</param>
        /// <param name="inputName">Element name.</param>
        /// <param name="onUploadDataLoaded">The action that will be called when user drag & drop a file to input named as "inputName".</param>
        public void SetupOnUploadDataLoadedEvent(string formName, string inputName, Action<string, byte[]> onUploadDataLoaded)
        {
            WebGL_SetupUploadEvent(gameObject.name, formName, inputName);
            _onUploadDataLoaded = onUploadDataLoaded;
        }

        /// <summary>
        /// Setups the text paste event.
        /// When the user pastes any text data into the webpage, the "onTextPaste" callback will be called.
        /// </summary>
        /// <param name="onTextPaste"></param>
        public void SetupOnTextPasteEvent(Action<string> onTextPaste)
        {
            WebGL_SetupPasteEvent(gameObject.name);
            _onTextPaste = onTextPaste;
        }

        /// <summary>
        /// This method receives an uploaded file data from Javascript and calls the configured action, if available.
        /// </summary>
        /// <param name="data">A <see cref="UploadFileResult"/>in JSON format.</param>
        private void UploadFieldUpdated(string data)
        {
            if (_onUploadDataLoaded != null)
            {
                var uploadFileResult = JsonUtility.FromJson<UploadFileResult>(data);
                var result = new byte[uploadFileResult.Size];
                if (uploadFileResult.Size != 0)
                {
                    var pointer = new IntPtr(uploadFileResult.Pointer);
                    Marshal.Copy(pointer, result, 0, uploadFileResult.Size);
                }
                _onUploadDataLoaded(uploadFileResult.Name, result);
            }
        }

        /// <summary>
        /// This method receives a clipboard pasted text data from Javascript and calls the configured action, if available.
        /// </summary>
        /// <param name="data">Pasted text data.</param>
        private void TextPasted(string data)
        {
            if (_onTextPaste != null)
            {
                _onTextPaste(data);
            }
        }
    }
}
