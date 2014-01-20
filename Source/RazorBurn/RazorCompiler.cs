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
        
        public ISet<string> NamespaceImports
        {
            get { return this.host.NamespaceImports; }
        }

        public ISet<string> ReferencedAssemblies
        {
            get;
            private set;
        }

        public RazorCompiler(RazorLanguage language)
        {
            if (language == RazorLanguage.VisualBasic)
            {
                this.language = new VBRazorCodeLanguage();
            }
            else
            {
                this.language = new CSharpRazorCodeLanguage();
            }

            this.host = new RazorEngineHost(this.language)
            {
                DefaultClassName = "Template",
                DefaultNamespace = "__CompiledRazorTemplates"
            };

            this.host.NamespaceImports.Add("System");

            this.ReferencedAssemblies = new HashSet<string>();
            this.ReferencedAssemblies.Add("mscorlib.dll");
            this.ReferencedAssemblies.Add("System.dll");
            this.ReferencedAssemblies.Add("System.Core.dll");
            this.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
        }
                
        public T Compile<T>(string template)
            where T : IRazorTemplate
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
                throw new RazorCompilationException("There were errors while generating code for the template. See the Errors array.", errors);
            }

            CodeDomProvider provider = (CodeDomProvider)Activator.CreateInstance(this.language.CodeDomProviderType);

            var compilerParameters = new CompilerParameters(new[] { typeof(RazorCompiler).Assembly.Location });

            foreach (string assembly in this.ReferencedAssemblies)
                compilerParameters.ReferencedAssemblies.Add(assembly);
                        
            if (!this.ReferencedAssemblies.Contains(baseTemplateType.Assembly.Location))
                compilerParameters.ReferencedAssemblies.Add(baseTemplateType.Assembly.Location);

            var compilerResults = provider.CompileAssemblyFromDom(compilerParameters, generatorResults.GeneratedCode);

            if (compilerResults.Errors.HasErrors)
            {
                string[] errors = compilerResults.Errors.Cast<CompilerError>().Select(x => x.ErrorText).ToArray();
                throw new RazorCompilationException("There were errors while compiling the template. See the Errors array.", errors);
            }

            var templateType = compilerResults.CompiledAssembly.GetType("__CompiledRazorTemplates.Template");

            if (templateType == null)
            {
                throw new MissingMemberException("Unable to find compiled template in assembly.");
            }

            return (T)Activator.CreateInstance(templateType);
        }
    }
}
