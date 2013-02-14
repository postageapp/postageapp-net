using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWG.PostageApp.Common;

namespace TWG.PostageApp.Converters
{
    /// <summary>
    /// Represents response JSON converter.
    /// </summary>
    public class PostageResponseConverter : JsonConverter
    {
        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter"/> to write to.</param><param name="value">The value.</param><param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new Exception("You don't nead to create a server responce on client.");
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader"/> to read from.</param><param name="objectType">Type of the object.</param><param name="existingValue">The existing value of object being read.</param><param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var responceJson = JObject.Load(reader);
            var uid = (string)responceJson["uid"];
            var message = (string)responceJson["message"];
            var statusCodeString = (string)responceJson["status"];
            var status = GetResponseStatusCode(statusCodeString);

            var response = new PostageResponse(uid, status, message);

/*            if (status != ResponseStatus.Ok)
            {
                throw new PostageResponseException<T>(response, null);
            }*/

            return response;
        }

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(PostageResponse))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parse response status string to code.
        /// </summary>
        /// <param name="statusCodeString"> Status code string. </param>
        /// <returns> Response status code. </returns>
        private static ResponseStatus GetResponseStatusCode(string statusCodeString)
        {
            ResponseStatus status;
            switch (statusCodeString)
            {
                case "ok":
                    {
                        status = ResponseStatus.Ok;
                        break;
                    }

                case "bad_request":
                    {
                        status = ResponseStatus.BadRequest;
                        break;
                    }

                case "not_found":
                    {
                        status = ResponseStatus.NotFound;
                        break;
                    }

                case "invalid_json":
                    {
                        status = ResponseStatus.InvalidJson;
                        break;
                    }

                case "unauthorized":
                    {
                        status = ResponseStatus.Unauthorized;
                        break;
                    }

                case "call_error":
                    {
                        status = ResponseStatus.CallError;
                        break;
                    }

                case "precondition_failed":
                    {
                        status = ResponseStatus.PreconditionFailed;
                        break;
                    }

                default:
                    {
                        status = ResponseStatus.Unknown;
                        break;
                    }
            }

            return status;
        }
    }
}