using Roslyn.Compilers.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Razor;

namespace RazorBurn
{
    public class RazorCompiler
    {
        RazorCodeLanguage language;
        RazorEngineHost host;
        IInternalRazorCompiler internalCompiler;
        
        public ISet<string> NamespaceImports
        {
            get { return this.host.NamespaceImports; }
        }

        public AssemblyReferenceCollection ReferencedAssemblies
        {
            get { return this.internalCompiler.ReferencedAssemblies; }
        }

        public RazorCompiler(RazorLanguage language)
        {
            if (language == RazorLanguage.VisualBasic)
            {
                this.language = new VBRazorCodeLanguage();
                this.internalCompiler = new RazorCompilerVisualBasic();
            }
            else
            {
                this.language = new CSharpRazorCodeLanguage();
                this.internalCompiler = new RazorCompilerCSharp();
            }

            this.host = new RazorEngineHost(this.language)
            {
                DefaultClassName = "Template",
                DefaultNamespace = "__CompiledRazorTemplates"
            };

            this.host.NamespaceImports.Add("System");

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
            return this.internalCompiler.Compile<T>(source);
        }
    }
}
