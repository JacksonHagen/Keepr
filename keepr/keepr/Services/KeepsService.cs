using System;
using System.Collections.Generic;
using keepr.Models;
using keepr.Repositories;

namespace keepr.Services
{
	public class KeepsService
	{
		private readonly KeepsRepository _repo;

		public KeepsService(KeepsRepository repo)
		{
			_repo = repo;
		}

		internal List<Keep> Get()
		{
			return _repo.Get();
		}

		internal Keep Get(int id)
		{
			Keep found = _repo.Get(id);
			found.Views += 1;
			if (found == null)
			{
				throw new Exception("Invalid Id");
			}
			AddView(id);
			return found;
		}
		internal void AddView(int id)
		{
			_repo.AddView(id);
		}

		internal Keep Create(Keep keepData)
		{
			return _repo.Create(keepData);
		}

		internal Keep Edit(Keep keepData)
		{
			Keep original = Get(keepData.Id);
			IsOwner(keepData.CreatorId, original.CreatorId);
			original.Img = keepData.Img ?? original.Img;
			original.Name = keepData.Name ?? original.Name;
			original.Description = keepData.Description ?? original.Description;
			_repo.Edit(original);
			return original;
		}

		internal List<Keep> GetKeepsByProfileId(string id)
		{
			return _repo.GetKeepsByProfileId(id);
		}

		//NOTE This is where we will add to views whenever someone gets a keep by id
		internal void Delete(int id, string userId)
		{
			Keep target = Get(id);
			IsOwner(target.CreatorId, userId);
			_repo.Delete(id);
		}
		private static void IsOwner(string id1, string id2)
		{
			if (id1 != id2)
			{
				throw new Exception("You are not the owner of this keep");
			}
		}
	}
}