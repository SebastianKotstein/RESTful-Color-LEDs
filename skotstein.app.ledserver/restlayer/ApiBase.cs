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
using skotstein.app.ledserver.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.restlayer
{
    public class ApiBase
    {
        public const string API_V1 = "/api/v1";
       

        /// <summary>
        /// Converts the passed string into its integer representation. The method throws an <see cref="BadRequestException"/>
        /// having the message <see cref="BadRequestException.MSG_INVALID_ID"/> if the passed string cannot be converted.
        /// </summary>
        /// <param name="id">string which should be converted</param>
        /// <returns>integer representation</returns>
        public static int ParseId(string id)
        {
            int iid = 0;
            if (!Int32.TryParse(id, out iid))
            {
                throw new BadRequestException(BadRequestException.MSG_INVALID_ID);
            }
            return iid;
        }

        /// <summary>
        /// Converts the passed string into its integer representation. The method throws an <see cref="BadRequestException"/>
        /// having the passed error message if the passed string cannot be converted.
        /// </summary>
        /// <param name="id">string which should be converted</param>
        /// <param name="errorMsg">error message being encompassed in the thrown <see cref="BadRequestException"/></param>
        /// <returns>integer representation</returns>
        public static int ParseInt(string id, string errorMsg)
        {
            int iid = 0;
            if (!Int32.TryParse(id, out iid))
            {
                throw new BadRequestException(errorMsg);
            }
            return iid;
        }
    }




}
