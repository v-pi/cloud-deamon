using CloudDaemon.Common.Entities;

namespace CloudDaemon.Common.Interfaces
{
    public interface ITaxesManager
    {
        void SaveTaxNotice(TaxNotice notice);

        bool TaxNoticeExists(TaxNotice notice);
    }
}
