using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System;

namespace MovieStore.Service.Handlers
{
    class RetryDelegatingHandler : HttpClientHandler
    {
        public TimeSpan PerRequestTimeout { get; set; }
        public RetryPolicy retryPolicy { get; set; }
        public RetryDelegatingHandler()
            : base()
        {
            retryPolicy = CustomRetryPolicy.MakeHttpRetryPolicy();
        }


        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage responseMessage = null;
            var currentRetryCount = 0;

            //On Retry => increments the retry count
            retryPolicy.Retrying += (sender, args) =>
            {
                currentRetryCount = args.CurrentRetryCount;
            };
            try
            {
                await retryPolicy.ExecuteAsync(async () =>
                {
                    responseMessage = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                    if ((int)responseMessage.StatusCode >= 500)
                    { //When it fails after the retries, it would throw the exception
                        throw new HttpRequestExceptionWithStatus(string.Format("Response status code {0} indicates server error", (int)responseMessage.StatusCode))
                        {
                            StatusCode = responseMessage.StatusCode,
                            CurrentRetryCount = currentRetryCount
                        };
                    }// returns the response to the main method(from the anonymous method)
                    return responseMessage;
                }, cancellationToken).ConfigureAwait(false);
                return responseMessage;// returns from the main method => SendAsync
            }
            catch (HttpRequestExceptionWithStatus exception)
            {
                if (exception.CurrentRetryCount >= 3)
                {
                    //write to log
                }
                if (responseMessage != null)
                {
                    return responseMessage;
                }
                throw;
            }
            catch (Exception)
            {
                if (responseMessage != null)
                {
                    return responseMessage;
                }
                throw;
            }
        }
    }
    //Retry Policy = Error Detection Strategy + Retry Strategy
    public static class CustomRetryPolicy
    {
        public static RetryPolicy MakeHttpRetryPolicy()
        {
            //The transient fault application block provides three retry policies that you can use. These are:
            //Incremental => Retry four times at one-second intervals
            //Fixed Interval => Retry four times, waiting one second before the first retry, then two seconds before the second retry, then three seconds before the third retry, and four
            //seconds before the fourth retry.
            //Expotential Backoff
            //////Retry four times, waiting two seconds before the first retry, then four seconds before the second retry, then eight seconds before the third retry, and sixteen seconds before
            // the fourth retry.
            //////This retry strategy also introduces a small amount of random variation into the intervals. This can be useful if the same operation is being called multiple times
            //simultaneously by the client application.
            var retryCount = 5;
            // Below Parameters are used to calculate the retry delay
            var minBackoff = TimeSpan.FromSeconds(1);
            var maxBackoff = TimeSpan.FromSeconds(10);
            var deltaBackoff = TimeSpan.FromSeconds(5);
            //A retry strategy with back-off parameters for calculating the exponential delay between retries.
            var exponentialBackoff = new ExponentialBackoff(retryCount, minBackoff, maxBackoff, deltaBackoff);
            //Error Detection Strategy
            var strategy = new HttpTransientErrorDetectionStrategy();
            //Retry Policy = Error Detection Strategy + Retry Strategy
            return new RetryPolicy(strategy, exponentialBackoff);
        }
    }
    //This class is responsible for deciding whether the response was an intermittent transient error or not.
    public class HttpTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        public bool IsTransient(Exception ex)
        {
            if (ex != null)
            {
                HttpRequestExceptionWithStatus httpException;
                if ((httpException = ex as HttpRequestExceptionWithStatus) != null)
                {
                    if (httpException.StatusCode == HttpStatusCode.ServiceUnavailable)
                    {
                        return true;
                    }
                    else if (httpException.StatusCode == HttpStatusCode.MethodNotAllowed)
                    {
                        return true;
                    }
                    else if (httpException.StatusCode == HttpStatusCode.GatewayTimeout)
                    {
                        return true;
                    }
                    else if (httpException.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
    }
    //Custom HttpRequestException to allow include additional properties on my exception, which can be used to help determine whether the exception is a transient error or not.
    public class HttpRequestExceptionWithStatus : HttpRequestException
    {
        public HttpRequestExceptionWithStatus() : base() { }
        public HttpRequestExceptionWithStatus(string message) : base(message) { }
        public HttpRequestExceptionWithStatus(string message, Exception inner) : base(message, inner) { }
        public HttpStatusCode StatusCode { get; set; }
        public int CurrentRetryCount { get; set; }
    }
}
