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
    /// This class represents a list resource containing a set of <see cref="Controller"/>. 
    /// A JSON representation is returned by calling <see cref="ControllerController.GetControllers(SKotstein.Net.Http.Context.HttpContext)"/>.
    /// </summary>
    [DisplayName("Controllers")]
    [Description("Represents a collection of controllers")]
    public class Controllers
    {
        public const string SCHEMA_NAME = "controllers";

        private IList<Controller> _controllers = new List<Controller>();

        /// <summary>
        /// Gets or sets the list of <see cref="Controller"/>s
        /// </summary>
        [DisplayName("Controllers")]
        [Description("Represents a collection of controllers")]
        [JsonProperty("controllers")]
        public IList<Controller> ControllerList
        {
            get
            {
                return _controllers;
            }

            set
            {
                _controllers = value;
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
                return new Hyperlink(ApiBase.API_V1 + "/controllers");
            }
        }


        /// <summary>
        /// Gets a <see cref="Hyperlink"/> object pointing to the action for creating a new controller resource.
        /// </summary>
        [DisplayName("Create Controller")]
        [Description("Hyperlink for creating a new controller")]
        [JsonProperty("create_new_controller")]
        public Hyperlink ActionCreate
        {
            get
            {
                return new Hyperlink(ApiBase.API_V1 + "/controllers") { Action = "POST"};
            }
        }

        /*
        [DisplayName("Meta Information")]
        [Description("Provides meta information about the document representation such as a link to its schema")]
        [JsonProperty("_meta")]
        public MetaInformation Meta
        {
            get
            {
                MetaInformation meta = new MetaInformation();
                meta.Schema = new Hyperlink(ApiBase.API_DOC + "/schemas/controllers") { Description = "Schema of this document" };
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_DOCUMENTATION + ":curies", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_DOCUMENTATION + "/curies"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_CONTROLLER + ":create", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_CONTROLLER + "/create"));
                meta.Curies.Add(new CompactUri(ApiBase.RELATION_NS_DOCUMENTATION + ":schema", ApiBase.API_DOC + "/rel/" + ApiBase.RELATION_NS_DOCUMENTATION + "/schema"));
                return meta;
            }
        }
        */
    }
}
