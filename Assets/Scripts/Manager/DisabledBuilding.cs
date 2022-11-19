using Buildings;
using ResourceManagement;

namespace Manager
{
    public struct DisabledBuilding
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