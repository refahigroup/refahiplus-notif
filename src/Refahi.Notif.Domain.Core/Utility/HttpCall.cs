using Polly;
using System.Linq.Expressions;
using System.Net;
using System.Text;

namespace Refahi.Notif.Domain.Core.Utility
{
    public class CallApiRequest
    {

        public HttpContent RequestContent { get; set; }

        public HttpMethod MethodType { get; set; } = HttpMethod.Get;

        public string Action { get; set; }

        public IDictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();


    }

    public class CallApiResponse
    {
        public string HttpResponseMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.RequestTimeout;

        public bool RequestSucceed { get; set; } = true;

        public bool DeserializationSucceed { get; set; }

        public string? RequestUri { get; set; }
        public Exception Exception { get; set; }
        public string RequestContent { get; set; }
        public Dictionary<string, string?>? ResponseHeader { get; set; } = new Dictionary<string, string>();
    }
    public static class HttpCall
    {

        public static Task<T> Get<T>(this HttpClient client, string url)
        {
            return SendJson<T>(client, HttpMethod.Get, url, null, null);
        }
        public static Task<T> Post<T>(this HttpClient client, string url, object data, Dictionary<string, string> headers = null)
        {
            return SendJson<T>(client, HttpMethod.Post, url, data, headers);
        }
        public static Task<string> PostString(this HttpClient client, string url, object data, Dictionary<string, string> headers = null)
        {
            return SendJson(client, HttpMethod.Post, url, data, headers);
        }
        public static Task<T> PostUrlEncoded<T>(this HttpClient client, string url, List<KeyValuePair<string, string>> data, Dictionary<string, string> headers = null)
        {
            return SendByte<T>(client, HttpMethod.Post, url, data, headers);
        }
        public static Task<string> PostUrlEncoded(this HttpClient client, string url, List<KeyValuePair<string, string>> data, Dictionary<string, string> headers = null)
        {
            return SendByte(client, HttpMethod.Post, url, data, headers);
        }
        public static Task<string> GetString(this HttpClient client, string url, Dictionary<string, string> headers = null, bool ensureSuccessStatusCode = true)
        {
            return SendJson(client, HttpMethod.Get, url, null, headers, ensureSuccessStatusCode);
        }
        public static Task Get(this HttpClient client, string url, Dictionary<string, string> headers = null)
        {
            return SendJson(client, HttpMethod.Get, url, null, headers);
        }

        private static async Task<T> SendJson<T>(HttpClient client, HttpMethod method, string url, object data, Dictionary<string, string> headers = null, bool ensureSuccessStatusCode = true)
        {
            return (await SendJson(client, method, url, data, headers, ensureSuccessStatusCode)).DeSerilize<T>();
        }

        private static async Task<string> SendJson(HttpClient client, HttpMethod method, string url, object data, Dictionary<string, string> headers = null, bool ensureSuccessStatusCode = true)
        {
            client.Timeout = TimeSpan.FromSeconds(200);
            var request = new HttpRequestMessage(method, url);
            var json = data.Serilize();
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            if (headers != null)
                foreach (var header in headers)
                    request.Headers.TryAddWithoutValidation(header.Key, header.Value);

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            try
            {
                if (ensureSuccessStatusCode)
                    response.EnsureSuccessStatusCode();
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error In Http Call : {result}", ex);
            }
            finally
            {
                request.Dispose();
                response.Dispose();
            }
        }

        private static async Task<T> SendByte<T>(HttpClient client, HttpMethod method, string url, List<KeyValuePair<string, string>> data, Dictionary<string, string> headers = null)
        {

            return (await SendByte(client, method, url, data, headers)).DeSerilize<T>();

        }

        private static async Task<string> SendByte(HttpClient client, HttpMethod method, string url, List<KeyValuePair<string, string>> data, Dictionary<string, string> headers = null)
        {
            client.Timeout = TimeSpan.FromSeconds(200);
            var request = new HttpRequestMessage(method, url);
            request.Content = new FormUrlEncodedContent(data);

            if (headers != null)
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            try
            {
                response.EnsureSuccessStatusCode();
                return result;

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                request.Dispose();
                response.Dispose();
            }
        }





        public class CallApiResponse<TResponse> : CallApiResponse where TResponse : class, new()
        {
            public TResponse Data { get; set; } = default;

        }




        public static async Task<CallApiResponse<TResponseEntity>> SendAsync<TResponseEntity>(
                this HttpClient client, CallApiRequest request, CancellationToken cancellationToken = default)
                where TResponseEntity : class, new()
        {
            var response = new CallApiResponse<TResponseEntity>();



            using var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = request.MethodType;
            httpRequestMessage.RequestUri = client.BaseAddress != null ? new Uri(client.BaseAddress, request.Action) : new Uri(request.Action);

            httpRequestMessage.Content = request.RequestContent;

            foreach (var header in request.Headers)
                httpRequestMessage.Headers.Add(header.Key, header.Value);

            try
            {
                // 4.Send request
                var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);
                response.HttpResponseMessage = await httpResponseMessage?.Content?.ReadAsStringAsync(cancellationToken)!;
                response.ResponseHeader = httpResponseMessage?.Headers?.ToDictionary(r => r.Key, r => r.Value.FirstOrDefault());
                response.StatusCode = httpResponseMessage?.StatusCode ?? HttpStatusCode.RequestTimeout;
                response.RequestSucceed = httpResponseMessage?.IsSuccessStatusCode ?? false;
                response.RequestUri = httpRequestMessage?.RequestUri?.ToString();

                // ** Get RequestContent
                try
                {
                    response.RequestContent =
                        httpRequestMessage?.Content != null
                            ?
                            await httpRequestMessage?.Content?.ReadAsStringAsync(cancellationToken)!
                            :
                            "Request Content Is Empty"
                        ;
                }
                catch
                {
                    /* Do Nothings*/
                }

                // 5.DeserializeObject
                if (response.RequestSucceed)
                {
                    try
                    {
                        response.Data = System.Text.Json.JsonSerializer.Deserialize<TResponseEntity>(response.HttpResponseMessage)!;
                        response.DeserializationSucceed = true;

                    }
                    catch (Exception e)
                    {
                        response.DeserializationSucceed = false;
                        response.Exception = e;
                    }
                }
            }
            // UnKnown or undefined exception
            catch (Exception exp)
            {

                response.Exception = exp;
            }


            // 7.Return response
            return response;
        }


        public static async Task<CallApiResponse> SendAsync(this HttpClient client,
               CallApiRequest request, CancellationToken cancellationToken = default)
        {
            var response = new CallApiResponse();

            using var httpRequestMessage = new HttpRequestMessage();

            httpRequestMessage.Method = request.MethodType;
            httpRequestMessage.RequestUri = client.BaseAddress != null ? new Uri(client.BaseAddress, request.Action) : new Uri(request.Action);

            httpRequestMessage.Content = request.RequestContent;

            foreach (var header in request.Headers)
                httpRequestMessage.Headers.Add(header.Key, header.Value);

            try
            {
                // 4.Send request
                var httpResponseMessage = await client.SendAsync(httpRequestMessage, cancellationToken);
                response.HttpResponseMessage = await httpResponseMessage?.Content?.ReadAsStringAsync(cancellationToken)!;
                response.ResponseHeader = httpResponseMessage?.Headers?.ToDictionary(r => r.Key, r => r.Value.FirstOrDefault());
                response.StatusCode = httpResponseMessage?.StatusCode ?? HttpStatusCode.RequestTimeout;
                response.RequestSucceed = httpResponseMessage?.IsSuccessStatusCode ?? false;
                response.RequestUri = httpRequestMessage?.RequestUri?.ToString();

                // ** Get RequestContent
                try
                {
                    response.RequestContent =
                        httpRequestMessage?.Content != null
                            ?
                            await httpRequestMessage?.Content?.ReadAsStringAsync(cancellationToken)!
                            :
                            "Request Content Is Empty"
                        ;
                }
                catch
                {
                    /* Do Nothings*/
                }

            }
            // UnKnown or undefined exception
            catch (Exception exp)
            {

                response.Exception = exp;
            }


            // 7.Return response
            return response;
        }


        public static async Task<CallApiResponse<TResponseEntity>> SendAsync<TResponseEntity>(
                this HttpClient client, CallApiRequest context, Expression<Func<CallApiResponse<TResponseEntity>, bool>> retryCondition, int retryCount = 2, int retryAttemptWaitInSeconds = 5, int retryTimeoutInSeconds = 30, CancellationToken cancellationToken = default)
            where TResponseEntity : class, new()
        {

            var retryPolicy = Policy
                            .HandleResult(retryCondition.Compile())
                            .WaitAndRetryAsync(retryCount, i => TimeSpan.FromSeconds(retryAttemptWaitInSeconds));

            var timeoutPolicy = Policy.TimeoutAsync<CallApiResponse<TResponseEntity>>(retryTimeoutInSeconds).WrapAsync(retryPolicy);


            return await timeoutPolicy.ExecuteAsync(() => client.SendAsync<TResponseEntity>(context, cancellationToken));
        }

        public static async Task<CallApiResponse> SendAsync(
            this HttpClient client, CallApiRequest context, Expression<Func<CallApiResponse, bool>> retryCondition, int retryCount = 2, int retryAttemptWaitInSeconds = 5, int retryTimeoutInSeconds = 30, CancellationToken cancellationToken = default)
        {
            var retryPolicy = Policy
                .HandleResult(retryCondition.Compile())
                .WaitAndRetryAsync(retryCount, i => TimeSpan.FromSeconds(retryAttemptWaitInSeconds));


            var timeoutPolicy = Policy.TimeoutAsync<CallApiResponse>(retryTimeoutInSeconds).WrapAsync(retryPolicy);

            return await timeoutPolicy.ExecuteAsync(() => client.SendAsync(context, cancellationToken));

        }
    }



}
