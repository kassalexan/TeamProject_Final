using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TeamProject.Models;

namespace TeamProject.Controllers
{
    [Authorize]
    public class MapController : Controller
    {

        List<Spot> locations = new List<Spot>();

        public ActionResult Map()
        {
            locations = getSpots();
            var jsonSerialiser = new JavaScriptSerializer();
            string json = jsonSerialiser.Serialize(locations);
            ViewData["action"] = "Map";
            return View((object)json);
        }


        //[HttpPost]
        public ActionResult ReserveSpot(int id)
        {
            locations = getSpots();
            using (AppContext db = new AppContext())
            {
                var spot = db.Spots.Find(locations[id].SpotId);
                spot.State = "Reserved";
                db.SaveChanges();
            }
            LocationsIdVM locationsIdVM = new LocationsIdVM();
            locations = getSpots();
            var jsonSerialiser = new JavaScriptSerializer();
            string json = jsonSerialiser.Serialize(locations);
            locationsIdVM.Locations = json;
            locationsIdVM.Id = id;
            return View(locationsIdVM);
        }

        public ActionResult CanceledReservationMap(int id)
        {
            using (AppContext db = new AppContext())
            {
                var spot = db.Spots.Find(id);
                spot.State = "Available";
                db.SaveChanges();
            }

            locations = getSpots();
            var jsonSerialiser = new JavaScriptSerializer();
            string json = jsonSerialiser.Serialize(locations);
            ViewData["action"] = "Map";
            return View("Map", (object)json);
        }

        public ActionResult MissedReservationMap(int id)
        {
            using (AppContext db = new AppContext())
            {
                var spot = db.Spots.Find(id);
                spot.State = "Available";
                db.SaveChanges();
            }

            locations = getSpots();
            var jsonSerialiser = new JavaScriptSerializer();
            string json = jsonSerialiser.Serialize(locations);
            ViewData["action"] = "Map";
            return View("Map", (object)json);
        }

        public ActionResult Parked(int id)
        {
            using (AppContext db = new AppContext())
            {
                var spot = db.Spots.Find(id);
                spot.State = "NotAvailable";
                db.SaveChanges();
            }

            locations = getSpots();
            var jsonSerialiser = new JavaScriptSerializer();
            string json = jsonSerialiser.Serialize(locations);
            ViewData["action"] = "Map";
            return View("Map", (object)json);
        }

        private List<Spot> getSpots()
        {
            User user = new User();
            using (AppContext db = new AppContext())
            {
                user = db.Users.SingleOrDefault(i => i.Username == User.Identity.Name);
                locations = db.Spots.ToList();
            }

            if (User.IsInRole("2"))
            {
                return locations;
            }
            else
            {

                return locations.Where(i => i.District == user.District).ToList();
            }
            
        }

    }
}