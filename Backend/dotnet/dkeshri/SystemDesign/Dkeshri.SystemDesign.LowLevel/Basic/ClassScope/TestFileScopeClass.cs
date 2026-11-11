using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dkeshri.SystemDesign.LowLevel.Basic.ClassScope
{
    internal class TestFileScopeClass
    {
        private void run()
        {
            // can not access file class from other class. it can only be accessed from same file.
            // FileClassScopeExample fileClassScopeExample = new FileClassScopeExample();
            // fileClassScopeExample.run();
        }
    }
}
