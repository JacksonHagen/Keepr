using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using keepr.Models;
using keepr.Services;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
	[ApiController]
	[Route("api/[controller]/{id}")]
	public class ProfilesController : ControllerBase
	{
		private readonly AccountService _as;
		private readonly VaultsService _vs;
		private readonly VaultKeepsService _vks;
		private readonly KeepsService _ks;

		public ProfilesController(AccountService accountService, VaultsService vs, VaultKeepsService vks, KeepsService ks)
		{
			_as = accountService;
			_vs = vs;
			_vks = vks;
			_ks = ks;
		}

		[HttpGet]
		public ActionResult<Profile> Get(string id)
		{
			try
			{
				Profile profile = _as.GetProfileById(id);
				return Ok(profile);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
		[HttpGet("keeps")]
		public ActionResult<List<Keep>> GetKeeps(string id)
		{
			try
			{
				_as.GetProfileById(id);
				List<Keep> keeps = _ks.GetKeepsByProfileId(id);
				return Ok(keeps);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}

		[HttpGet("vaults")]
		public async Task<ActionResult<List<Vault>>> GetVaults(string id)
		{
			try
			{
				_as.GetProfileById(id);
				Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
				List<Vault> vaults = _vs.GetVaultsByProfileId(id, userInfo?.Id);
				return Ok(vaults);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
		}
	}
}