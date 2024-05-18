using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

public class BoomUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateRes());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator UpdateRes()
    {
        var initHandler = Addressables.InitializeAsync(false);
        
        yield return initHandler;

        var checkForCatlogUpdateHandler =  Addressables.CheckForCatalogUpdates(false);
        yield return checkForCatlogUpdateHandler;
        
        var catlog = checkForCatlogUpdateHandler.Result;
        var upHandler = Addressables.UpdateCatalogs(catlog, false);
        yield return upHandler;
        
        Debug.Log("Luo完成 catalog update");
        yield return null;


        var re = Addressables.ResourceLocators;
        
        List<object> update = new List<object>();
        
        foreach (var resource in re)
        {
            foreach (var key in resource.Keys)
            {
                Debug.Log("Luo" + key);
                
            }
            
            update.AddRange(resource.Keys);
        }

        var handler = Addressables.LoadResourceLocationsAsync(update, Addressables.MergeMode.Union);

        yield return handler;

        foreach (var location in handler.Result)
        {
            if (location.Data is AssetBundleRequestOptions)
            {
                var aro = (AssetBundleRequestOptions) location.Data;
                Debug.Log( "Luo   " + location.PrimaryKey + aro.ComputeSize(location, Addressables.ResourceManager));
            }
        }
        
        var loadHandler = Addressables.DownloadDependenciesAsync(update, Addressables.MergeMode.None, false);
        yield return loadHandler;
            
        Debug.Log("Luo" +"完成更新");

    }
}
