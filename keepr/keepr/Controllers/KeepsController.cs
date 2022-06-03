using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class KeepsController : ControllerBase
	{
		private readonly KeepsService _ks;
		public KeepsController(KeepsService ks)
		{
			_ks = ks;
		}
		[HttpGet]
		public ActionResult<List<Keep>> Get()
		{
			try
			{
				List<Keep> keeps = _ks.Get();
				return Ok(keeps);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		[HttpGet("{id}")]
		public ActionResult<Keep> Get(int id)
		{
			try
			{
				Keep keep = _ks.Get(id);
				return Ok(keep);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Keep>> Create([FromBody] Keep keepData)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				keepData.CreatorId = userInfo.Id;
				Keep newKeep = _ks.Create(keepData);
				newKeep.Creator = userInfo;
				return Ok(newKeep);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<Keep>> Edit([FromBody] Keep keepData, int id)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				keepData.CreatorId = userInfo.Id;
				keepData.Id = id;
				Keep editedKeep = _ks.Edit(keepData);
				return Ok(editedKeep);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpDelete("{id}")]
		[Authorize]
		public async Task<ActionResult<String>> Delete(int id)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				_ks.Delete(id, userInfo.Id);
				return Ok("Keep Deleted");
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}