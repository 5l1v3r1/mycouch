﻿using System.Net.Http;
using EnsureThat;
using MyCouch.Net;
using MyCouch.Requests;
using MyCouch.Serialization;

namespace MyCouch.HttpRequestFactories
{
    public class PostEntityHttpRequestFactory
    {
        protected ISerializer Serializer { get; private set; }

        public PostEntityHttpRequestFactory(ISerializer serializer)
        {
            Ensure.Any.IsNotNull(serializer, nameof(serializer));

            Serializer = serializer;
        }

        public virtual HttpRequest Create<T>(PostEntityRequest<T> request) where T : class
        {
            Ensure.Any.IsNotNull(request, nameof(request));

            return new HttpRequest(HttpMethod.Post, GenerateRelativeUrl(request))
                .SetRequestTypeHeader(request.GetType())
                .SetJsonContent(SerializeEntity(request.Entity));
        }

        protected virtual string SerializeEntity<T>(T entity) where T : class
        {
            return Serializer.Serialize(entity);
        }

        protected virtual string GenerateRelativeUrl<T>(PostEntityRequest<T> request) where T : class
        {
            var urlParams = new UrlParams();

            urlParams.AddIfTrue("batch", request.Batch, "ok");

            return string.Format("/{0}", new QueryString(urlParams));
        }
    }
}