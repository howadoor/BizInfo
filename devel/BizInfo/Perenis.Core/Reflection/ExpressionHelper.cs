using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Perenis.Core.Reflection
{
    /// <summary>
    /// Expression evaluation helper class
    /// </summary>
    public static class ExpressionHelper
    {
        /// <summary>
        /// Caches evaluator objects for diferent expressions and owners types
        /// </summary>
        private static readonly Dictionary<string, object> evaluatorCache = new Dictionary<string, object>();

        /// <summary>
        /// evaluates c# expression where "this" is replaced by given type. 
        /// ONLY PUBLIC PROPERTIES CAN BE USED!
        /// </summary>
        /// <param name="csCode"> expression code where "this" repersents expressionOwner object</param>
        /// <param name="expressionOwner">objects on which the expression is executed</param>
        /// <returns></returns>
        public static object Eval(string csCode, object expressionOwner)
        {
            object o = GetExpressionEvaluator(expressionOwner, csCode);

            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("EvalCode");

            object s = mi.Invoke(o, new[] {expressionOwner});
            return s;
        }

        /// <summary>
        /// evaluates expression in form similar to DebuggerDisplay attribute
        /// format: "some text {object's property or function} some other text {some other property or function} ..."
        /// ONLY PUBLIC PROPERTIES CAN BE USED!
        /// </summary>
        /// <param name="simpleExpression"> expression code</param>
        /// <param name="expressionOwner">objects on which the expression is executed</param>
        /// <returns>value of expression</returns>
        public static object EvalInSimplyfiedFormat(string simpleExpression, object expressionOwner)
        {
            string csCode = "\"" + simpleExpression + "\"";
            csCode = csCode.Replace("{", "\" + this.");
            csCode = csCode.Replace("}", " + \"");
            return Eval(csCode, expressionOwner);
        }

        /// <summary>
        /// Creates the expression evaluator or return existing instance from the cache
        /// </summary>
        /// <param name="expressionOwner"></param>
        /// <param name="csCode"></param>
        /// <returns></returns>
        private static object GetExpressionEvaluator(object expressionOwner, string csCode)
        {
            string key = expressionOwner.GetType() + "|" + csCode;
            if (!evaluatorCache.ContainsKey(key))
            {
                evaluatorCache[key] = CreateExpressionEvaluator(expressionOwner, csCode);
            }
            return evaluatorCache[key];
        }

        /// <summary>
        /// Creates new expression evaluator
        /// </summary>
        /// <param name="expressionOwner">expression owner</param>
        /// <param name="csCode">expression text</param>
        /// <returns></returns>
        private static object CreateExpressionEvaluator(object expressionOwner, string csCode)
        {
            var c = new CSharpCodeProvider();
            ICodeCompiler icc = c.CreateCompiler();
            var cp = new CompilerParameters();

            Type ownerType = expressionOwner.GetType();

            //add reference to owners dll and referenced assamblies
            var assemblyPaths = new List<string>();
            RegisterAssemblyWithReferences(assemblyPaths, ownerType.Assembly);
            cp.ReferencedAssemblies.AddRange(assemblyPaths.ToArray());

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            var sb = new StringBuilder("");
            sb.Append("using System;\n");

            sb.Append("namespace CSCodeEvaler{\n");
            sb.Append("public class CSCodeEvaler{\n");
            sb.Append("public object EvalCode(" + ownerType + " instance){\n");
            sb.Append("return " + csCode.Replace("this", "instance") + ";\n");
            sb.Append("}\n");
            sb.Append("}\n");
            sb.Append("}\n");

            CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());

            Assembly a = cr.CompiledAssembly;
            return a.CreateInstance("CSCodeEvaler.CSCodeEvaler");
        }

        private static void RegisterAssemblyWithReferences(List<string> assemblies, Assembly assembly)
        {
            var uri = new UriBuilder(assembly.CodeBase);
            string assemblyPath = Uri.UnescapeDataString(uri.Path);
            if (!assemblies.Contains(assemblyPath))
            {
                assemblies.Add(assemblyPath);
                foreach (AssemblyName refAssemblyName in assembly.GetReferencedAssemblies())
                {
                    Assembly refAssembly = Assembly.Load(refAssemblyName);
                    RegisterAssemblyWithReferences(assemblies, refAssembly);
                }
            }
        }
    }
}