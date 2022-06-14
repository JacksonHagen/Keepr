using System;
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
	public class VaultKeepsController : ControllerBase
	{
		private readonly VaultKeepsService _vks;

		public VaultKeepsController(VaultKeepsService vks)
		{
			_vks = vks;
		}
		[HttpPost]
		[Authorize]
		public async Task<ActionResult<VaultKeep>> Create([FromBody] VaultKeep vk)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				vk.CreatorId = userInfo.Id;
				VaultKeep newVaultKeep = _vks.Create(vk);
				return Ok(newVaultKeep);
			}
			catch(Exception e)
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
				_vks.Delete(id, userInfo.Id);
				return Ok("Successfully deleted");
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}