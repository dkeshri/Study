using CoreLogic.Extensibility.Interfaces.Base;

namespace CoreLogic.Service.Logic
{
    public class ServiceDependencyResolver : AbstractDependencyResolver
    {
        protected override void OnSetUp()
        {
            // interface namespace
            AddServiceDescriptors("Store.Services.Interfaces");
        }
    }
}
