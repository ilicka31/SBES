using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.Models
{
    [DataContract]
    public enum EServiceState
    {
        [EnumMemberAttribute]
        UNKNOWN,
        [EnumMemberAttribute]
        PRIMARY,
        [EnumMemberAttribute]
        SECONDARY
    }
}
