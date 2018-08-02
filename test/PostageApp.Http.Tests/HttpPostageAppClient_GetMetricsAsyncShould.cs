using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PostageApp.Abstractions;
using Xunit;

namespace PostageApp.Http.Tests
{
    public class HttpPostageAppClient_GetMetricsAsyncShould
    {
        [Fact]
        public async Task ReturnSuccededGivenExistingApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("successfull_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetMetricsAsync();

            // Assert
            Assert.True(result.Succeeded);
            Assert.Null(result.Error);

            Assert.Equal("ok", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("metrics", result.ResponseMeta.Message);

            Assert.NotNull(result.Data.Metrics.Hour);
            Assert.NotNull(result.Data.Metrics.Date);
            Assert.NotNull(result.Data.Metrics.Week);
            Assert.NotNull(result.Data.Metrics.Month);

            Assert.NotNull(result.Data.Metrics.Hour.Delivered);
            Assert.NotNull(result.Data.Metrics.Hour.Opened);
            Assert.NotNull(result.Data.Metrics.Hour.Clicked);
            Assert.NotNull(result.Data.Metrics.Hour.Failed);
            Assert.NotNull(result.Data.Metrics.Hour.Rejected);
            Assert.NotNull(result.Data.Metrics.Hour.Spammed);
            Assert.NotNull(result.Data.Metrics.Hour.Created);
            Assert.NotNull(result.Data.Metrics.Hour.Queued);

            Assert.NotNull(result.Data.Metrics.Date.Delivered);
            Assert.NotNull(result.Data.Metrics.Date.Opened);
            Assert.NotNull(result.Data.Metrics.Date.Clicked);
            Assert.NotNull(result.Data.Metrics.Date.Failed);
            Assert.NotNull(result.Data.Metrics.Date.Rejected);
            Assert.NotNull(result.Data.Metrics.Date.Spammed);
            Assert.NotNull(result.Data.Metrics.Date.Created);
            Assert.NotNull(result.Data.Metrics.Date.Queued);

            Assert.NotNull(result.Data.Metrics.Week.Delivered);
            Assert.NotNull(result.Data.Metrics.Week.Opened);
            Assert.NotNull(result.Data.Metrics.Week.Clicked);
            Assert.NotNull(result.Data.Metrics.Week.Failed);
            Assert.NotNull(result.Data.Metrics.Week.Rejected);
            Assert.NotNull(result.Data.Metrics.Week.Spammed);
            Assert.NotNull(result.Data.Metrics.Week.Created);
            Assert.NotNull(result.Data.Metrics.Week.Queued);

            Assert.NotNull(result.Data.Metrics.Month.Delivered);
            Assert.NotNull(result.Data.Metrics.Month.Opened);
            Assert.NotNull(result.Data.Metrics.Month.Clicked);
            Assert.NotNull(result.Data.Metrics.Month.Failed);
            Assert.NotNull(result.Data.Metrics.Month.Rejected);
            Assert.NotNull(result.Data.Metrics.Month.Spammed);
            Assert.NotNull(result.Data.Metrics.Month.Created);
            Assert.NotNull(result.Data.Metrics.Month.Queued);

            Assert.Equal(100.0, result.Data.Metrics.Hour.Delivered.CurrentPercent);
            Assert.Equal(100.0, result.Data.Metrics.Hour.Delivered.PreviousPercent);
            Assert.Equal(0.0, result.Data.Metrics.Hour.Delivered.DiffPercent);
            Assert.Equal(1, result.Data.Metrics.Hour.Delivered.CurrentValue);
            Assert.Equal(1, result.Data.Metrics.Hour.Delivered.PreviousValue);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnUnauthorizedGivenUnknownApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("none_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetMetricsAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMetricsErrorCode.Unauthorized, result.Error.Value);

            Assert.Equal("unauthorized", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("Invalid or inactive project API key used.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ReturnLockedGivenLockedApiKey()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("locked_api_key");

            var client = builder.Build();

            // Act
            var result = await client.GetMetricsAsync();

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(GetMetricsErrorCode.Locked, result.Error.Value);

            Assert.Equal("locked", result.ResponseMeta.Status);
            Assert.Null(result.ResponseMeta.Uid);
            Assert.Equal("The specified API key is not valid.", result.ResponseMeta.Message);

            Assert.Null(result.Data);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ThrowInvalidOperationExceptionGivenConflictRequest()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("conflict_api_key");

            var client = builder.Build();

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => client.GetMetricsAsync());

            // Assert
            Assert.Equal("Invalid API version.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task ThrowInvalidOperationExceptionGivenCallError()
        {
            // Arrange
            var builder = new HttpPostageAppClientBuider()
                .WithBaseUri("http://test")
                .WithApiKey("call_error_api_key");

            var client = builder.Build();

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => client.GetMetricsAsync());

            // Assert
            Assert.Equal("The posted content is not valid JSON.", exception.Message);

            builder.MockHttp.VerifyNoOutstandingRequest();
        }
    }
}
