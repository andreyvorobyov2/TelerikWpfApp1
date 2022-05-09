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
    public abstract class Adapter
    {
        Connector connector = Connector.GetInstance();

        public bool IsExternalUpdate { get; protected set; }

        public bool IsInnerCleaning { get; protected set; }

        protected string GetResponse(Connector.Parameters parameters)
        {
            return connector.SendRequest(parameters);
        }
    }

    public class AdapterCollection<T, K> : Adapter
        where T : RemoteCollection<K>
        where K : RemoteEntity
    {
        public void PopulateObject(T remoteCollection, Connector.Parameters parameters)
        {
            this.IsExternalUpdate = true;
            string json = GetResponse(parameters);
            remoteCollection.Clear();
            JsonConvert.PopulateObject(json, remoteCollection);
            this.IsExternalUpdate = false;
        }
    }

    public class AdapterObject<T> : Adapter
        where T : RemoteEntity, IRemoteContext
    {
        public void PopulateObject(T remoteObject, Connector.Parameters parameters)
        {
            this.IsExternalUpdate = true;

            string json = GetResponse(parameters);
            remoteObject.Context = JsonConvert.DeserializeObject<RemoteContext>(json);

            PropertyInfo[] props = remoteObject.GetType().GetProperties();
           
            this.IsInnerCleaning = true;
            foreach (PropertyInfo prop in props)
            {
                if(isImplementInterface(prop.PropertyType, typeof(IReference)))
                    prop.SetValue(remoteObject, null);
            }
            this.IsInnerCleaning = false;

            JsonConvert.PopulateObject(remoteObject.Context.ObjectPresentation, remoteObject);

            foreach (PropertyInfo prop in props)
            {
                if (isImplementGenericInterface(prop.PropertyType, typeof(ISubRemoteObject<>)))
                {
                    dynamic value = prop.GetValue(remoteObject);
                    value.RemoteOwner = remoteObject;
                    foreach (dynamic i in value)
                        i.RemoteOwner = remoteObject;
                }
            }

            this.IsExternalUpdate = false;
        }

        private bool isImplementGenericInterface(Type type_object, Type type_interface)
        {
            Type[] interfaces = type_object.GetInterfaces();
            return interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type_interface);
        }

        private bool isImplementInterface(Type type_object, Type type_interface)
        {
            return type_interface.IsAssignableFrom(type_object);
        }
    }
}
