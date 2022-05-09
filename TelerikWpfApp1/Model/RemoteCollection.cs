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
    public abstract class RemoteCollection<T> : RemoteEntity, IBindingList
    {
        IBindingList bindingList = new BindingList<T>();

        public event ListChangedEventHandler ListChanged;

        public void NotifyItemAdded()
        {
            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedType.ItemAdded, bindingList.Count - 1));
        }

        public void NotifyItemUpdated(int index)
        {
            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedType.ItemChanged, index));
        }

        public void NotifyListReset()
        {
            ListChanged?.Invoke(this, new ListChangedEventArgs(ListChangedType.Reset, -1));
        }

        public object this[int index]
        {
            get => bindingList[index];
            set => bindingList[index] = value;
        }

        public bool AllowEdit => bindingList.AllowEdit;

        public bool AllowNew => bindingList.AllowNew;

        public bool AllowRemove => bindingList.AllowRemove;

        public int Count => bindingList.Count;

        #region Add

        public int Add(object value)
        {
            if (value != null)
            {
                T new_value = JsonConvert.DeserializeObject<T>(value.ToString());
                return AddOrUpdate(new_value);
            }
            return bindingList.Count - 1;
        }

        protected virtual int AddOrUpdate(T value)
        {
            return _add(value); // add by default
        }

        protected int _add(T value)
        {
            bindingList.Add(value);
            NotifyItemAdded();
            return bindingList.Count - 1;
        }

        protected int _update(T value, int index)
        {
            this[index] = value;
            NotifyItemUpdated(index);
            return index;
        }

        #endregion

        public object AddNew()
        {
            return AddNewRemote();
        }

        protected virtual object AddNewRemote()
        {
            T newRow = (T)bindingList.AddNew();
            NotifyItemAdded();
            return newRow;
        }

        public void Clear()
        {
            bindingList.Clear();
            NotifyListReset();
        }

        public bool Contains(object value)
        {
            return bindingList.Contains(value);
        }

        public void CopyTo(Array array, int index)
        {
            bindingList.CopyTo(array, index);
        }

        public IEnumerator GetEnumerator()
        {
            return bindingList.GetEnumerator();
        }

        public int IndexOf(object value)
        {
            return bindingList.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            bindingList.Insert(index, value);
        }

        public void Remove(object value)
        {
            bindingList.Remove(value);
        }

        public void RemoveAt(int index)
        {
            bindingList.RemoveAt(index);
        }

        public void AddIndex(PropertyDescriptor property) => bindingList.AddIndex(property);

        public void ApplySort(PropertyDescriptor property, ListSortDirection direction) => bindingList.ApplySort(property, direction);

        public int Find(PropertyDescriptor property, object key)
        {
            for (int index = 0; index < Count; ++index)
            {
                T item = (T)bindingList[index];
                object value = item.GetType().GetProperty(property.Name).GetValue(item);
                if (value.Equals(key))
                    return index;
            }

            return -1;
        }

        public void RemoveSort() => bindingList.RemoveSort();

        public void RemoveIndex(PropertyDescriptor property) => bindingList.RemoveIndex(property);

        public bool IsSorted => bindingList.IsSorted;

        public ListSortDirection SortDirection => bindingList.SortDirection;

        public PropertyDescriptor SortProperty => bindingList.SortProperty;

        public bool SupportsChangeNotification => bindingList.SupportsChangeNotification;

        public bool SupportsSearching => bindingList.SupportsSearching;

        public bool SupportsSorting => bindingList.SupportsSorting;

        public bool IsFixedSize => bindingList.IsFixedSize;

        public bool IsReadOnly => bindingList.IsReadOnly;

        public bool IsSynchronized => bindingList.IsSynchronized;

        public object SyncRoot => bindingList.SyncRoot;

    }

}
