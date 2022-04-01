using Sitecore.XConnect;
using System;

namespace SitecoreVDD.Extension.Foundation.Facets.Model
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class Policy : Facet
    {
        public string PolicyNumber { get; set; }
        public DateTime? PolicyStartDate { get; set; }
        public DateTime? PolicyEndDate { get; set; }
        public string CustomerSegment { get; set; }
        public int? PolicyPremium { get; set; }
        public string CustomerStatus { get; set; }

        public const string DefaultFacetKey = "Policy";
    }
}