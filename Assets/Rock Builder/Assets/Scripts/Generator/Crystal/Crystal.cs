﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Crystal : MonoBehaviour
    {
        [HideInInspector]
        public float radius;
        [HideInInspector]
        public float height;
        [HideInInspector]
        public float heightPeak;
        [HideInInspector]
        public int edges;
        [HideInInspector]
        public bool smooth;
        [HideInInspector]
        public int lodCount;
        [HideInInspector]
        public Transform[] childrens;
        [HideInInspector]
        public List<Vector3> vertexPositions;

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

        public void RemoveCrystalClass()
        {
            DestroyImmediate(this.GetComponent<Crystal>());
        }

        public void RemoveMeshCollider()
        {
            if (this.GetComponent<MeshCollider>())
            {
                DestroyImmediate(this.GetComponent<MeshCollider>());
            }
        }
    }
}
