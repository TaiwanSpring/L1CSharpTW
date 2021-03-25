using LineageServer.Interfaces;

namespace LineageServer.Models
{
    abstract class ContainerObject
    {
        static IContainerAdapter container;
        protected IContainerAdapter Container { get; }
        public static void SetContainerAdapter(IContainerAdapter containerAdapter)
        {
            container = containerAdapter;
        }
        public ContainerObject()
        {
            Container = container;
        }
    }
}
