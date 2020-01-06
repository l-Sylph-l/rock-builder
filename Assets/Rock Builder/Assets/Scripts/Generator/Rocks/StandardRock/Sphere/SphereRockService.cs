using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RockBuilder
{
    public class SphereRockService
    {
        private static SphereRockService instance = null;
        private static readonly object padlock = new object();

        SphereRockService()
        {
        }

        public static SphereRockService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SphereRockService();
                    }
                    return instance;
                }
            }
        }

        public CubeRock CreateEmptyStandardRock()
        {
            CubeRock standardRock = new GameObject().AddComponent(typeof(CubeRock)) as CubeRock;
            standardRock.smoothFlag = false;
            standardRock.lodCount = 0;
            standardRock.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            standardRock.transform.position = CalculateStandardRockSpawnPosition();
            SceneView.lastActiveSceneView.camera.transform.LookAt(standardRock.transform);
            FocusStandardRock(standardRock);
            return standardRock;
        }

        public CubeRock CreateEmptyStandardRock(string name)
        {
            CubeRock standardRock = CreateEmptyStandardRock();
            standardRock.name = name;
            return standardRock;
        }

        public CubeRock CreateStandardRock(CubeRock standardRock, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(standardRock.transform);
            FocusStandardRock(standardRock);

            standardRock.mesh = CubeRockMeshGenerator.Instance.CreateRockMesh(standardRock);
            standardRock.GetComponent<MeshRenderer>().material = material;
            //CreateLods(standardRock);
            CreateMeshCollider(standardRock);
            return standardRock;
        }

        public CubeRock GetStandardRockFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<CubeRock>();
            }

            return null;
        }

        private void FocusStandardRock(CubeRock standardRock)
        {
            Selection.activeGameObject = standardRock.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateStandardRockSpawnPosition()
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (3f * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(CubeRock standardRock)
        {
            standardRock.RemoveMeshCollider();
            if (standardRock.colliderFlag)
            {
                MeshCollider meshCollider = standardRock.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = standardRock.mesh;
                meshCollider.convex = true;
            }
        } 

        // public void CreateLods(StandardRock standardRock)
        // {
        //     if (standardRock.childrens != null)
        //     {
        //         standardRock.RemoveLOD();
        //     }

        //     int lodCounter = standardRock.lodCount;

        //     if (lodCounter != 0 && 3 <= standardRock.edges / standardRock.lodCount)
        //     {
        //         // Programmatically create a LOD group and add LOD levels.
        //         // Create a GUI that allows for forcing a specific LOD level.
        //         lodCounter += 1;
        //         LODGroup group = standardRock.gameObject.AddComponent<LODGroup>();
        //         Transform[] childrens = new Transform[lodCounter - 1];

        //         // Add 4 LOD levels
        //         LOD[] lods = new LOD[lodCounter];
        //         for (int i = 0; i < lodCounter; i++)
        //         {

        //             Renderer[] renderers;
        //             StandardRock childStandardRock;

        //             if (i != 0)
        //             {
        //                 childStandardRock = new GameObject().AddComponent(typeof(StandardRock)) as StandardRock;
        //                 childStandardRock.edges = standardRock.edges / (i + 1);
        //                 childStandardRock.radius = standardRock.radius;
        //                 childStandardRock.height = standardRock.height;
        //                 childStandardRock.heightPeak = standardRock.heightPeak;
        //                 childStandardRock.smoothFlag = standardRock.smoothFlag;
        //                 childStandardRock.vertexPositions = StandardRockMeshGenerator.Instance.CreateVertexPositions(childStandardRock);
        //                 childStandardRock.mesh = StandardRockMeshGenerator.Instance.CreateMesh(childStandardRock);
        //                 childStandardRock.name = standardRock.name + "_LOD_0" + i;
        //                 childStandardRock.transform.parent = standardRock.transform;
        //                 childStandardRock.transform.localPosition = Vector3.zero;
        //                 childStandardRock.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        //                 childStandardRock.GetComponent<MeshRenderer>().material = standardRock.GetComponent<MeshRenderer>().sharedMaterial;
        //                 renderers = new Renderer[1];
        //                 renderers[0] = childStandardRock.GetComponent<Renderer>();
        //                 childrens[i - 1] = childStandardRock.transform;
        //                 childStandardRock.RemoveStandardRockClass();
        //             }
        //             else
        //             {
        //                 renderers = new Renderer[1];
        //                 renderers[0] = standardRock.GetComponent<Renderer>();
        //             }

        //             if (i != lodCounter - 1)
        //             {
        //                 lods[i] = new LOD((1f / lodCounter) * (lodCounter - i - 1) / 2, renderers);
        //             }
        //             else
        //             {
        //                 lods[i] = new LOD(0f, renderers);
        //             }

        //         }
        //         standardRock.childrens = childrens;
        //         group.SetLODs(lods);
        //         group.RecalculateBounds();
        //     }
        // }
    }
}
