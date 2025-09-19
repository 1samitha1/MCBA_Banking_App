namespace CustomerPortal.Models;

public enum AccountType : byte { Checking = (byte)'C', Savings = (byte)'S' }   // 'C'/'S'
public enum TransactionType : byte { Deposit = (byte)'D', Withdraw = (byte)'W', Transfer = (byte)'T', ServiceCharge = (byte)'S' } // 'D','W','T','S'
public enum BillPeriod : byte { OneOff = (byte)'O', Monthly = (byte)'M' }      // 'O'/'M'