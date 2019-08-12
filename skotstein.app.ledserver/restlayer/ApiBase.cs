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
