using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TelerikWpfApp1
{
    [DataContract]
    public abstract class DocumentObject<T> : RemoteObject<T>
       where T : DocumentObject<T>
    { }

}
