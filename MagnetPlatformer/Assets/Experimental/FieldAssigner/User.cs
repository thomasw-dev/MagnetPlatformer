using UnityEngine;

namespace Experimental.FieldAssigner
{
    public class User : MonoBehaviour
    {
        MyClass sourceObject = new MyClass();
        AnotherClass targetObject = new AnotherClass();

        private void Start()
        {
            // Assign fields from sourceObject to targetObject
            FieldAssigner.AssignFields(sourceObject, targetObject);

            // Print the values of targetObject's fields
            Debug.Log("field1: " + targetObject.field1);
            Debug.Log("field2: " + targetObject.field2);
            Debug.Log("field3: " + targetObject.field3);
            // ... print other fields

            // Output should match the values of sourceObject's fields
        }
    }
}