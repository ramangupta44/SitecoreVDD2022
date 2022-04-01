using Sitecore.Modules.EmailCampaign.Core.Personalization;
using SitecoreVDD.Extension.Foundation.Facets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace SitecoreVDD.Extension.Feature.CustomTokens.Model
{
    class CustomerPropertyTokenMap : DefaultRecipientPropertyTokenMap
    {
        protected static readonly MethodInfo GetPolicyNumberFacetValue = typeof(FacetExtensions).GetMethod(nameof(FacetExtensions.GetPolicyNumberFacetValueExt), new[] { typeof(Policy) });
        protected static readonly MethodInfo GetPolicyStartDateFacetValue = typeof(FacetExtensions).GetMethod(nameof(FacetExtensions.GetPolicyStartDateFacetValueExt), new[] { typeof(Policy) });
        protected static readonly MethodInfo GetPolicyPremiumFacetValue = typeof(FacetExtensions).GetMethod(nameof(FacetExtensions.GetPolicyPremiumFacetValueExt), new[] { typeof(Policy) });

        static CustomerPropertyTokenMap()
        {
            if (TokenBindings == null)
            {
                TokenBindings = new Dictionary<Token, RecipientPropertyTokenBinding>();
            }
            RecipientPropertyTokenBinding PolicyNumberTokenBinding = RecipientPropertyTokenBinding.Build<Policy>(new Token("PolicyNumber"), null, GetPolicyNumberFacetValue);
            RecipientPropertyTokenBinding PolicyStartDateTokenBinding = RecipientPropertyTokenBinding.Build<Policy>(new Token("PolicyStartDate"), null, GetPolicyStartDateFacetValue);
            RecipientPropertyTokenBinding PolicyPremiumTokenBinding = RecipientPropertyTokenBinding.Build<Policy>(new Token("PolicyPremium"), null, GetPolicyPremiumFacetValue);
            TokenBindings.Add(PolicyNumberTokenBinding.Token, PolicyNumberTokenBinding);
            TokenBindings.Add(PolicyStartDateTokenBinding.Token, PolicyStartDateTokenBinding);
            TokenBindings.Add(PolicyPremiumTokenBinding.Token, PolicyPremiumTokenBinding);
        }
    }

    public static class FacetExtensions
    {
        public static string GetPolicyNumberFacetValueExt(this Policy facet)
        {
            if (facet != null)
            {
                return facet.PolicyNumber.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetPolicyStartDateFacetValueExt(this Policy facet)
        {
            if (facet != null)
            {
                return facet.PolicyStartDate.Value.ToString("dd-MM-yyyy");
            }
            else
            {
                return string.Empty;
            }
        }
        public static string GetPolicyPremiumFacetValueExt(this Policy facet)
        {
            if (facet != null)
            {
                return facet.PolicyPremium.Value.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}