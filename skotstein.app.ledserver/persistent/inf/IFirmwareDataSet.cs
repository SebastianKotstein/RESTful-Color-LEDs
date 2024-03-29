﻿// MIT License
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
    /// An implementation of <see cref="IFirmwareDataSet"/> includes the meta data of a firmware file. By using an implementation of <see cref="IFirmwareStorage"/>, an instance of <see cref="IFirmwareDataSet"/> can be stored persistently.
    /// </summary>
    public interface IFirmwareDataSet
    {
        /// <summary>
        /// Gets or sets the identifier of the firmware.
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the minor version of the firmware.
        /// </summary>
        int MinorVersion { get; set; }

        /// <summary>
        /// Gets or sets the major version of the firmware.
        /// </summary>
        int MajorVersion { get; set; }

        /// <summary>
        /// Gets or sets the name of the device which compatible to this firmware.
        /// </summary>
        string DeviceName { get; set; }
        
        /// <summary>
        /// Gets or sets the file extension of the firmware file.
        /// </summary>
        string FirmwareFileExtension { get; set; }
    
    }
}
