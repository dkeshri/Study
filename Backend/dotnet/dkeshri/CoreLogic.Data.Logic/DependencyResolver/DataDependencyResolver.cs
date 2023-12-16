using CoreLogic.Extensibility.Interfaces;
using CoreLogic.Extensibility.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLogic.Data.Logic.DependencyResolver
{
    [Export(typeof(IDependencyResolver))]
    public class DataDependencyResolver : AbstractDependencyResolver
    {
        protected override void OnSetUp()
        {
            // interface namespace
            AddServiceDescriptors("CoreLogic.Data.Interfaces.Interfaces");
        }
    }
}
