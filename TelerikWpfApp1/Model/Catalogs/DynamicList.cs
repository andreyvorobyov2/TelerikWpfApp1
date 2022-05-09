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
    public interface IDynamicList
    {
        public void ReadList();
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FilterDefinition : Attribute
    {
        public string Name { get; }
        public FilterDefinition(string name) => Name = name;
    }

    public abstract class DynamicList<T, K> : RemoteCollection<K>, IDynamicList
        where T : DynamicList<T, K>
        where K : RemoteEntity
    {
        class Filter
        {
            public string Name { get; set; }
            public dynamic Value { get; set; }
        }

        public const string ACTION_READ_LIST = "READ_LIST";

        AdapterCollection<T, K> adapter = new AdapterCollection<T, K>();

        public void ReadList()
        {
            // filter definitions - test
            List<Filter> _filters = new List<Filter>();
            PropertyInfo[] props = this.GetType().GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(typeof(FilterDefinition), true);
                foreach (FilterDefinition attr in attrs)
                {
                    _filters.Add(new Filter() { Name = attr.Name, Value = prop.GetValue(this) });
                }
            }

            Connector.Parameters parameters = new Connector.Parameters()
            {
                Action = ACTION_READ_LIST,
                EntityName = this.EntityName,
                Data = JsonConvert.SerializeObject(_filters)
            };
            adapter.PopulateObject((T)this, parameters);
        }
    }

    [EntityName("Catalog.Items")]
    public class CatalogList_Items : 
        DynamicList<CatalogList_Items, CatalogRef_Items> 
    { }

    [EntityName("Catalog.ItemKeys")]
    public class CatalogList_ItemKeys : 
        DynamicList<CatalogList_ItemKeys, CatalogRef_ItemKeys>
    {
        [FilterDefinition("FilterByItem")]
        public CatalogRef_Items FilterByItem { get; set; }
    }

    [EntityName("Catalog.Partners")]
    public class CatalogList_Partners : 
        DynamicList<CatalogList_Partners, CatalogRef_Partners> 
    { }

    [EntityName("Catalog.Agreements")]
    public class CatalogList_Agreements :
        DynamicList<CatalogList_Agreements, CatalogRef_Agreements>
    { }

    [EntityName("Catalog.PriceTypes")]
    public class CatalogList_PriceTypes :
        DynamicList<CatalogList_PriceTypes, CatalogRef_PriceTypes>
    { }
}
