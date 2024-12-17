namespace Nine.Animations.Parametric;

public interface IParametric<out T>
{
    T Bake(IDictionary<string, object?>? parameters = null);
}
