<template>
  <div
    class="modal fade"
    id="modalLg"
    tabindex="-1"
    role="dialog"
    aria-labelledby="modelTitleId"
    aria-hidden="true"
    v-if="keep"
  >
    <div class="modal-dialog modal-lg" role="document">
      <div class="modal-content">
        <div class="modal-body">
          <div class="row">
            <div class="col-6">
              <img :src="keep.img" alt="" class="img-fluid rounded" />
            </div>
            <div class="col-6 text-center py-2 d-flex flex-column">
              <div class="row">
                <div class="col-6 d-flex justify-content-end">
                  <h5 title="Views" class="text-secondary">
                    <i class="mdi mdi-eye text-primary"></i>
                    {{ keep.views }}
                  </h5>
                </div>
                <div class="col-6 d-flex">
                  <h5 title="Saved in vaults" class="text-secondary">
                    <i class="mdi mdi-safe text-primary"></i>
                    {{ keep.kept }}
                  </h5>
                </div>
              </div>
              <div class="row">
                <h1 class="amatic my-3">
                  {{ keep.name }}
                </h1>
                <div class="col-12">
                  <p class="comfortaa text-start">
                    {{ keep.description }}
                  </p>
                  <hr class="mx-4" />
                </div>
              </div>
              <div class="row mt-auto">
                <div class="col-5 text-start">
                  <label for="vaultSelect" class="">Add to a vault:</label>
                  <div class="mb-3">
                    <select
                      class="form-control text-primary"
                      name="vaultSelect"
                      id="vaultSelect"
                      @input="addToVault"
                      v-model="vault.vaultId"
                    >
                      <option disabled selected>Add to vault</option>
                      <option v-for="v in myVaults" :key="v.id" :value="v.id">
                        {{ v.name }}
                      </option>
                    </select>
                  </div>
                </div>
                <div
                  class="
                    col-1
                    d-flex
                    align-items-end
                    pb-2
                    justify-content-center
                  "
                  v-if="keep.creatorId === account.id"
                >
                  <h3>
                    <i class="mdi mdi-delete-outline text-danger"></i>
                  </h3>
                </div>
                <div class="col-6 d-flex align-items-end">
                  <p
                    class="selectable rounded"
                    @click="goToProfile(keep.creatorId)"
                  >
                    <img
                      :src="keep.creator?.picture"
                      class="rounded"
                      width="40"
                      height="40"
                    />
                    {{ keep.creator?.name }}
                  </p>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import { computed, ref } from '@vue/reactivity'
import { AppState } from '../AppState.js'
import { Modal } from 'bootstrap'
import { useRouter } from 'vue-router'
import { vaultKeepsService } from "../services/VaultKeepsService.js";
import Pop from '../utils/Pop.js'
export default {
  setup() {
    const router = useRouter()
    const vault = ref({})
    return {
      vault,
      keep: computed(() => AppState.activeKeep),
      myVaults: computed(() => AppState.myVaults),
      account: computed(() => AppState.account),
      goToProfile(id) {
        Modal.getOrCreateInstance(document.getElementById('modalLg')).hide()
        router.push({ name: "Profile", params: { id } })
      },
      async addToVault() {
        const vaultKeep = {
          vaultId: vault.value.vaultId,
          keepId: this.keep.id
        }
        try {
          await vaultKeepsService.addKeepToVault(vaultKeep)
          Pop.toast('Added to vault!', 'success')
          vault.value.vaultId = {}
        }
        catch (error) {
          console.error("[COULD_NOT_ADD_TO_VAULT]", error.message);
          Pop.toast(error.message, "error");
          vault.value.vaultId = {}
        }
      }
    }
  }
}
</script>


<style lang="scss" scoped>
</style>