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
    /// This interface defines methods for storing <see cref="IGroupDataSet"/> entities persistently.
    /// </summary>
    public interface IGroupStorage
    {
        /// <summary>
        /// Returns a list containing all <see cref="IGroupDataSet"/>s.
        /// </summary>
        /// <returns>list with all <see cref="IGroupDataSet"/>s</returns>
        IList<IGroupDataSet> GetAllGroups();

        /// <summary>
        /// Returns the <see cref="IGroupDataSet"/> having the passed ID or null, if there is no <see cref="IGroupDataSet"/> matching the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/></param>
        /// <returns>the <see cref="IGroupDataSet"/></returns>
        IGroupDataSet GetGroup(int id);

        /// <summary>
        /// Returns true, if this <see cref="IGroupStorage"/> has an <see cref="IGroupDataSet"/> matching the passed ID, else false. 
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/></param>
        /// <returns>true or false</returns>
        bool HasGroup(int id);

        /// <summary>
        /// Creates a new <see cref="IGroupDataSet"/> having the passed ID and name. The method throws an <see cref="Exception"/> if there is already an <see cref="IGroupDataSet"/>
        /// having the specified ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/> which should be created</param>
        /// <param name="name">name of the <see cref="IGroupDataSet"/> which should be created</param>
        void AddGroup(int id, string name);

        /// <summary>
        /// Deletes the <see cref="IGroupDataSet"/> having the passed ID. Nothing will happen if there is no <see cref="IGroupDataSet"/> having the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/> which should be deleted</param>
        void DeleteGroup(int id);

        /// <summary>
        /// Changes the name of the <see cref="IGroupDataSet"/> having the passed ID. The method throws an <see cref="Exception"/> if there is no <see cref="IGroupDataSet"/> matching the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/> whose name should be changed</param>
        /// <param name="name">the new name</param>
        void SetName(int id, string name);

        /// <summary>
        /// Adds an LED identified by the passed ID to the <see cref="IGroupDataSet"/> having the specified ID. The method throws an <see cref="Exception"/> of the LED having the passed ID is already member of the <see cref="IGroupDataSet"/>
        /// or if there is no <see cref="IGroupDataSet"/> matching the passed ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/></param>
        /// <param name="ledId">ID of the LED which should be added</param>
        void AddLed(int id, string ledId);

        /// <summary>
        /// Overwrittes the list of LEDs of the <see cref="IGroupDataSet"/> having the specified ID. The method throws an <see cref="Exception"/> if the <see cref="IGroupDataSet"/> does not exist.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/></param>
        /// <param name="ledIds">list with IDs of the LEDs which should overwritte the current LED member setup</param>
        void SetLeds(int id, IList<string> ledIds);

        /// <summary>
        /// Removes an LED identified by the passed ID from the <see cref="IGroupDataSet"/> having the specified ID. If the specified LED has removed before, nothing will happen. The method
        /// throws an <see cref="Exception"/> if the <see cref="IGroupDataSet"/> does not exist.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/></param>
        /// <param name="ledId">ID of the LED which should be removed</param>
        void RemoveLed(int id, string ledId);

        /// <summary>
        /// Removes all LEDs of the <see cref="IGroupDataSet"/> having the specified ID. The method throws an <see cref="Exception"/> if the <see cref="IGroupDataSet"/> does not exist.
        /// </summary>
        /// <param name="id">ID of the <see cref="IGroupDataSet"/></param>
        void ClearLeds(int id);

        /// <summary>
        /// Removes all LEDs of the <see cref="IControllerDataSet"/> having the passed ID until the specified offset (i.e. LEDs whose number is greater or equals than the offset) from the groups handled by this <see cref="IGroupStorage"/>.
        /// </summary>
        /// <param name="controllerId">ID of the <see cref="IGroupDataSet"/></param>
        /// <param name="offset">offset</param>
        void CleanLeds(int controllerId, int offset);
    }
}
