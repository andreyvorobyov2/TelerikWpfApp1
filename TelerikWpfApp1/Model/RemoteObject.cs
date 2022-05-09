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
    public interface IRemoteContext
    {
        RemoteContext Context { get; set; }
    }

    public interface ISubRemoteObject<T> where T : RemoteObject<T>
    {
        T RemoteOwner { get; set; }
    }

    public class RemoteContext
    {
        public string LinkedContext { get; set; } = "";
        public string ObjectPresentation { get; set; } = "";
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class EntityName : Attribute
    {
        public string Name { get; }
        public EntityName(string name) => Name = name;
    }

    public abstract class RemoteEntity
    { 
        public string EntityName { get; }

        public RemoteEntity()
        {
            object[] attrs = this.GetType().GetCustomAttributes(typeof(EntityName), true);
            if (attrs.Length > 0)
                this.EntityName = ((EntityName)attrs[0]).Name;
        }
    }

    public abstract class RemoteObject<T> : RemoteEntity, INotifyPropertyChanged, IRemoteContext
        where T : RemoteObject<T>
    {
        class Property
        {
            public const string ACTION_SET_PROPERTY = "SET_PROPERTY";

            public string Name { get; set; }

            public dynamic Value { get; set; }

            public string EntityName { get; set; } = "";

            public Property(string name, dynamic value)
            {
                Name = name;
                Value = value;

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

        public const string ACTION_CREATE_OBJECT = "CREATE_OBJECT";

        AdapterObject<T> adapter = new AdapterObject<T>();
        
        public AdapterObject<T> Adapter { get => adapter; }

        public event PropertyChangedEventHandler PropertyChanged;

        public RemoteContext Context { get; set; } = new RemoteContext();
        
        public RemoteObject()
        {
            Connector.Parameters parameters = new Connector.Parameters()
            {
                EntityName = this.EntityName,
                Action = ACTION_CREATE_OBJECT
            };
            adapter.PopulateObject((T)this, parameters);
        }

        protected void SetProperty(dynamic value, [CallerMemberName] string name = null)
        {
            if (adapter.IsExternalUpdate)
            {
                if(!adapter.IsInnerCleaning)
                    PropertyChanged?.Invoke((T)this, new PropertyChangedEventArgs(name));
            }
            else
            {
                Property prop = new Property(name, value);
                Connector.Parameters parameters = new Connector.Parameters()
                {
                    EntityName = this.EntityName,
                    Action = Property.ACTION_SET_PROPERTY,
                    Data = prop.GetData(this)
                };
                adapter.PopulateObject((T)this, parameters);
            }
        }
    }

}
