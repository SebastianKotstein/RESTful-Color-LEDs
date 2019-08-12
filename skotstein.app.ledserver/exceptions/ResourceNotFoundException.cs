using skotstein.app.ledserver.model;
using skotstein.app.ledserver.tools;
using SKotstein.Net.Http.Context;
using SKotstein.Net.Http.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.exceptions
{

    /// <summary>
    /// The exception that is thrown when a requested resource cannot be found. A thrown <see cref="ResourceNotFoundException"/> is automatically catched by the underlying
    /// RESTful.NET Framework and converted into an appropriate HTTP Response (i.e. there is no need to catch this exception before sending an HTTP Response). Use the constructor
    /// <see cref="ResourceNotFoundException.ResourceNotFoundException(string)"/> to specify the error message which is converted into a JSON structure (see <see cref="ErrorMessage"/>) and embedded into the payload
    /// of the HTTP response.
    /// </summary>
    public class ResourceNotFoundException : HttpRequestException
    {
        public const string MSG_CONTROLLER_NOT_FOUND = "The controller having the 'ID':'{VALUE}' does not exist";
        public const string MSG_GROUP_NOT_FOUND = "The group having the 'ID':'{VALUE}' does not exist";
        public const string MSG_LED_NOT_FOUND = "The LED having the 'ID':'{VALUE}' does not exist";
        public const string MSG_FIRMWARE_NOT_FOUND = "The firmware having the 'ID':'{VALUE}' does not exist";
        public const string MSG_SCHEMA_NOT_FOUND = "The requested schema is unknown";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException"/>.
        /// </summary>
        /// <param name="message">the error message</param>
        public ResourceNotFoundException(string message)
        {
            this.ErrorMessage = JsonSerializer.SerializeJson(new ErrorMessage(404, message));
            this.ContentType = MimeType.APPLICATION_JSON;
            this.Status = HttpStatus.NotFound;
            
        }
    }
}
