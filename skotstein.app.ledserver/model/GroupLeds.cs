using Newtonsoft.Json;
using skotstein.app.ledserver.restlayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skotstein.app.ledserver.model
{
    /// <summary>
    /// This class represents a sub resource of <see cref="Group"/> containing a list of all <see cref="Led"/>s being member of the group.
    /// A JSON representation is returned by calling <see cref="GroupController.SetLedsOfGroup(SKotstein.Net.Http.Context.HttpContext, string)"/>
    /// </summary>
    [DisplayName("Group LEDs")]
    [Description("Represents a collection of LEDs being member of a group")]
    public class GroupLeds
    {
        public const string SCHEMA_NAME = "groupleds";

        IList<Led> _leds = new List<Led>();
        private int _groupId;

        /// <summary>
        /// Gets or sets the list of <see cref="Led"/>s being member of the group 
        /// </summary>
        [DisplayName("Group LEDs")]
        [Description("Represents a collection of LEDs being member of a group")]
        [JsonProperty("leds")]
        public IList<Led> Leds
        {
            get
            {
                return _leds;
            }

            set
            {
                _leds = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the underlying <see cref="Group"/> resource
        /// </summary>
        [DisplayName("Group")]
        [Description("Hyperlink to the underlying group")]
        [JsonProperty("group")]
        public Hyperlink ToGroup
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId);
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self")]
        public Hyperlink ToSelf
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId + "/leds");
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for changing the membership of the group.
        /// </summary>
        [DisplayName("Create Controller")]
        [Description("Hyperlink for changing the membership of the group")]
        [JsonProperty("set_leds_of_group")]
        public Hyperlink SetLeds
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId + "/leds") { Action = "PUT" };
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for setting the color of all <see cref="Leds"/> being member of the group.
        /// </summary>
        [DisplayName("Set LED Color")]
        [Description("Hyperlink for setting the color of all LEDs being member of the group")]
        [JsonProperty("set_color_of_group_leds")]
        public Hyperlink SetLedColor
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _groupId + "/leds") { Action = "POST" };
            }
        }


        /// <summary>
        /// Gets or sets the ID of the underlying <see cref="Group"/>
        /// </summary>
        [JsonIgnore]
        public int GroupId
        {
            get
            {
                return _groupId;
            }

            set
            {
                _groupId = value;
            }
        }
    }
}
