<template>
  <div class="card my-2 selectable" @click="openLgModal">
    <img :src="keep.img" alt="" class="card-img" />
    <div class="card-img-overlay text-light text-shadow d-flex align-items-end">
      <h1 class="fw-light">
        {{ keep.name }}
      </h1>
    </div>
  </div>
</template>


<script>
import { Modal } from 'bootstrap'
import { AppState } from '../AppState.js'
import { keepsService } from '../services/KeepsService.js'
export default {
  props: {
    keep: {
      type: Object,
      required: true
    }
  },
  setup(props) {
    return {
      async openLgModal() {
        await keepsService.get(props.keep.id)
        Modal.getOrCreateInstance(document.getElementById('modalLg')).toggle()
      }
    }
  }
}
</script>


<style lang="scss" scoped>
</style>