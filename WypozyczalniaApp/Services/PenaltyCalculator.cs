using System;
using WypozyczalniaApp.Domain;
namespace WypozyczalniaApp.Services {
    public interface IPenaltyCalculator { decimal Calculate(Rental r, DateTime now); }
    public class StandardPenaltyCalculator : IPenaltyCalculator {
        public decimal Calculate(Rental r, DateTime now) => r.IsOverdue(now) ? r.GetDaysOverdue(now) * 10m : 0m;
    }
}