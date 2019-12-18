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
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.tools
{
    public class JsonSerializer
    {
        /// <summary>
        /// Converts a JSON structure into an object.
        /// Returns null, if a exception while converting occurs.
        /// </summary>
        /// <typeparam name="T">type of the class</typeparam>
        /// <param name="json">JSON structure</param>
        /// <returns>object</returns>
        public static T DeserializeJson<T>(string json) where T : class
        {
            try
            {
                //throws an exception if JSON is null!
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Converts an object into a JSON structure.
        /// Returns null, if a exception while converting occurs or the passed object is null.
        /// </summary>
        /// <param name="o">object</param>
        /// <returns>JSON structure</returns>
        public static string SerializeJson(object o)
        { 
            try
            {
                //returns null, if o is null
                return JsonConvert.SerializeObject(o);
            }
            catch (Exception e)
            {
                return null;
            }

           

        }

        
    }
}
