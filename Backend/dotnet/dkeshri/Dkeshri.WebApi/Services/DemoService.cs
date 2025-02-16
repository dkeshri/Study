namespace Dkeshri.WebApi.Services
{
    public interface ISingleton
    {
        Guid Id { get; }
    }
    public interface IScope : ISingleton { }
    public interface ITransient : ISingleton { }
    public class DemoService : ISingleton, ITransient, IScope
    {
        public Guid Id { get; }


        public DemoService()
        {
            Id = Guid.NewGuid();
        }
    }
}
