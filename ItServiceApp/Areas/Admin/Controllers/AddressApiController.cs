using System;
using System.Linq;
using DevExtreme.AspNet.Data;
using ItServiceApp.Data;
using ItServiceApp.Extensions;
using ItServiceApp.Models.Entities;
using ItServiceApp.ViewModels;
using Iyzipay.Model.V2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ItServiceApp.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AddressApiController : Controller
    {
        private readonly MyContext _dbContext;

        public AddressApiController(MyContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions options)
        {
            var data = _dbContext.Addresses;

            return Ok(DataSourceLoader.Load(data, options));
        }
        [HttpGet]
        public IActionResult Detail(Guid id, DataSourceLoadOptions loadOptions)
        {
            var data = _dbContext.Addresses.Where(x => x.Id == id);

            return Ok(DataSourceLoader.Load(data, loadOptions));
        }
        [HttpPost]
        public IActionResult Insert(string values)
        {
            var data = new Address();
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(new JsonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = ModelState.ToFullErrorString()
                });
            _dbContext.Addresses.Add(data);

            var result = _dbContext.SaveChanges();
            if (result == 0)
                return BadRequest(new JsonResponseViewModel
                {
                    IsSuccess = false,
                    ErrorMessage = "Yeni adres kaydedilemedi."
                });
            return Ok(new JsonResponseViewModel());
        }
        [HttpPut("update")]
        public IActionResult Update(Guid key, string values)
        {
            var data = _dbContext.Addresses.Find(key);
            if (data == null)
                return BadRequest(new JsonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = ModelState.ToFullErrorString()
                });

            JsonConvert.PopulateObject(values, data);
            if (!TryValidateModel(data))
                return BadRequest(ModelState.ToFullErrorString());

            var result = _dbContext.SaveChanges();
            if (result == 0)
                return BadRequest(new JsonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = "Adres güncellenemedi"
                });
            return Ok(new JsonResponseViewModel());
        }
    }
}
