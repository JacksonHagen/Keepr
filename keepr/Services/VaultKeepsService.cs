using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
	public class VaultKeepsService
	{
		private readonly VaultKeepsRepository _repo;
		private readonly VaultsRepository _vRepo;
		private readonly KeepsRepository _kRepo;

		public VaultKeepsService(VaultKeepsRepository repo, VaultsRepository vRepo, KeepsRepository kRepo)
		{
			_repo = repo;
			_vRepo = vRepo;
			_kRepo = kRepo;
		}

		private VaultKeep Get(int id)
		{
			VaultKeep found = _repo.Get(id);
			if (found == null)
			{
				throw new Exception("Invalid Id");
			}
			return found;
		}

		internal VaultKeep Create(VaultKeep vk)
		{
			Vault vault = _vRepo.Get(vk.VaultId);
			if (vault.CreatorId != vk.CreatorId)
			{
				throw new Exception("You are not authorized to add a keep to this vault");
			}
			List<VaultKeepViewModel> vaultKeeps = _repo.GetKeeps(vk.VaultId);
			vaultKeeps = vaultKeeps?.FindAll(v => v.Id == vk.KeepId);
			if (vaultKeeps?.Count >= 1)
			{
				throw new Exception("This vault already has this keep");
			}
			_kRepo.AddToKeepCount(vk.KeepId);
			return _repo.Create(vk);
		}

		internal void Delete(int id, string userId)
		{
			VaultKeep found = Get(id);
			if (found.CreatorId != userId)
			{
				throw new Exception("This is not your vault to delete keeps from");
			}
			_kRepo.SubtractFromKeepCount(found.KeepId);
			_repo.Delete(id);
		}

		internal List<VaultKeepViewModel> GetKeeps(int vaultId, string userId)
		{
			Vault vault = _vRepo.Get(vaultId);
			if (vault.IsPrivate && vault.CreatorId != userId)
			{
				throw new Exception("You are not authorized to view this vault");
			}
			return _repo.GetKeeps(vaultId);

		}
	}
}