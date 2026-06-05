using System.Reflection;

namespace SdmFramework.Utils;

public class CustomParameterInfo
{
    public ParameterInfo OriginalParameter { get; }
    public Attribute[] CustomAttributes { get; }

    public CustomParameterInfo(ParameterInfo originalParameter, Attribute[] customAttribute)
    {
        OriginalParameter = originalParameter ?? throw new ArgumentNullException(nameof(originalParameter));
        CustomAttributes = customAttribute;
    }
}