using Nine.Animations;

namespace Nine.Assets.Animation;

public interface IPropertyParser<TObject>
{
    (IProperty<TObject>, Type) Parse(string property);
}
