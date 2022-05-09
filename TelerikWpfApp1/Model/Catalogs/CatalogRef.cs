using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections;

namespace TelerikWpfApp1
{
    public interface IReference
    {
        public string Ref { get; set; } //public Guid Ref { get; set; }

        public string Presentation { get; set; }

        public Type ListOfReferences { get;} // list for choise ref
    }


    [DataContract]
    public abstract class CatalogRef<T> : RemoteEntity, IReference
    {
        [DataMember, JsonProperty("Ref")]
        public string Ref { get; set; }
        
        [DataMember, JsonProperty("Presentation")]
        public string Presentation { get; set; }

        [DataMember, JsonProperty("Code")]
        public int Code { get; set; }

        public override string ToString() => Presentation;

        public abstract Type ListOfReferences { get; }
    }

    [EntityName("Catalog.Items")]
    public sealed class CatalogRef_Items : CatalogRef<CatalogRef_Items>
    {
        public override Type ListOfReferences { get => typeof(CatalogList_Items); }
    }

    [EntityName("Catalog.ItemKeys")]
    public sealed class CatalogRef_ItemKeys : CatalogRef<CatalogRef_ItemKeys>
    {
        public override Type ListOfReferences { get => typeof(CatalogList_ItemKeys); }

        [DataMember, JsonProperty("Item")]
        public CatalogRef_Items Item { get; set; } // this test property
    }

    [EntityName("Catalog.Partners")]
    public sealed class CatalogRef_Partners : CatalogRef<CatalogRef_Partners>
    {
        public override Type ListOfReferences { get => typeof(CatalogList_Partners); }
    }

    [EntityName("Catalog.Agreements")]
    public sealed class CatalogRef_Agreements : CatalogRef<CatalogRef_Agreements>
    {
        public override Type ListOfReferences { get => typeof(CatalogList_Agreements); }
    }

    [EntityName("Catalog.PriceTypes")]
    public sealed class CatalogRef_PriceTypes : CatalogRef<CatalogRef_PriceTypes>
    {
        public override Type ListOfReferences { get => typeof(CatalogList_PriceTypes); }
    }
}
