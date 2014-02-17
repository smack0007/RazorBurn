using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace RazorBurn
{
    public class RazorCompiler
    {
        RazorCodeLanguage language;
        RazorEngineHost host;
        int compileCount;
        
        public ISet<string> NamespaceImports
        {
            get { return this.host.NamespaceImports; }
        }

        public AssemblyReferenceCollection ReferencedAssemblies
        {
            get;
            private set;
        }

        public RazorCompiler()
        {
            this.language = new CSharpRazorCodeLanguage();
            
            this.host = new RazorEngineHost(this.language)
            {
                DefaultClassName = "Template",
                DefaultNamespace = "RazorBurn.CompiledTemplates"
            };

            this.host.NamespaceImports.Add("System");

            this.ReferencedAssemblies = new AssemblyReferenceCollection();
            this.ReferencedAssemblies.Add("mscorlib");
            this.ReferencedAssemblies.Add("System");
            this.ReferencedAssemblies.Add("System.Core");
            this.ReferencedAssemblies.Add("System.Web"); // TODO: Make this reference not be automatic.
            this.ReferencedAssemblies.Add("Microsoft.CSharp");
        }

        public string GenerateSource<T>(string template)
            where T : RazorTemplate
        {
            Type baseTemplateType = typeof(T);

            this.host.DefaultBaseClass = baseTemplateType.FullName;

            var engine = new RazorTemplateEngine(this.host);
            GeneratorResults generatorResults;
            using (TextReader reader = new StringReader(template))
            {
                generatorResults = engine.GenerateCode(reader);
            }

            if (!generatorResults.Success)
            {
                string[] errors = generatorResults.ParserErrors.Select(x => x.Message).ToArray();
                throw new RazorCompilationException("There were errors while generating code for the template. See the Errors array.", template, errors);
            }

            CodeDomProvider provider = (CodeDomProvider)Activator.CreateInstance(this.language.CodeDomProviderType);

            StringWriter sw = new StringWriter();
            provider.GenerateCodeFromCompileUnit(generatorResults.GeneratedCode, sw, new CodeGeneratorOptions());

            return sw.ToString();
        }

        public T Compile<T>(string template)
            where T : RazorTemplate
        {
            string source = this.GenerateSource<T>(template);

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
            string assemblyName = "RazorBurn.CompiledTemplates.Template" + this.compileCount.ToString();

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
                return (T)assembly.CreateInstance("RazorBurn.CompiledTemplates.Template");
            }
        }
    }
}
