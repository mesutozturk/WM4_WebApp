using DevExtreme.AspNet.Data;
using ItServiceApp.Data;
using ItServiceApp.Extensions;
using ItServiceApp.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using ItServiceApp.Business.Repository;
using ItServiceApp.Core.Entities;
using ItServiceApp.Core.ViewModels;

namespace ItServiceApp.Areas.Admin.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SubscriptionTypeApiController : Controller
    {
        private readonly SubscriptionTypeRepo _repo;

        public SubscriptionTypeApiController(SubscriptionTypeRepo repo)
        {
            _repo = repo;
        }

        #region Crud
        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions options)
        {
            var data = _repo.Get();

            return Ok(DataSourceLoader.Load(data, options));
        }
        [HttpGet]
        public IActionResult Detail(Guid id, DataSourceLoadOptions loadOptions)
        {
            var data = _repo.Get(x => x.Id == id);

            return Ok(DataSourceLoader.Load(data, loadOptions));
        }
        [HttpPost]
        public IActionResult Insert(string values)
        {
            var data = new SubscriptionType();
            JsonConvert.PopulateObject(values, data);

            if (!TryValidateModel(data))
                return BadRequest(new JsonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = ModelState.ToFullErrorString()
                });
            try
            {
                var result = _repo.Insert(data);
            }
            catch
            {
                return BadRequest(new JsonResponseViewModel
                {
                    IsSuccess = false,
                    ErrorMessage = "Yeni üyelik tipi kaydedilemedi."
                });
            }

            return Ok(new JsonResponseViewModel());
        }
        [HttpPut]
        public IActionResult Update(Guid key, string values)
        {
            var data = _repo.GetById(key);
            if (data == null)
                return BadRequest(new JsonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = ModelState.ToFullErrorString()
                });

            JsonConvert.PopulateObject(values, data);
            if (!TryValidateModel(data))
                return BadRequest(ModelState.ToFullErrorString());

            var result = _repo.Update(data);
            if (result == 0)
                return BadRequest(new JsonResponseViewModel()
                {
                    IsSuccess = false,
                    ErrorMessage = "Üyelik tipi güncellenemedi"
                });
            return Ok(new JsonResponseViewModel());
        }
        [HttpDelete]
        public IActionResult Delete(Guid key)
        {
            var data = _repo.GetById(key);
            if (data == null)
                return StatusCode(StatusCodes.Status409Conflict, "Üyelik tipi bulunamadı");

            var result = _repo.Delete(data.Id);
            if (result == 0)
                return BadRequest("Silme işlemi başarısız");
            return Ok(new JsonResponseViewModel());
        }
        #endregion
    }
}