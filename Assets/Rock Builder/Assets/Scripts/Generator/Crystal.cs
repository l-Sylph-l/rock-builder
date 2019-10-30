using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Crystal : MonoBehaviour
    {
        public float radius { get; set; } = 1f;
        public float height { get; set; } = 1f;
        public float heightPeak { get; set; } = 1f;
        public int edges { get; set; } = 3;
        public bool smooth { get; set; } = false;
        public Transform[] childrens { get; set; }

        public void SetMesh(Mesh mesh)
        {
            GetComponent<MeshFilter>().sharedMesh = mesh;
        }

        public void SetMaterial(Material material)
        {
            GetComponent<MeshRenderer>().material = material;
        }

        public void SetLODGroup(LODGroup lodGroup)
        {
            if (GetComponent<LODGroup>() == null)
            {
                LODGroup newLodGroup = gameObject.AddComponent(typeof(LODGroup)) as LODGroup;
                newLodGroup = lodGroup;
            }
            else
            {
                LODGroup newLodGroup = GetComponent<LODGroup>();
                newLodGroup = lodGroup;
            }
        }

        private void RemoveLOD()
        {
            if (GetComponent<LODGroup>() != null)
            {
                DestroyImmediate(GetComponent<LODGroup>());
            }

            foreach (Transform child in childrens)
            {
                if (child != null)
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
}
