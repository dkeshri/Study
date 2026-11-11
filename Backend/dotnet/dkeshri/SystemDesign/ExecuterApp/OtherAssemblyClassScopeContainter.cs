using Dkeshri.SystemDesign.LowLevel.Basic.ClassScope;
using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuterApp
{
    public class OtherAssemblyClassScopeContainter : IExecute
    {
        
        public void run()
        {
            PublicClass publicClass = new PublicClass();
            publicClass.run();

            // can not access internal class from other assembly.
            //InternalClass internalClass = new InternalClass();


            BaseClassScopeExample.NestedPublicClass nestedPublicClass = new BaseClassScopeExample.NestedPublicClass();
            nestedPublicClass.run();

            // can not access protected class from other assembly.
            // BaseClassScopeExample.NestedProtectedClass nestedProtectedClass = new BaseClassScopeExample.NestedProtectedClass();
            // nestedProtectedClass.run();

            // can not access private class from other class.
            //BaseClassScopeExample.NestedPrivateClass nestedPrivateClass = new BaseClassScopeExample.NestedPrivateClass();

            // can not access nested internal class from other assembly.
            //BaseClassScopeExample.NestedInternalClass nestedInternalClass = new BaseClassScopeExample.NestedInternalClass();
            // nestedInternalClass.run();

            // can not access protected internal class from other assembly.
            //BaseClassScopeExample.ProtectedInternalClass protectedInternalClass = new BaseClassScopeExample.ProtectedInternalClass();
            // protectedInternalClass.run();

            // can not access private protected class from other assembly.
            //BaseClassScopeExample.PrivateProtectedClass privateProtectedClass = new BaseClassScopeExample.PrivateProtectedClass();
            // privateProtectedClass.run();
        }
    }

    public class DerivedOtherAssemblyClassScopeExample : BaseClassScopeExample
    {
        public void run()
        {
            BaseClassScopeExample.NestedPublicClass nestedPublicClass = new BaseClassScopeExample.NestedPublicClass();
            nestedPublicClass.run();

            // can not access protected class from other assembly.
            BaseClassScopeExample.NestedProtectedClass nestedProtectedClass = new BaseClassScopeExample.NestedProtectedClass();
            nestedProtectedClass.run();

            // can not access private class from other class.
            // BaseClassScopeExample.NestedPrivateClass nestedPrivateClass = new BaseClassScopeExample.NestedPrivateClass();

            // can not access nested internal class from other assembly.
            // BaseClassScopeExample.NestedInternalClass nestedInternalClass = new BaseClassScopeExample.NestedInternalClass();
            // nestedInternalClass.run();

            BaseClassScopeExample.ProtectedInternalClass protectedInternalClass = new BaseClassScopeExample.ProtectedInternalClass();
            protectedInternalClass.run();

            // can not access private protected class from other assembly.
            // BaseClassScopeExample.PrivateProtectedClass privateProtectedClass = new BaseClassScopeExample.PrivateProtectedClass();
            // privateProtectedClass.run();
        }
    }
}
