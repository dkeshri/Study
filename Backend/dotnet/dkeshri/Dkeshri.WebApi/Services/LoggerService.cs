namespace Dkeshri.WebApi.Services
{
    
    public class LoggerService
    {
        ISingleton singleton;
        IScope scope;
        ITransient transient;
        public LoggerService(ISingleton singleton, IScope scope, ITransient transient)
        {
            this.singleton = singleton;
            this.transient = transient;
            this.scope = scope;
        }

        public void PrintGuid()
        {
            Console.WriteLine("Logger Service");
            Console.WriteLine("Singletone " + singleton.Id);
            Console.WriteLine("Scope " + scope.Id);
            Console.WriteLine("Transient " + transient.Id);
        }
    }
}
