using Dkeshri.SystemDesign.LowLevel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.Basic.ClassScope
{
    public class ClassScopeContainer : IExecute
    {
        public void run()
        {
            PublicClass publicClass = new PublicClass();
            publicClass.run();

            InternalClass internalClass = new InternalClass();
            internalClass.run();


            BaseClassScopeExample baseClassScopeExample = new BaseClassScopeExample();

            BaseClassScopeExample.NestedPublicClass nestedPublicClass = new BaseClassScopeExample.NestedPublicClass();
            nestedPublicClass.run();

            BaseClassScopeExample.NestedInternalClass nestedInternalClass = new BaseClassScopeExample.NestedInternalClass();
            nestedInternalClass.run();

            BaseClassScopeExample.ProtectedInternalClass protectedInternalClass = new BaseClassScopeExample.ProtectedInternalClass();
            protectedInternalClass.run();


            // can not access private and protected class from other class.
            // BaseClassScopeExample.PrivateProtectedClass privateProtectedClass = new BaseClassScopeExample.PrivateProtectedClass();


            // can not access private class from other class.
            // BaseClassScopeExample.NestedPrivateClass nestedPrivateClass = new BaseClassScopeExample.NestedPrivateClass();

            // can not access protected class from other class.
            // BaseClassScopeExample.NestedProtectedClass nestedProtectedClass = new BaseClassScopeExample.NestedProtectedClass();


            FileClassScopeExample fileClassScopeExample = new FileClassScopeExample();
            fileClassScopeExample.run();

        }
    }


    public class DerivedClassScopeExample : BaseClassScopeExample
    {
        public void run()
        {

            BaseClassScopeExample.NestedPublicClass nestedPublicClass = new BaseClassScopeExample.NestedPublicClass();
            nestedPublicClass.run();

            BaseClassScopeExample.NestedProtectedClass nestedProtectedClass = new BaseClassScopeExample.NestedProtectedClass();
            nestedProtectedClass.run();

            // can not access private class from other class.
            // BaseClassScopeExample.NestedPrivateClass nestedPrivateClass = new BaseClassScopeExample.NestedPrivateClass();

            BaseClassScopeExample.NestedInternalClass nestedInternalClass = new BaseClassScopeExample.NestedInternalClass();
            nestedInternalClass.run();

            BaseClassScopeExample.ProtectedInternalClass protectedInternalClass = new BaseClassScopeExample.ProtectedInternalClass();
            protectedInternalClass.run();

            BaseClassScopeExample.PrivateProtectedClass privateProtectedClass = new BaseClassScopeExample.PrivateProtectedClass();
            privateProtectedClass.run();

            
        }
    }


    public class PublicClass
    {
        public void run()
        {
            Console.WriteLine("Public Class");
        }
    }

    internal class InternalClass
    {
        public void run()
        {

            Console.WriteLine("Internal Class");
        }
    }


    public class BaseClassScopeExample
    {
         
        public class NestedPublicClass
        {
            public void run()
            {
                Console.WriteLine("Nested Public Class");
            }
        }

        private class NestedPrivateClass
        {
            public void run()
            {
                Console.WriteLine("Private Class");
            }
        }

        protected class NestedProtectedClass
        {
            public void run()
            {
                Console.WriteLine("Nested Protected Class");
            }
        }

        internal class NestedInternalClass
        {
            public void run()
            {
                Console.WriteLine("nested Internal Class");
            }
        }

        protected internal class ProtectedInternalClass
        {
            public void run()
            {
                Console.WriteLine("Protected Internal Class");
            }
        }

        private protected class PrivateProtectedClass
        {
            public void run()
            {
                Console.WriteLine("Private Protected Class");
            }
        }

        public void run()
        {
            NestedPrivateClass privateClass = new NestedPrivateClass();
            privateClass.run();
            
            ProtectedInternalClass protectedInternalClass = new ProtectedInternalClass();
            protectedInternalClass.run();
            
            NestedInternalClass nestedInternalClass = new NestedInternalClass();
            nestedInternalClass.run();

            NestedPublicClass nestedPublicClass = new NestedPublicClass();
            nestedPublicClass.run();

            NestedProtectedClass nestedProtectedClass = new NestedProtectedClass();
            nestedProtectedClass.run();

            PrivateProtectedClass privateProtectedClass = new PrivateProtectedClass();
            privateProtectedClass.run();
        }

        
    }



    
    file class FileClassScopeExample
    {
        // can not access file class from other class. it can only be accessed from same file.
        public void run()
        {
            Console.WriteLine("File Scope Class");
        }
    }

}
