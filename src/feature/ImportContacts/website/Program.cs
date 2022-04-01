using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using SitecoreVDD.Extension.Foundation.Facets.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("");
            Console.WriteLine("END OF PROGRAM.");
            Console.ReadKey();
        }
        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Which environment do you wish to connect to? 1=LocalDev, 2=Other");
            Console.WriteLine();
            Console.WriteLine("1 = LocalDev");
            Console.WriteLine("2 = Other");
            var environment = Console.ReadLine();

            string xConnectUrlBase;
            string thumbPrint;

            #region configurehttp
            switch (environment)
            {
                case "1":
                    {
                        Console.WriteLine("Configuring LocalDev connection");
                        xConnectUrlBase = "https://VDD.xconnect";
                        thumbPrint = "372ea36497642a5388f1f763afefca5e6ba3bd65";
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("Other");
                        xConnectUrlBase = "[your xConnect URL here]";
                        thumbPrint = "[your thumbprint here]";
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Nothing to see here");
                        xConnectUrlBase = "???";
                        thumbPrint = "???";
                        return;
                    }
            }

            CertificateHttpClientHandlerModifierOptions options = CertificateHttpClientHandlerModifierOptions.Parse($"StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue={thumbPrint}");

            var certificateModifier = new CertificateHttpClientHandlerModifier(options);

            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            // Ensure configuration pointing to appropriate instance of xconnect
            var collectionClient = new CollectionWebApiClient(new Uri($"{xConnectUrlBase}/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri($"{xConnectUrlBase}/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri($"{xConnectUrlBase}/configuration"), clientModifiers, new[] { certificateModifier });

            // For exisitng facets
            //XdbModel[] models = { CollectionModel.Model };
            // For custom Facets
            XdbModel[] models = { CollectionModel.Model, CustomerCollectionModel.Model };

            //var cfg = new XConnectClientConfiguration(new XdbRuntimeModel(models), collectionClient, searchClient, configurationClient);
            var cfg = new XConnectClientConfiguration(new XdbRuntimeModel(models), collectionClient, searchClient, configurationClient, true);
            try
            {
                await cfg.InitializeAsync();
            }
            catch (XdbModelConflictException ce)
            {
                Console.WriteLine("ERROR:" + ce.Message);
                return;
            }
            #endregion            

            Console.WriteLine("What action are you performing?");
            Console.WriteLine("1=Update Contact Facet");
            Console.WriteLine("2=Create contact with values");
            var action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    {
                        Console.WriteLine("Enter an xDB contact ID. You can keep this empty to update facets based on email.");
                        var contactId = Console.ReadLine();
                        Console.WriteLine("First name?");
                        var firstName = Console.ReadLine();
                        Console.WriteLine("Last name?");
                        var lastName = Console.ReadLine();
                        Console.WriteLine("Email Address?");
                        var email = Console.ReadLine();
                        Console.WriteLine("Country code?");
                        var countryCode = Console.ReadLine();
                        Console.WriteLine("Mobile number?");
                        var mobileNumber = Console.ReadLine();
                        Console.WriteLine("Policy Number?");
                        var policyNumber = Console.ReadLine();
                        Console.WriteLine("Policy start date?");
                        var policystartdate = Console.ReadLine();
                        DateTime policyStartDate = DateTime.Parse(policystartdate);
                        Console.WriteLine("Policy end date?");
                        var policyenddate = Console.ReadLine();
                        DateTime policyEndDate = DateTime.Parse(policyenddate);
                        Console.WriteLine("Customer Segment?");
                        var customerSegment = Console.ReadLine();
                        Console.WriteLine("Policy Premium?");
                        var policypremium = Console.ReadLine();
                        int policyPremium = int.Parse(policypremium);
                        Console.WriteLine("Customer status?");
                        var customerStatus = Console.ReadLine();
                        await UpdateContactFacet(cfg, contactId, firstName, lastName, email, countryCode, mobileNumber, policyNumber, policyStartDate, policyEndDate,
                            customerSegment, policyPremium, customerStatus);
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine("First name?");
                        var firstName = Console.ReadLine();
                        Console.WriteLine("Last name?");
                        var lastName = Console.ReadLine();
                        Console.WriteLine("Email Address?");
                        var email = Console.ReadLine();
                        Console.WriteLine("Country code?");
                        var countryCode = Console.ReadLine();
                        Console.WriteLine("Mobile number?");
                        var mobileNumber = Console.ReadLine();
                        Console.WriteLine("Policy Number?");
                        var policyNumber = Console.ReadLine();
                        Console.WriteLine("Policy start date?");
                        var policystartdate = Console.ReadLine();
                        DateTime policyStartDate = DateTime.Parse(policystartdate);
                        Console.WriteLine("Policy end date?");
                        var policyenddate = Console.ReadLine();
                        DateTime policyEndDate= DateTime.Parse(policyenddate);
                        Console.WriteLine("Customer Segment?");
                        var customerSegment = Console.ReadLine();
                        Console.WriteLine("Policy Premium?");
                        var policypremium = Console.ReadLine();
                        int policyPremium = int.Parse(policypremium);
                        Console.WriteLine("Customer status?");
                        var customerStatus = Console.ReadLine();
                        await CreateContacts(cfg, firstName, lastName, email, countryCode, mobileNumber, policyNumber, policyStartDate, policyEndDate,
                            customerSegment, policyPremium, customerStatus);
                        break;
                    }
                default:
                    {
                        return;
                    }
            }
            Console.WriteLine();
            Console.ReadLine();
        }

        private static async Task CreateContacts(XConnectClientConfiguration xConfig, string firstName, string lastName, string email, string countryCode, string mobileNumber, string policyNumber, DateTime PolicyStartDate, DateTime policyEndDate, string customerSegment, int policyPremium, string customerStatus)
        {
            DateTime DateOfBirth = new DateTime(1947, 12, 9);
            // Initialize a client using the validated configuration
            using (var client = new XConnectClient(xConfig))
            {
                try
                {
                    IdentifiedContactReference reference = new IdentifiedContactReference("CustomerRef", email);
                    Contact existingContact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] {
                                PersonalInformation.DefaultFacetKey,
                                EmailAddressList.DefaultFacetKey,
                                PhoneNumberList.DefaultFacetKey,
                                Policy.DefaultFacetKey
                            }));

                    if (existingContact != null)
                    {
                        Console.WriteLine("This contact already exists.Please run the update option.");
                    }
                    else
                    {
                        var identifiers = new ContactIdentifier[]
                        {
                            new ContactIdentifier("CustomerRef", $"{email}",  ContactIdentifierType.Known)
                        };

                        // Create a new contact with the identifier
                        Contact newContact = new Contact(identifiers);

                        PersonalInformation personalInfoFacet = new PersonalInformation();
                        personalInfoFacet.FirstName = firstName;
                        personalInfoFacet.LastName = lastName;
                        personalInfoFacet.Birthdate = DateOfBirth;

                        client.SetFacet<PersonalInformation>(newContact, PersonalInformation.DefaultFacetKey, personalInfoFacet);

                        EmailAddressList newEmailFacet = new EmailAddressList(new EmailAddress(email, true), "Work");
                        client.SetFacet<EmailAddressList>(newContact, newEmailFacet);

                        PhoneNumberList newPhoneFacet = new PhoneNumberList(new PhoneNumber(countryCode, mobileNumber), "Mobile");
                        client.SetFacet<PhoneNumberList>(newContact, newPhoneFacet);

                        Policy policyFacets = new Policy();
                        policyFacets.PolicyNumber = policyNumber;
                        policyFacets.PolicyStartDate = PolicyStartDate;
                        policyFacets.PolicyEndDate= policyEndDate;
                        policyFacets.CustomerSegment = customerSegment;
                        policyFacets.PolicyPremium = policyPremium;
                        policyFacets.CustomerStatus = customerStatus;
                        client.SetFacet<Policy>(newContact, Policy.DefaultFacetKey, policyFacets);

                        client.AddContact(newContact);

                        // Submit contact
                        await client.SubmitAsync();

                        // Get the last batch that was executed
                        var operations = client.LastBatch;

                        Console.WriteLine("RESULTS...");

                        //// Loop through operations and check status
                        foreach (var operation in operations)
                        {
                            Console.WriteLine(operation.OperationType + operation.Target.GetType().ToString() + " Operation: " + operation.Status);
                        }
                    }


                }
                catch (XdbExecutionException ex)
                {
                    // Deal with exception
                }
            }
        }

        private static async Task UpdateContactFacet(XConnectClientConfiguration xConfig, string contactId, string firstName, string lastName, string email, string countryCode, string mobileNumber, string policyNumber, DateTime PolicyStartDate, DateTime policyEndDate, string customerSegment, int policyPremium, string customerStatus)
        {
            // Initialize a client using the validated configuration
            using (var client = new XConnectClient(xConfig))
            {
                try
                {
                    Contact contact;
                    //IdentifiedContactReference reference;
                    //ContactReference reference;
                    if (!string.IsNullOrEmpty(contactId))
                    {
                        var reference = new Sitecore.XConnect.ContactReference(Guid.Parse(contactId));
                        contact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] {
                                PersonalInformation.DefaultFacetKey,
                                EmailAddressList.DefaultFacetKey,
                                PhoneNumberList.DefaultFacetKey,
                                Policy.DefaultFacetKey
                            }));
                    }
                    else
                    {
                        IdentifiedContactReference reference = new IdentifiedContactReference("CustomerRef", email);
                        contact = client.Get<Contact>(reference, new ContactExpandOptions(new string[] {
                                PersonalInformation.DefaultFacetKey,
                                EmailAddressList.DefaultFacetKey,
                                PhoneNumberList.DefaultFacetKey,
                                Policy.DefaultFacetKey
                            }));
                    }

                    if (contact != null)
                    {
                        var policyFacet = contact.GetFacet<Policy>(Policy.DefaultFacetKey);

                        if (policyFacet != null)
                        {
                            // Change facet properties
                            policyFacet.PolicyNumber = policyNumber;
                            policyFacet.PolicyEndDate = policyEndDate;
                            policyFacet.PolicyStartDate= PolicyStartDate;
                            policyFacet.CustomerSegment = customerSegment;
                            policyFacet.PolicyPremium = policyPremium;
                            policyFacet.CustomerStatus = customerStatus;
                            //Update facet in contact
                            client.SetFacet(contact, policyFacet);
                        }
                        else
                        {
                            // Facet is new
                            Policy newCustomerFacet = new Policy()
                            {
                                PolicyNumber = policyNumber,
                                PolicyStartDate=PolicyStartDate,
                                PolicyEndDate = policyEndDate,
                                CustomerSegment = customerSegment,
                                PolicyPremium = policyPremium,
                                CustomerStatus = customerStatus
                            };
                            client.SetFacet(contact, newCustomerFacet);
                        }

                        PersonalInformation existingContactPersonalFacet = contact.GetFacet<PersonalInformation>(PersonalInformation.DefaultFacetKey);
                        if (existingContactPersonalFacet != null)
                        {
                            existingContactPersonalFacet.FirstName = firstName;
                            existingContactPersonalFacet.LastName = lastName;
                            client.SetFacet(contact, PersonalInformation.DefaultFacetKey, existingContactPersonalFacet);
                        }
                        else
                        {
                            PersonalInformation newPersonalFacet = new PersonalInformation()
                            {
                                FirstName = firstName,
                                LastName = lastName
                            };
                            client.SetFacet(contact, PersonalInformation.DefaultFacetKey, newPersonalFacet);
                        }

                        PhoneNumberList existingContactPhonFacet = contact.GetFacet<PhoneNumberList>(PhoneNumberList.DefaultFacetKey);
                        if (existingContactPhonFacet != null)
                        {
                            existingContactPhonFacet.PreferredPhoneNumber = new PhoneNumber(countryCode, mobileNumber);
                            existingContactPhonFacet.PreferredKey = "Mobile";
                            client.SetFacet(contact, PhoneNumberList.DefaultFacetKey, existingContactPhonFacet);
                        }
                        else
                        {
                            PhoneNumberList newContactPhonFacet = new PhoneNumberList(new PhoneNumber(countryCode, mobileNumber), "Mobile");
                            client.SetFacet(contact, PhoneNumberList.DefaultFacetKey, newContactPhonFacet);
                        }

                        await client.SubmitAsync();
                    }

                    Console.ReadLine();

                }
                catch (XdbExecutionException ex)
                {
                    // Deal with exception
                }
            }
        }
    }
}
