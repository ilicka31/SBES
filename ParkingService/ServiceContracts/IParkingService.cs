using SecurityManager;
using ServiceContracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    [ServiceContract]
    public interface IParkingService
    {
        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool ModifyParkingZone(ParkingZone parkingZone);

        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool AddParkingZone(ParkingZone parkingZone);

        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool PayParking(Car car, ParkingZone parkingZone);

        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool CheckPayment(string registration, string zone);

        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool AddParkingTicket(string registration, string zone);

        [OperationContract]
        [FaultContract(typeof(SecurityFault))]
        bool RemoveParkingTicket(string registration);

        [OperationContract]
        List<ParkingZone> GetAllParkingZones();

        [OperationContract]
        List<Car> GetAllCars();

        [OperationContract]
        List<Payment> GetAllPayments();

        [OperationContract]
        List<Car> GetAllCarsWithoutPayment();

        [OperationContract]
        EServiceState GetServerState();
    }
}
