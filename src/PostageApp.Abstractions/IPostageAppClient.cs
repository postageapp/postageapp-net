using System;
using System.Threading;
using System.Threading.Tasks;
using PostageApp.Abstractions.GetMessages;

namespace PostageApp.Abstractions
{
    public interface IPostageAppClient
    {
        /// <summary>
        /// Provides information about the account.
        /// </summary>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetAccountInfoResult> GetAccountInfoAsync(string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Provides information about the project.
        /// </summary>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetProjectInfoResult> GetProjectInfoAsync(string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// If you need to get a list of all message UIDs within your project,
        /// for subsequent use in collection statistics or open rates for example, you can supply the following data:
        /// </summary>
        /// <param name="page">Page</param>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMessagesResult> GetMessagesAsync(int page = 1, string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns status information for all messages for a given project.
        /// </summary>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMessagesHistoryResult> GetMessagesHistoryAsync(string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns detailed status information for all transmissions for a given project.
        /// </summary>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMessagesHistoryDetailedResult> GetMessagesHistoryDetailedAsync(DateTime? from = null, DateTime? to = null, string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// To get data on aggregate delivery and open status for a project,
        /// broken down by current hour, current day, current week, current month with the previous of each as a comparable.
        /// </summary>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMetricsResult> GetMetricsAsync(string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns a list of all recipients in a suppressed state.
        /// This includes email addreses which are not receiving email because they are having temporary problems,
        /// such as mailbox full, mail server not responding,as well as those that are more permanent in nature,
        /// such as account deleted, or spam reports from the recipient.
        /// </summary>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetSuppressionListResult> GetSuppressionListAsync(string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends message.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="uid">Uid of the message, null for PostageApp server generation</param>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<SendMessageResult> SendMessageAsync(Message message, string uid = null, string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// If you need to confirm that message with a particular
        /// UID exists within your project just send a request with the following:
        /// </summary>
        /// <param name="uid">Uid of the message</param>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMessageReceiptResult> GetMessageReceiptAsync(string uid, string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// To get data on individual recipients' delivery and open status,
        /// you can pass a particular message UID and receive a JSON encoded set of data for each recipient within that message.
        /// </summary>
        /// <param name="uid">Uid of the message</param>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMessageTransmissionsResult> GetMessageTransmissionsAsync(string uid, string apiKey = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns status information for a single message based on UID.
        /// </summary>
        /// <param name="uid">Uid of the message</param>
        /// <param name="apiKey">Project Api Key, null for default</param>
        /// <returns></returns>
        Task<GetMessageDeliveryStatusResult> GetMessageDeliveryStatusAsync(string uid, string apiKey = null, CancellationToken cancellationToken = default);
    }
}
