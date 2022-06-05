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
}

export const vaultsService = new VaultsService();