using System;

namespace WypozyczalniaApp.Domain
{
    public class Rental
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public Guid UserId { get; init; }
        public Guid EquipmentId { get; init; }
        public DateTime RentDate { get; init; }
        public DateTime DueDate { get; init; }
        public DateTime? ReturnDate { get; set; }

        public Rental(Guid userId, Guid equipmentId, DateTime rentDate, TimeSpan duration)
        {
            UserId = userId;
            EquipmentId = equipmentId;
            RentDate = rentDate;
            DueDate = rentDate.Add(duration);
        }

        public bool IsOverdue(DateTime currentDate)
        {
            return ReturnDate == null && currentDate > DueDate;
        }
        
        public int GetDaysOverdue(DateTime currentDate)
        {
            if (!IsOverdue(currentDate)) return 0;
            
            TimeSpan overdueTime = currentDate - DueDate;
            return (int)Math.Ceiling(overdueTime.TotalDays);
        }
    }
}