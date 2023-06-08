using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebScraping.Models.HelperModels;
using WebScraping.Models.Siniflar;

namespace WebScraping.Controllers
{
    public class EmploymentController : Controller
    {
        // GET: Employment
        WebScrapingDBEntities db = new WebScrapingDBEntities();

        public ActionResult Index(Homes model)
        {
            List<Homes> homesAllDatas = new List<Homes>();

            //Verileri okunulması istenilen web sitesinin url’ i string veri tipine dönüştürülür.
            //Url içerisindeki Xpath alanları bulunarak o alanların htmlnode olarak gelmesi sağlanır.
            //Node’ların içerisinde istenilen veriler bulunmaktadır. Bu şekilde veriler görüntülenebilir ve kaydedilebilir.

            //hepsiemlaktan çekilen veriler buradan çekiliyor.

            List<Homes> homes = new List<Homes>();
            for (int i = 3; i < 36; i++)
            {
                var web = new HtmlWeb();
                var doc = web.Load("https://www.hepsiemlak.com/kiralik?page=" + i);
                foreach (var item in doc.DocumentNode.SelectNodes("//div[@class='list-view-line']"))
                {
                    int price = 0;

                    if (item.ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText == "GBP")
                    {
                        price = Convert.ToInt32(item.ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText.Trim().Replace("GBP", "").TrimEnd().Replace(".", ""));
                    }
                    else
                    {
                        price = Convert.ToInt32(item.ChildNodes[2].ChildNodes[2].ChildNodes[0].ChildNodes[0].InnerText.Trim().Replace("TL", "").TrimEnd().Replace(".", ""));
                    }
                    string room = item.ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                    int metre = Convert.ToInt32(item.ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[4].ChildNodes[0].InnerText.Trim().Replace("m2", "").TrimEnd().Replace(".", "").Split(' ')[0]);
                    string link = "https://www.hepsiemlak.com" + item.ChildNodes[2].ChildNodes[0].ChildNodes[0].GetAttributeValue("href", "").Trim();
                    string img = item.ChildNodes[0].ChildNodes[2].ChildNodes[2].InnerHtml.Replace("data-src=", "~").Replace("width", "~width").Replace("\"", "").Split('~')[1].Trim();

                    #region Floor


                    int floor = 0;
                    var floorDeger = item.ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[8].ChildNodes[0].InnerText.Trim();
                    var katDeger = floorDeger.Split('.')[0];

                    if (isInt(katDeger) == false)
                    {
                        floor = 0;
                    }
                    else if (isInt(katDeger) == true)
                    {
                        floor = Convert.ToInt32(katDeger);
                    }
                    else
                    {
                        floor = 0;
                    }


                    #endregion

                    #region Age

                    int age = 0;
                    string ageDeger = item.ChildNodes[2].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[2].ChildNodes[6].ChildNodes[0].InnerText.Trim();

                    if (ageDeger == "Sıfır Bina")
                        age = 100;
                    else
                    {
                        age = Convert.ToInt32(ageDeger.Split(' ')[0]);
                    }

                    #endregion

                    var doc2 = web.Load(link);

                    foreach (var item2 in doc2.DocumentNode.SelectNodes("//div[@class='cont-inner']"))
                    {
                        var shortcut = item2.ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[1];
                        var shortcut1 = item2.ChildNodes[0].ChildNodes[6].ChildNodes[2].ChildNodes[0];
                        string heating = "";
                        int totalFloor = 0;
                        string isFurnished = "";
                        string formHousing = "";
                        int bathroom = 0;

                        int sınır = shortcut.ChildNodes.Count;
                        int sınır1 = shortcut1.ChildNodes.Count;

                        if (sınır > sınır1)
                        {
                            sınır = sınır1;
                        }
                        for (int j = 0; j < sınır; j++)
                        {
                            var ana = shortcut.ChildNodes[j].ChildNodes[0].ChildNodes[0].InnerText.Trim();
                            var ana1 = shortcut1.ChildNodes[j].ChildNodes[0].ChildNodes[0].InnerText.Trim();

                            if (ana == "Isınma Tipi")
                            {
                                heating = shortcut.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                            }
                            else if (ana == "Kat Sayısı")
                            {
                                totalFloor = Convert.ToInt32(shortcut.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim().Split(' ')[0]);
                            }
                            else if (ana == "Eşya Durumu")
                            {
                                isFurnished = shortcut.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                            }
                            else if (ana == "Konut Şekli")
                            {
                                formHousing = shortcut.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                            }
                            else if (ana == "Banyo Sayısı")
                            {
                                bathroom = Convert.ToInt32(shortcut.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim().Split('+')[0]);
                            }

                            if (ana1 == "Isınma Tipi")
                            {
                                heating = shortcut1.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                            }
                            else if (ana1 == "Kat Sayısı")
                            {
                                totalFloor = Convert.ToInt32(shortcut1.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim().Split(' ')[0]);
                            }
                            else if (ana1 == "Eşya Durumu")
                            {
                                isFurnished = shortcut1.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                            }
                            else if (ana1 == "Konut Şekli")
                            {
                                formHousing = shortcut1.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                            }

                            else if (ana1 == "Banyo Sayısı")
                            {
                                bathroom = Convert.ToInt32(shortcut1.ChildNodes[j].ChildNodes[2].ChildNodes[0].InnerText.Trim().Split('+')[0]);
                            }
                        }

                        string city = item2.ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText.Trim();
                        string county = item2.ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[2].ChildNodes[0].InnerText.Trim();
                        string district = item2.ChildNodes[0].ChildNodes[4].ChildNodes[0].ChildNodes[4].ChildNodes[0].InnerText.Trim();

                        homes.Add(new Homes()
                        {
                            price = price,
                            city = city,
                            county = county,
                            age = age,
                            floor = floor,
                            room = room,
                            metre = metre,
                            district = district,
                            totalFloor = totalFloor,
                            heating = heating,
                            isFurnished = isFurnished,
                            formHousing = formHousing,
                            bathroom = bathroom,
                            link = link,
                            image = img
                        }); ;
                        break;
                    }
                }
            }

            //Emlakjetten çekilen veriler buradan çekiliyor.

            List<HomesSahibinden> homesEmlakJet = new List<HomesSahibinden>();
            for (int i = 1; i < 20; i++)
            {
                var web = new HtmlWeb();
                var doc = web.Load("https://www.emlakjet.com/kiralik-konut/" + i);

                foreach (var item in doc.DocumentNode.SelectNodes("//div[@class='styles_listingItem__1asTK']"))
                {
                    int price = Convert.ToInt32(item.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText.Trim().Replace(".", ""));
                    string link = "https://www.emlakjet.com/" + item.ChildNodes[0].GetAttributeValue("href", "").Trim();
                    string img = item.ChildNodes[0].ChildNodes[1].ChildNodes[0].GetAttributeValue("data-src", "").Trim();
                    int metre = Convert.ToInt32(item.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[3].ChildNodes[1].InnerText.Trim().Replace("m2", "").TrimEnd().Replace(".", "").Split(' ')[0]);
                    string room = item.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[1].ChildNodes[1].InnerText.Trim();
                    string formHousing = item.ChildNodes[0].ChildNodes[2].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[1].InnerText.Trim();

                    var doc2 = web.Load(link);
                    foreach (var item2 in doc2.DocumentNode.SelectNodes("//div[@class='ej64 ej100 ej156']"))
                    {
                        string citycountydistrict = item2.ChildNodes[3].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[1].ChildNodes[0].InnerText.Trim();

                        string[] ayrac;
                        ayrac = citycountydistrict.Split('-');

                        string city = ayrac[0].Trim();
                        string county = ayrac[1].Trim();
                        string district = ayrac[2].Trim();

                        var shortcut = item2.ChildNodes[4].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[0];
                        var shortcut1 = item2.ChildNodes[4].ChildNodes[0].ChildNodes[1].ChildNodes[0].ChildNodes[0].ChildNodes[1];
                        string heating = "";
                        int totalFloor = 0;
                        string isFurnished = "";
                        int bathroom = 0;
                        int age = 0;
                        int floor = 0;

                        int sınır = shortcut.ChildNodes.Count;
                        int sınır1 = shortcut1.ChildNodes.Count;

                        if (sınır > sınır1)
                        {
                            sınır = sınır1;
                        }

                        for (int j = 0; j < sınır; j++)
                        {
                            var ana = shortcut.ChildNodes[j].ChildNodes[0].ChildNodes[0].InnerText.Trim();
                            var ana1 = shortcut1.ChildNodes[j].ChildNodes[0].ChildNodes[0].InnerText.Trim();

                            if (ana == "Isıtma Tipi")
                            {
                                var heatingDeger = shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim();
                                if (heatingDeger == "Klimalı")
                                    heating = "Klima";
                                else
                                    heating = heatingDeger;
                            }
                            else if (ana == "Binanın Kat Sayısı")
                            {
                                totalFloor = Convert.ToInt32(shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim());
                            }
                            else if (ana == "Eşya Durumu")
                            {
                                var isFurnishedDeger = shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim();
                                if (isFurnishedDeger == "Boş")
                                    isFurnished = "Eşyalı Değil";
                                else
                                    isFurnished = isFurnishedDeger;
                            }
                            else if (ana == "Banyo Sayısı")
                            {

                                if (shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim() == "Yok")
                                    bathroom = 0;
                                else
                                    bathroom = Convert.ToInt32(shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim().Split('+')[0]);
                            }
                            else if (ana == "Binanın Yaşı")
                            {
                                //age = Convert.ToInt32(shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim());
                                #region Age

                                string ageDeger = shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim();

                                if (ageDeger == "Sıfır Bina")
                                    age = 100;
                                else
                                {
                                    if (ageDeger.Contains('-'))
                                        age = Convert.ToInt32(ageDeger.Split('-')[0]);
                                    else
                                        age = Convert.ToInt32(ageDeger.Split(' ')[0]);
                                }

                                #endregion
                            }
                            else if (ana == "Bulunduğu Kat")
                            {
                                floor = Convert.ToInt32(shortcut.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim());
                            }


                            if (ana1 == "Isıtma Tipi")
                            {
                                var heatingDeger = shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim();
                                if (heatingDeger == "Klimalı")
                                    heating = "Klima";
                                else
                                    heating = heatingDeger;
                            }
                            else if (ana1 == "Binanın Kat Sayısı")
                            {
                                totalFloor = Convert.ToInt32(shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim());
                            }
                            else if (ana1 == "Eşya Durumu")
                            {
                                var isFurnishedDeger = shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim();
                                if (isFurnishedDeger == "Boş")
                                    isFurnished = "Eşyalı Değil";
                                else
                                    isFurnished = isFurnishedDeger;
                            }
                            else if (ana1 == "Banyo Sayısı")
                            {
                                bathroom = Convert.ToInt32(shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim().Split('+')[0]);
                            }
                            else if (ana1 == "Binanın Yaşı")
                            {
                                age = Convert.ToInt32(shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim());
                            }
                            else if (ana1 == "Bulunduğu Kat")
                            {
                                //floor = Convert.ToInt32(shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim());

                                #region Floor

                                var floorDeger = shortcut1.ChildNodes[j].ChildNodes[1].ChildNodes[0].InnerText.Trim();
                                var katDeger = floorDeger.Split('.')[0];

                                if (isInt(katDeger) == false)
                                {
                                    floor = 0;
                                }
                                else if (isInt(katDeger) == true)
                                {
                                    floor = Convert.ToInt32(katDeger);
                                }
                                else
                                {
                                    floor = 0;
                                }


                                #endregion
                            }
                        }

                        homesEmlakJet.Add(new HomesSahibinden()
                        {
                            price = price,
                            city = city,
                            county = county,
                            age = age,
                            floor = floor,
                            room = room,
                            metre = metre,
                            district = district,
                            totalFloor = totalFloor,
                            heating = heating,
                            isFurnished = isFurnished,
                            formHousing = formHousing,
                            bathroom = bathroom,
                            link = link,
                            image = img
                        }); ;
                        break;
                    }
                }
            }

            //Veritabanına kayıt işlemleri

            //Önce eski kayıtlar silinir
            var getHome = db.Homes.ToList();
            var getHomeSahibinden = db.HomesSahibinden.ToList();

            db.Homes.RemoveRange(getHome);
            db.HomesSahibinden.RemoveRange(getHomeSahibinden);

            //Sonra yeni kayıtlar eklenir.
            db.Homes.AddRange(homes);
            db.HomesSahibinden.AddRange(homesEmlakJet);
            if (db.SaveChanges() > 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

            //homesAllDatas.AddRange(homes);
            //homesAllDatas.AddRange(homesSahibinden);

            return View(homesAllDatas);
        }

        //Burada kullanılan JSON metodu ile il bazında filtreleme yapıldığında seçilen ilin ilçelerinin listelenmesi sağlanıyor.
        public JsonResult Il(long ilId)
        {
            var ilce = db.ilce.Where(a => a.ilId == ilId).OrderBy(a => a.ilce1).ToList();

            return Json(ilce, JsonRequestBehavior.AllowGet);
        }
        //Burada kullanılan JSON metodu ile ilçe bazında filtreleme yapıldığında seçilen ilçenin mahallelerinin listelenmesi sağlanıyor.
        public JsonResult Ilce(long ilceId)
        {
            var mahalle = db.mahalle.Where(a => a.ilceId == ilceId).OrderBy(a => a.mahalle1).ToList();

            return Json(mahalle, JsonRequestBehavior.AllowGet);
        }

        //Post metodu ile istenilen ev özellerinin bulunduğu ilanlar filtreleme işleminden sonra getirilir. 
        [HttpPost]
        public JsonResult EvAra(string il, string ilce, string mahalle, string minfiyat,
            string maxfiyat, string minmk, string maxmk, string oda,
            string yas, string kat, string totalkat, string isinma,
            string banyo, string esya)
        {
            var _minfiyat = Convert.ToInt32(minfiyat == "" ? "0" : minfiyat);
            var _maxfiyat = Convert.ToInt32(maxfiyat == "" ? "0" : maxfiyat);
            var _minmk = Convert.ToInt32(minmk == "" ? "0" : minmk);
            var _maxmk = Convert.ToInt32(maxmk == "" ? "0" : maxmk);
            var _yas = Convert.ToInt32(yas == "" ? "0" : yas);
            var _kat = Convert.ToInt32(kat == "" ? "0" : kat);
            var _totalkat = Convert.ToInt32(totalkat == "" ? "0" : totalkat);
            var _banyo = Convert.ToInt32(banyo == "" ? "0" : banyo);


            List<Homes> homeAllListLast = db.Homes.ToList();
            List<HomesSahibinden> homeTwiceAllListLast = db.HomesSahibinden.ToList();

            if (il != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.city.ToLower().Contains(il.ToLower())).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.city.ToLower().Contains(il.ToLower())).ToList();
            }
            if (ilce != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.county.ToLower().Contains(ilce.ToLower())).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.county.ToLower().Contains(ilce.ToLower())).ToList();
            }
            if (mahalle != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.district.ToLower().Contains(mahalle.ToLower())).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.district.ToLower().Contains(mahalle.ToLower())).ToList();
            }
            if (minfiyat != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.price >= _minfiyat).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.price >= _minfiyat).ToList();
            }
            if (minfiyat != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.price <= _maxfiyat).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.price <= _maxfiyat).ToList();
            }
            if (minmk != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.metre >= _minmk).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.metre >= _minmk).ToList();
            }
            if (maxmk != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.metre <= _maxmk).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.metre <= _maxmk).ToList();
            }
            if (oda != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.room.ToLower().Contains(oda.ToLower())).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.room.ToLower().Contains(oda.ToLower())).ToList();
            }
            if (yas != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.age == _yas).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.age == _yas).ToList();
            }
            if (kat != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.floor == _kat).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.floor == _kat).ToList();
            }
            if (totalkat != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.totalFloor == _totalkat).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.totalFloor == _totalkat).ToList();
            }
            if (isinma != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.heating.ToLower().Contains(isinma.ToLower())).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.heating.ToLower().Contains(isinma.ToLower())).ToList();
            }
            if (banyo != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.bathroom == _banyo).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.bathroom == _banyo).ToList();
            }
            if (esya != "")
            {
                homeAllListLast = homeAllListLast.Where(a => a.isFurnished.ToLower().Contains(esya.ToLower())).ToList();
                homeTwiceAllListLast = homeTwiceAllListLast.Where(a => a.isFurnished.ToLower().Contains(esya.ToLower())).ToList();
            }


            List<KiralikEvList> totalList = new List<KiralikEvList>();

            foreach (var item in homeAllListLast)
            {
                totalList.Add(new KiralikEvList
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

            foreach (var itemjet in homeTwiceAllListLast)
            {
                totalList.Add(new KiralikEvList
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

            return Json(totalList, JsonRequestBehavior.AllowGet);
        }

        //Karşılaştır butonuna basıldığında seçilen iki ilanın özelliklerinin karşılaştırılması yapılır.
        public ActionResult Karsilastir()
        {
            ViewBag.Homes = TempData["homes"] as Homes;
            ViewBag.HomesSahibinden = TempData["homessahibinden"] as HomesSahibinden;

            var ilList = db.il.OrderBy(a => a.il1).ToList();

            ViewBag.il = ilList;

            return View("Karsilastir");
        }

        //Gelen değerin integer değeri olup olmadığının kontrolünü yapar. Bu metoda price verilerini çekerken ihtiyacımız oldu. 
        bool isInt(string deger)
        {
            try
            {
                int.Parse(deger);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}