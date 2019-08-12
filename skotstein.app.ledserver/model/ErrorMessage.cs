using Newtonsoft.Json;
using SKotstein.Net.Http.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.model
{
    /// <summary>
    /// Represents a error message where <see cref="StatusCode"/> contains the HTTP status code and <see cref="Message"/> gives further details about the error.
    /// An instance of <see cref="ErrorMessage"/> can be converted into an appropriate JSON structure (use <see cref="JsonSerializer"/>) and returned in the form of the payload of the HTTP response.
    /// </summary>
    [DisplayName("Error Message")]
    [Description("Represents an error message containing a status code and a description (message)")]
    public class ErrorMessage
    {
        public const string SCHEMA_NAME = "errormessage";

        private int _statusCode;
        private string _message;

        /// <summary>
        /// Gets or sets the HTTP status code (the three digit code) being associated with this error message
        /// </summary>
        [DisplayName("Code")]
        [Description("Status code of the error which has been occured")]
        [JsonProperty("code")]
        public int StatusCode
        {
            get
            {
                return _statusCode;
            }

            set
            {
                _statusCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the error message giving further details about the error
        /// </summary>
        [DisplayName("Message")]
        [Description("Further details about the error which has been occured")]
        [JsonProperty("message")]
        public string Message
        {
            get
            {
                return _message;
            }

            set
            {
                _message = value;
            }
        }

        /// <summary>
        /// Initializes an instance of <see cref="ErrorMessage"/>
        /// </summary>
        /// <param name="statusCode">the HTTP status code</param>
        /// <param name="message">error message for further details</param>
        public ErrorMessage(int statusCode, string message)
        {
            _statusCode = statusCode;
            _message = message;
        }

    }
}
