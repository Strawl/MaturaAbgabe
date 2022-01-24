using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe2;
using SPG_Fachtheorie.Aufgabe2.Model;
using SPG_Fachtheorie.Aufgabe3Mvc.Services;
using System;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe3Mvc.Controllers
{

    public class OfferController : Controller
    {
        private readonly AuthService _authService;
        private readonly AppointmentContext _appointmentContext;

        public OfferController(AuthService authService, AppointmentContext appointmentContext)
        {
            _authService = authService;
            _appointmentContext = appointmentContext;
        }

        private Guid? IsCoach()
        {
            var coach = _appointmentContext.Students.ToList().Find(person => person.Username == _authService.Username);
            if (coach != null && coach is Coach)
                return coach.Id;
            return null;
        }

        // /Offers/Index
        [Authorize()]
        public IActionResult Index()
        {
            Guid? coachId = IsCoach();

            var context = _appointmentContext.Offers
                .Where(offer => offer.TeacherId == coachId)
                .Include(x => x.Subject)
                .Include(x => x.Teacher);
            return View(context);
        }

        // /Offer/Details/{offerGuid}
        [Authorize()]
        public IActionResult Details(Guid? id)
        {
            Guid? coachId = IsCoach();
            if (id == null || coachId == null)
                return Redirect("/");

            var appointment = _appointmentContext.Appointments
                .Include(i => i.Student)
                .Where(x => x.OfferId == id);

            return View(appointment);
        }

        [Authorize()]
        public IActionResult Add()
        {
            Guid? coachId = IsCoach();
            if (coachId == null)
                return Redirect("/");

            return View();
        }
    }
}
