using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using keepr.Interfaces;
using keepr.Models;

namespace keepr.Repositories
{
	public class KeepsRepository : IRepository<Keep, int>
	{
		private readonly IDbConnection _db;

		public KeepsRepository(IDbConnection db)
		{
			_db = db;
		}

		public List<Keep> Get()
		{
			string sql = @"
			SELECT
				k.*,
				a.*
			FROM keeps k
			JOIN accounts a ON a.id = k.creatorId
			";
			return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
			{
				keep.Creator = profile;
				return keep;
			}).ToList();
		}

		public Keep Get(int id)
		{
			string sql = @"
			SELECT 
				k.*,
				a.*
			FROM keeps k 
			JOIN accounts a ON a.id = k.creatorId
			WHERE k.id = @id";
			return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
			{
				keep.Creator = profile;
				return keep;
			}, new { id }).FirstOrDefault();
		}

		internal void AddToKeepCount(int keepId)
		{
			string sql = @"
			UPDATE keeps
			SET kept = kept + 1
			WHERE id = @keepId";
			_db.Execute(sql, new { keepId });
		}
		internal void SubtractFromKeepCount(int keepId)
		{
			string sql = @"
			UPDATE keeps
			SET kept = kept - 1
			WHERE id = @keepId";
			_db.Execute(sql, new { keepId });
		}
		internal List<Keep> GetKeepsByProfileId(string id)
		{
			string sql = @"
			SELECT
				k.*,
				a.*
			FROM keeps k
			JOIN accounts a ON a.id = k.creatorId
			WHERE a.id = @id";
			return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) =>
			{
				keep.Creator = profile;
				return keep;
			}, new { id }).ToList();
		}
		internal void AddView(int id)
		{
			string sql = @"
			UPDATE keeps
			SET views = views + 1
			WHERE id = @id
			";
			_db.Execute(sql, new { id });
		}
		internal Keep Create(Keep keepData)
		{
			string sql = @"
			INSERT INTO keeps
			(name, description, img, views, kept, creatorId)
			VALUES
			(@Name, @Description, @Img, @Views, @Kept, @CreatorId);
			SELECT LAST_INSERT_ID();";
			keepData.Id = _db.ExecuteScalar<int>(sql, keepData);
			return keepData;
		}


		internal void Edit(Keep original)
		{
			string sql = @"
			UPDATE keeps
			SET
				name = @Name,
				description = @Description,
				img = @Img
			WHERE id = @Id;
			";
			_db.Execute(sql, original);
		}
		internal void Delete(int id)
		{
			string sql = "DELETE FROM keeps WHERE id = @id LIMIT 1";
			_db.Execute(sql, new { id });
		}

	}
}