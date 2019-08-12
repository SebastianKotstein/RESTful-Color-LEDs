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
    /// This class represents a single group resource.
    /// A JSON representation is returned by calling <see cref="GroupController.GetGroup(SKotstein.Net.Http.Context.HttpContext, string)"/>.
    /// </summary>
    [DisplayName("Group")]
    [Description("Represents a group of LEDs")]
    public class Group
    {
        public const string SCHEMA_NAME = "group";

        private int _id;
        private string _name;

        /// <summary>
        /// Gets or sets the ID of the group
        /// </summary>
        [DisplayName("Group ID")]
        [Description("The ID of the group")]
        [JsonProperty("id")]
        public int Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the group
        /// </summary>
        [DisplayName("Group name")]
        [Description("The name of the group")]
        [JsonProperty("name")]
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to a sub resource containing all LEDs being member of this group
        /// </summary>
        [DisplayName("Group LEDs")]
        [Description("Hyperlink to the LEDs being member of this group")]
        [JsonProperty("leds")]
        public Hyperlink ToLeds
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id + "/leds");
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to this group resource itself
        /// </summary>
        [DisplayName("Self")]
        [Description("Hyperlink to this resource")]
        [JsonProperty("self")]
        public Hyperlink ToSelf
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id);
            }
        }

    
        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for changing the name of this group resource
        /// </summary>
        [DisplayName("Change Name")]
        [Description("Hyperlink to the LEDs being member of this group")]
        [JsonProperty("change_name")]
        public Hyperlink ChangeName
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id){Action="PUT"};
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for deleting this group resource
        /// </summary>
        [DisplayName("Remove Group")]
        [Description("Hyperlink for deleting this group")]
        [JsonProperty("remove_group")]
        public Hyperlink DeleteGroup
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups/" + _id) { Action = "DELETE" };
            }
        }

    }
}
