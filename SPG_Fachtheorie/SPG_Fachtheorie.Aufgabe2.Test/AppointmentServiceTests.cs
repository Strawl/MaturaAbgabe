using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SPG_Fachtheorie.Aufgabe2;
using Microsoft.EntityFrameworkCore;

namespace SPG_Fachtheorie.Aufgabe2.Test
{
    [Collection("Sequential")]
    public class AppointmentServiceTests
    {
        /// <summary>
        /// Legt die Datenbank an und befüllt sie mit Musterdaten. Die Datenbank ist
        /// nach Ausführen des Tests ServiceClassSuccessTest in
        /// SPG_Fachtheorie\SPG_Fachtheorie.Aufgabe2.Test\bin\Debug\net6.0\Appointment.db
        /// und kann mit SQLite Manager, DBeaver, ... betrachtet werden.
        /// </summary>
        private AppointmentContext GetAppointmentContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source=Appointment.db")
                .Options;

            var db = new AppointmentContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
            return db;
        }
        [Fact]
        public void ServiceClassSuccessTest()
        {
            using var db = GetAppointmentContext();
            Assert.True(db.Students.Count() > 0);
            var service = new AppointmentService(db);
        }
        [Fact]
        public void AskForAppointmentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var offer = db.Offers.First();
            var student = db.Students.First();

            if (offer == null)
                throw new ArgumentNullException("Offer is null");
            if (student == null)
                throw new ArgumentNullException("Student is null");
            DateTime dt = offer.From.AddSeconds(1);
            bool output = service.AskForAppointment(offer.Id, student.Id, dt);
            Assert.True(output);    
        }
        [Fact]
        public void AskForAppointmentReturnsFalseIfNoOfferExists()
        {
            
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var student = db.Students.First();

            if (student == null)
                throw new ArgumentNullException("Student is null");
            bool result = service.AskForAppointment(Guid.NewGuid(), student.Id, DateTime.Now);
            Assert.False(result);
        }
        [Fact]
        public void AskForAppointmentReturnsFalseIfOutOfDate()
        {

            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var offer = db.Offers.First();
            var student = db.Students.First();

            if (offer == null)
                throw new ArgumentNullException("Offer is null");
            if (student == null)
                throw new ArgumentNullException("Student is null");
            DateTime dt = offer.To.AddDays(30);
            bool output = service.AskForAppointment(offer.Id, student.Id, dt);
            Assert.False(output);    
        }
        [Fact]
        public void ConfirmAppointmentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var appointment = db.Appointments.FirstOrDefault(appoint => appoint.State == Model.AppointmentState.AskedFor);

            if (appointment == null)
                throw new ArgumentNullException("Appointment is null");
            bool output = service.ConfirmAppointment(appointment.Id);
            Assert.True(output);
        }
        [Fact]
        public void ConfirmAppointmentReturnsFalseIfStateIsInvalid()
        {
            
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var appointment = db.Appointments.FirstOrDefault(appoint => appoint.State == Model.AppointmentState.Confirmed);

            if (appointment == null)
                throw new ArgumentNullException("Appointment is null");
            bool output = service.ConfirmAppointment(appointment.Id);
            Assert.False(output);
        }
        [Fact]
        public void CancelAppointmentStudentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var appointment = db.Appointments.FirstOrDefault(appoint => appoint.State == Model.AppointmentState.AskedFor);

            if (appointment == null)
                throw new ArgumentNullException("Appointment is null");

            bool output = service.CancelAppointment(appointment.Id, appointment.StudentId);
            Assert.True(output);
        }
        [Fact]
        public void CancelAppointmentCoachSuccessTest()
        {

            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var appointment = db.Appointments.FirstOrDefault(appoint => appoint.State == Model.AppointmentState.AskedFor);

            if (appointment == null || appointment.Offer.Teacher == null)
                throw new ArgumentNullException("Appointment is null");

            bool output = service.CancelAppointment(appointment.Id, appointment.Offer.TeacherId);
            Assert.True(output);
        }
        [Fact]
        public void CancelAppointmentStudentReturnsFalseIfStateIsInvalid()
        {
            
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var appointment = db.Appointments.FirstOrDefault(appoint => appoint.State == Model.AppointmentState.Confirmed);

            if (appointment == null)
                throw new ArgumentNullException("Appointment is null");

            bool output = service.CancelAppointment(appointment.Id, appointment.StudentId);
            Assert.False(output);
        }
        [Fact]
        public void CancelAppointmentCoachReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);

            var appointment = db.Appointments.FirstOrDefault(appoint => appoint.State == Model.AppointmentState.TookPlace);

            if (appointment == null || appointment.Offer.Teacher == null)
                throw new ArgumentNullException("Appointment is null");

            bool output = service.CancelAppointment(appointment.Id, appointment.Offer.TeacherId);
            Assert.False(output);
        }
    }
}
