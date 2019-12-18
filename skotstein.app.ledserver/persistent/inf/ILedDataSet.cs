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
    /// An implementation of <see cref="ILedDataSet"/> includes the data of a single LED. By using an implementation of <see cref="ILedStorage"/>, an instance of <see cref="ILedDataSet"/> can
    /// be stored persistently.
    /// </summary>
    public interface ILedDataSet
    {
        /// <summary>
        /// Returns the full ID of the LED which has the format CONTROLLER_ID:LED_NUMBER.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets or sets the number of the LED
        /// </summary>
        int LedNumber { get; set; }

        /// <summary>
        /// Gets or sets the ID of the <see cref="IControllerDataSet"/> this LED belongs to.
        /// </summary>
        int ControllerId { get; set; }

        /// <summary>
        /// Gets or sets the RGB value of this LED
        /// </summary>
        string RgbValue { get; set; }


        /// <summary>
        /// Returns true if this LED is handled by the <see cref="IControllerDataSet"/> having the passed ID, else false.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <returns>true or false</returns>
        bool IsLedOfController(int controllerId);

        /// <summary>
        /// Returns true if the LED has the passed ID, else false.
        /// </summary>
        /// <param name="id">ID of the LED having the format CONTROLLER_ID:LED_ID</param>
        /// <returns>true or false</returns>
        bool HasId(string id);

        /// <summary>
        /// Returns true if the LED belongs to the <see cref="IControllerDataSet"/> having the passed ID and if the LED has the passed number.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="number">number of the LED</param>
        /// <returns>true or false</returns>
        bool HasId(int controllerId, int number);

    }
}
