using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Interfaces;
using keepr.Models;

namespace keepr.Repositories
{
	public class VaultsRepository : IRepository<Vault, int>
	{
		private readonly IDbConnection _db;
		public VaultsRepository(IDbConnection db)
		{
			_db = db;
		}

		public List<Vault> Get()
		{
			string sql = @"
			SELECT 
				v.*,
				a.*
			FROM vaults v
			JOIN accounts a ON a.id = v.CreatorId
			";
			return _db.Query<Vault, Profile, Vault>(sql, (v, p) =>
			{
				v.Creator = p;
				return v;
			}).ToList();
		}

		public Vault Get(int id)
		{
			string sql = @"
			SELECT 
				v.*,
				a.*
			FROM vaults v
			JOIN accounts a ON a.id = v.CreatorId
			WHERE v.id = @id
			";
			return _db.Query<Vault, Profile, Vault>(sql, (v, p) =>
			{
				v.Creator = p;
				return v;
			}, new { id }).FirstOrDefault();
		}
		internal List<Vault> GetVaultsByProfileId(string id)
		{
			string sql = @"
			SELECT
				v.*,
				a.*
			FROM vaults v
			JOIN accounts a ON a.id = v.CreatorId
			WHERE a.id = @id";
			return _db.Query<Vault, Profile, Vault>(sql, (v, p) =>
			{
				v.Creator = p;
				return v;
			}, new { id }).ToList();
		}

		internal Vault Create(Vault vaultData)
		{
			string sql = @"
			INSERT INTO vaults
			(name, description, isPrivate, creatorId)
			VALUES
			(@Name, @Description, @IsPrivate, @CreatorId);
			SELECT LAST_INSERT_ID();";
			vaultData.Id = _db.ExecuteScalar<int>(sql, vaultData);
			return vaultData;
		}

		internal void Edit(Vault original)
		{
			string sql = @"
			UPDATE vaults
				SET
				name = @Name,
				description = @Description
			WHERE id = @Id
			";
			_db.Execute(sql, original);
		}
		internal void Delete(int id)
		{
			string sql = "DELETE FROM vaults WHERE id = @id LIMIT 1";
			_db.Execute(sql, new { id });
		}
	}
}