using Zenject;

public class WorldChangeInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<WorldChangeController>().FromComponentInHierarchy().AsCached();
    }
}
