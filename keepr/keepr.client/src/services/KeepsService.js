import { AppState } from "../AppState.js";
import { logger } from "../utils/Logger.js";
import { api } from "./AxiosService.js";

class KeepsService
{
	async getAll() {
		const res = await api.get('api/keeps')
		AppState.keeps = res.data
	}
	async get(id) {
		const res = await api.get('api/keeps/' + id)
		AppState.activeKeep = res.data
	}
	async create(keepData) {
		const res = await api.post('api/keeps', keepData)
		logger.log('create keep', res.data)
		AppState.activeUsersKeeps.unshift(res.data)
	}
}

export const keepsService = new KeepsService();