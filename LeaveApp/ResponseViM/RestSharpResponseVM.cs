using System;
using System.Collections.Generic;

namespace LeaveApp.ResponseViM
{
    public class Wallet
    {
        public int WalletID { get; set; }
        public int SubWalletID { get; set; }
        public string WalletAccountNumber { get; set; }
        public object WalletBalance { get; set; }
        public DateTime DateCreated { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessName { get; set; }
        public string BusinessDescription { get; set; }
        public string BankName { get; set; }
        public string WalletName { get; set; }
        public bool IsActive { get; set; }
        public int ApprovedBy { get; set; }
        public string ApprovedBytName { get; set; }
        public string RcNumber { get; set; }
        public DateTime DateApproved { get; set; }
        public string MainWallet { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
    }

    public class ProfileViews
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string DateOfBirth { get; set; }
        public object Image { get; set; }
        public int WalletId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public bool IsSignUp { get; set; }
        public bool IsVerified { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int CreatedBy { get; set; }
        public int ApprovedBy { get; set; }
        public bool IsApproved { get; set; }
        public int ApprovedBy2 { get; set; }
        public object RcNumber { get; set; }
        public string Role { get; set; }
        public int AggregatorID { get; set; }
        public object AggregatorCode { get; set; }
        public object AggregatorTypeID { get; set; }
        public object AggregatorType { get; set; }
        public object AgentTypeID { get; set; }
        public string ResidentialState { get; set; }
        public object Lga { get; set; }
        public string WalletAccountNumber { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress { get; set; }
        public bool IsSpecial { get; set; }
        public object BankAccountCode { get; set; }
        public bool IsUseFingerprint { get; set; }
        public object TerminalId { get; set; }
        public string WalletBalance { get; set; }
        public string OpeningBalance { get; set; }
        public string ClosingBalance { get; set; }
        public string AccountType { get; set; }
        public List<Wallet> Wallets { get; set; }
    }

    public class ResponseObject
    {
        public ProfileViews ProfielViews { get; set; }
        public string Token { get; set; }
        public bool Result { get; set; }
        public List<string> Error { get; set; }
        public string StatusCode { get; set; }
        public object DeviceInfo { get; set; }
        public string Message { get; set; }
        public object Usertype { get; set; }
        public string Userid { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
