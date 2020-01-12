using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class CubeRock : MonoBehaviour
    {
        [HideInInspector]
        public float height;
        [HideInInspector]
        public float width;
        [HideInInspector]
        public float depth;
         [HideInInspector]
        public float noise;
        [HideInInspector]
        public float bezelSize;
        [HideInInspector]
        public int divider;
        [HideInInspector]
        public int lodCount;
        [HideInInspector]
        public bool smoothFlag;
        [HideInInspector]
        public bool colliderFlag;
        [HideInInspector]
        public List<Vector3> vertexPositions;
        [HideInInspector]
        public List<Vector3> bottomPlaneVertices;
        [HideInInspector]
        public List<Vector3> upperPlaneVertices;
         [HideInInspector]
        public List<Vector3> bottomBezelsVertices;
        [HideInInspector]
        public List<Vector3> upperBezelsVertices;
        [HideInInspector]
        public List<Vector3> bottomVerticalBezelsVertices;
        [HideInInspector]
        public List<Vector3> upperVerticalBezelsVertices;
        [HideInInspector]
        public Transform[] childrens;
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
            //
            if (bottomPlaneVertices != null && upperPlaneVertices != null && bottomBezelsVertices != null && upperBezelsVertices != null)
            {
                // update vertex positions
                CubeRock cubeRockCopy = CubeRockMeshGenerator.Instance.CreateVertexPositions(this);
                CubeRockPreview.Instance.DrawGizmo(cubeRockCopy);
            }
        }


        public void RemoveCubeRockClass()
        {
            DestroyImmediate(this.GetComponent<CubeRock>());
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
