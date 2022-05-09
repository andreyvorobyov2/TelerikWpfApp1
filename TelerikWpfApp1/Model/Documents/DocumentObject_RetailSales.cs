using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace TelerikWpfApp1
{
    [EntityName("Document.RetailSalesReceipt")]
    public sealed class DocumentObject_RetailSales : DocumentObject<DocumentObject_RetailSales>
    {
        private string _testPresentationProperty;

        [DataMember, JsonProperty("TestPresentationProperty")]
        public string TestPresentationProperty
        {
            get => _testPresentationProperty;
            set
            {
                _testPresentationProperty = value;
                base.SetProperty(value);
            }
        }

        private CatalogRef_Partners _partner;
        [DataMember, JsonProperty("Partner")]
        public CatalogRef_Partners Partner
        {
            get => _partner;
            set
            {
                _partner = value;
                base.SetProperty(value);
            }
        }

        private CatalogRef_Agreements _agreement;
        [DataMember, JsonProperty("Agreement")]
        public CatalogRef_Agreements Agreement
        {
            get => _agreement;
            set
            {
                _agreement = value;
                base.SetProperty(value);
            }
        }

        [DataMember, JsonProperty("ItemList")]
        public RetailSales_ItemList ItemList { get; set; }
    }

    public sealed class RetailSales_ItemList_Row : TabularRow_ItemList<DocumentObject_RetailSales> 
    { }

    public sealed class RetailSales_ItemList :
       TabularSection_ItemList<RetailSales_ItemList_Row, DocumentObject_RetailSales>
    { }

    
}
