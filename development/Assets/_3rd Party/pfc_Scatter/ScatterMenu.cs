using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace pfc.Biolum
{
#if UNITY_EDITOR
    public class ScatterMenu : EditorWindow
    {
        // Start is called before the first frame update
        [MenuItem("pfc/Scatter Tools/Scatter Now")]
        static void ScatterNowEditor()
        {

        }

        [MenuItem("pfc/Scatter Tools/Update Scatteritems")]
        static void UpdateScatterItems()
        {
            
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
#endif
}
