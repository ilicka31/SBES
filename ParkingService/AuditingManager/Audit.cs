using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditingManager
{
    public class Audit : IDisposable
    {
        private static EventLog customLog = null;
        const string SourceName = "AuditingManager.Audit";
        const string LogName = "ParkingService";


        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void AuthenticationSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserAuthenticationSuccess = AuditEvents.AuthenticationSuccess;
                string message = String.Format(UserAuthenticationSuccess, userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }


        public static void AuthenticationFailure(string error)
        {
            if (customLog != null)
            {
                string AuthenticationFailure = AuditEvents.AuthenticationFailure;
                string message = String.Format(AuthenticationFailure, error);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationFailure));
            }
        }


        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            if (customLog != null)
            {
                string AuthorizationSuccess = AuditEvents.AuthorizationSuccess;
                string message = String.Format(AuthorizationSuccess, userName, serviceName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationSuccess));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="serviceName"> should be read from the OperationContext as follows: OperationContext.Current.IncomingMessageHeaders.Action</param>
        /// <param name="reason">permission name</param>
        public static void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            if (customLog != null)
            {
                string AuthorizationFailed = AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationFailed, userName, serviceName, reason);

                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationFailed));
            }
        }

        public static void ParkingZoneSuccess(string zoneName, string operation)
        {

            if (customLog != null)
            {
                string ParkingZoneSuccess = AuditEvents.ParkingZoneSuccess;
                string message = String.Format(ParkingZoneSuccess, zoneName, operation);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ParkingZoneSuccess));
            }
        }

        public static void ParkingZoneFailure(string operation, string errorMessage)
        {
            if (customLog != null)
            {
                string ParkingZoneFailure = AuditEvents.ParkingZoneFailure;
                string message = String.Format(ParkingZoneFailure, operation, errorMessage);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ParkingZoneFailure));
            }
        }

        public static void PayParkingSuccess()
        {
            if (customLog != null)
            {
                string PayParkingSuccess = AuditEvents.PayParkingSuccess;
                string message = String.Format(PayParkingSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.PayParkingSuccess));
            }
        }

        public static void PayParkingFailure(string errorMessage)
        {
            if (customLog != null)
            {
                string PayParkingFailure = AuditEvents.PayParkingFailure;
                string message = String.Format(PayParkingFailure, errorMessage);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.PayParkingFailure));
            }
        }


        public static void CheckPaymentSuccess()
        {
            if (customLog != null)
            {
                string CheckPaymentSuccess = AuditEvents.CheckPaymentSuccess;
                string message = String.Format(CheckPaymentSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.CheckPaymentSuccess));
            }
        }

        public static void CheckPaymentFailure(string errorMessage)
        {
            if (customLog != null)
            {
                string CheckPaymentFailure = AuditEvents.CheckPaymentFailure;
                string message = String.Format(CheckPaymentFailure, errorMessage);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.CheckPaymentFailure));
            }
        }


        public static void AddParkingTicketSuccess()
        {
            if (customLog != null)
            {
                string AddParkingTicketSuccess = AuditEvents.AddParkingTicketSuccess;
                string message = String.Format(AddParkingTicketSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AddParkingTicketSuccess));
            }
        }


        public static void AddParkingTicketFailure(string errorMessage)
        {
            if (customLog != null)
            {
                string AddParkingTicketFailure = AuditEvents.AddParkingTicketFailure;
                string message = String.Format(AddParkingTicketFailure);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.AddParkingTicketFailure));
            }
        }

        public static void RemoveParkingTicketSuccess()
        {
            if (customLog != null)
            {
                string RemoveParkingTicketSuccess = AuditEvents.RemoveParkingTicketSuccess;
                string message = String.Format(RemoveParkingTicketSuccess);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.RemoveParkingTicketSuccess));
            }
        }


        public static void RemoveParkingTicketFailure(string errorMessage)
        {
            if (customLog != null)
            {
                string RemoveParkingTicketFailure = AuditEvents.RemoveParkingTicketFailure;
                string message = String.Format(RemoveParkingTicketFailure, errorMessage);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.RemoveParkingTicketSuccess));
            }
        }

        public static void ReplicatorSuccess(string transferedData)
        {
            if (customLog != null)
            {
                string ReplicatorSuccess = AuditEvents.ReplicatorSuccess;
                string message = String.Format(ReplicatorSuccess, transferedData);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ReplicatorSuccess));
            }
        }

        public static void ReplicatorFailure(string errorMessage)
        {
            if (customLog != null)
            {
                string ReplicatorFailure = AuditEvents.ReplicatorFailure;
                string message = String.Format(ReplicatorFailure, errorMessage);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.", (int)AuditEventTypes.ReplicatorFailure));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
