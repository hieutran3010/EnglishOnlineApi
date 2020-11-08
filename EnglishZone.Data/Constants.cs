namespace EnglishZone.Data
{
    public class PaymentType
    {
        public const string Cash = "CASH";
        public const string BankTransfer = "BANK_TRANSFER";
        public const string CashAndBankTransfer = "CASH_AND_BANK_TRANSFER";
    }

    public class InternationalParcelVendor
    {
        public const string DHL_VN = "DHL VN";
        public const string DHL_SING = "DHL SING";
        public const string UPS = "UPS";
        public const string TNT = "TNT";
        public const string Fedex = "FEDEX";
    }

    public class BillStatus
    {
        public const string License = "LICENSE";
        public const string Accountant = "ACCOUNTANT";
        public const string Done = "DONE";
    }

    public class ParamsKey
    {
        public const string VAT_PARAMS_KEY = "VAT";
        public const string USD_EXCHANGE_RATE = "USD_EXCHANGE_RATE";
    }

    public class UserRoleKey
    {
        public const string LICENSE = "license";
        public const string ACCOUNTANT = "accountant";
        public const string ADMIN = "admin";
        public const string SALE = "sale";
    }

    public class ExportSessionStatus
    {
        public const string WORKING = "WORKING";
        public const string DONE = "DONE";
    }

    public class ExportType
    {
        public const string BILL_REPORT = "BILL REPORT";
    }
}