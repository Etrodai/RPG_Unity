namespace ResourceManagement
{
    [System.Serializable]
    public struct Resource
    {
        #region Variables

        public ResourceTypes resource;
        public int value;
        public float eventResourceDemandValue;

        #endregion
    }
}