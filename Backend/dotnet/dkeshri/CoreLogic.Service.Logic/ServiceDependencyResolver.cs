using CoreLogic.Extensibility.Interfaces;
using CoreLogic.Extensibility.Interfaces.Base;
using System.ComponentModel.Composition;

namespace CoreLogic.Service.Logic
{
    [Export(typeof(IDependencyResolver))]
    public class ServiceDependencyResolver : AbstractDependencyResolver
    {
        protected override void OnSetUp()
        {
            // interface namespace
            AddServiceDescriptors("CoreLogic.Service.Interfaces.Interfaces");
        }
    }
}
