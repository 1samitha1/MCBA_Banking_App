using CustomerPortal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomerPortal.Data;

public class McbaContext: DbContext
{
    public McbaContext(DbContextOptions<McbaContext> options) : base(options) { }
    
    
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Login>  Logins { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<BillPay>  BillPay { get; set; }
    public DbSet<Payee> Payees { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- map enums as single CHARs ---
        var accountTypeConv = new ValueConverter<AccountType, string>(
            v => ((char)v).ToString(),
            v => (AccountType)(byte)v[0]);

        var txnTypeConv = new ValueConverter<TransactionType, string>(
            v => ((char)v).ToString(),
            v => (TransactionType)(byte)v[0]);

        var billPeriodConv = new ValueConverter<BillPeriod, string>(
            v => ((char)v).ToString(),
            v => (BillPeriod)(byte)v[0]);
        
        
        //set relationships (just to verify for EF core
        //Login
        modelBuilder.Entity<Login>()
            .HasOne(l => l.Customer)
            .WithOne(c => c.Login)
            .HasForeignKey<Login>(l => l.CustomerID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Account>()
            .HasOne(c => c.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerID);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(a => a.AccountNumber);
        
        modelBuilder.Entity<Transaction>()
            .HasOne(d => d.DestinationAccount)
            .WithMany()
            .HasForeignKey(d => d.DestinationAccountNumber);
        
        modelBuilder.Entity<BillPay>()
            .HasOne(a => a.Account)
            .WithMany(a => a.BillPays)
            .HasForeignKey(a => a.AccountNumber);
        
        modelBuilder.Entity<BillPay>()
            .HasOne(b => b.Payee)
            .WithMany(p => p.BillPays)
            .HasForeignKey(p => p.PayeeID);
    }
    
}