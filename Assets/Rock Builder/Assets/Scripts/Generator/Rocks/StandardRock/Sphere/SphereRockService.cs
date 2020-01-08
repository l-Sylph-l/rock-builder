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

        public SphereRock CreateEmptySphereRock()
        {
            SphereRock standardRock = new GameObject().AddComponent(typeof(SphereRock)) as SphereRock;
            standardRock.smoothFlag = false;
            standardRock.lodCount = 0;
            standardRock.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            standardRock.transform.position = CalculateSphereRockSpawnPosition();
            SceneView.lastActiveSceneView.camera.transform.LookAt(standardRock.transform);
            FocusSphereRock(standardRock);
            return standardRock;
        }

        public SphereRock CreateEmptySphereRock(string name)
        {
            SphereRock standardRock = CreateEmptySphereRock();
            standardRock.name = name;
            return standardRock;
        }

        public SphereRock CreateSphereRock(SphereRock standardRock, Material material)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(standardRock.transform);
            FocusSphereRock(standardRock);

            standardRock.mesh = SphereRockMeshGenerator.Instance.CreateRockMesh(standardRock);
            standardRock.GetComponent<MeshRenderer>().material = material;
            //CreateLods(standardRock);
            CreateMeshCollider(standardRock);
            return standardRock;
        }

        public SphereRock GetSphereRockFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<SphereRock>();
            }

            return null;
        }

        private void FocusSphereRock(SphereRock standardRock)
        {
            Selection.activeGameObject = standardRock.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateSphereRockSpawnPosition()
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (3f * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(SphereRock standardRock)
        {
            standardRock.RemoveMeshCollider();
            if (standardRock.colliderFlag)
            {
                MeshCollider meshCollider = standardRock.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = standardRock.mesh;
                meshCollider.convex = true;
            }
        } 

        // public void CreateLods(SphereRock standardRock)
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
        //             SphereRock childSphereRock;

        //             if (i != 0)
        //             {
        //                 childSphereRock = new GameObject().AddComponent(typeof(SphereRock)) as SphereRock;
        //                 childSphereRock.edges = standardRock.edges / (i + 1);
        //                 childSphereRock.radius = standardRock.radius;
        //                 childSphereRock.height = standardRock.height;
        //                 childSphereRock.heightPeak = standardRock.heightPeak;
        //                 childSphereRock.smoothFlag = standardRock.smoothFlag;
        //                 childSphereRock.vertexPositions = SphereRockMeshGenerator.Instance.CreateVertexPositions(childSphereRock);
        //                 childSphereRock.mesh = SphereRockMeshGenerator.Instance.CreateMesh(childSphereRock);
        //                 childSphereRock.name = standardRock.name + "_LOD_0" + i;
        //                 childSphereRock.transform.parent = standardRock.transform;
        //                 childSphereRock.transform.localPosition = Vector3.zero;
        //                 childSphereRock.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        //                 childSphereRock.GetComponent<MeshRenderer>().material = standardRock.GetComponent<MeshRenderer>().sharedMaterial;
        //                 renderers = new Renderer[1];
        //                 renderers[0] = childSphereRock.GetComponent<Renderer>();
        //                 childrens[i - 1] = childSphereRock.transform;
        //                 childSphereRock.RemoveSphereRockClass();
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
