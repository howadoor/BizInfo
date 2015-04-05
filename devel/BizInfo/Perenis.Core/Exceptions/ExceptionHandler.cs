using System;
using System.Configuration;
using Perenis.Core.Component;
using Perenis.Core.Exceptions.Configuration;
using Perenis.Core.Pattern;
using Perenis.Core.Reflection;

#if NUNIT
using NUnit.Framework;
#endif

namespace Perenis.Core.Exceptions
{
    /// <summary>
    /// Provides configurable policy-based exception handling capabilities.
    /// </summary>
    public class ExceptionHandler
    {
        /// <summary>
        /// The name of the default exception policy.
        /// </summary>
        public const string DefaultPolicyName = "Default";

        private static ExceptionHandler _default;

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ExceptionHandler(ExceptionPolicyConfigurationSection exceptionPolicyConfiguration)
        {
            ExceptionPolicyConfiguration = exceptionPolicyConfiguration;
        }

        /// <summary>
        /// The default instance of an exception handler which uses the 
        /// <see cref="ExceptionPolicyConfigurationSection.Default"/> configuration section.
        /// </summary>
        public static ExceptionHandler Default
        {
            get
            {
                if (_default == null)
                {
                    _default = new ExceptionHandler(ExceptionPolicyConfigurationSection.Default);
                }
                return _default;
            }
        }

        /// <summary>
        /// The last-chance exception handler for faulted exception handlers.
        /// </summary>
        public IExceptionHandler LastChanceHandler { get; set; }

        /// <summary>
        /// The configuration of exception policies used by this handler.
        /// </summary>
        public ExceptionPolicyConfigurationSection ExceptionPolicyConfiguration { get; set; }

        /// <summary>
        /// Handles the given exception using the current exception handling configuration.
        /// </summary>
        /// <param name="ex">The exception to be handled.</param>
        /// <param name="policyObject">The exception policy object.</param>
        /// <returns><c>true</c> if the exception shall be re-thrown; otherwise <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="policyObject"/> is a null reference.</exception>
        public bool Handle(Exception ex, object policyObject)
        {
            return Handle(ex, GetPolicyName(policyObject), policyObject);
        }

        /// <summary>
        /// Handles the given exception using the current exception handling configuration.
        /// </summary>
        /// <param name="ex">The exception to be handled.</param>
        /// <param name="policyName">The name of the exception policy to be applied.</param>
        /// <param name="policyObject">The exception policy object.</param>
        /// <returns><c>true</c> if the exception has been handled; <c>false</c> if it shall be re-thrown.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ex"/> is a null reference.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="policyName"/> is a null reference.</exception>
        /// <exception cref="ArgumentException"><paramref name="policyName"/> is an empty string.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="policyObject"/> is a null reference.</exception>
        public bool Handle(Exception ex, string policyName, object policyObject)
        {
            if (ex == null) throw new ArgumentNullException("ex");
            if (policyName == null) throw new ArgumentNullException("policyName");
            if ((policyName != null && String.IsNullOrEmpty(policyName))) throw new ArgumentException("policyName");
            if (policyObject == null) throw new ArgumentNullException("policyObject");

            // handle the exception using the provided exception handling configuration
            Exception failureException;
            bool? result = HandleInternal(ex, policyName, policyObject, out failureException);
            if (result != null) return result.Value;

            // execute the last chance handler
            if (LastChanceHandler == null || LastChanceHandler.Handle(failureException, policyName, policyObject)) throw failureException;

            return true;
        }

        /// <summary>
        /// Retrieves the name of an exception policy specified by the given <paramref name="policyObject"/>.
        /// </summary>
        /// <param name="policyObject">The exception policy object.</param>
        /// <returns>The name of the exception policy.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="policyObject"/> is a null reference.</exception>
        /// <remarks>
        /// Policy name-resolution is based on the <see cref="IStructuredNamingResolver{T}"/> mechanism.
        /// </remarks>
        public static string GetPolicyName(object policyObject)
        {
            if (policyObject == null) throw new ArgumentNullException("policyObject");
            string result = policyNameResolver.GetStructuredName(policyObject);
            if (result == null) throw new InvalidOperationException(String.Format("Unsupported exception policy object of type “{0}” supplied.", policyObject.GetType().FullName));
            return result;
        }

        /// <summary>
        /// Assigns the given exception a unique handling instance ID, having not been assigned previously.
        /// </summary>
        /// <param name="ex"></param>
        public static void SetHandlingInstanceId(Exception ex)
        {
            SetHandlingInstanceId(ex, Guid.NewGuid());
        }

        /// <summary>
        /// Assigns the given exception a specific handling instance ID.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="handlingInstanceId"></param>
        public static void SetHandlingInstanceId(Exception ex, Guid handlingInstanceId)
        {
            if (!ex.Data.Contains("HandlingInstanceId"))
            {
                ex.Data["HandlingInstanceId"] = handlingInstanceId;
            }
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Exception policy name resolver.
        /// </summary>
        /// <remarks>
        /// <see cref="StringStructuredNamingResolver"/> is used to allow exception policy definitions in configuration files.
        /// </remarks>
        private static readonly StructuredNamingResolver<object> policyNameResolver = new StructuredNamingResolver<object>(Singleton<EnumStructuredNamingResolver>.Instance, Singleton<ExceptionPolicyStructuredNamingResolver>.Instance,
                                                                                                                           Singleton<StringStructuredNamingResolver>.Instance);

        /// <summary>
        /// Handles the given exception using the current exception handling configuration.
        /// </summary>
        /// <param name="ex">The exception to be handled.</param>
        /// <param name="policyName">The name of the exception policy to be applied.</param>
        /// <param name="policyObject">The exception policy object.</param>
        /// <param name="failureException">A possible exception thrown during the handling of the 
        /// given exception.</param>
        /// <returns><c>true</c> if the exception has been handled; <c>false</c> if it shall be re-thrown.</returns>
        private bool? HandleInternal(Exception ex, string policyName, object policyObject, out Exception failureException)
        {
            SetHandlingInstanceId(ex);
            failureException = null;
            try
            {
                string orgPolicyName = policyName;
                Type exType = ex.GetType();

                // locate exception policy and rule
                ExceptionPolicyElement policy = null;
                ExceptionPolicyRuleElement policyRule = null;
                do
                {
                    // try loading the policy
                    policy = ExceptionPolicyConfiguration.ExceptionPolicies.GetByName(policyName);
                    if (policy == null && policyName == DefaultPolicyName) break;

                    // locate matching policy rule
                    if (policy != null)
                    {
                        // process inline rules
                        policyRule = GetMatchingRule(policy, exType);

                        // process template rules
                        if (policyRule == null && !String.IsNullOrEmpty(policy.Template))
                        {
                            ExceptionPolicyTemplateElement policyTemplate = ExceptionPolicyConfiguration.ExceptionPolicyTemplates.GetByName(policy.Template);
                            if (policyTemplate == null) throw new ConfigurationErrorsException(String.Format("Configuration not found for exception policy template '{0}' while processing exception policy '{1}'", policy.Template, orgPolicyName));
                            policyRule = GetMatchingRule(policyTemplate, exType);
                        }

                        // did any rule match?
                        if (policyRule != null || policyName == DefaultPolicyName) break;
                    }

                    // try using the parent exception policy, or resort to the default exception policy
                    int lastDot = policyName.LastIndexOf('.');
                    policyName = lastDot < 0 ? DefaultPolicyName : policyName.Substring(0, lastDot);
                } while (true);

                // it is an error if no policy and/or rule was loaded
                if (policy == null) throw new ConfigurationErrorsException(String.Format("Configuration not found for exception policy '{0}'", orgPolicyName));
                if (policyRule == null) throw new ConfigurationErrorsException(String.Format("Matching rule not found for exception policy '{0}'", orgPolicyName));

                // locate the handler
                ExceptionHandlerElement handler = ExceptionPolicyConfiguration.ExceptionHandlers.GetByName(policyRule.HandlerName);
                if (handler == null) throw new ConfigurationErrorsException(String.Format("Configuration not not found for exception handler '{0}' while processing exception policy '{1}'", policyRule.HandlerName, orgPolicyName));

                // execute all rules of the handler
                bool handled = false;
                foreach (ExceptionHandlerRuleElement handlerRule in handler)
                {
                    handled |= handlerRule.Instance.Handle(ex, policyName, policyObject);
                    if (handled && !handler.All) break;
                }

                // return whether to rethrow the exception
                switch (policyRule.Rethrow)
                {
                    case ExceptionPolicyRethrow.Unhandled:
                        return !handled;
                    case ExceptionPolicyRethrow.Handled:
                        return handled;
                    case ExceptionPolicyRethrow.Always:
                        return true;
                    case ExceptionPolicyRethrow.Never:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            catch (Exception e)
            {
                failureException = e;
            }
            return null;
        }

        /// <summary>
        /// Finds a matching exception policy rule.
        /// </summary>
        /// <param name="policy">The exception policy being applied.</param>
        /// <param name="exType">The type of the exception being handled.</param>
        /// <returns>Matching rule or a null reference.</returns>
        private static ExceptionPolicyRuleElement GetMatchingRule(ExceptionPolicyTemplateElement policy, Type exType)
        {
            int i = 0;
            while (i < policy.Count)
            {
                var policyRule = (ExceptionPolicyRuleElement) policy[i];
                if (Types.IsDescendantOrEqual(exType, policyRule.Type)) return policyRule;
                i++;
            }
            return null;
        }

        #endregion
    }

    #region ------ Unit tests of the ExceptionHandler class ---------------------------------------

#if NUNIT

    /// <summary>
    /// Unit tests of the <see cref="ExceptionHandler"/> class.
    /// </summary>
    [TestFixture]
    public class ExceptionHandler_Tests
    {
        private ExceptionHandler Ex;

        private class TestHandlerA : IExceptionHandler
        {
            public bool Handle(Exception ex, string policyName, object policyObject)
            {
                ex.Data["handler"] = this.GetType().Name;
                return false;
            }
        }

        private class TestHandlerB : IExceptionHandler
        {
            public bool Handle(Exception ex, string policyName, object policyObject)
            {
                ex.Data["handler"] = this.GetType().Name;
                return true;
            }
        }

        private class TestHandlerC : IExceptionHandler
        {
            public bool Handle(Exception ex, string policyName, object policyObject)
            {
                ex.Data["handler"] = this.GetType().Name;
                return true;
            }
        }

        private class TestHandlerD : IExceptionHandler
        {
            public bool Handle(Exception ex, string policyName, object policyObject)
            {
                ex.Data["handler"] = this.GetType().Name;
                return true;
            }
        }

        /// <summary>
        /// Initializes the test exception policy configuration.
        /// </summary>
        [TestFixtureSetUp]
        public void TestSetup()
        {
            ExceptionHandlerElement handlerA = new ExceptionHandlerElement{Name = "handlerA"};
            handlerA.Add(new ExceptionHandlerRuleElement{TypeName = typeof(TestHandlerA).AssemblyQualifiedName});

            ExceptionHandlerElement handlerB = new ExceptionHandlerElement { Name = "handlerB" };
            handlerB.Add(new ExceptionHandlerRuleElement { TypeName = typeof(TestHandlerB).AssemblyQualifiedName });
            
            ExceptionHandlerElement handlerAB = new ExceptionHandlerElement{Name = "handlerAB"};
            handlerAB.Add(new ExceptionHandlerRuleElement{TypeName = typeof(TestHandlerA).AssemblyQualifiedName});
            handlerAB.Add(new ExceptionHandlerRuleElement{TypeName = typeof(TestHandlerB).AssemblyQualifiedName});
            
            ExceptionHandlerElement handlerABC = new ExceptionHandlerElement{Name = "handlerABC", All = true};
            handlerABC.Add(new ExceptionHandlerRuleElement{TypeName = typeof(TestHandlerA).AssemblyQualifiedName});
            handlerABC.Add(new ExceptionHandlerRuleElement{TypeName = typeof(TestHandlerB).AssemblyQualifiedName});
            handlerABC.Add(new ExceptionHandlerRuleElement{TypeName = typeof(TestHandlerC).AssemblyQualifiedName});

            ExceptionHandlerElement handlerD = new ExceptionHandlerElement { Name = "handlerD" };
            handlerD.Add(new ExceptionHandlerRuleElement { TypeName = typeof(TestHandlerD).AssemblyQualifiedName });

            ExceptionHandlerElementCollection handlers = new ExceptionHandlerElementCollection();
            handlers.Add(handlerA);
            handlers.Add(handlerB);
            handlers.Add(handlerAB);
            handlers.Add(handlerABC);
            handlers.Add(handlerD);

            ExceptionPolicyTemplateElement templateA = new ExceptionPolicyTemplateElement{Name = "templateA"};
            templateA.Add(new ExceptionPolicyRuleElement { Rethrow = ExceptionPolicyRethrow.Unhandled, TypeName = typeof(ApplicationException).AssemblyQualifiedName, HandlerName = "handlerB" });
            templateA.Add(new ExceptionPolicyRuleElement { Rethrow = ExceptionPolicyRethrow.Always, TypeName = typeof(InvalidOperationException).AssemblyQualifiedName, HandlerName = "handlerB" });
            templateA.Add(new ExceptionPolicyRuleElement { Rethrow = ExceptionPolicyRethrow.Handled, TypeName = typeof(NotImplementedException).AssemblyQualifiedName, HandlerName = "handlerB" });
            templateA.Add(new ExceptionPolicyRuleElement { Rethrow = ExceptionPolicyRethrow.Unhandled, TypeName = typeof(Exception).AssemblyQualifiedName, HandlerName = "handlerA" });

            ExceptionPolicyTemplateElementCollection templates = new ExceptionPolicyTemplateElementCollection();
            templates.Add(templateA);

            ExceptionPolicyElement policyA = new ExceptionPolicyElement{Name = "ExceptionHandler_Tests.Policy.Basic.A", Template = "templateA"};

            ExceptionPolicyElement policyC = new ExceptionPolicyElement{Name = "ExceptionHandler_Tests.Policy.Basic.C", Template = "templateA"};
            policyC.Add(new ExceptionPolicyRuleElement { Rethrow = ExceptionPolicyRethrow.Unhandled, TypeName = typeof(ArgumentException).AssemblyQualifiedName, HandlerName = "handlerABC" });

            ExceptionPolicyElement policyBasic = new ExceptionPolicyElement { Name = "ExceptionHandler_Tests.Policy.Basic" };
            policyBasic.Add(new ExceptionPolicyRuleElement {Rethrow = ExceptionPolicyRethrow.Never, TypeName = typeof (ApplicationException).AssemblyQualifiedName, HandlerName = "handlerB"});
            policyBasic.Add(new ExceptionPolicyRuleElement {Rethrow = ExceptionPolicyRethrow.Never, TypeName = typeof (Exception).AssemblyQualifiedName, HandlerName = "handlerA"});

            ExceptionPolicyElement policyDefault = new ExceptionPolicyElement { Name = ExceptionHandler.DefaultPolicyName };
            policyDefault.Add(new ExceptionPolicyRuleElement { Rethrow = ExceptionPolicyRethrow.Unhandled, TypeName = typeof(Exception).AssemblyQualifiedName, HandlerName = "handlerD" });
            
            ExceptionPolicyElementCollection policies = new ExceptionPolicyElementCollection();
            policies.Add(policyA);
            policies.Add(policyC);
            policies.Add(policyBasic);
            policies.Add(policyDefault);

            ExceptionPolicyConfigurationSection section = new ExceptionPolicyConfigurationSection();
            section.ExceptionHandlers = handlers;
            section.ExceptionPolicyTemplates = templates;
            section.ExceptionPolicies = policies;

            Ex = new ExceptionHandler(section);
        }

        private class Policy
        {
            public enum Basic
            {
                A,
                B,
                C,
            }
            public enum Unknown
            {
                X
            }
        }

        private class Policy2 : IExceptionPolicy
        {
            public string ExceptionPolicyName
            {
                get { return "Policy2"; }
            }
        }

        /// <summary>
        /// Tests of policy naming conventions.
        /// </summary>
        [Test]
        public void TestPolicyName()
        {
            Assert.AreEqual("ExceptionHandler_Tests.Policy.Basic.A", ExceptionHandler.GetPolicyName(Policy.Basic.A));
            Assert.AreEqual("Policy2", ExceptionHandler.GetPolicyName(new Policy2()));
        }

        /// <summary>
        /// Tests rethrowing modes and policy template lookup.
        /// </summary>
        [Test]
        [ExpectedException(typeof(Exception), UserMessage = "Test")]
        public void TestRethrow1()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                // must stay unhandled by handler A; rethrown after being unhandled
                if (Ex.Handle(ex, Policy.Basic.A))
                {
                    Assert.AreEqual(typeof(TestHandlerA).Name, ex.Data["handler"]);
                    throw;
                }
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests rethrowing modes and policy template lookup.
        /// </summary>
        [Test]
        public void TestRethrow2()
        {
            try
            {
                throw new ApplicationException("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler B; not rethrown after being handled
                if (Ex.Handle(ex, Policy.Basic.A)) throw;
                Assert.AreEqual(typeof(TestHandlerB).Name, ex.Data["handler"]);
            }
        }

        /// <summary>
        /// Tests rethrowing modes and policy template lookup.
        /// </summary>
        [Test]
        [ExpectedException(typeof(NotImplementedException), UserMessage = "Test")]
        public void TestRethrow3()
        {
            try
            {
                throw new NotImplementedException("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler B; rethrown after being handled
                if (Ex.Handle(ex, Policy.Basic.A))
                {
                    Assert.AreEqual(typeof(TestHandlerB).Name, ex.Data["handler"]);
                    throw;
                }
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests rethrowing modes and policy template lookup.
        /// </summary>
        [Test]
        [ExpectedException(typeof(InvalidOperationException), UserMessage = "Test")]
        public void TestRethrow4()
        {
            try
            {
                throw new InvalidOperationException("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler B; rethrown always
                if (Ex.Handle(ex, Policy.Basic.A))
                {
                    Assert.AreEqual(typeof(TestHandlerB).Name, ex.Data["handler"]);
                    throw;
                }
                Assert.Fail();
            }
        }

        /// <summary>
        /// Tests policy unrolling and rethrow-never setting.
        /// </summary>
        [Test]
        public void TestUnroll1()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler A; never rethrown
                if (Ex.Handle(ex, Policy.Basic.B)) throw;
                Assert.AreEqual(typeof(TestHandlerA).Name, ex.Data["handler"]);
            }
        }

        /// <summary>
        /// Tests policy unrolling and rethrow-never setting.
        /// </summary>
        [Test]
        public void TestUnroll2()
        {
            try
            {
                throw new ApplicationException("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler A; never rethrown
                if (Ex.Handle(ex, Policy.Basic.B)) throw;
                Assert.AreEqual(typeof(TestHandlerB).Name, ex.Data["handler"]);
            }
        }

        /// <summary>
        /// Tests policy unrolling up to the default policy.
        /// </summary>
        [Test]
        public void TestUnroll3()
        {
            try
            {
                throw new Exception("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler D; never rethrown
                if (Ex.Handle(ex, Policy.Unknown.X)) throw;
                Assert.AreEqual(typeof(TestHandlerD).Name, ex.Data["handler"]);
            }
        }

        /// <summary>
        /// Tests all-handlers execution.
        /// </summary>
        [Test]
        public void TestAll()
        {
            try
            {
                throw new ArgumentException("Test");
            }
            catch (Exception ex)
            {
                // must be handled by handler C; never rethrown
                if (Ex.Handle(ex, Policy.Basic.C)) throw;
                Assert.AreEqual(typeof(TestHandlerC).Name, ex.Data["handler"]);
            }
        }
    }

#endif

    #endregion
}