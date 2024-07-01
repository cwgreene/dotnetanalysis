using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FunctionCallExtractor
{
    class Program
    {
        static void Maing(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: FunctionCallExtractor <file-path> <function-name>");
                return;
            }

            string filePath = args[0];
            string functionName = args[1];

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return;
            }

            string code = File.ReadAllText(filePath);
            var syntaxTree = CSharpSyntaxTree.ParseText(code);
            var root = syntaxTree.GetRoot();

            var methodDeclarations = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

            var method = methodDeclarations.FirstOrDefault(m => m.Identifier.Text == functionName);
            if (method == null)
            {
                Console.WriteLine($"Function '{functionName}' not found in file.");
                return;
            }
            var references = AppDomain.CurrentDomain.GetAssemblies()
                                     .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
                                     .Select(a => MetadataReference.CreateFromFile(a.Location))
                                     .Cast<MetadataReference>();

            var compilation = CSharpCompilation.Create("Analysis")
                                                .AddReferences(references)
                                                .AddSyntaxTrees(syntaxTree);
            var semanticModel = compilation.GetSemanticModel(syntaxTree);

            var invocationExpressions = method.DescendantNodes().OfType<InvocationExpressionSyntax>();

            foreach (var invocation in invocationExpressions)
            {

                var symbolInfo = semanticModel.GetSymbolInfo(invocation);
                if (symbolInfo.Symbol is IMethodSymbol methodSymbol)
                {
                    Console.WriteLine(methodSymbol.ToDisplayString());
                }
            }
        }
    }
}
