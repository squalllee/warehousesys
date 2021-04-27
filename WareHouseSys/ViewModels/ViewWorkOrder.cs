using System;

namespace WareHouseSys.ViewModel
{
    public class ViewWorkOrder
    {
        public string WorkNo { get; set; }

        public string ReportMan { get; set; }

        public string ReportManName { get; set; }

        public string ReportUnit { get; set; }

        public string ReportUnitName { get; set; }

        public DateTime? BrokenDateTime { get; set; }

        public string Line { get; set; }

        public string Station { get; set; }

        public string BrokenPlace { get; set; }

        public string ResponsibleUnit { get; set; }

        public string Item { get; set; }

        public string RepairLevel { get; set; }

        public string SystemName { get; set; }

        public string SubSystemName { get; set; }

        public string ReportReason { get; set; }

        public DateTime? RepairDateTime { get; set; }

        public string ContractNo { get; set; }

        public DateTime? CompleteDateTime { get; set; }

        public DateTime? CloseDateTime { get; set; }

        public string BrokenReason { get; set; }

        public string RepairResult { get; set; }

        public string CurrentStatus { get; set; }

        public string CurrentMan { get; set; }

        public DateTime? CreateDateTime { get; set; }

        public DateTime? UpdateDateTime { get; set; }
    }
}