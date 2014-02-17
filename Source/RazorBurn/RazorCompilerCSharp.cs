using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RazorBurn
{
    internal class RazorCompilerCSharp : IInternalRazorCompiler
    {
        int compileCount;

        public AssemblyReferenceCollection ReferencedAssemblies
        {
            get;
            private set;
        }

        public RazorCompilerCSharp()
        {
            this.ReferencedAssemblies = new AssemblyReferenceCollection();
        }

        public T Compile<T>(string source) where T : RazorTemplate
        {
            Type baseTemplateType = typeof(T);

            SyntaxTree syntaxTree = SyntaxTree.ParseText(source);

            var executeMethod = syntaxTree.GetRoot().DescendantNodes()
                .OfType<MethodDeclarationSyntax>()
                .Where(x => x.Identifier.ToString() == "Execute")
                .Single();

            var newExecuteMethod = executeMethod.WithModifiers(
                Syntax.TokenList(
                    Syntax.Token(SyntaxKind.ProtectedKeyword, Syntax.TriviaList(Syntax.Whitespace(" "))),
                    Syntax.Token(SyntaxKind.OverrideKeyword, Syntax.TriviaList(Syntax.Whitespace(" ")))
                ));

            var newSyntaxTree = SyntaxTree.Create(syntaxTree.GetRoot().ReplaceNode(executeMethod, newExecuteMethod));

            this.compileCount++;            
            string assemblyName = "RazorBurn_" + this.compileCount.ToString();

            var compilation = Compilation.Create(assemblyName, new CompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            compilation = compilation
                .AddReferences(new MetadataFileReference(Assembly.GetExecutingAssembly().Location))
                .AddReferences(new MetadataFileReference(baseTemplateType.Assembly.Location));

            foreach (var assemblyReference in this.ReferencedAssemblies)
            {
                if (!assemblyReference.IsFile)
                {
                    compilation = compilation.AddReferences(MetadataReference.CreateAssemblyReference(assemblyReference.Name));
                }
                else
                {
                    compilation = compilation.AddReferences(new MetadataFileReference(assemblyReference.Name));
                }
            }
                
            compilation = compilation.AddSyntaxTrees(newSyntaxTree);

            using (MemoryStream stream = new MemoryStream())
            {
                var result = compilation.Emit(stream);

                if (!result.Success)
                {
                    throw new RazorCompilationException("There were errors while compiling the template. See the Errors array.", source, result.Diagnostics.Select(x => x.ToString()).ToArray());
                }

                Assembly assembly = Assembly.Load(stream.ToArray());
                return (T)assembly.CreateInstance("__CompiledRazorTemplates.Template");
            }
        }
    }
}
