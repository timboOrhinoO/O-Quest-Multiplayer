using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace pfc.Biolum
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Scatter))]
    public class ScatterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();

            if (EditorGUI.EndChangeCheck())
                foreach(Scatter sc in targets)
                    DelayedScatter(sc);
            else foreach (Scatter scatter in targets)
                if (scatter && scatter.transform.hasChanged)
                    TransformScatter(scatter);

            if (targets.Length == 1 && targets[0])
                EditorGUILayout.LabelField("Max Scatter Count", (targets[0] as Scatter).maxScatterCount.ToString());

            if(targets.Length > 1)
            {
                if (GUILayout.Button("Refresh All [" + targets.Length + "]")) { 
                    foreach(Scatter sc in targets) { 
                        if(sc) sc.InternalScatterNow(false);
                    }
                }
            }
            else
                if (GUILayout.Button("Refresh") && targets.Length == 1 && targets[0])
                    (targets[0] as Scatter).InternalScatterNow(false);
        }

        private int propertyChanged = 0;

        private async void DelayedScatter(Scatter scatter)
        {
            if (!scatter) return;

            var previewCount = propertyChanged += 1;
            scatter.InternalScatterNow(true);
            await Task.Delay(200);
            if (previewCount == propertyChanged)
            {
                scatter.InternalScatterNow(false);
                propertyChanged %= int.MaxValue;
            }
        }

        private async void TransformScatter(Scatter scatter)
        {
            if (!scatter) return;

            scatter.transform.hasChanged = false;
            scatter.InternalScatterNow(true);
            await Task.Delay(200);
            if (scatter.transform.hasChanged == false)
            {
                scatter.InternalScatterNow(false);
            }
        }
    }
}