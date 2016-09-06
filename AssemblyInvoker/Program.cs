using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AssemblyInvoker
{
    public class Program
    {
        private static string _assemblyDirectory;

        public static void Main(string[] args)
        {
            if (args == null || args.Length < 3)
            {
                Console.WriteLine("Arguments: [assembly] [class] [method] [args...]");
                Environment.Exit(42);
            }

            string assemblyPath = args[0];
            string typeName = args[1];
            string methodName = args[2];

            if (!File.Exists(assemblyPath))
            {
                throw new Exception($"Assembly not found: {assemblyPath}");
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            _assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            currentDomain.AssemblyResolve += LoadFromSameFolder;

            var assembly = Assembly.LoadFile(assemblyPath);

            var type = assembly.GetType(typeName);

            if (type == null)
            {
                throw new Exception($"Type not found: {typeName}");
            }

            var methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static);

            if (methodInfo == null)
            {
                throw new Exception($"Method not found: {methodName}");
            }

            object[] arguments = args.Skip(3).Cast<object>().ToArray();
            methodInfo.Invoke(null, arguments);
        }

        static Assembly LoadFromSameFolder(object sender, ResolveEventArgs args)
        {
            string assemblyPath = Path.Combine(_assemblyDirectory, $"{new AssemblyName(args.Name).Name}.dll");

            if (!File.Exists(assemblyPath))
            {
                return null;
            }

            return Assembly.LoadFrom(assemblyPath);
        }
    }
}