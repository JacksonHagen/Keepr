import { AppState } from "../AppState.js";
import { logger } from "../utils/Logger.js";
import { api } from "./AxiosService.js";

class VaultKeepsService
{
	// TODO remove vaults from select when the keep is in that vault
	async addKeepToVault(vaultKeepData) {
		const res = await api.post('api/vaultkeeps', vaultKeepData)
	}
	async getVaultKeepsByVault(vaultId) {
		const res = await api.get('api/vaults/' + vaultId + '/keeps')
		AppState.activeVaultKeeps = res.data
	}
}

export const vaultKeepsService = new VaultKeepsService();