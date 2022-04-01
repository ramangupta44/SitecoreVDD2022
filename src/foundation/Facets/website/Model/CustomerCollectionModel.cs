using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreVDD.Extension.Foundation.Facets.Model
{
    public static class CustomerCollectionModel
    {
        public static XdbModel Model { get; } = CreateModel();

        private static XdbModel CreateModel()
        {
            XdbModelBuilder builder = new XdbModelBuilder("CustomFacets.Xconnect.CustomerCollectionModel", new XdbModelVersion(1, 0));
            builder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            builder.DefineFacet<Contact, Policy>(Policy.DefaultFacetKey);
            return builder.BuildModel();
        }
    }
}