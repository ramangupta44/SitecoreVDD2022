using SitecoreVDD.Extension.Foundation.Facets.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace website
{
    class Program
    {
        static void Main(string[] args)
        {
            var model = Sitecore.XConnect.Serialization.XdbModelWriter.Serialize(CustomerCollectionModel.Model);
            File.WriteAllText(CustomerCollectionModel.Model.FullName + ".json", model);
            Console.WriteLine("Completed");
            Console.Read();
        }
    }
}
