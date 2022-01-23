using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2
{
    public class AppointmentService
    {
        private readonly AppointmentContext _db;

        public AppointmentService(AppointmentContext db)
        {
            _db = db;
        }

        public bool AskForAppointment(Guid offerId, Guid studentId, DateTime date)
        {
            // TOTO: Implementiere die Methode

            Offer offer = _db.Offers.FirstOrDefault(offer => offer.Id == offerId);
            if (offer == null)
            {
                return false;
            }
            if (offer.From > date || offer.To < date)
            {
                return false;
            }
            Student student = _db.Students.FirstOrDefault(student => student.Id == studentId);
            if (student == null)
            {
                return false;
            }
            Appointment appointment = new Appointment()
            {
                Id = Guid.NewGuid(),
                Student = student,
                Location = "Vienna",
                Date = date,
                Offer = offer,
                State = AppointmentState.AskedFor
            };
            var output = _db.Appointments.Add(appointment);
            try
            {
                int numberofinserted = _db.SaveChanges();
                return numberofinserted == 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool ConfirmAppointment(Guid appointmentId)
        {
            Appointment appointment = _db.Appointments.FirstOrDefault(appointment => appointment.Id == appointmentId);
            if (appointment == null || appointment.State != AppointmentState.AskedFor)
            {
                return false;
            }
            appointment.State = AppointmentState.Confirmed;

            try
            {
                int numberofinserted = _db.SaveChanges();
                return numberofinserted == 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public bool CancelAppointment(Guid appointmentId, Guid studentId)
        {
            // TOTO: Implementiere die Methode
            Appointment appointment = _db.Appointments.FirstOrDefault(appointment => appointment.Id == appointmentId);
            Student student = _db.Students.FirstOrDefault(student => student.Id == studentId);
            
            if (appointment == null || student == null)
            {
                return false;
            }
            if(student is Coach)
            {
                if (appointment.State != AppointmentState.AskedFor && appointment.State != AppointmentState.Confirmed)
                    return false;
            }
            else
            {
                if (appointment.State != AppointmentState.AskedFor)
                    return false;
            }


            appointment.State = AppointmentState.Cancelled;
            try
            {
                int numberofinserted = _db.SaveChanges();
                return numberofinserted == 1;
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
