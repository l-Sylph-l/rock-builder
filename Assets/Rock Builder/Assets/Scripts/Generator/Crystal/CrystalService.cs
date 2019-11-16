using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RockBuilder
{
    public class CrystalService
    {
        private static CrystalService instance = null;
        private static readonly object padlock = new object();

        CrystalService()
        {
        }

        public static CrystalService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new CrystalService();
                    }
                    return instance;
                }
            }
        }

        public Crystal CreateEmptyCrystal()
        {
            Crystal crystal = new GameObject().AddComponent(typeof(Crystal)) as Crystal;
            crystal.radius = 1f;
            crystal.height = 1f;
            crystal.heightPeak = 1f;
            crystal.edges = 3;
            crystal.smoothFlag = false;
            crystal.lodCount = 0;
            crystal.colliderFlag = true;
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            crystal.vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(crystal);
            crystal.transform.position = CalculateCrystalSpawnPosition(crystal);
            SceneView.lastActiveSceneView.camera.transform.LookAt(crystal.transform);
            FocusCrystal(crystal);
            return crystal;
        }

        public Crystal CreateEmptyCrystal(string name)
        {
            Crystal crystal = CreateEmptyCrystal();
            crystal.name = name;
            return crystal;
        }

        public Crystal CreateCrystal(Crystal crystal)
        {
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
            SceneView.lastActiveSceneView.camera.transform.LookAt(crystal.transform);
            FocusCrystal(crystal);
            crystal.vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(crystal);
            crystal.mesh = CrystalMeshGenerator.Instance.CreateMesh(crystal);
            CreateLods(crystal);
            CreateMeshCollider(crystal);
            return crystal;
        }

        public Crystal GetCrystalFromSelection()
        {
            if (Selection.activeGameObject)
            {
                return Selection.activeGameObject.GetComponent<Crystal>();
            }

            return null;
        }

        private void FocusCrystal(Crystal crystal)
        {
            Selection.activeGameObject = crystal.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
        }

        private Vector3 CalculateCrystalSpawnPosition(Crystal crystal)
        {
            Transform cameraTransform = SceneView.lastActiveSceneView.camera.transform;
            return (cameraTransform.forward * (crystal.radius * 3f + crystal.height * 2f)) + cameraTransform.position;
        }

        private void CreateMeshCollider(Crystal crystal)
        {
            crystal.RemoveMeshCollider();
            if (crystal.colliderFlag)
            {
                MeshCollider meshCollider = crystal.gameObject.AddComponent<MeshCollider>();
                meshCollider.sharedMesh = crystal.mesh;
                meshCollider.convex = true;
            }
        } 

        public void CreateLods(Crystal crystal)
        {
            if (crystal.childrens != null)
            {
                crystal.RemoveLOD();
            }

            int lodCounter = crystal.lodCount;

            if (lodCounter != 0 && 3 <= crystal.edges / crystal.lodCount)
            {
                // Programmatically create a LOD group and add LOD levels.
                // Create a GUI that allows for forcing a specific LOD level.
                lodCounter += 1;
                LODGroup group = crystal.gameObject.AddComponent<LODGroup>();
                Transform[] childrens = new Transform[lodCounter - 1];

                // Add 4 LOD levels
                LOD[] lods = new LOD[lodCounter];
                for (int i = 0; i < lodCounter; i++)
                {

                    Renderer[] renderers;
                    Crystal childCrystal;

                    if (i != 0)
                    {
                        childCrystal = new GameObject().AddComponent(typeof(Crystal)) as Crystal;
                        childCrystal.edges = crystal.edges / (i + 1);
                        childCrystal.height = crystal.height;
                        childCrystal.heightPeak = crystal.heightPeak;
                        childCrystal.smoothFlag = crystal.smoothFlag;
                        childCrystal.vertexPositions = CrystalMeshGenerator.Instance.CreateVertexPositions(childCrystal);
                        childCrystal.mesh = CrystalMeshGenerator.Instance.CreateMesh(childCrystal);
                        childCrystal.name = crystal.name + "_LOD_0" + i;
                        childCrystal.transform.parent = crystal.transform;
                        childCrystal.transform.localPosition = new Vector3(0f, 0f, 0f);
                        childCrystal.GetComponent<MeshRenderer>().sharedMaterial = crystal.GetComponent<MeshRenderer>().sharedMaterial;
                        renderers = new Renderer[1];
                        renderers[0] = childCrystal.GetComponent<Renderer>();
                        childrens[i - 1] = childCrystal.transform;
                        childCrystal.RemoveCrystalClass();
                    }
                    else
                    {
                        renderers = new Renderer[1];
                        renderers[0] = crystal.GetComponent<Renderer>();
                    }

                    if (i != lodCounter - 1)
                    {
                        lods[i] = new LOD((1f / lodCounter) * (lodCounter - i - 1) / 2, renderers);
                    }
                    else
                    {
                        lods[i] = new LOD(0f, renderers);
                    }

                }
                crystal.childrens = childrens;
                group.SetLODs(lods);
                group.RecalculateBounds();
            }
        }
    }
}
