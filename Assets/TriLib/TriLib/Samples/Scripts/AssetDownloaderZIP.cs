using System;
using System.IO;
using System.Collections;
using UnityEngine;
#if TRILIB_USE_ZIP
#if !UNITY_EDITOR && UNITY_WINRT
using System.IO.Compression;
#else
using ICSharpCode.SharpZipLib.Zip;
#endif
#endif
namespace TriLib
{
    public class AssetDownloaderZIP : MonoBehaviour
    {
        //Stores a reference for file downloaders
        private WWW _fileDownloader;

        //Start logic
        private void Start()
        {
			//You can change this to the URL of the file you want to download
			var url = "http://ricardoreis.net/trilib/test1.zip";
			
            //Loads the file ,if existing, or download the file
            LoadOrDownload(url);
        }
		
		//This function will run when the object is loaded
		private void OnFileDownloaded(GameObject loadedGameObject) {
			//Move camera away to fit the loaded object in view
			CameraExtensions.FitToBounds(Camera.main, loadedGameObject.transform, 3f);
		}

		//Loads or downloads a model from the given URL
		private void LoadOrDownload(string url) {	
			//Gets current file name, extension, local path and local filename
			var fileName = FileUtils.GetFilenameWithoutExtension(url);
			var fileExtension = FileUtils.GetFileExtension(url);
			var localFilePath = string.Format("{0}/{1}", Application.persistentDataPath, fileName);
			var localFilename = string.Format("{0}/{1}{2}", localFilePath, fileName, fileExtension);

			//Checks if local path exists, which indicates the file has been downloaded
			if (Directory.Exists(localFilePath))
			{
				//Loads local file
				LoadFile(fileExtension, localFilename);
			}
			else
			{
				//If local path doesn't exists, download the file and create the local folder
				StartCoroutine(DownloadFile(url, fileExtension, localFilePath, localFilename));
			}
		}
		
        //Searches inside a path and returns the first path of an asset loadable by TriLib
        private string GetReadableAssetPath(string path)
        {
            var supportedExtensions = AssetLoaderBase.GetSupportedFileExtensions();
            foreach (var file in Directory.GetFiles(path))
            {
                var fileExtension = FileUtils.GetFileExtension(file);
                if (supportedExtensions.Contains(fileExtension))
                {
                    return file;
                }
            }
            foreach (var directory in Directory.GetDirectories(path))
            {
                var assetPath = GetReadableAssetPath(directory);
                if (assetPath != null)
                {
                    return assetPath;
                }
            }
            return null;
        }

        //Loads an existing local file
        private void LoadFile(string fileExtension, string localFilename)
        {
			GameObject loadedGameObject;
			
            //Creates a new AssetLoader instance
            using (var assetLoader = new AssetLoader())
            {
                //Checks if the url is a ZIP file
                if (fileExtension == ".zip")
                {
#if TRILIB_USE_ZIP
                    var localFilePath = FileUtils.GetFileDirectory(localFilename);

                    //Gets the first asset loadable by TriLib on the folder
                    var assetPath = GetReadableAssetPath(localFilePath);
                    if (assetPath == null)
                    {
                        Debug.LogError("No TriLib readable file could be found on the given directory");
                        return;
                    }

                    //Loads the found asset
                    loadedGameObject = assetLoader.LoadFromFile(assetPath);
#else
                    throw new Exception("Please enable TriLib ZIP loading");
#endif
                }
                else
                {
                    //If the url is not a ZIP file, loads the file inside the folder
                    loadedGameObject = assetLoader.LoadFromFile(localFilename);
                }
				
				OnFileDownloaded(loadedGameObject);
            }
        }

        //Downloads a file to a local path or extract all ZIP file contents to the local path in case of ZIP files, then loads the file
        private IEnumerator DownloadFile(string url, string fileExtension, string localFilePath, string localFilename)
        {
            _fileDownloader = new WWW(url);
            yield return _fileDownloader;
            if (fileExtension == ".zip")
            {
#if TRILIB_USE_ZIP
                using (var memoryStream = new MemoryStream(_fileDownloader.bytes))
                {
                    UnzipFromStream(memoryStream, localFilePath);
                }
#else
                throw new Exception("Please enable TriLib ZIP loading");
#endif
            }
            Directory.CreateDirectory(localFilePath);
            File.WriteAllBytes(localFilename, _fileDownloader.bytes);
            LoadFile(fileExtension, localFilename);
            _fileDownloader = null;
        }

#if TRILIB_USE_ZIP
        //Helper function to extract all ZIP file contents to a local folder
        private void UnzipFromStream(Stream zipStream, string outFolder)
        {
#if !UNITY_EDITOR && (NET_4_6 || NETFX_CORE)
            var zipFile = new ZipArchive(zipStream, ZipArchiveMode.Read);
            foreach (ZipArchiveEntry zipEntry in zipFile.Entries)
            {
                var zipFileStream = zipEntry.Open();
#else
            var zipFile = new ZipFile(zipStream);
            foreach (ZipEntry zipEntry in zipFile)
            {
                if (!zipEntry.IsFile)
                {
                    continue;
                }
                var zipFileStream = zipFile.GetInputStream(zipEntry);
#endif
                var entryFileName = zipEntry.Name;
                var buffer = new byte[4096];
                var fullZipToPath = Path.Combine(outFolder, entryFileName);
                var directoryName = Path.GetDirectoryName(fullZipToPath);
                if (directoryName.Length > 0)
                {
                    Directory.CreateDirectory(directoryName);
                }
                var fileName = Path.GetFileName(fullZipToPath);
                if (fileName.Length == 0)
                {
                    continue;
                }
                using (var streamWriter = File.Create(fullZipToPath))
                {
#if !UNITY_EDITOR && (NET_4_6 || NETFX_CORE)
                    zipFileStream.CopyTo(streamWriter);
#else
                    ICSharpCode.SharpZipLib.Core.StreamUtils.Copy(zipFileStream, streamWriter, buffer);
#endif
                }
            }
        }
#endif
    }
}