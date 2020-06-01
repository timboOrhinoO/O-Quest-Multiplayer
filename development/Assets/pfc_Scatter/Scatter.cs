using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
// using Unity.Physics.Authoring;
using UnityEditor;
using UnityEditor.EventSystems;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;

#endif

namespace pfc.Biolum
{
    [ExecuteInEditMode]
    public class Scatter : MonoBehaviour
    {
        [Header("Prefab")]
        public Transform prefab;
        public new HideFlags hideFlags = HideFlags.DontSave;

        [HideInInspector] public List<Transform> instances = new List<Transform>();
        [Header("Scatter Settings")]
        public LayerMask scatterLayer = 1; //Layer Default
        public int seed = 834723478;
        public Texture2D weightMask;
        public int scatterCount = 10;
        public float radius = 1f;
        public AnimationCurve radiusCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
        public float maxDistance = 10f;

        [Header("Rotation")]
        public Vector3 rotationJitter;
        public float rotationJitterMultiplier = 1f;
        [Range(0, 1)] public float normalAlignment = 0.5f;
        [Range(0, 1)] public float upAlignment = 0.0f;

        [Header("Scale")]
        public bool uniformScale = false;
        public bool ignoreParentScale = false;
        public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
        public AnimationCurve thicknessDistributionCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));

        [Header("Misc")]
        public float pushIn = 0;

        [Header("Preview")]
        public int previewScatterCount = 100;

        public int maxScatterCount => 25000;
        [HideInInspector] [SerializeField]
        private bool hasScatteredObjects = false;
        
        private void Start()
        {
            ScatterNow();
        }

        [ContextMenu("Scatter Now")]
        public void ScatterNow()
        {
            this.InternalScatterNow(false);
        }

        public void InternalScatterNow(bool isPreview = false)
        {
            Clear();
            scatterCount = Mathf.Min(scatterCount, maxScatterCount);

            //if (!isPreview || previewScatterCount >= scatterCount)
                Random.InitState(seed);

            var t = this.transform;
            var count = isPreview ? Mathf.Min(scatterCount, previewScatterCount) : scatterCount;
            var matrix = Matrix4x4.TRS(t.position, t.rotation, Vector3.one);
            for (var i = 0; i < count; i++)
            {
                var rand = Random.value;

                var c = Random.insideUnitCircle;

                if (weightMask)
                {
                    var foundValidPoint = false;
                    const int MAX_TRIES = 10;
                    for (var k = 0; k < MAX_TRIES; k++)
                    {
                        if (foundValidPoint) break;
                        c = Random.insideUnitCircle;
                        var pixel = weightMask.GetPixel(Mathf.FloorToInt((c.x * .5f + .5f) * weightMask.width),
                            Mathf.FloorToInt((c.y * .5f + .5f) * weightMask.height));
                        if (Random.value < pixel.r)
                            foundValidPoint = true;
                    }

                    if (!foundValidPoint) continue;
                }

                var mult = radius * (1 + radiusCurve.Evaluate(c.magnitude));
                var p = new Vector4(c.x * mult, 0, c.y * mult, 1);

                var ray = new Ray(matrix * p, matrix * -Vector3.up);
                if (!Physics.Raycast(ray, out var hit, maxDistance, scatterLayer)) continue;

#if UNITY_EDITOR
                var instance = PrefabUtility.InstantiatePrefab(prefab, t) as Transform;
                if (instance == null) continue;
                if (PrefabStageUtility.GetCurrentPrefabStage() != null)
                {
                    instance.gameObject.hideFlags = HideFlags.DontSave;
                }
                else
                {
                    instance.gameObject.hideFlags = hideFlags;
                    if (!Application.isPlaying)
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                    hasScatteredObjects = true;
                }
#else
                var instance = Instantiate(prefab, t) as Transform;
#endif
                instance.gameObject.layer = this.gameObject.layer;
#if UNITY_EDITOR
                GameObjectUtility.SetStaticEditorFlags(instance.gameObject, GameObjectUtility.GetStaticEditorFlags(this.gameObject));
#endif

                //instance.gameObject.isStatic = this.gameObject.isStatic;
                var jitter = Quaternion.Euler(
                    Random.Range(-rotationJitter.x, rotationJitter.x) * rotationJitterMultiplier, 
                    Random.Range(-rotationJitter.y, rotationJitter.y) * rotationJitterMultiplier, 
                    Random.Range(-rotationJitter.z, rotationJitter.z) * rotationJitterMultiplier);
                
                instance.position = hit.point - hit.normal * pushIn;
                var parentScale = (ignoreParentScale ? 1 / instance.parent.lossyScale.x : 1) * prefab.lossyScale.x;
                var thickness = thicknessDistributionCurve.Evaluate(rand) * parentScale;
                var sc = scaleCurve.Evaluate(c.magnitude) * parentScale;
                instance.localScale = uniformScale ? Vector3.one * sc : new Vector3(thickness, sc, thickness);
                instance.rotation = Quaternion.Slerp(Quaternion.Slerp(t.rotation, Quaternion.identity, upAlignment), Quaternion.LookRotation(Vector3.Slerp(hit.normal, Vector3.up, upAlignment), t.up) * Quaternion.Euler(90, 0, 0), normalAlignment);
                instance.rotation *= prefab.rotation;
                instance.rotation *= jitter;
                instances.Add(instance);
            }
        }

        [ContextMenu("Clear")]
        private void Clear()
        {
            if (instances == null) instances = new List<Transform>();
            for (var i = 0; i < instances.Count; i++)
            {
                if (instances[i])
                    DestroyImmediate(instances[i].gameObject);
            }

            instances.Clear();

            // remaining childs?
            var cc = transform.childCount;
            for (var i = cc - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            var t = transform;
            var localToWorldMatrix = Matrix4x4.TRS(t.position, t.rotation, Vector3.one);
            Gizmos.matrix = localToWorldMatrix;
            var size = radius * (1 + radiusCurve.Evaluate(1f));
//            Gizmos.DrawWireSphere(Vector3.zero, size);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(Vector3.zero, -Vector3.up * size);
            Handles.matrix = localToWorldMatrix;
            Handles.color = Color.blue;
            Handles.DrawWireDisc(Vector3.zero, Vector3.down, size);
            Handles.color = new Color(.2f, .2f, .8f, 1f);
            Handles.DrawWireDisc(maxDistance * Vector3.down, Vector3.down, size);
        }
#endif
    }
}