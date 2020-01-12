using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using RockBuilder;
using UnityEngine;

namespace Tests
{
    public class SphereRockServiceTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CreateEmptySphereRock_Test()
        {
            //ARRANGE
            SphereRock sphereRock = SphereRockService.Instance.CreateEmptySphereRock();

            //ACT


            //ASSERT
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshFilter>());
            Assert.IsNotNull(sphereRock.gameObject.GetComponent<MeshRenderer>());

        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator SphereRockMeshGeneratorWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
