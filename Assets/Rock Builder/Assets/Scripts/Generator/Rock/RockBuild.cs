using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RockBuilder
{
    public class RockBuild : MonoBehaviour
    {
        //[HideInInspector]
        public List<List<Vector3>> vertexPositions;
        public List<Vector3> rockBuildPoints;
        private int verticalIterations;
        private int verticesPerIteration;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddNewBuildPoint()
        {
            AddNewBuildPoint(Vector3.up);
        }

        public void AddNewBuildPoint(Vector3 position)
        {
            rockBuildPoints.Add(position);
        }

    }
}
