import { AppState } from "../AppState.js";
import { logger } from "../utils/Logger.js";
import { api } from "./AxiosService.js";

class VaultKeepsService
{
	// TODO remove vaults from select when the keep is in that vault
	async addKeepToVault(vaultKeepData) {
		const res = await api.post('api/vaultkeeps', vaultKeepData)
		logger.log(res.data)
	}
	async getVaultKeepsByVault(vaultId) {
		const res = await api.get('api/vaults/' + vaultId + '/keeps')
		logger.log('vaultkeeps', res.data)
		AppState.activeVaultKeeps = res.data
	}
	async removeFromVault(vkId) {
		const res = await api.delete('api/vaultkeeps/' + vkId)
		logger.log('vk delete', res.data) 
		const index = AppState.activeVaultKeeps.findIndex(vk => vk.vaultKeepId === vkId)
		AppState.activeVaultKeeps.splice(index, 1)
	}
}

export const vaultKeepsService = new VaultKeepsService();