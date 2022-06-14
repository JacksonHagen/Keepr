import { AppState } from "../AppState.js";
import { logger } from "../utils/Logger.js";
import { api } from "./AxiosService.js";

class VaultsService
{
	async getMyVaults() {
		const res = await api.get('account/vaults')
		logger.log(res.data)
		AppState.myVaults = res.data
	}
	async create(vaultData) {
		const res = await api.post('api/vaults', vaultData)
		logger.log(res.data)
		AppState.activeUsersVaults.push(res.data)
	}
	async getById(vaultId) {
		const res = await api.get('api/vaults/' + vaultId)
		logger.log(res.data)
		AppState.activeVault = res.data
	}
	async delete(id) {
		await api.delete('api/vaults/' + id)
		const index = AppState.myVaults.find(v => v.id === id)
		AppState.myVaults.splice(index, 1)
	}
}

export const vaultsService = new VaultsService();