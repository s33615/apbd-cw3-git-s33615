using System;
using System.Collections.Generic;
using System.Linq;
using WypozyczalniaApp.Domain;
namespace WypozyczalniaApp.Repositories {
    public interface IEquipmentRepository { void Add(Equipment e); Equipment GetById(Guid id); IEnumerable<Equipment> GetAvailable(); }
    public interface IUserRepository { void Add(User u); User GetById(Guid id); }
    public interface IRentalRepository { void Add(Rental r); Rental GetById(Guid id); IEnumerable<Rental> GetActiveRentalsForUser(Guid uid); }

    public class InMemoryEquipmentRepository : IEquipmentRepository {
        private List<Equipment> _e = new();
        public void Add(Equipment e) => _e.Add(e);
        public Equipment GetById(Guid id) => _e.First(x => x.Id == id);
        public IEnumerable<Equipment> GetAvailable() => _e.Where(x => x.IsAvailable);
    }
    public class InMemoryUserRepository : IUserRepository {
        private List<User> _u = new();
        public void Add(User u) => _u.Add(u);
        public User GetById(Guid id) => _u.First(x => x.Id == id);
    }
    public class InMemoryRentalRepository : IRentalRepository {
        private List<Rental> _r = new();
        public void Add(Rental r) => _r.Add(r);
        public Rental GetById(Guid id) => _r.First(x => x.Id == id);
        public IEnumerable<Rental> GetActiveRentalsForUser(Guid uid) => _r.Where(x => x.UserId == uid && x.ReturnDate == null);
    }
}