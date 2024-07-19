using System;
using System.Reflection;

namespace Experimental.FieldAssigner
{
    public class FieldAssigner
    {
        public static void AssignFields(object source, object target)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            FieldInfo[] sourceFields = sourceType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            FieldInfo[] targetFields = targetType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo sourceField in sourceFields)
            {
                foreach (FieldInfo targetField in targetFields)
                {
                    if (sourceField.Name == targetField.Name && sourceField.FieldType == targetField.FieldType)
                    {
                        targetField.SetValue(target, sourceField.GetValue(source));
                        break;
                    }
                }
            }
        }
    }
}