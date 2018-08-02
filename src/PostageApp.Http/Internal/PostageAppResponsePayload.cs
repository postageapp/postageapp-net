using PostageApp.Abstractions;

namespace PostageApp.Http
{
    internal class PostageAppResponsePayload<TData> : PostageAppResponsePayload<PostageAppResponseMeta, TData>
    {
    }

    internal class PostageAppResponsePayload<TMeta, TData> : PostageAppResponseEmptyPayload<TMeta>
    {
        public TData Data { get; set; }
    }

    internal class PostageAppResponseEmptyPayload<TMeta>
    {
        public TMeta Response { get; set; }
    }
}
