using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Rockbuilder;

namespace Rockbuilder.Tests
{
    public class SphereRockMeshGeneratorTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void SphereRockMeshGeneratorSimplePasses()
        {
            
            // Use the Assert class to test conditions
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
