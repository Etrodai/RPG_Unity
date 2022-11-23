using Manager;
using ResourceManagement.Manager;

namespace DebugMenus
{
    public class CheatCodeLogic //Made by Robin
    {
        public void EnergyPlus100()
        {
            MainManagerSingleton.Instance.EnergyManager.SavedResourceValue += 100;
        }

        public void FoodPlus100()
        {
            MainManagerSingleton.Instance.FoodManager.SavedResourceValue += 100;
        }

        public void WaterPlus100()
        {
            MainManagerSingleton.Instance.WaterManager.SavedResourceValue += 100;
        }

        public void MaterialPlus100()
        {
            MainManagerSingleton.Instance.MaterialManager.SavedResourceValue += 100;
        }
    }
}