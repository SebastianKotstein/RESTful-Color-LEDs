// MIT License
//
// Copyright (c) 2019 Sebastian Kotstein
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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
        /// The HTTP status code (the three digit code) being associated with this error message
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
        /// The error message giving further details about the error
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
