namespace Boom.Patterns.Broadcasts
{
    public interface IBroadcast
    {

    }
    public interface IBroadcastState
    {

    }

    public enum DataState
    {
        None, Loading, Ready
    }
    public struct BroadcastStateWrapper<T> : IBroadcastState where T : class
    {
        public DataState state;
        public T config;

        public BroadcastStateWrapper(T config)
        {
            this.config = config;
            state = config != null ? DataState.Ready : DataState.None;
        }
        public void SetIsLoading()
        {
            state = DataState.Loading;
        }
        public void AsNone()
        {
            state = DataState.None;
        }
        public void AsReady()
        {
            state = DataState.Ready;
        }
    }
}