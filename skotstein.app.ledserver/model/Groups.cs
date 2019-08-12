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
    /// This class represents a list resource containing a set of <see cref="Group"/>s.
    /// A JSON representation is returned by calling <see cref="GroupController.GetGroups(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Groups")]
    [Description("Represents a collection of groups")]
    public class Groups
    {
        public const string SCHEMA_NAME = "groups";

        private IList<Group> _groups = new List<Group>();

        /// <summary>
        /// Gets or sets the list of <see cref="Group"/>s
        /// </summary>
        [DisplayName("Groups")]
        [Description("Represents a collection of groups")]
        [JsonProperty("groups")]
        public IList<Group> GroupList
        {
            get
            {
                return _groups;
            }

            set
            {
                _groups = value;
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
                return new Hyperlink(ApiBase.API_V1 + "/groups");
            }
        }

        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for creating a new group resource.
        /// </summary>
        [DisplayName("Create Controller")]
        [Description("Hyperlink for creating a new group")]
        [JsonProperty("create_new_group")]
        public Hyperlink CreateGroup
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/groups") { Action = "POST"};
            }
        }
    }
}
