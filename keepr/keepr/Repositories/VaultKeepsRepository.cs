using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
	public class VaultKeepsRepository
	{
		private readonly IDbConnection _db;

		public VaultKeepsRepository(IDbConnection db)
		{
			_db = db;
		}


		internal VaultKeep Get(int id)
		{
			string sql = "SELECT * FROM vaultkeeps WHERE id = @id";
			return _db.QueryFirstOrDefault<VaultKeep>(sql, new { id });
		}
		internal VaultKeep Create(VaultKeep vaultKeep)
		{
			string sql = @"
			INSERT INTO vaultkeeps
			(vaultId, keepId, creatorId)
			VALUES
			(@VaultId, @KeepId, @CreatorId);
			SELECT LAST_INSERT_ID();";
			vaultKeep.Id = _db.ExecuteScalar<int>(sql, vaultKeep);
			return vaultKeep;
		}

		internal void Delete(int id)
		{
			string sql = "DELETE FROM vaultkeeps WHERE id = @id LIMIT 1";
			_db.Execute(sql, new { id });
		}

		internal List<VaultKeepViewModel> GetKeeps(int vaultId)
		{
			string sql = @"
			SELECT
				a.*,
				k.*,
				vk.id as vaultKeepId
			FROM vaultkeeps vk
			JOIN keeps k ON k.id = vk.keepId
			JOIN accounts a ON a.id = k.creatorId
			WHERE vk.vaultId = @vaultId";
			return _db.Query<Profile, VaultKeepViewModel, VaultKeepViewModel>(sql, (p, vk) =>
			{
				vk.Creator = p;
				return vk;
			}, new { vaultId }).ToList();
		}
	}
}
