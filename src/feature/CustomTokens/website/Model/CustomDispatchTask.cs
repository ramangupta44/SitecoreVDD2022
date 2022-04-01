using Sitecore.EmailCampaign.Cm.Dispatch;
using Sitecore.EmailCampaign.Cm.Managers;
using Sitecore.EmailCampaign.Model.XConnect.Facets;
using Sitecore.ExM.Framework.Distributed.Tasks.TaskPools.ShortRunning;
using Sitecore.Modules.EmailCampaign.Core;
using Sitecore.Modules.EmailCampaign.Core.Contacts;
using Sitecore.Modules.EmailCampaign.Core.Data;
using Sitecore.Modules.EmailCampaign.Core.Dispatch;
using Sitecore.Modules.EmailCampaign.Core.Pipelines.HandleMessageEventBase;
using Sitecore.Modules.EmailCampaign.Factories;
using Sitecore.XConnect;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Operations;
using System.Collections.Generic;
using System.Linq;
using SitecoreVDD.Extension.Foundation.Facets.Model;

namespace SitecoreVDD.Extension.Feature.CustomTokens.Model
{
    public class CustomDispatchTask : DispatchTask
    {
        private IContactService _contactService;
        public CustomDispatchTask(ShortRunningTaskPool taskPool, IRecipientValidator recipientValidator, IContactService contactService, EcmDataProvider dataProvider, 
            ItemUtilExt itemUtil, IEventDataService eventDataService, IDispatchManager dispatchManager, EmailAddressHistoryManager emailAddressHistoryManager, 
            IRecipientManagerFactory recipientManagerFactory, SentMessageManager sentMessageManager)
        : base(taskPool, recipientValidator, contactService, dataProvider, itemUtil, eventDataService, dispatchManager, emailAddressHistoryManager, 
              recipientManagerFactory, sentMessageManager)
        {
            _contactService = contactService;
        }

        protected override IReadOnlyCollection<IEntityLookupResult<Contact>> GetContacts(List<DispatchQueueItem> dispatchQueueItems)
        {
            return _contactService.GetContacts(dispatchQueueItems.Select(x => x.ContactIdentifier), PersonalInformation.DefaultFacetKey, 
                EmailAddressList.DefaultFacetKey, ConsentInformation.DefaultFacetKey, PhoneNumberList.DefaultFacetKey, ListSubscriptions.DefaultFacetKey, 
                EmailAddressHistory.DefaultFacetKey, ExmKeyBehaviorCache.DefaultFacetKey, Policy.DefaultFacetKey);
        }
    }
}