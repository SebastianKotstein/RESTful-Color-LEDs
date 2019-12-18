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
    public interface ILedStorage
    {
        /// <summary>
        /// Returns a list containing all <see cref="ILedDataSet"/>s.
        /// </summary>
        /// <returns>list with all <see cref="ILedDataSet"/>s</returns>
        IList<ILedDataSet> GetAllLeds();

        /// <summary>
        /// Returns a list containing all <see cref="ILedDataSet"/>s belonging to the <see cref="IControllerDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <returns>list with all <see cref="ILedDataSet"/>s belonging to the <see cref="IControllerDataSet"/> having the passed ID</returns>
        IList<ILedDataSet> GetAllLedsOfController(int controllerId);

        /// <summary>
        /// Returns the <see cref="ILedDataSet"/> matching the passed tuple of <see cref="IControllerDataSet.Id"/> and <see cref="ILedDataSet.LedNumber"/> or null, if there is no <see cref="ILedDataSet"/>.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="number">number of the LED</param>
        /// <returns>the <see cref="ILedDataSet"/> or null</returns>
        ILedDataSet GetLed(int controllerId, int number);

        /// <summary>
        /// Returns the <see cref="ILedDataSet"/> matching the passed ID or null, if there is not <see cref="ILedDataSet"/> matching the ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="ILedDataSet"/></param>
        /// <returns>the <see cref="ILedDataSet"/> or null</returns>
        ILedDataSet GetLed(string id);

        /// <summary>
        /// Returns true, if this <see cref="ILedStorage"/> has an <see cref="ILedDataSet"/> matching the passed tuple of <see cref="IControllerDataSet.Id"/> and <see cref="ILedDataSet.LedNumber"/>, else false.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="number">number of the LED</param>
        /// <returns>true or false</returns>
        bool HasLed(int controllerId, int number);

        /// <summary>
        /// Returns true, if this <see cref="ILedStorage"/> has an <see cref="ILedDataSet"/> matching the passed ID, else false.
        /// </summary>
        /// <param name="id">ID of the <see cref="ILedDataSet"/>"/></param>
        /// <returns>true or false</returns>
        bool HasLed(string id);

        /// <summary>
        /// Registers a new <see cref="ILedDataSet"/> beloging to the <see cref="IControllerDataSet"/> having the passed ID and having the passed position (number).
        /// The new <see cref="ILedDataSet"/> is initialized with the default RGB value #000000 (off). This method throws an <see cref="Exception"/> if there is already an <see cref="ILedDataSet"/>
        /// having the specified tuple consisting of <see cref="IControllerDataSet.Id"/> and <see cref="ILedDataSet.LedNumber"/>.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="ledId">number of the LED</param>
        void AddLed(int controllerId, int ledId);

        /// <summary>
        /// Deletes the <see cref="ILedDataSet"/> having the passed ID. Nothing will happen if such an <see cref="ILedDataSet"/> does not exist.
        /// </summary>
        /// <param name="id">ID of the <see cref="ILedDataSet"/></param>
        void DeleteLed(string id);

        /// <summary>
        /// Deletes the <see cref="ILedDataSet"/> belonging to the <see cref="IControllerDataSet"/> having the passed ID and being at the specified position (number). Nothing will happen if such an <see cref="ILedDataSet"/> does not exist.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="ledId">number of the LED</param>
        void DeleteLed(int controllerId, int ledId);

        /// <summary>
        /// Removes all LEDs of the <see cref="IControllerDataSet"/> having the specified ID until the specified offset (i.e. LEDs whose number is greated than or equals the offset)
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="offset"></param>
        void CleanLeds(int controllerId, int offset);

        /// <summary>
        /// Sets the color of the <see cref="ILedDataSet"/> belonging to the <see cref="IControllerDataSet"/> having the passed ID and being at the specified position (number). The method throws an
        /// <see cref="Exception"/> if such an <see cref="ILedDataSet"/> does not exist. The passed RGB value has the format #RRGGBB where each digit represents a hexadecimal value (e.g. #FF0000 for color red).
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IControllerDataSet"/></param>
        /// <param name="number">number of the LED</param>
        /// <param name="rgb">RGB value having the format #RRGGBB</param>
        void SetColor(int controllerId, int number, string rgb);

        /// <summary>
        /// Sets the color of the <see cref="ILedDataSet"/> having the passed ID. The method throws an <see cref="Exception"/> if such an <see cref="ILedDataSet"/> does not exist. 
        /// The passed RGB value has the format #RRGGBB where each digit represents a hexadecimal value (e.g. #FF0000 for color red).
        /// </summary>
        /// <param name="id">ID of the <see cref="ILedDataSet"/></param>
        /// <param name="rgb">RGB value having the format #RRGGBB</param>
        void SetColor(string id, string rgb);
    }
}
