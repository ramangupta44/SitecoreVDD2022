# Sitecore Virtual Developer Day 2022

This repository contains code for the demo which I've presented during Sitecore Virtual developer day 2022 (13th April 2022). Here is the link for the video of my session. https://www.youtube.com/watch?v=fkXZEYxKnGY&t=17781s
In this session, I talked about multi-layered personalization in Sitecore experience platform. Three main pillars for multi-layered personalization are:

**1. Segment your mailing list:** To showcase this, I've created custom segmented list rule during my session, you can find all related source code for this at src/feature/CustomRules/website/Model/SegmentedListRules/. In this (SitecoreVDD.Extension.Feature.CustomRules) project, I’ve created segemented rule to filter out customers based on policy start date.

**2. Component Personalization:** For this, I've created custom Email personalization rule to send personalized email to customers based on their segment value. Code for this can be find at: src/feature/CustomRules/website/Model/PersonalizationRules/.

**3. Getting Personal with Email Campaigns:** This is called token personalization. To demo this, I've created custom tokens like Policy Number, Policy Start Date, Policy Premium etc. You can see code at : SitecoreVDD2022/src/feature/CustomTokens/website/

**How to set up:**

**Pre-requisites: **
1. Set up vanilla Sitecore 10.2 XP instance.
2. Clone the code repository and build the solution (SitecoreVDD.Extension.sln).


**Set up the custom facets**
1. Make this _SitecoreVDD.Extension.Feature.GenerateModel_ project as start-up project and run (in release mode) the project in visual studio. On execution of the console application, it will generate a CustomFacets.Xconnect.CustomerCollectionModel.json file at bin/Release folder. Copy that JSON file and paste at the below places:
- <x-connect root path>\App_data\Models
- <x-connectroot path>\App_data\jobs\continuous\IndexWorker\App_data\Models
 
2. Also deploy your Custom facet class project DLL (SitecoreVDD.Extension.Foundation.Facets.dll) at the following places:
- <x-connect root path>\App_data\jobs\continuous\AutomationEngine\
- <x-connect root path>\App_data\jobs\continuous\IndexWorker\
- <x-connect root path>\bin
- <Instance_Name>.sc\bin
  
3. Publish the SitecoreVDD.Extension.Foundation.Facets project to the root folder.
4. From this path src/foundation/Facets/website/App_Config, copy the MarketingAutomation_patch folder and place at <XConnectRootPath>\App_Data\jobs\continuous\AutomationEngine\App_Data\Config\sitecore\

**Import contacts in XDB**
1. Update src/feature/ImportContacts/website/Program.cs with your xconnect instance url and thumbprint value. 
2. Make this SitecoreVDD.Extension.Feature.ImportContacts as start up project and execute this console application.
3. In the console, first choose 1 = LocalDev and then create contacts, then enter relevant values. Contact will be created in XDB.

**Set up custom segmented rules**
1. Publish the SitecoreVDD.Extension.Feature.CustomRules project at Sitecore CM root path.
2. Install this Sitecore package (Custom Rules Sitecore Items.zip)
  
**Set up custom segmented rules**
1. Publish this SitecoreVDD.Extension.Feature.CustomTokens project at Sitecore CM root path.
  
**Refer to my blogs:**
  
https://sitecorewithraman.wordpress.com/2020/08/02/sitecore-xconnect-custom-facets-part-i/
https://sitecorewithraman.wordpress.com/2020/08/22/sitecore-xconnect-custom-facets-part-ii/
https://sitecorewithraman.wordpress.com/2021/05/02/custom-rules-segmented-list-marketing-automation/
https://sitecorewithraman.wordpress.com/2021/05/02/custom-rules-for-sitecore-email-personalization/
https://sitecorewithraman.wordpress.com/2021/01/10/sitecore-exm-custom-tokens/
