namespace Nine.Screens;

public interface IVisualConfigurable<TVisualState>
{
    TVisualState? GetDefaultVisualState() => default;

    void EnterConfigurationMode();

    void ApplyVisualState(TVisualState visualState);

    void ExitConfigurationMode();
}
