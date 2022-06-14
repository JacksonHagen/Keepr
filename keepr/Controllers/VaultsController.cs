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
	public class VaultsController : ControllerBase
	{
		private readonly VaultsService _vs;
		private readonly VaultKeepsService _vks;

		public VaultsController(VaultsService vs, VaultKeepsService vks)
		{
			_vs = vs;
			_vks = vks;
		}

		[HttpGet]
		public ActionResult<List<Vault>> Get()
		{
			try
			{
				List<Vault> vaults = _vs.Get();
				return Ok(vaults);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Vault>> Get(int id)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				Vault vault = _vs.Get(id, userInfo?.Id);
				return Ok(vault);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet("{id}/keeps")]
		public async Task<ActionResult<List<VaultKeepViewModel>>> GetKeeps(int id)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				List<VaultKeepViewModel> keeps = _vks.GetKeeps(id, userInfo?.Id);
				return Ok(keeps);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<Vault>> Create([FromBody] Vault vaultData)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				vaultData.CreatorId = userInfo.Id;
				Vault newVault = _vs.Create(vaultData);
				newVault.Creator = userInfo;
				return Ok(newVault);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpPut("{id}")]
		[Authorize]
		public async Task<ActionResult<Vault>> Edit([FromBody] Vault vaultData, int id)
		{
			try
			{
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				vaultData.CreatorId = userInfo.Id;
				vaultData.Id = id;
				Vault editedVault = _vs.Edit(vaultData);
				return Ok(editedVault);
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
				_vs.Delete(id, userInfo.Id);
				return Ok("Vault Deleted");
			}
			catch(Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}