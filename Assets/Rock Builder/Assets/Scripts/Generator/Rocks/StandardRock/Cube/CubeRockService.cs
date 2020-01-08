using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RockBuilder
{
    public class CubeRockService
    {
        private static CubeRockService instance = null;
        private static readonly object padlock = new object();

        CubeRockService()
        {
        }

        public static CubeRockService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CubeRockService();
                    }
                    return instance;
                }
            }
        }

        public CubeRock CreateEmptyCubeRock()
        {
            CubeRock standardRock = new GameObject().AddComponent(typeof(CubeRock)) as CubeRock;
            standardRock.smoothFlag = false;
            standardRock.lodCount = 0;
            standardRock.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            standardRock = CubeRockMeshGenerator.Instance.CreateVertexPositions(standardRock);
            standardRock.transform.position = CalculateCubeRockSpawnPosition();
            SceneView.lastActiveSceneView.camera.transform.LookAt(standardRock.transform);
            FocusCubeRock(standardRock);
            return standardRock;
        }

        public CubeRock CreateEmptyCubeRock(string name)
        {
            CubeRock standardRock = CreateEmptyCubeRock();
            standardRock.name = name;
            return standardRock;
        }

        public CubeRock CreateCubeRock(CubeRock standardRock, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(standardRock.transform);
            FocusCubeRock(standardRock);

            standardRock.mesh = CubeRockMeshGenerator.Instance.CreateRockMesh(standardRock);
            standardRock.GetComponent<MeshRenderer>().material = material;
            //CreateLods(standardRock);
            CreateMeshCollider(standardRock);
            return standardRock;
        }

        public CubeRock GetCubeRockFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<CubeRock>();
            }

            return null;
        }

        private void FocusCubeRock(CubeRock standardRock)
        {
            Selection.activeGameObject = standardRock.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateCubeRockSpawnPosition()
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

        // public void CreateLods(CubeRock standardRock)
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
        //             CubeRock childCubeRock;

        //             if (i != 0)
        //             {
        //                 childCubeRock = new GameObject().AddComponent(typeof(CubeRock)) as CubeRock;
        //                 childCubeRock.edges = standardRock.edges / (i + 1);
        //                 childCubeRock.radius = standardRock.radius;
        //                 childCubeRock.height = standardRock.height;
        //                 childCubeRock.heightPeak = standardRock.heightPeak;
        //                 childCubeRock.smoothFlag = standardRock.smoothFlag;
        //                 childCubeRock.vertexPositions = CubeRockMeshGenerator.Instance.CreateVertexPositions(childCubeRock);
        //                 childCubeRock.mesh = CubeRockMeshGenerator.Instance.CreateMesh(childCubeRock);
        //                 childCubeRock.name = standardRock.name + "_LOD_0" + i;
        //                 childCubeRock.transform.parent = standardRock.transform;
        //                 childCubeRock.transform.localPosition = Vector3.zero;
        //                 childCubeRock.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        //                 childCubeRock.GetComponent<MeshRenderer>().material = standardRock.GetComponent<MeshRenderer>().sharedMaterial;
        //                 renderers = new Renderer[1];
        //                 renderers[0] = childCubeRock.GetComponent<Renderer>();
        //                 childrens[i - 1] = childCubeRock.transform;
        //                 childCubeRock.RemoveCubeRockClass();
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
