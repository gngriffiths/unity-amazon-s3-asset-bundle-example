using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadAssetBundleImage : MonoBehaviour
{
    [SerializeField] RawImage rawImage;

    AssetBundleLoader assetBundleLoader;    

    private void Start()
    {
        assetBundleLoader = this.GetComponent<AssetBundleLoader>();        
    }

    private void OnEnable()
    {
        // Listen for event from AssetBundleLoader
        AssetBundleLoader.OnAssetBundleLoaded += LoadImage;
    }

    private void OnDisable()
    {
        // Always stop listening if the GO is disabled
        AssetBundleLoader.OnAssetBundleLoaded -= LoadImage;
    }
    
    // Triggered when the asset bundle is loaded
    private void LoadImage()
    {
        BundleListNames(assetBundleLoader.loadedAssetBundle);
    }

    private void BundleListNames(AssetBundle _loadedAssetBundle)
    {
        // Make an array of the image files
        //string[] imageAssetNames = _loadedAssetBundle.GetAllAssetNames();
        string[] imageAssetNames = _loadedAssetBundle.GetAllAssetNames();

        // Remove path and extension from each image in the array
        for (int i = 0; i < imageAssetNames.Length; i++)
        {
            imageAssetNames[i] = Path.GetFileNameWithoutExtension(imageAssetNames[i]);
        }
        
        // Load the first image in the Assetbundle
        StartCoroutine(LoadRawImage(imageAssetNames[0], rawImage));
    }

    // Load image from AssetBundle then add as a texture on a prefab
    IEnumerator LoadRawImage(string imageName, RawImage image)
    {
        // Load the image from the loaded AssetBundle   
        var assetLoadRequest = assetBundleLoader.loadedAssetBundle.LoadAssetAsync<Texture2D>(imageName);  
        yield return assetLoadRequest;

        // Check that the AssetBundle object exists
        if (assetLoadRequest == null)
        {
            Debug.Log("Failed to load asset");
            yield break;
        }

        // Load the image on to the raw image GO
        rawImage.texture = assetLoadRequest.asset as Texture2D;       
    }  
    
}
