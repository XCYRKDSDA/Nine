namespace Nine.Animations;

public interface IParametric<out T>
{
    T Bake(IDictionary<string, object?>? parameters = null);
}
