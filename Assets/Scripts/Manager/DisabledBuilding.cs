using Buildings;
using ResourceManagement;

namespace Manager
{
    public struct DisabledBuilding //Made by Robin
    {
        public readonly Building building;
        public readonly ResourceType type;

        public DisabledBuilding(Building building, ResourceType type)
        {
            this.building = building;
            this.type = type;
        }
    }
}