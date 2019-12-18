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
    public class MemoryBasedLedStorage : ILedStorage
    {
        private IList<ILedDataSet> _leds;

        /// <summary>
        /// Initializes the storage.
        /// </summary>
        public void Initialize()
        {
            _leds = new List<ILedDataSet>();
        }

        public IList<ILedDataSet> GetAllLeds()
        {
            IList<ILedDataSet> copy = new List<ILedDataSet>();
            foreach (ILedDataSet lds in _leds)
            {
                copy.Add(lds);
            }
            return copy;
        }

        public IList<ILedDataSet> GetAllLedsOfController(int controllerId)
        {
            IList<ILedDataSet> copy = new List<ILedDataSet>();
            foreach (ILedDataSet lds in _leds)
            {
                if (lds.IsLedOfController(controllerId))
                {
                    copy.Add(lds);
                }
            }
            return copy;
        }



        public ILedDataSet GetLed(int controllerId, int number)
        {
            return GetLed(controllerId + ":" + number);
        }


        public ILedDataSet GetLed(string id)
        {
            foreach (LedDataSetNonPersistent lds in _leds)
            {
                if (lds.HasId(id))
                {
                    return lds;
                }
            }
            return null;
        }

        public bool HasLed(int controllerId, int number)
        {
            return GetLed(controllerId, number) != null;
        }

        public bool HasLed(string id)
        {
            return GetLed(id) != null;
        }

        public void AddLed(int controllerId, int number)
        {
            if (HasLed(controllerId, number))
            {
                throw new Exception("LED does already exist");
            }
            else
            {
                LedDataSetNonPersistent lds = new LedDataSetNonPersistent();
                lds.ControllerId = controllerId;
                lds.LedNumber = number;
                lds.RgbValue = "#000000"; //off

                _leds.Add(lds);
            }

        }

        public void DeleteLed(string id)
        {
            for (int i = 0; i < _leds.Count; i++)
            {
                if (_leds[i].HasId(id))
                {
                    _leds.RemoveAt(i);
                }
            }
        }

        public void DeleteLed(int controllerId, int number)
        {
            DeleteLed(controllerId + ":" + number);
        }

        public void CleanLeds(int controllerId, int offset)
        {
            IList<ILedDataSet> toBeRemoved = new List<ILedDataSet>();
            for (int i = 0; i < _leds.Count; i++)
            {
                if (_leds[i].IsLedOfController(controllerId) && _leds[i].LedNumber >= offset)
                {
                    toBeRemoved.Add(_leds[i]);
                }
            }

            //remove LEDs
            foreach (LedDataSetNonPersistent lds in toBeRemoved)
            {
                _leds.Remove(lds);
            }
        }

        public void SetColor(int controllerId, int number, string rgb)
        {
            SetColor(controllerId + ":" + number, rgb);
        }

        public void SetColor(string id, string rgb)
        {
            ILedDataSet lds = GetLed(id);
            if (lds == null)
            {
                throw new Exception("Unknown LED");
            }
            else
            {
                lds.RgbValue = rgb;
            }
        }

    }
}
