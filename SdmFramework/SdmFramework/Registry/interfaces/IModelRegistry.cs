
using SdmFramework.Model;

namespace SdmFramework.Registry.interfaces
{
    public interface IModelRegistry
    {
        void RegisterModel(string name, object modelInstance);
        ModelWrapper GetModel(string name);
        Dictionary<string, ModelWrapper> GetAllModels();
    }
}