using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using Sitecore.XConnect.Segmentation.Predicates;
using SitecoreVDD.Extension.Foundation.Facets.Model;

namespace SitecoreVDD.Extension.Feature.CustomRules.Model.PersonalizationRules
{
    public class CustomerSegmentMatches<T> : WhenCondition<T> where T : RuleContext
    {
        public string SegmentValue { get; set; }
        public StringOperationType Comparison { get; set; }

        protected override bool Execute(T ruleContext)
        {
            var xConnectFacet = Sitecore.Analytics.Tracker.Current.Contact.GetFacet<Sitecore.Analytics.XConnect.Facets.IXConnectFacets>("XConnectFacets");
            var allFacets = xConnectFacet.Facets;
            Policy customerInfo = allFacets[Policy.DefaultFacetKey] as Policy;
            if (customerInfo.CustomerSegment == SegmentValue)
            {
                return true;
            }
            return false;
        }
    }
}