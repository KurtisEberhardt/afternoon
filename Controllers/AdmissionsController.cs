using System;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using afternoon.Models;
using afternoon.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace afternoon.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AdmissionsController : ControllerBase
    {

        public AdmissionsService _service { get; set; }
        public AdmissionsController(AdmissionsService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<Admission> Get()
        {
            try
            {
                return Ok(_service.GetAll());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Admission> Get(int id)
        {
            try
            {
                return Ok(_service.GetById(id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Admission>> CreateAsync([FromBody] Admission newAdmission)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newAdmission.BuyerId = userInfo.Id;
                Admission created = _service.Create(newAdmission);
                //this is your 'populate' for create
                created.Buyer = userInfo;
                return Ok(created);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Admission>> EditAsync([FromBody] Admission editData, int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                editData.Id = id;
                editData.BuyerId = userInfo.Id;
                Admission editedAdmission = _service.Edit(editData);
                editedAdmission.Buyer = userInfo;
                return Ok(editedAdmission);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Admission>> DeleteAsync(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_service.Delete(id, userInfo.Id));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}