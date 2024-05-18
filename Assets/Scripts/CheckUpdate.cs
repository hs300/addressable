using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class CheckUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
        StartCoroutine(CheckUpdateForCatlog());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckUpdateForCatlog()
    {
        var initHandler = Addressables.InitializeAsync(false);
        
        yield return initHandler;

        var re = initHandler.Result;

        var list = re.Keys;
        
        var total = Addressables.GetDownloadSizeAsync(list);

        yield return total;
        
        Debug.Log(total.Result);

        if (total.Result > 0)
        {
            var loadHandler = Addressables.DownloadDependenciesAsync(list, Addressables.MergeMode.Union, false);
            yield return loadHandler;
            
            Debug.Log("完成更新");
        }
        else
        {
            Debug.Log("检测完成 没有更新");
        }

        var location = Addressables.LoadResourceLocationsAsync(list, Addressables.MergeMode.Union);
        yield return location;
        
        Debug.Log(location.Result.Count);
        
        
        
        Debug.Log("ResolveInternalId " + Addressables.ResolveInternalId("Cube"));
        
        //var id = Addressables.ResourceManager.TransformInternalId(initHandler.Result)
       
        
        ///*
        var checkForCatlogUpdateHandler =  Addressables.CheckForCatalogUpdates(false);

        yield return checkForCatlogUpdateHandler;

        if (checkForCatlogUpdateHandler.Status == AsyncOperationStatus.Succeeded)
        {
           

            var catlog = checkForCatlogUpdateHandler.Result;
            if (catlog != null && catlog.Count > 0)
            {
                Debug.Log("检测完成 等待更新");
               
                var upHandler = Addressables.UpdateCatalogs(catlog, false);
                //yield return upHandler;
               
            
               
                
                Debug.Log("完成更新");
                
            }
            else
            {
                Debug.Log("检测完成 没有更新");
            }
        }
        else
        {
            Debug.Log("检测失败");
        }

        //*/
    }
}
