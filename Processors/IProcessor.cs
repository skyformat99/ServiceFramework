namespace ServiceFramework.Processors
{
    public interface IProcessor
    {
        void ExecuteHealthCheck();

        void Process();

        void Start();

        void Stop();
    }
}