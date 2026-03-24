using System;
using System.Linq;
using WypozyczalniaApp.Domain;
using WypozyczalniaApp.Repositories;
namespace WypozyczalniaApp.Services {
    public class RentalService {
        private readonly IEquipmentRepository _er; private readonly IUserRepository _ur;
        private readonly IRentalRepository _rr; private readonly IPenaltyCalculator _pc;
        public RentalService(IEquipmentRepository er, IUserRepository ur, IRentalRepository rr, IPenaltyCalculator pc) {
            _er = er; _ur = ur; _rr = rr; _pc = pc;
        }
        public void RentEquipment(Guid uid, Guid eid, TimeSpan dur) {
            var u = _ur.GetById(uid); var e = _er.GetById(eid);
            if (!e.IsAvailable) throw new InvalidOperationException("Niedostępny");
            if (_rr.GetActiveRentalsForUser(uid).Count() >= u.MaxRentalsLimit) throw new InvalidOperationException("Limit");
            e.IsAvailable = false; _rr.Add(new Rental(uid, eid, DateTime.Now, dur));
        }
        public decimal ReturnEquipment(Guid rid, DateTime now) {
            var r = _rr.GetById(rid); var e = _er.GetById(r.EquipmentId);
            r.ReturnDate = now; e.IsAvailable = true; return _pc.Calculate(r, now);
        }
    }
}