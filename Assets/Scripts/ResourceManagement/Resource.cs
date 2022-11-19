namespace ResourceManagement
{
    [System.Serializable]
    public struct Resource
    {
        #region Variables

        public ResourceType resource;
        public int value;
        public float eventResourceDemandValue;

        #endregion
    }
}