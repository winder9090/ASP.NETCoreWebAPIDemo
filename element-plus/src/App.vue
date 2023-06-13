<template>
  <router-view></router-view>
</template>
<script lang="ts">

// import { ref, provide } from "vue";
// import router from './router/index'

export default {
  name: "App",
  setup() {
    // const width = document.documentElement.clientWidth;
    // const menuVisible = ref(width <= 500 ? false : true);
    // provide("menuVisible", menuVisible); // set
    // router.afterEach(() => {
    //   if (width <= 500) {
    //     menuVisible.value = false;
    //   }
    // });
  },
}
</script>

<style>
#app {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
}
</style>