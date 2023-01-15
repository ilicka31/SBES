using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    [ServiceContract]
    public interface IReplicator
    {
        [OperationContract]
        KeyValuePair<byte[], byte[]> TransferCars();

        [OperationContract]
        KeyValuePair<byte[], byte[]> TransferPayments();

        [OperationContract]
        KeyValuePair<byte[], byte[]> TransferZones();
      
    }
}
