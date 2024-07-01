using System;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.X509Certificates;
using ICSharpCode.Decompiler;
using ICSharpCode.Decompiler.CSharp;
using ICSharpCode.Decompiler.Disassembler;
using ICSharpCode.Decompiler.IL;
using ICSharpCode.Decompiler.Metadata;
using ICSharpCode.Decompiler.TypeSystem;

namespace DecompilerExample
{
    class Decompiler
    {
        static void Main(string[] args) {
            if (args[0] == "decompile") {
                string dllPath = args[1];
                DecompileFunction(dllPath);
            } else if (args[0] == "il") {
                DisassembleFunction(args[1], args[2]);
            }
        }

        static FullTypeName FindFunction(string Name, MetadataReader mdr) {
            // Find the function in the assembl
            foreach(var handle in mdr.TypeDefinitions ) {
                var tdef = mdr.GetTypeDefinition(handle);
                if (mdr.GetString(tdef.Name) == Name) {
                    return tdef.GetFullTypeName(mdr);
                }
            } 
            throw new Exception($"Failed to find {Name}");
        }

        static void DisassembleFunction(string dllPath, string functionName) {
            PEFile pefile = new PEFile(dllPath);
            var reader = pefile.Reader;
            var mdr = pefile.Metadata;
            FullTypeName func = FindFunction(functionName, mdr);

            var module = pefile.
            ILReader ilReader = new ILReader(module);
        }

        static void DecompileFunction(string dllPath)
        {
            // Path to the DLL

            // Create a PEFile object representing the DLL
            //PEFile module = new PEFile(dllPath);
            using var fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            PEReader reader = new PEReader(fs);

            // Create a decompiler object
            DecompilerSettings settings = new DecompilerSettings();
            CSharpDecompiler decompiler = new CSharpDecompiler(dllPath, settings);

            MetadataReader metadataReader = reader.GetMetadataReader();

            // Find the function in the assembl
            foreach(var handle in metadataReader.TypeDefinitions ) {
                var tdef = metadataReader.GetTypeDefinition(handle);
                FullTypeName ftn = tdef.GetFullTypeName(metadataReader);
                Console.WriteLine(ftn);
                if (metadataReader.GetString(tdef.Namespace) == "DecompilerExample") {
                    Console.WriteLine(decompiler.DecompileTypeAsString(ftn));
                }
            }

            /*if (type != null)
            {
                IMethod method = type.Methods.FirstOrDefault(m => m.Name == "MethodName");

                if (method != null)
                {
                    // Decompile the function
                    string decompiledCode = decompiler.DecompileAsString(method.MetadataToken);
                    Console.WriteLine(decompiledCode);
                }
                else
                {
                    Console.WriteLine($"Method {functionName} not found.");
                }
            }
            else
            {
                Console.WriteLine($"Type {functionName} not found.");
            }*/
        }
    }
}