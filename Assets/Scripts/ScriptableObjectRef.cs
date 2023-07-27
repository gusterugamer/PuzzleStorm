using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using System.IO;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using GusteruStudio.Editor;
#endif

//Purpose of this script is to reference scriptable objects in scene whenever there is a change in
//asset database ( or a new scriptable object is created in the scriptable object folder or any of the
//subfolders) 

[ExecuteInEditMode]
public class ScriptableObjectRef : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> _so = new List<ScriptableObject>();

#if UNITY_EDITOR
    private void Awake()
    {
        AssetsDataBaseRefreshListener.onDatabaseRefreshed += FetchSOs;
    }

    private void OnDestroy()
    {
        AssetsDataBaseRefreshListener.onDatabaseRefreshed -= FetchSOs;
    }

    [Button]
    private void FetchSOs()
    {
        _so.Clear();
        string[] pathsToSOs = Directory.GetFiles(Application.dataPath + "/ScriptableObjects", "*.asset", SearchOption.AllDirectories);

        foreach (string path in pathsToSOs)
        {
            string correctedPath = "Assets" + path.Replace(Application.dataPath, "").Replace("\\", "/");
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(correctedPath);
            _so.Add(so);
        }
    }
#endif
}
