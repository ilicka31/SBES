using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AuditingManager
{
    public enum AuditEventTypes
    {
        AuthenticationSuccess = 0,
        AuthorizationSuccess = 1,
        AuthorizationFailed = 2,
        ParkingZoneSuccess = 3,
        ParkingZoneFailure = 4,
        PayParkingSuccess = 5,
        PayParkingFailure = 6,
        CheckPaymentSuccess = 7,
        CheckPaymentFailure = 8,
        AddParkingTicketSuccess = 9,
        AddParkingTicketFailure = 10,
        RemoveParkingTicketSuccess = 11,
        RemoveParkingTicketFailure = 12,
        AuthenticationFailure = 13,
        ReplicatorSuccess = 14,
        ReplicatorFailure = 15
    }

    public class AuditEvents
    {
        private static ResourceManager resourceManager = null;
        private static object resourceLock = new object();

        private static ResourceManager ResourceMgr
        {
            get
            {
                lock (resourceLock)
                {
                    if (resourceManager == null)
                    {
                        resourceManager = new ResourceManager
                            (typeof(AuditEventFile).ToString(),
                            Assembly.GetExecutingAssembly());
                    }
                    return resourceManager;
                }
            }
        }

        public static string AuthenticationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
            }
        }

        public static string AuthenticationFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthenticationFailure.ToString());
            }
        }

        public static string AuthorizationSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
            }
        }

        public static string AuthorizationFailed
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
            }
        }

        public static string ParkingZoneSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ParkingZoneSuccess.ToString());
            }
        }


        public static string ParkingZoneFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ParkingZoneFailure.ToString());
            }
        }

        public static string PayParkingSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.PayParkingSuccess.ToString());
            }
        }

        public static string PayParkingFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.PayParkingFailure.ToString());
            }
        }

        public static string CheckPaymentSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CheckPaymentSuccess.ToString());
            }
        }

        public static string CheckPaymentFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.CheckPaymentFailure.ToString());
            }
        }


        public static string AddParkingTicketSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddParkingTicketSuccess.ToString());
            }
        }

        public static string AddParkingTicketFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.AddParkingTicketFailure.ToString());
            }
        }

        public static string RemoveParkingTicketSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RemoveParkingTicketSuccess.ToString());
            }
        }

        public static string RemoveParkingTicketFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.RemoveParkingTicketFailure.ToString());
            }
        }

        public static string ReplicatorSuccess
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReplicatorSuccess.ToString());
            }
        }

        public static string ReplicatorFailure
        {
            get
            {
                return ResourceMgr.GetString(AuditEventTypes.ReplicatorFailure.ToString());
            }
        }
    }
}
