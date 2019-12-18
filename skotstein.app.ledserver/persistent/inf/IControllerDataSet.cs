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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// An implementation of <see cref="IControllerDataSet"/> includes the data of a single controller. By using an implementation of <see cref="IControllerStorage"/>, an instance of <see cref="IControllerDataSet"/> can be stored persistently.
    /// </summary>
    public interface IControllerDataSet
    {
        /// <summary>
        /// Gets or sets the identifer
        /// </summary>
       
        int Id
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the UUID
        /// </summary>
        string UuId
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the friendly name
        /// </summary>
        string FriendlyName
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the LED Count
        /// </summary>
        int LedCount
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the minor version of the installed firmware
        /// </summary>
        int MinorVersion
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the major version of the installed firmware
        /// </summary>
        int MajorVersion
        { 
            get;set;
        }

        /// <summary>
        /// Gets or sets the ID of the installed firmware
        /// </summary>
        string FirmwareId
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the device name
        /// </summary>
        string DeviceName
        {
            get;set;
        }

        /// <summary>
        /// Gets or sets the timestamp showing the time, when the state has been updated at last
        /// </summary>
        DateTime Timestamp
        {
            get;set;
        }
    }
}
