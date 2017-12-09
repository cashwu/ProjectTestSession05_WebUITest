using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Lab.Repository;
using Lab.WebApplication.Models;
using PagedList;

namespace Lab.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private IWifiSpotRepository WifiSpotRepository { get; set; }

        public HomeController(IWifiSpotRepository wifiSpotRepository)
        {
            this.WifiSpotRepository = wifiSpotRepository;
        }

        /// <summary>
        /// Indexes the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="districts">The district.</param>
        /// <param name="types">The type.</param>
        /// <param name="companys">The company.</param>
        /// <returns>ActionResult.</returns>
        public ActionResult Index(int? page,
                                  string districts,
                                  string types,
                                  string companys)
        {
            // 區域
            var allDistricts = this.WifiSpotRepository.GetDistricts();
            ViewBag.Districts = this.GetSelectList(allDistricts, districts);
            ViewBag.SelectedDistrict = districts;

            // 熱點分類
            var allTypes = this.WifiSpotRepository.GetTypes();
            var typeSelectList = this.GetSelectList(allTypes, types);
            ViewBag.Types = typeSelectList.ToList();
            ViewBag.SelectedType = types;

            // 業者
            var allCompanies = this.WifiSpotRepository.GetCompanies();
            var companySelectList = this.GetSelectList(allCompanies, companys);
            ViewBag.Companys = companySelectList.ToList();
            ViewBag.SelectedCompany = companys;

            var models = this.WifiSpotRepository.GetByCondition(districts, types, companys);

            int pageIndex = page ?? 1;
            int pageSize = 10;
            int totalCount = models.Count;

            var source = models.OrderBy(x => x.District)
                               .Skip((pageIndex - 1) * pageSize)
                               .Take(pageSize);

            var pageData = Mapper.Map<List<WifiSpotModel>, List<WifiSpot>>(source.ToList());

            var pagedResult = new StaticPagedList<WifiSpot>
            (
                pageData,
                pageIndex,
                pageSize,
                totalCount
            );

            return View(pagedResult);
        }

        /// <summary>
        /// 產生下拉選單項目.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="selectedItem">The selected item.</param>
        /// <returns>List&lt;SelectListItem&gt;.</returns>
        private List<SelectListItem> GetSelectList(IEnumerable<string> source,
                                                   string selectedItem)
        {
            var selectList = source.Select
            (
                item => new SelectListItem
                {
                    Text = item,
                    Value = item,
                    Selected = !string.IsNullOrWhiteSpace(selectedItem)
                               &&
                               item.Equals(selectedItem, StringComparison.OrdinalIgnoreCase)
                }
            );

            return selectList.ToList();
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
    }
}