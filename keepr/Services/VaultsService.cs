using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
	public class VaultsService
	{
		private readonly VaultsRepository _repo;

		public VaultsService(VaultsRepository repo)
		{
			_repo = repo;
		}

		internal List<Vault> Get()
		{
			List<Vault> vaults = _repo.Get();
			vaults = vaults.FindAll(f => f.IsPrivate == false);
			return vaults;
		}
		internal Vault Get(int id)
		{
			Vault found = _repo.Get(id);
			if (found == null)
			{
				throw new Exception("Invalid Vault Id");
			}
			return found;
		}
		internal Vault Get(int vaultId, string userId)
		{
			Vault found = Get(vaultId);
			if (found.IsPrivate && found.CreatorId != userId)
			{
				throw new Exception("You are not authorized to view this vault");
			}
			return found;
		}

		internal Vault Create(Vault vaultData)
		{
			return _repo.Create(vaultData);
		}

		internal Vault Edit(Vault vaultData)
		{
			Vault original = Get(vaultData.Id);
			if (original.CreatorId != vaultData.CreatorId)
			{
				throw new Exception("You are not authorized to edit this vault");
			}
			original.Name = vaultData.Name ?? original.Name;
			original.Description = vaultData.Description ?? original.Description;
			_repo.Edit(original);
			return original;
		}
		internal List<Vault> GetVaultsByProfileId(string id, string userId)
		{
			List<Vault> vaults = _repo.GetVaultsByProfileId(id);
			if(id == userId) {
				return vaults;
			}
			vaults = vaults.FindAll(v => v.IsPrivate == false);
			return vaults;
		}

		internal void Delete(int id, string userId)
		{
			Vault original = Get(id);
			if (original.CreatorId != userId)
			{
				throw new Exception("You are not authorized to delete this vault");
			}
			_repo.Delete(id);
		}
	}
}