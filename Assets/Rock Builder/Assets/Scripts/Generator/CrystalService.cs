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
            //Undo.RegisterCreatedObjectUndo(diamondGenerator, "Created diamond");
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
            crystal.mesh = CrystalMeshGenerator.Instance.CreateMesh(crystal);
            //MeshCollider meshCollider = crystal.gameObject.AddComponent<MeshCollider>();
            //meshCollider = CrystalMeshGenerator.Instance.CreateCollider(crystal.mesh);
            //crystal.GetComponent<MeshCollider>().convex = true;
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
    }
}
