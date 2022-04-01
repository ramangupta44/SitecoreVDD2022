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
    public class CheckCustomerStatus : ICondition, IMappableRuleEntity, IContactSearchQueryFactory
    {
        public StringOperationType Comparison { get; set; }
        public string Customer_Status { get; set; }
        public Expression<Func<Contact, bool>> CreateContactSearchQuery(IContactSearchQueryContext context)
        {
            return (Expression<Func<Contact, bool>>)(contact => this.Comparison.Evaluate(contact.GetFacet<Policy>(Policy.DefaultFacetKey).CustomerStatus, this.Customer_Status));
        }

        public bool Evaluate(IRuleExecutionContext context)
        {
            var customerStatus = context.Fact<Contact>((string)null).GetFacet<Policy>(Policy.DefaultFacetKey).CustomerStatus;
            if (this.Comparison.Evaluate(customerStatus, this.Customer_Status))
            {
                return true;
            }
            else
            { return false; }
        }
    }
}