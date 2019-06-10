using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetBundleLoader : MonoBehaviour
{
    [SerializeField] Button ButtonLoadAssetBundle = null;

    // Set up an event for when an Assetbundle is laoded
    public delegate void AssetBundleLoaded();
    public static event AssetBundleLoaded OnAssetBundleLoaded;

    public AssetBundle loadedAssetBundle { get; private set; }

    AWSSDK.Examples.S3Example s3example;
    
    void Start()
    {
        s3example = GameObject.Find("S3").GetComponent<AWSSDK.Examples.S3Example>();
        // Create a Listener event for the button
        ButtonLoadAssetBundle.onClick.AddListener(() => { StartCoroutine(Loadbundle()); });
    }
    
    // LoadBUndle is triggered from the LoadBundle button
    private IEnumerator Loadbundle() 
    {
        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(s3example.AssetBundlePath);
        yield return bundleLoadRequest;

        loadedAssetBundle = bundleLoadRequest.assetBundle;
        // If there are listeners to the event, trigger the event
        OnAssetBundleLoaded?.Invoke();

        Debug.Log("Assetbundle loaded");
    }
}
