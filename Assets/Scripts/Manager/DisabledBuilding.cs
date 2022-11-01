using Buildings;
using ResourceManagement;

namespace Manager
{
    public struct DisabledBuilding
    {
        public readonly Building building;
        public readonly ResourceTypes type;

        public DisabledBuilding(Building building, ResourceTypes type)
        {
            this.building = building;
            this.type = type;
        }
    }
}