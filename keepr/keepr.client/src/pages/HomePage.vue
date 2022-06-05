<template>
  <div class="container-fluid">
    <div class="masonry-with-columns">
      <Keep v-for="k in keeps" :key="k.id" :keep="k" />
    </div>
  </div>

</template>

<script>
import { computed } from '@vue/reactivity'
import { AppState } from '../AppState.js'
import { onMounted } from '@vue/runtime-core'
import { keepsService } from '../services/KeepsService.js'

import Pop from '../utils/Pop.js'
export default {
  setup() {
    onMounted(async () => {
      try {
        await keepsService.getAll()
      }
      catch (error) {
        console.error("[COULD_NOT_LOAD_KEEPS]", error.message);
        Pop.toast(error.message, "error");
      }
    })

    return {
      keeps: computed(() => AppState.keeps)
    }
  }
}
</script>

<style scoped lang="scss">
.home {
  display: grid;
  height: 80vh;
  place-content: center;
  text-align: center;
  user-select: none;
  .home-card {
    width: 50vw;
    > img {
      height: 200px;
      max-width: 200px;
      width: 100%;
      object-fit: contain;
      object-position: center;
    }
  }
}
</style>
