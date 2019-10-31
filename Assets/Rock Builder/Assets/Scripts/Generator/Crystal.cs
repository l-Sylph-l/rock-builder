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
        public int lodCount { get; set; } = 0;
        public Transform[] childrens { get; set; }
        public List<Vector3> vertexPositions { get; set; }

        public Mesh mesh
        {
            get
            {
                //Some other code
                return GetComponent<MeshFilter>().sharedMesh;
            }
            set
            {
                //Some other code
                GetComponent<MeshFilter>().sharedMesh = value;
            }
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

        public void RemoveLOD()
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

        private void OnDrawGizmosSelected()
        {
            vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(this);
            CrystalPreview.Instance.DrawGizmo(this);
        }
    }
}
