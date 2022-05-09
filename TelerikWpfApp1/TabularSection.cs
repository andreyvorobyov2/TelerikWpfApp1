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
    public abstract class ExtTabularSection<T, K> : RemoteCollection<T>, ISubRemoteObject<K> 
        where T : ExtTabularRow<K>, ISubRemoteObject<K>
        where K : RemoteObject<K>
    {
        public K RemoteOwner { get; set; }

    }

    public abstract class ExtTabularRow<T> : RemoteEntity, ISubRemoteObject<T>
        where T : RemoteObject<T>
    {
        public T RemoteOwner { get; set; }
    }

    [DataContract]
    public abstract class DocumentTabularSection<T, K> : ExtTabularSection<T, K>
        where T : DocumentTabularRow<K>, new()
        where K : DocumentObject<K>
    {

        public const string ACTION_ADD_NEW_ROW = "ADD_NEW_ROW";

        protected override int AddOrUpdate(T value)
        {
            // find row by key
            PropertyDescriptor key_descriptor = TypeDescriptor.GetProperties(value).Find("KEY", true);
            int index = this.Find(key_descriptor, value.Key);
            if (index == -1)
                return base._add(value); // row not found
            return base._update(value, index);
        }

        protected override object AddNewRemote()
        {
            var data = new
            {
                LinkedContext = RemoteOwner.Context.LinkedContext,
            };

            Connector.Parameters parameters = new Connector.Parameters()
            {
                Action = ACTION_ADD_NEW_ROW,
                EntityName = RemoteOwner.EntityName + "." + this.EntityName,
                Data = JsonConvert.SerializeObject(data),
            };

            RemoteOwner.Adapter.PopulateObject(RemoteOwner, parameters);
            return this[this.Count-1]; // last row is added row
        }
    }

    [DataContract]
    public abstract class DocumentTabularRow<T> : ExtTabularRow<T>
        where T : DocumentObject<T>
    {
        class Cell
        {
            public const string ACTION_SET_CELL = "SET_CELL";

            public string ColumnName { get; set; }

            public string RowKey { get; set; }

            public dynamic Value { get; set; }

            public string EntityName { get; set; } = "";

            public Cell(string name, dynamic value, string key)
            {
                ColumnName = name;
                Value = value;
                RowKey = key;

                RemoteEntity extEntity = value as RemoteEntity;
                if (extEntity != null)
                    EntityName = extEntity.EntityName;
            }

            public string GetData(IRemoteContext remoteContext)
            {
                var data = new
                {
                    LinkedContext = remoteContext.Context.LinkedContext,
                    Value = JsonConvert.SerializeObject(this)
                };
                return JsonConvert.SerializeObject(data);
            }
        }

        [DataMember, JsonProperty("Key")]
        public string Key { get; set; }

        protected void SetCell(dynamic value, [CallerMemberName] string name = null)
        {
            if (RemoteOwner == null)
                return;
            Cell cell = new Cell(name, value, Key);
            Connector.Parameters parameters = new Connector.Parameters()
            {
                EntityName =  RemoteOwner.EntityName + "." + EntityName,
                Action = Cell.ACTION_SET_CELL,
                Data = cell.GetData(RemoteOwner),
            };
            RemoteOwner.Adapter.PopulateObject(RemoteOwner, parameters);

        }

    }

    [EntityName("TabularSection.ItemList")]
    public abstract class TabularSection_ItemList<T, K> : DocumentTabularSection<T, K>
    where T : DocumentTabularRow<K>, new()
    where K : DocumentObject<K>
    { }

    [EntityName("TabularSection.ItemList.Row")]
    public class TabularRow_ItemList<K> : DocumentTabularRow<K>
        where K : DocumentObject<K>
    {
        CatalogRef_Items _item;
        [DataMember, JsonProperty("Item")]
        public CatalogRef_Items Item
        { 
            get => _item; 
            set
            {
                _item = value;
            }
        }

        CatalogRef_ItemKeys _itemKey;
        [DataMember, JsonProperty("ItemKey")]
        public CatalogRef_ItemKeys ItemKey
        {
            get => _itemKey; 
            set
            {
                _itemKey = value;
                base.SetCell(value);
            }
        }

        CatalogRef_PriceTypes _price_types;
        [DataMember, JsonProperty("PriceType")]
        public CatalogRef_PriceTypes PriceType
        {
            get => _price_types;
            set
            {
                _price_types = value;
                base.SetCell(value);
            }
        }

        decimal _price;
        [DataMember, JsonProperty("Price")]
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                base.SetCell(value);
            }
        }

        decimal _quantity;
        [DataMember, JsonProperty("Quantity")]
        public decimal Quantity 
        { 
            get => _quantity;
            set
            {
                _quantity = value;
                base.SetCell(value);
            } 
        }

        decimal _total_amount;
        [DataMember, JsonProperty("TotalAmount")]
        public decimal TotalAmount 
        { 
            get => _total_amount;
            set
            {
                _total_amount = value;
                base.SetCell(value);
            } 
        }
    }
}
