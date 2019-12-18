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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.persistent
{
    /// <summary>
    /// This implementation of <see cref="ILedDataSet"/> allows to store the data of a single LED into memory (i.e. not persistently).
    /// </summary>
    public class LedDataSetNonPersistent : ILedDataSet
    {
        private int _ledId;
        private int _controllerId;
        private string _rgb;

        public string Id
        {
            get
            {
                return ControllerId + ":" + LedNumber;
            }
        }

        public int LedNumber
        {
            get
            {
                return _ledId;
            }

            set
            {
                _ledId = value;
            }
        }

        public int ControllerId
        {
            get
            {
                return _controllerId;
            }

            set
            {
                _controllerId = value;
            }
        }

        public string RgbValue
        {
            get
            {
                return _rgb;
            }

            set
            {
                _rgb = value;
            }
        }

        public bool IsLedOfController(int controllerId)
        {
            return controllerId == _controllerId;
        }

        public bool HasId(string id)
        {
            return id.CompareTo(Id) == 0;
        }

        public bool HasId(int controllerId, int ledId)
        {
            return HasId(controllerId + ":" + ledId);
        }

     
    }
}
