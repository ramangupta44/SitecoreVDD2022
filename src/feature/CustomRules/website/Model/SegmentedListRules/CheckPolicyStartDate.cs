using Sitecore.Framework.Rules;
using Sitecore.XConnect;
using Sitecore.XConnect.Segmentation.Predicates;
using SitecoreVDD.Extension.Foundation.Facets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SitecoreVDD.Extension.Feature.CustomRules.Model.SegmentedListRules
{
    public class CheckPolicyStartDate : IMappableRuleEntity, IContactSearchQueryFactory
    {
        public NumericOperationType Comparison { get; set; }
        public int Number { get; set; }
        public TimeUnit TimeUnit { get; set; }

        public Expression<Func<Contact, bool>> CreateContactSearchQuery(IContactSearchQueryContext context)
        {
            DateTime minDate = DateTime.UtcNow;
            if (this.Number < 0)
            {
                minDate = DateTime.UtcNow.SubtractTimeUnit(this.TimeUnit, Math.Abs(this.Number));
            }
            else
            {
                minDate = DateTime.UtcNow.AddTimeUnit(this.TimeUnit, this.Number);
            }
            var result = (Expression<Func<Contact, bool>>)(contact => this.Comparison.Evaluate(contact.GetFacet<Policy>(Policy.DefaultFacetKey).PolicyStartDate.Value, minDate));
            return result;
        }
    }
}