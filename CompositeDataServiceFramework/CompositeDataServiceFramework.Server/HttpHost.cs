using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Services;
using System.IO;
using System.IO.Compression;
using System.Net;

namespace CompositeDataService
{
    /// <summary>
    /// Simple implementation of IDataServiceHost on top of
    /// the ASP.NET API
    /// </summary>
    public class HttpHost : IDataServiceHost, IDataServiceHost2
    {
        private Uri serviceUri;
        private Uri absoluteUri;
        private Stream _stream = null;
        private Stream _responseStream = new MemoryStream(65535);
        private string responseDataServiceVersion = string.Empty;
        private string responseCacheControl = string.Empty;
        private string responseContentType = string.Empty;
        private string responseLocation = string.Empty;
        private string responseETag = string.Empty;

        public HttpHost(Uri serviceUri, string request)
        {
            if (serviceUri == null) throw new ArgumentNullException("serviceUri");

            this.serviceUri = serviceUri;

            string realUriPath = serviceUri.ToString() + request;
            absoluteUri = new Uri(realUriPath);
        }

        #region IDataServiceHost Members

        public Uri AbsoluteRequestUri
        {
            get
            {
                return this.absoluteUri;
            }
        }

        public Uri AbsoluteServiceUri
        {
            get { return this.serviceUri; }
        }

        public string GetQueryStringItem(string item)
        {
            return this.context.Request[item];
        }

        public void ProcessException(HandleExceptionArgs args)
        {
            // this empty implementation results in the default behavior staying as-is
            context.Response.Write(args.Exception.ToString());
            context.Response.Flush();
            context.Response.Close();
        }

        public string RequestAccept
        {
            get { return this.context.Request.Headers["Accept"]; }
        }

        public string RequestAcceptCharSet
        {
            get { return this.context.Request.Headers["Accept-Charset"]; }
        }

        public string RequestContentType
        {
            get { return this.context.Request.Headers["Content-Type"]; }
        }

        public string RequestHttpMethod
        {
            get
            {
                // if life was simpler this would just be context.Request.HttpMethod,
                // but in order to support HTTP method tunneling (required for
                // Silverlight among other cases), we need to do a bit more

                string method = this.context.Request.Headers["X-HTTP-Method"];

                if (string.IsNullOrEmpty(method))
                {
                    method = this.context.Request.HttpMethod;
                }
                else if (this.context.Request.HttpMethod.Equals("POST", StringComparison.InvariantCultureIgnoreCase))
                {
                    if (method.Equals("PUT", StringComparison.InvariantCultureIgnoreCase) ||
                        method.Equals("DELETE", StringComparison.InvariantCultureIgnoreCase) ||
                        method.Equals("MERGE", StringComparison.InvariantCultureIgnoreCase))
                    {
                        throw new InvalidOperationException("HTTP method tunneling will only tunnel PUT, DELETE and MERGE requests");
                    }
                }
                else
                {
                    throw new InvalidOperationException("HTTP method tunneling should be done through POST requests.");
                }

                return method;
            }
        }

        public string RequestIfMatch
        {
            get { return this.context.Request.Headers["if-match"]; }
        }

        public string RequestIfNoneMatch
        {
            get { return this.context.Request.Headers["if-none-match"]; }
        }

        public string RequestMaxVersion
        {
            get { return this.context.Request.Headers["MaxDataServiceVersion"]; }
        }

        public System.IO.Stream RequestStream
        {
            get { return this.context.Request.InputStream; }
        }

        public string RequestVersion
        {
            get { return this.context.Request.Headers["DataServiceVersion"]; }
        }

        public string ResponseCacheControl
        {
            get
            {
                return this.responseCacheControl;
            }
            set
            {
                this.context.Response.AddHeader("cache-control", value);
                this.responseCacheControl = value;
            }
        }

        public string ResponseContentType
        {
            get
            {
                return this.responseContentType;
            }
            set
            {
                if (String.IsNullOrEmpty(this.responseContentType))
                {
                    this.context.Response.AddHeader("Content-Type", value);
                    this.responseContentType = value;
                }
            }
        }

        public string ResponseETag
        {
            get
            {
                return this.responseETag;
            }
            set
            {
                this.context.Response.AddHeader("ETag", value);
                this.responseETag = value;
            }
        }

        public string ResponseLocation
        {
            get
            {
                return this.responseLocation;
            }
            set
            {
                this.context.Response.AddHeader("location", value);
                this.responseLocation = value;
            }
        }

        public int ResponseStatusCode
        {
            get
            {
                return this.context.Response.StatusCode;
            }
            set
            {
                this.context.Response.StatusCode = value;
            }
        }

        public System.IO.Stream ResponseStream
        {
            get
            {
             /*   string encoding = context.Response.Headers["Content-Encoding"];
                if (encoding == "deflate")
                    return _stream = new DeflateStream(this.context.Response.OutputStream, CompressionMode.Compress);
                else if (encoding == "gzip")
                    return _stream = new GZipStream(this.context.Response.OutputStream, CompressionMode.Compress);
                */
                return _stream = _responseStream;
            }
        }

        public string ResponseVersion
        {
            get
            {
                return this.responseDataServiceVersion;
            }
            set
            {
                this.context.Response.AddHeader("DataServiceVersion", value);
                this.responseDataServiceVersion = value;
            }
        }

        #endregion

        #region IDataServiceHost2 Members

        public System.Net.WebHeaderCollection RequestHeaders
        {
            get
            {
                var whc = new WebHeaderCollection();
                whc.Add(context.Request.Headers);
                return whc;
            }
        }

        public System.Net.WebHeaderCollection ResponseHeaders
        {
            get
            {
                var whc = new WebHeaderCollection();
                whc.Add(context.Response.Headers);
                return whc;
            }
        }

        #endregion

        public void Flush()
        {
            _stream.Flush();
        }

        public void Close()
        {
            _stream.Close();
        }
    }
}