using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Remoting;
using UnknownObjects.Model;

namespace RunThis {

    internal class Program {

        // Path to UnknownObjects.dll
        // Using an absolute path like this is usually not recommended. Relative paths are a must, but could not be bothered...
        public static string UnknownObjectsDllPath = @"C:\Users\Tobias Jacobsen\Dropbox\Datamatiker\4. semester\C#\Project\rapport\Review\ReflectionTests\UnknownObjects\bin\Debug\UnknownObjects.dll";

        private static void Main() {

            Console.WriteLine("When type is KNOWN -->");
            KnownType();

            Console.WriteLine("When type is UNKNOWN -->");
            UnknownType();
        }

        /// <summary>
        /// Look in Solution Explorer under "References" and observe that the project "UnknownObjects" has been added.
        /// In this example type "Person" is known, meaning that it is possible to include a reference to the assembly as we have done.
        /// It is therefore possible to refer to the type and its members, like we do in line 28 or line 36-40 
        /// </summary>
        private static void KnownType() {

            // uncomment below to info about type
            //TypeInfo t = typeof(Person).GetTypeInfo(); //System.Reflection.TypeInfo
            //WriteTypeInfo(t);

            // assemblyName, typeName (remember to include including full namespac!)

            // option A: use constructor with input parameters
            ObjectHandle handle = Activator.CreateInstance("UnknownObjects", "UnknownObjects.Model.Person", true, 0, null, new object[] {"Kurt", "Hansen", 30}, null, null);

            // option B: use empty constructor 
            // ObjectHandle handle = Activator.CreateInstance("UnknownObjects", "UnknownObjects.Model.Person");

            Person p = (Person)handle.Unwrap();

            // when using option B, set values afterwards
            /*
            p.FirstName = "Kurt";
            p.LastName = "Hansen";
            p.Age = 30;
            */

            Console.WriteLine("Hello: " + p.Hello("Hej"));
            Console.WriteLine("ToString: " + p);
            PauseAndSpace();
        }

        /// <summary>
        /// This represents a more typical example that showcases the strengths of Reflection when type is unknown. 
        /// In this example type "Person" is unknown, meaning that we do not/cannot include a reference to the assembly 
        /// or otherwise refer to the type's members (attributes, methods etc.) beforehand.
        /// Notice that we never refer to the Person-type explicitly, but instead load the assembly at runtime.
        /// </summary>
        private static void UnknownType() {

            Assembly assem = Assembly.LoadFrom(UnknownObjectsDllPath);

            // assem.CreateInstance() uses Activator.CreateInstance behind the scenes...

            // option A: uses constructor with input parameters
            var p = assem.CreateInstance("UnknownObjects.Model.Person", true, 0, null, new object[] { "Kurt", "Hansen", 30 }, null, null); // uses Activator.CreateInstance behind the scenes...

            // option B: uses empty constructor 
            //var p = assem.CreateInstance("UnknownObjects.Model.Person"); 

            Type t = p.GetType(); //notice that System.Type is used and not System.Reflection.TypeInfo


            // when using option B, set values afterwards
            /*
            PropertyInfo propFirstName = t.GetProperty("FirstName");
            if (propFirstName != null) {
                propFirstName.SetValue(p, "Kurt");
            }

            PropertyInfo propLastName = t.GetProperty("LastName");
            if (propLastName != null) {
                propLastName.SetValue(p, "Hansen");
            }

            PropertyInfo propAge = t.GetProperty("Age");
            if (propAge != null) {
                propAge.SetValue(p, 30);
            }

            */

            MethodInfo fn = t.GetMethod("Hello");
            var fullName = fn.Invoke(p, new object[] { "Hej" });
            if (fullName != null) {
                Console.WriteLine("Hello: " + fullName);
            }
            
            MethodInfo method = t.GetMethod("ToString");
            var toString = method.Invoke(p, null);
            if (toString != null) {
                Console.WriteLine("ToString: " + toString);
            }
            PauseAndSpace();
        }

        private static void PauseAndSpace() {
            Console.ReadKey();
            Console.WriteLine();
        }

        /// <summary>
        /// A helper method that showcases some examples of information that can be extracted from TypeInfo from System.Reflection.TypeInfo
        /// Similar functionality exists on Type, when using System.Type
        /// </summary>
        /// <param name="t"></param>
        private static void WriteTypeInfo(TypeInfo t) {

            Console.WriteLine("Type's full name: " + t.FullName);

            PauseAndSpace();

            IEnumerable<PropertyInfo> pList = t.DeclaredProperties;
            IEnumerable<MethodInfo> mList = t.DeclaredMethods;
            IEnumerable<FieldInfo> fList = t.DeclaredFields;
            IEnumerable<ConstructorInfo> cList = t.DeclaredConstructors;

            Console.WriteLine("--Fields--");
            foreach (var field in fList) {
                Console.WriteLine(field);
                Console.WriteLine(field.Name);
            }

            PauseAndSpace();

            Console.WriteLine("--Properties--");
            foreach (var property in pList) {
                Console.WriteLine(property);
            }

            PauseAndSpace();

            Console.WriteLine("--Methods--");
            foreach (var method in mList) {
                Console.WriteLine(method);
            }

            PauseAndSpace();

            Console.WriteLine("--Constructors--");
            foreach (var constructor in cList) {
                Console.WriteLine(constructor);
            }

            PauseAndSpace();
        }
    }
}
