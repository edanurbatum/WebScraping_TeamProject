using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebScraping.Models.HelperModels;
using WebScraping.Models.Siniflar;

namespace WebScraping.Controllers
{
    public class HomeController : Controller
    {
        WebScrapingDBEntities db = new WebScrapingDBEntities();

        //Hepsiemlak ve emlakjetten çekilen verilerin anasayfada listelenmesi için sayfalama yapılır.
        //Çekilen veriler her sayfada 10'ar tane veri olacak şekilde listeleniyor.
        //Çekilen veri sayısı arttıkça sayfa sayısı da 1 den başlayarak ve artarak sayfalama yapılır.

        public ActionResult Index(int? page)
        {
            var getHome = db.Homes.Where(a => !a.image.Contains("width")).ToList();
            var getHomeEmlakjet = db.HomesSahibinden.Where(a => !a.image.Contains(" ")).ToList();

            List<KiralikEvList> list = new List<KiralikEvList>();

            foreach (var item in getHome)
            {
                list.Add(new KiralikEvList
                {
                    ID = item.ID,
                    age = item.age,
                    bathroom = item.bathroom,
                    city = item.city,
                    county = item.county,
                    district = item.district,
                    floor = item.floor,
                    formHousing = item.formHousing,
                    heating = item.heating,
                    image = item.image,
                    isFurnished = item.isFurnished,
                    link = item.link,
                    metre = item.metre,
                    price = item.price,
                    room = item.room,
                    totalFloor = item.totalFloor
                });
            }

            foreach (var itemjet in getHomeEmlakjet)
            {
                list.Add(new KiralikEvList
                {
                    ID = itemjet.ID,
                    age = itemjet.age,
                    bathroom = itemjet.bathroom,
                    city = itemjet.city,
                    county = itemjet.county,
                    district = itemjet.district,
                    floor = itemjet.floor,
                    formHousing = itemjet.formHousing,
                    heating = itemjet.heating,
                    image = itemjet.image,
                    isFurnished = itemjet.isFurnished,
                    link = itemjet.link,
                    metre = itemjet.metre,
                    price = itemjet.price,
                    room = itemjet.room,
                    totalFloor = itemjet.totalFloor
                });
            }

            List<KiralikEvList> listSlider = new List<KiralikEvList>();
            var getHomeSlider = db.Homes.Where(a => !a.image.Contains("width")).OrderBy(a => a.ID).Skip(76).ToList();
            var getHomeEmlakjetSlider = db.HomesSahibinden.Where(a => !a.image.Contains(" ")).OrderBy(a => a.ID).Skip(120).ToList();

            foreach (var item in getHomeSlider)
            {
                listSlider.Add(new KiralikEvList
                {
                    ID = item.ID,
                    age = item.age,
                    bathroom = item.bathroom,
                    city = item.city,
                    county = item.county,
                    district = item.district,
                    floor = item.floor,
                    formHousing = item.formHousing,
                    heating = item.heating,
                    image = item.image,
                    isFurnished = item.isFurnished,
                    link = item.link,
                    metre = item.metre,
                    price = item.price,
                    room = item.room,
                    totalFloor = item.totalFloor
                });
            }

            foreach (var itemjet in getHomeEmlakjetSlider)
            {
                listSlider.Add(new KiralikEvList
                {
                    ID = itemjet.ID,
                    age = itemjet.age,
                    bathroom = itemjet.bathroom,
                    city = itemjet.city,
                    county = itemjet.county,
                    district = itemjet.district,
                    floor = itemjet.floor,
                    formHousing = itemjet.formHousing,
                    heating = itemjet.heating,
                    image = itemjet.image,
                    isFurnished = itemjet.isFurnished,
                    link = itemjet.link,
                    metre = itemjet.metre,
                    price = itemjet.price,
                    room = itemjet.room,
                    totalFloor = itemjet.totalFloor
                });
            }

            ViewBag.Slider = listSlider;

            var ilList = db.il.OrderBy(a => a.il1).ToList();
            
            ViewBag.il = ilList;

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //Karşılaştırma yapmak için seçilen ev ilanlarının özelliklerinin karşılaştırılması yapılırken geriye JSON veri döndürür.
        public JsonResult Karsilastirma(string ev1, string ev2)
        {
            var evDetay1 = ev1.Split('-');
            var evDetay2 = ev2.Split('-');

            var evid1 = Convert.ToInt32(evDetay1[0]);
            var evid2 = Convert.ToInt32(evDetay2[0]);

            var evmetre1 = Convert.ToInt32(evDetay1[4]);
            var evmetre2 = Convert.ToInt32(evDetay2[4]);

            var evcity1 = evDetay1[1];
            var evcity2 = evDetay2[1];

            var evcounty1 = evDetay1[2];
            var evcounty2 = evDetay2[2];


            Homes evDetayBilgileri1 = new Homes();
            Homes evDetayBilgileri2 = new Homes();

            HomesSahibinden evDetayBilgileriSahibinden1 = new HomesSahibinden();
            HomesSahibinden evDetayBilgileriSahibinden2 = new HomesSahibinden();

            evDetayBilgileri1 = db.Homes.FirstOrDefault(a => a.ID == evid1 && a.city.ToLower().Contains(evcity1.ToLower()) && a.county.ToLower().Contains(evcounty1.ToLower()) && a.metre == evmetre1);
            evDetayBilgileri2 = db.Homes.FirstOrDefault(a => a.ID == evid2 && a.city.ToLower().Contains(evcity2.ToLower()) && a.county.ToLower().Contains(evcounty2.ToLower()) && a.metre == evmetre2);

            evDetayBilgileriSahibinden1 = db.HomesSahibinden.FirstOrDefault(a => a.ID == evid1 && a.city.ToLower().Contains(evcity1.ToLower()) && a.county.ToLower().Contains(evcounty1.ToLower()) && a.metre == evmetre1);
            evDetayBilgileriSahibinden2 = db.HomesSahibinden.FirstOrDefault(a => a.ID == evid2 && a.city.ToLower().Contains(evcity2.ToLower()) && a.county.ToLower().Contains(evcounty2.ToLower()) && a.metre == evmetre2);


            var home = evDetayBilgileri1 == null ? evDetayBilgileri2 : evDetayBilgileri1;
            var homesahibinden = evDetayBilgileriSahibinden1 == null ? evDetayBilgileriSahibinden2 : evDetayBilgileriSahibinden1;

            if (evDetayBilgileri1 != null)
            {
                home = evDetayBilgileri1;

                if (evDetayBilgileri2 != null)
                {
                    HomesSahibinden h = new HomesSahibinden
                    {
                        ID = evDetayBilgileri2.ID,
                        age = evDetayBilgileri2.age,
                        bathroom = evDetayBilgileri2.bathroom,
                        city = evDetayBilgileri2.city,
                        county = evDetayBilgileri2.county,
                        district = evDetayBilgileri2.district,
                        floor = evDetayBilgileri2.floor,
                        formHousing = evDetayBilgileri2.formHousing,
                        heating = evDetayBilgileri2.heating,
                        image = evDetayBilgileri2.image,
                        isFurnished = evDetayBilgileri2.isFurnished,
                        link = evDetayBilgileri2.link,
                        metre = evDetayBilgileri2.metre,
                        price = evDetayBilgileri2.price,
                        room = evDetayBilgileri2.room,
                        totalFloor = evDetayBilgileri2.totalFloor
                    };

                    homesahibinden = h;

                }
            }
            else
            {
                if (evDetayBilgileri2 != null)
                {
                    home = evDetayBilgileri2;
                }
            }

            if (evDetayBilgileriSahibinden1 != null)
            {
                homesahibinden = evDetayBilgileriSahibinden1;

                if (evDetayBilgileriSahibinden2 != null)
                {
                    Homes h = new Homes
                    {
                        ID = evDetayBilgileriSahibinden2.ID,
                        age = evDetayBilgileriSahibinden2.age,
                        bathroom = evDetayBilgileriSahibinden2.bathroom,
                        city = evDetayBilgileriSahibinden2.city,
                        county = evDetayBilgileriSahibinden2.county,
                        district = evDetayBilgileriSahibinden2.district,
                        floor = evDetayBilgileriSahibinden2.floor,
                        formHousing = evDetayBilgileriSahibinden2.formHousing,
                        heating = evDetayBilgileriSahibinden2.heating,
                        image = evDetayBilgileriSahibinden2.image,
                        isFurnished = evDetayBilgileriSahibinden2.isFurnished,
                        link = evDetayBilgileriSahibinden2.link,
                        metre = evDetayBilgileriSahibinden2.metre,
                        price = evDetayBilgileriSahibinden2.price,
                        room = evDetayBilgileriSahibinden2.room,
                        totalFloor = evDetayBilgileriSahibinden2.totalFloor
                    };

                    home = h;
                }
            }
            else
            {
                if (evDetayBilgileriSahibinden2 != null)
                {
                    homesahibinden = evDetayBilgileriSahibinden2;
                }
            }


            TempData["homes"] = home;
            TempData["homessahibinden"] = homesahibinden;

            return Json(true, JsonRequestBehavior.AllowGet);

        }

    }
}