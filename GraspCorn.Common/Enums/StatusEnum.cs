using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraspCorn.Common.Enums
{
    public enum StatusEnum

    {
        [Description("1")]
        Open,
        [Description("2")]
        InProcess,
        [Description("5")]
        Completed,
        [Description("6")]
        Closed,
        [Description("Z1")]
        Z1InProcess,
        [Description("Z2")]
        Pending,
        [Description("Z3")]
        PendingDueToSpares,
        [Description("Z4")]
        Z4Completed,
        [Description("Z5")]
        Cancelled,
        [Description("Z6")]
        PendingforCancellation,

        [Description("Order created by Sponsor")]
        OrderCreated=5,

        [Description("QCCompleted")]
        QCCompleted = 15,

        [Description("LogisticsTicketRaised")]
        LogisticsTicketUpdated = 18,

        [Description("Payment not initiated")]
        PaymentUnsuccessfull = 45,

        [Description("Payment Failed")]
        PaymentFailed = 46,

        [Description("Payment Successful")]
        Paymentsuccessfull = 47,

        [Description("Delivered")]
        InstallationOfnewProduct = 17,

        [Description("Cancelled")]
        OrderCancell = 6,

        [Description("Call Assigned for QC & Data transferred to SVC")]
        CallAssignedforQC = 9,

        [Description("QC Appointment taken")]
        QCAppointmenttaken = 10,

        [Description("Appointment rescheduled")]
        QCAppointmentrescheduled = 11,

        [Description("QC appointment declined by customer")]
        QCappointmentdeclinedbycustomer = 12,

        [Description("QC In Progress")]
        QCInProgress = 13,

        [Description("QC fail")]
        QCfail = 14,

        [Description("QC Amount Confirmation Declined by customer after QC")]
        QCAmounDeclinedbycustomer = 16,

        [Description("Self QC by Customer")]
        SelfQC = 33,

        [Description("Reopen for QC After Decline")]
        ReopenforQC = 41,

        [Description("Bypass QC")]
        BypassQC = 42,

        [Description("Waiting for customer Approval")]
        WaitingforcustomerApproval = 48,

        [Description("Pending For Upper Cap By QC Admin")]
        PendingForUppeCap = 53,

        [Description("Customer not Responding")]
        CustomernotResponding = 57,

        [Description("QC Appointment Rescheduled > 3 times")]
        QCRescheduledMoreThenThree = 58,

        [Description("Customer dailer recall")]
        Customerdailerrecall = 60,

    }
    public enum LodhaGroupEnum
    {
        [Description("SCHEDULED")]
        SCHEDULED = 1,

        [Description("IN PROGRESS")]
        INPROGRESS = 2,

        [Description("COMPLETED")]
        COMPLETED = 3,

        [Description("CANCELLED")]
        CANCELLED=4
    }
}
