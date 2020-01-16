using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tests
{
    public class TestUtilities
    {
        private static TestUtilities instance = null;
        private static readonly object padlock = new object();

        TestUtilities()
        {
        }

        public static TestUtilities Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new TestUtilities();
                    }
                    return instance;
                }
            }
        }

        public string PrintMessageOnWrongDefaultValue(string testObject, string property)
        {
            return testObject + " has not the default value for the property '" + property + "'";
        }

        public string PrintMessageOnMissingComponents(string testObject, string component)
        {
            return component + " Component is not to the " + testObject + " Gameobject attached.";
        }
    }
}